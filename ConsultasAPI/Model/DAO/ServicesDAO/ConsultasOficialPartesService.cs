using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sicoj.Utils.Models;
using Sicoj.Utils.ViewModels;
using ConsultasAPI.Model.Entities;
using ConsultasAPI.Model.IDAO;
using ConsultasAPI.Model.IDAO.IServiceDAO;
using ConsultasAPI.Model.DTO;
using Sicoj.Utils.Extentions;
using ConsultasAPI.Model.DTO.CatalogsContracts;
using Microsoft.Extensions.Configuration;
using Sicoj.Utils.Enums;
using ConsultasAPI.Model.ViewModels.Enums;
using Sicoj.Utils.Redis;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

namespace ConsultasAPI.Model.DAO.ServicesDAO
{
    public class ConsultasOficialPartesService : IConsultasOficialPartesService
    {

        private readonly IConsultaRepositoryOficialPartes _repository;
        private readonly IApiService _apiService;
        private readonly IRedisClient _redisClient;
        private readonly ProxyEnpoints _proxyEnpoints;
        private readonly CatalogosEnpoints _catologosEnpoints;


        public ConsultasOficialPartesService(
        IConfiguration configuration,
        IConsultaRepositoryOficialPartes repository,
        IApiService apiService,
        IRedisClient redisClient,
        IOptions<CatalogosEnpoints> catologosEnpoints,
        IOptions<ProxyEnpoints> proxyEnpoints
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(redisClient));
            _catologosEnpoints = catologosEnpoints.Value ?? throw new ArgumentNullException(nameof(catologosEnpoints));
            _proxyEnpoints = proxyEnpoints.Value ?? throw new ArgumentNullException(nameof(proxyEnpoints));
        }

        #region  Bandejas

        //Bandeja de pendientes cumplimentación

        public async Task<ResultOperation> GetBandejaPendientesCumplimentacionService(int Fetch, int Page, string? OrderByColumn, bool OrderDesc, UserInformationView userInformationView)
        {
            try
            {
                var countResult = await _repository.GetBandejaCumplimentacionCountAsyncRepository(
                        null!, userInformationView.IdAdministracionCentral
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseConsultaByFilters>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }

                Filters.GetDefaultPage(countResult.GetValueOrDefault(), ref Page, Fetch);

                var result = await _repository.GetBandejaPendientesCumplimentacionAsyncRepository(
                    Fetch,
                    Page,
                    OrderByColumn,
                    OrderDesc,
                    null!,
                    userInformationView.IdAdministracionCentral
                );

                var bandejaList = result.ToList();
                if (bandejaList.Any())
                {
                    _redisClient.ValidateTakeList(ref bandejaList, EnumModulosRedis.CONSULTAS);
                }

                if (result is null)
                {
                    return ResultOperation<List<ResponseConsultaByFilters>>.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableView<ResponseConsultaByFilters>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Bandeja de Pendientes
        public async Task<ResultOperation> GetBandejaPendientesService(int Fetch, int Page, string? OrderByColumn, bool OrderDesc, UserInformationView userInformationView)
        {
            try
            {
                var countResult = await _repository.GetAllByFiltersCountAsyncRepository(
                        null!, userInformationView.IdAdministracionCentral
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseConsultaByFilters>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }

                Filters.GetDefaultPage(countResult.GetValueOrDefault(), ref Page, Fetch);

                var result = await _repository.GetBandejaPendientesAsyncRepository(
                    Fetch,
                    Page,
                    OrderByColumn,
                    OrderDesc,
                    null!,
                    userInformationView.IdAdministracionCentral
                );

                var bandejaList = result.ToList();
                if (bandejaList.Any())
                {
                    _redisClient.ValidateTakeList(ref bandejaList, EnumModulosRedis.CONSULTAS);
                }

                if (result is null)
                {
                    return ResultOperation<List<ResponseConsultaByFilters>>.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableView<ResponseConsultaByFilters>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Bandeja de Historico
        public async Task<ResultOperation> GetBandejaHistoricoByFiltersService(int Fetch, int Page, string? OrderByColumn, bool OrderDesc, string? noAsunto, DateTime? fechaDesde, DateTime? fechaHasta, string? rfc, string? promovente, List<int>? TipoAsunto, List<int>? EstadoTarea, List<int>? TipoModalidad, List<int>? EstadoProcesal, UserInformationView userInformationView)
        {
            try
            {
                var countResult = await _repository.OPHistoricoCountAsyncRepository(
                        noAsunto,
                        fechaDesde,
                        fechaHasta,
                        rfc,
                        promovente,
                        TipoAsunto,
                        EstadoTarea,
                        TipoModalidad,
                        EstadoProcesal,
                        userInformationView.IdAdministracionCentral
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseConsultaByFilters>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }

                var result = await _repository.GetAllByFiltersAsyncRepository(
                    Fetch,
                    Page,
                    OrderByColumn,
                    OrderDesc,
                    noAsunto,
                    fechaDesde,
                    fechaHasta,
                    rfc,
                    promovente,
                    TipoAsunto,
                    EstadoTarea,
                    TipoModalidad,
                    EstadoProcesal,
                    userInformationView.IdAdministracionCentral
                );

                if (result is null)
                {
                    return ResultOperation<List<ResponseConsultaByFilters>>.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableView<ResponseConsultaByFilters>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public async Task<ResultOperation> AddConsultaService(Consulta entity, UserInformationView userInformationView)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                if (entity.fecha_presentacion.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de presentación no puede ser mayor a la fecha actual.");
                }

                if (entity.fecha_recepcion.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de recepcion no puede ser mayor a la fecha actual.");
                }

                if (entity.fecha_recepcion.Date < entity.fecha_presentacion.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse("La fecha de recepcion no puede ser menor a la fecha de presentacion.");
                }

                var responseUnidadAdministrativa = await _apiService.GetResultOperationAsync<AdministracionResponse>($"{_catologosEnpoints.RouteAdministracion}/{entity.id_administracion}");
                if (responseUnidadAdministrativa is null || !responseUnidadAdministrativa.Success || responseUnidadAdministrativa.Result is null)
                {
                    return ResultOperation.FailureWarningResponse<int>("No se pudo validar la unidad administrativa");
                }

                if (userInformationView.EsCentral.GetValueOrDefault() && !responseUnidadAdministrativa.Result.isCentral)
                    return ResultOperation.FailureWarningResponse<int>("El usuario solo puede registrar información de administraciones centrales.");
                else if (!userInformationView.EsCentral.GetValueOrDefault() && responseUnidadAdministrativa.Result.isCentral)
                    return ResultOperation.FailureWarningResponse<int>("El usuario solo puede registrar información de administraciones desconcentradas.");

                entity.id_Subadministracion = responseUnidadAdministrativa.Result.isCentral ? null! : entity.id_administracion;

                var responseFechaVencimiento = await _apiService.GetResultOperationAsync<string>($"{_proxyEnpoints.RouteFechasAutoCalculadas}?idModule={EnumModulosSicoj.CONSULTAS.GetHashCode()}&baseDate={entity.fecha_presentacion.ToString("yyyy-MM-dd")}&addDays={90}&nextDay={true}");
                if (responseFechaVencimiento is null || !responseFechaVencimiento.Success || string.IsNullOrEmpty(responseFechaVencimiento.Result))
                {
                    return ResultOperation.FailureWarningResponse<int>("No se pudo calcular la fecha de vencimiento");
                }

                if (!DateTime.TryParse(responseFechaVencimiento.Result, out DateTime fechaVencimiento))
                {
                    return ResultOperation.FailureWarningResponse<int>("La fecha de vencimiento no tiene el formato correcto.");
                }

                entity.fecha_vencimiento = fechaVencimiento;


                var result = await _repository.AddAsyncRepository(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResultOperation> UpdateConsultaService(Consulta entity, UserInformationView userInformationView)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                if (entity.fecha_presentacion > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de presentación no puede ser mayor a la fecha actual.");
                }

                if (entity.fecha_recepcion > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de presentación no puede ser mayor a la fecha actual.");
                }

                if (entity.fecha_recepcion < entity.fecha_recepcion)
                {
                    return ResultOperation<int?>.FailureWarningResponse("La fecha de presentación no puede ser menor a la fecha de recepción.");
                }

                var responseUnidadAdministrativa = await _apiService.GetResultOperationAsync<AdministracionResponse>($"{_catologosEnpoints.RouteAdministracion}/{entity.id_administracion}");
                if (responseUnidadAdministrativa is null || !responseUnidadAdministrativa.Success || responseUnidadAdministrativa.Result is null)
                {
                    return ResultOperation.FailureWarningResponse<int>("No se pudo validar la unidad administrativa");
                }

                if (userInformationView.EsCentral.GetValueOrDefault() && !responseUnidadAdministrativa.Result.isCentral)
                    return ResultOperation.FailureWarningResponse<int>("El usuario solo puede registrar información de administraciones centrales.");
                else if (!userInformationView.EsCentral.GetValueOrDefault() && responseUnidadAdministrativa.Result.isCentral)
                    return ResultOperation.FailureWarningResponse<int>("El usuario solo puede registrar información de administraciones desconcentradas.");

                entity.id_Subadministracion = responseUnidadAdministrativa.Result.isCentral ? null! : entity.id_administracion;

                var responseFechaVencimiento = await _apiService.GetResultOperationAsync<string>($"{_proxyEnpoints.RouteFechasAutoCalculadas}?idModule={EnumModulosSicoj.CONSULTAS.GetHashCode()}&baseDate={entity.fecha_presentacion.ToString("yyyy-MM-dd")}&addDays={90}&nextDay={true}");
                if (responseFechaVencimiento is null || !responseFechaVencimiento.Success || string.IsNullOrEmpty(responseFechaVencimiento.Result))
                {
                    return ResultOperation.FailureWarningResponse<int>("No se pudo calcular la fecha de vencimiento");
                }

                if (!DateTime.TryParse(responseFechaVencimiento.Result, out DateTime fechaVencimiento))
                {
                    return ResultOperation.FailureWarningResponse<int>("La fecha de vencimiento no tiene el formato correcto.");
                }

                entity.fecha_vencimiento = fechaVencimiento;
                var result = await _repository.UpdateAsyncRepository(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation<ResponseConsultaTurnar>> TurnarConsultaService(Consulta entity)
        {
            try
            {
                List<Message> messages = new List<Message>();
                var result = await _repository.TurnarConsultaRepository(entity);
                var (numeroAsunto, unidadAdministrativa) = await _repository.GetNumeroAsuntoYUnidadAdministrativaById(entity.id);

                if (!result.Success)
                    return ResultOperation.FailureErrorResponse<ResponseConsultaTurnar>($"{result.MsgError!}:{result.DetailError}");

                var resultOperation = ResultOperation.SuccessResponseNoMessage(new ResponseConsultaTurnar());


                var entityExists = await _repository.GetByIdAsync(result.Result.GetValueOrDefault());
                if (entityExists is not null)
                {
                    resultOperation.Result.noAsunto = entityExists.no_asunto!;
                    // var value = await _apiService.GetResultOperationAsync<AdminsitracionCentral>($"{_routeUnidadAdministrativa}/{entity.id_administracion}");
                    // if (value is not null && value.Success && value.Result is not null)
                    // {
                    //     resultOperation.Result.unidadAdministrativa = value.Result.nombre;

                    // }
                    // else
                    //     resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la unidad administrativa/subadminsitración.");
                    var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.Administracion, entity.id_administracion.ToString());
                    if (!string.IsNullOrEmpty(catalogValue))
                    {
                        resultOperation.Result.unidadAdministrativa = catalogValue;
                    }
                    else
                        resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la unidad administrativa/subadminsitración.");

                }
                else
                    resultOperation.AddWarningMessage("No se pudo recuperar el la información de la consulta despues de turnarla.");



                entityExists = await _repository.GetByIdDisconnected(entity.id);
                if (entityExists is not null)
                {
                    resultOperation.Result.noAsunto = entityExists.no_asunto!;
                }

                return resultOperation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> DeleteConsultaService(Consulta entity)
        {
            try
            {

                var result = await _repository.DeleteAsyncRepository(entity.id);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);//, "El registro se ha eliminado exitosamente.");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<ResponseConsultaRfc>> GetRfcampliadosyncService(string nombre) =>
       await _repository.GetRfcampliadosyncRepository(nombre);

        public async Task<ResultOperation<int>> AddArchivosAsyncService(ArchivoConsulta entity, DataFile dataFile)
        {

            try
            {

                var result = await _repository.AddFileAsyncRepository(entity, dataFile);
                if (!result.Success)
                    return ResultOperation.FailureErrorResponse<int>($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result.GetValueOrDefault());
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResponseArchivosConsulta> GetByIdArchivoDeleteService(int id) =>
        await _repository.GetByIdArchivoAsyncRepository(id);

        public async Task<ResponseArchivosConsulta> GetByIdArchivoService(int id) =>
        await _repository.GetByIdArchivoAsyncRepository(id);

        public async Task<ArchivoConsulta> GetIdArchivoConsultaService(int id) =>
        await _repository.GetIdArchivoConsultaAsyncRepository(id);

        public async Task<ResultOperation<int>> UpdateArchivoService(ArchivoConsulta entity, DataFile dataFile)
        {
            var result = await _repository.UpdateDocumentoAsync(entity, dataFile);
            if (!result.Success)
                return ResultOperation.FailureErrorResponse<int>($"{result.MsgError!}:{result.DetailError}");

            return ResultOperation.SuccessResponseNoMessage(result.Result.GetValueOrDefault());
        }

        public async Task<List<ResponseArchivosConsulta>> GetAllArchivoService(int id_registro) =>
        await _repository.GetArchivosByIdRegistroAsync_Repository(id_registro);

        public async Task<ResultOperation> DeleteArchivoConsultaService(ResponseArchivosConsulta entity)
        {
            try
            {

                var result = await _repository.DeleteByIdArchivoAsyncRepository(entity.id);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }





        public async Task<ResultOperation> GetByIdService(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsyncRepository(id);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Consulta> GetByAllService(int id) =>
        await _repository.GetByIdAllAsyncRepository(id);
        public async Task<RequestTurnarConsulta> GetByIdConsultaTurnadoService(int id) =>
        await _repository.GetByIdConsultaTurnadoRepository(id);

        public async Task<ResponseConsultaList> GetByIdDeleteService(int id) =>
        await _repository.GetbyDeleteRepository(id);


        public async Task<ResultOperation> GetByIdDisconnected(int id)
        {
            try
            {
                var result = await _repository.GetByIdDisconnected(id);
                if (result is null)
                {
                    return ResultOperation.FailureWarningResponse<ResponseConsultaList>("No se encontraron resultados.");
                }
                _redisClient.ValidateTake(result, EnumModulosRedis.CONSULTAS);

                var resultOperation = ResultOperation.SuccessResponseNoMessage(result);

                if (result.idTipoAsunto > 0)
                {
                    var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.TipoAsuntoConsulta, result.idTipoAsunto.ToString());
                    if (!string.IsNullOrEmpty(catalogValue))
                    {
                        resultOperation.Result.tipoAsunto = catalogValue;
                    }
                    else
                        resultOperation.AddWarningMessage("No se pudo recuperar el nombre del tipo asunto.");



                }

                if (result.idTipoModalidad > 0)
                {
                    var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.TipoEntradaConsulta, result.idTipoModalidad.ToString());
                    if (!string.IsNullOrEmpty(catalogValue))
                    {
                        resultOperation.Result.tipoModalidad = catalogValue;
                    }
                    else
                        resultOperation.AddWarningMessage("No se pudo recuperar el nombre del tipo de modalidad.");
                }

                if (result.idAdministracion > 0)
                {
                    var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.Administracion, result.idAdministracion.ToString());
                    if (!string.IsNullOrEmpty(catalogValue))
                    {
                        resultOperation.Result.administracion = catalogValue;
                    }
                    else
                        resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la unidad administrativa/subadminsitración.");

                }

                // if (result.idSubadministracion > 0)
                // {
                //     var response= await _apiService.GetResultOperationAsync<CatalogSicoj>($"{_routeSubadministracion}/{result.idSubadministracion}");
                //     if (response is not null && response.Success && response.Result is not null)
                //     {
                //         resultOperation.Result.subadministracion = response.Result.nombre!;
                //     }
                //     else
                //         resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la unidad administrativa/subadminsitración.");
                // }

                if (result.idEstadoProcesal > 0)
                {

                    var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.EstadoProcesalConsulta, result.idEstadoProcesal.ToString());
                    if (!string.IsNullOrEmpty(catalogValue))
                    {
                        resultOperation.Result.estadoProcesal = catalogValue;
                    }
                    else
                        resultOperation.AddWarningMessage("No se pudo recuperar el nombre del estado procesal.");
                }

                if (result.idEstadoTarea > 0)
                {
                    var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.TareaConsulta, result.idEstadoTarea.ToString());
                    if (!string.IsNullOrEmpty(catalogValue))
                    {
                        resultOperation.Result.estadoTarea = catalogValue;
                    }
                    else
                        resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la tarea.");
                }


                return resultOperation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> GetArchivoByFiltersService
       (
          int Fetch,
          int Page,
          string? OrderByColumn,
          bool OrderDesc,
          int? idRol,
          string? folio,
          int? idSeccion,
          int? idConsulta,
          int? idRemision,
          bool? estatus,
          DateTime? fechaCreacionDesde,
          DateTime? fechaCreacionHasta,
          bool? remplazable,
          bool? permanente,
          UserInformationView userInformationView
       )
        {
            try
            {
                var countResult = await _repository.GetArchivosByFiltersCountAsyncRepository(
                        idRol,
                        folio,
                        idSeccion,
                        idConsulta,
                        idRemision,
                        estatus,
                        fechaCreacionDesde,
                        fechaCreacionHasta,
                        remplazable,
                        permanente
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseArchivosConsulta>>.SuccessResponseNoMessage("No hay elementos que mostrar");// .FailureWarningResponse("No se encontraron resultados"); ;
                }

                var result = await _repository.GetArchivoByFiltersAsyncRepository(
                    Fetch,
                    Page,
                    OrderByColumn,
                    OrderDesc,
                    idRol,
                    folio,
                    idSeccion,
                    idConsulta,
                    idRemision,
                    estatus,
                    fechaCreacionDesde,
                    fechaCreacionHasta,
                    remplazable,
                    permanente
                );

                if (result is null)
                {
                    return ResultOperation<List<ResponseArchivosConsulta>>.SuccessResponseNoMessage("No hay elementos que mostrar");//.FailureWarningResponse("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableView<ResponseArchivosConsulta>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Solicitud Transparencia

        public async Task<ResultOperation> AddSolicitudTransparenciaService(SolicitudTransparencia entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var consultaDetails = await _repository.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                DateTime dateTime = DateTime.Now;
                if (entity.fechaSolicitud.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de solicitud no debe ser mayor a la fecha actual.");
                }


                var result = await _repository.AddAsyncSolicitudTransparenciaService(entity, entityDocumento, dataFile);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SolicitudTransparencia> GetByIdSolicitudTransparenciaService(int id) =>
        await _repository.GetByIdSolicitudTransparenciaRepository(id);

        public async Task<ResultOperation> UpdateSolicitudTransparenciaService(SolicitudTransparencia entity)
        {
            try
            {
                var consultaDetails = await _repository.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                DateTime dateTime = DateTime.Now;
                if (entity.fechaSolicitud.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de solicitud no debe ser mayor a la fecha actual.");
                }

                var result = await _repository.UpdateSolicitudTransparenciaRepository(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> GetByIdSolicitudTransparenciaServices(int id)
        {
            try
            {
                var result = await _repository.GetByIdSolicitudTransparenciaRepositorys(id);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> GetTablaSolicitudTransparenciaService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertas.VERDE.GetHashCode();
                string alerta = EnumAlertas.VERDE.ToString();


                var countResult = await _repository.GetTablaSolicitudTransparenciaCountRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertas.GRIS.GetHashCode();
                    alerta = EnumAlertas.GRIS.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                        null!)
                    );
                }

                /*
                var countAlertaResult = await _repository.GetTablaAlertaSolicitudTransparenciaCountRepository(idConsulta);
                
                if ( countAlertaResult > 0)
                {
                    idAlerta=EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta=EnumAlertaSeccion.EN_PROCESO.ToString();
                }
                */
                var result = await _repository.GetTablaSolicitudTransparenciaRepository(idConsulta);

                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseSolicitudTransparenciaList>(new(idAlerta, alerta),
                    null!)
                );
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseSolicitudTransparenciaList>(new(idAlerta, alerta),
                       result)
                   );

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> DeleteSolicitudTransparenciaService(SolicitudTransparencia entity)
        {
            try
            {

                var result = await _repository.DeleteSolicitudTransparenciaRepository(entity.id);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region Cumplimentacion

        public async Task<ResultOperation> AddCumplimentacionService(Cumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var result2 = await _repository.GetByNoAsuntoConsulta(entity.no_asunto_consulta!);
                if (result2 is null)
                {
                    return ResultOperation.FailureWarningResponse<ResponseConsultaList>("No se encontraron resultados.");

                }
                entity.id_consulta = result2.id;
                if (entityDocumento is not null)
                {
                    entityDocumento.id_consulta = result2.id;
                }
                var consultaDetails = await _repository.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");

                }

                if (consultaDetails.idEstadoProcesal != 8 && consultaDetails.idEstadoProcesal != 7)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se puede cumplimentar el número de asunto " + consultaDetails.no_asunto + " debido a que el estado procesal es diferente a “Concluido notificado” o “Resuelto notificado” y no puede continuar el registro");

                }

                var responseUnidadAdministrativa = await _apiService.GetResultOperationAsync<AdministracionResponse>($"{_catologosEnpoints.RouteAdministracion}/{entity.id_administracion}");
                if (responseUnidadAdministrativa is null || !responseUnidadAdministrativa.Success || responseUnidadAdministrativa.Result is null)
                {
                    return ResultOperation.FailureWarningResponse<int>("No se pudo validar la unidad administrativa");

                }

                var responseUnidadAdministrativaSolicita = await _apiService.GetResultOperationAsync<AdministracionResponse>($"{_catologosEnpoints.RouteAdministracion}/{entity.idUnidadAdministrativaSolicitaCump}");
                if (responseUnidadAdministrativaSolicita is null || !responseUnidadAdministrativaSolicita.Success || responseUnidadAdministrativaSolicita.Result is null)
                {
                    return ResultOperation.FailureWarningResponse<int>("No se pudo validar la unidad administrativa");

                }

                if (entity.plazoCumplimentar == 1 || entity.plazoCumplimentar == 2)
                {
                    if (entity.plazoCumplimentar == 1)
                    {
                        var responseFechaVencimiento = await _apiService.GetResultOperationAsync<string>($"{_proxyEnpoints.RouteFechasAutoCalculadas}?idModule={EnumModulosSicoj.CONSULTAS.GetHashCode()}&baseDate={entity.fecha_firmeza!.Value.ToString("yyyy-MM-dd")}&addDays={122}&nextDay={true}");
                        if (responseFechaVencimiento is null || !responseFechaVencimiento.Success || string.IsNullOrEmpty(responseFechaVencimiento.Result))
                        {
                            return ResultOperation.FailureWarningResponse<int>("No se pudo calcular la fecha de vencimiento");

                        }

                        if (!DateTime.TryParse(responseFechaVencimiento.Result, out DateTime fechaVencimiento))
                        {
                            return ResultOperation.FailureWarningResponse<int>("La fecha de vencimiento no tiene el formato correcto.");

                        }

                        entity.fecha_vencimiento = fechaVencimiento;
                    }
                    else if (entity.plazoCumplimentar == 2)
                    {
                        var responseFechaVencimiento = await _apiService.GetResultOperationAsync<string>($"{_proxyEnpoints.RouteFechasAutoCalculadas}?idModule={EnumModulosSicoj.CONSULTAS.GetHashCode()}&baseDate={entity.fecha_firmeza!.Value.ToString("yyyy-MM-dd")}&addDays={30}&nextDay={true}");
                        if (responseFechaVencimiento is null || !responseFechaVencimiento.Success || string.IsNullOrEmpty(responseFechaVencimiento.Result))
                        {
                            return ResultOperation.FailureWarningResponse<int>("No se pudo calcular la fecha de vencimiento");

                        }

                        if (!DateTime.TryParse(responseFechaVencimiento.Result, out DateTime fechaVencimiento))
                        {
                            return ResultOperation.FailureWarningResponse<int>("La fecha de vencimiento no tiene el formato correcto.");

                        }

                        entity.fecha_vencimiento = fechaVencimiento;
                    }
                }

                var result = await _repository.AddCumplimentacionRepository(entity, entityDocumento!, dataFile);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation<ResponseConsultaTurnar>> TurnarCumplimentacionService(Cumplimentacion entity)
        {
            try
            {
                List<Message> messages = new List<Message>();
                var result = await _repository.TurnarCumplimentacionRepository(entity);
                var (numeroAsunto, unidadAdministrativa) = await _repository.GetNumeroAsuntoYUnidadAdministrativaById(entity.id);

                if (!result.Success)
                    return ResultOperation.FailureErrorResponse<ResponseConsultaTurnar>($"{result.MsgError!}:{result.DetailError}");

                var resultOperation = ResultOperation.SuccessResponseNoMessage(new ResponseConsultaTurnar());


                var entityExists = await _repository.GetByIdAsync(result.Result.GetValueOrDefault());
                if (entityExists is not null)
                {
                    resultOperation.Result.noAsunto = entityExists.no_asunto!;
                    // var value = await _apiService.GetResultOperationAsync<AdminsitracionCentral>($"{_routeUnidadAdministrativa}/{entity.id_administracion}");
                    // if (value is not null && value.Success && value.Result is not null)
                    // {
                    //     resultOperation.Result.unidadAdministrativa = value.Result.nombre;

                    // }
                    // else
                    //     resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la unidad administrativa/subadminsitración.");
                    var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.Administracion, entity.id_administracion.ToString());
                    if (!string.IsNullOrEmpty(catalogValue))
                    {
                        resultOperation.Result.unidadAdministrativa = catalogValue;
                    }
                    else
                        resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la unidad administrativa/subadminsitración.");

                }
                else
                    resultOperation.AddWarningMessage("No se pudo recuperar el la información de la consulta despues de turnarla.");

                entityExists = await _repository.GetByIdDisconnected(entity.id);
                if (entityExists is not null)
                {
                    resultOperation.Result.noAsunto = entityExists.no_asunto!;
                }

                return resultOperation;


            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<ResultOperation> UpdateCumplimentacionService(Cumplimentacion entity, UserInformationView userInformationView)
        {
            try
            {
                var responseUnidadAdministrativa = await _apiService.GetResultOperationAsync<AdministracionResponse>($"{_catologosEnpoints.RouteAdministracion}/{entity.id_administracion}");
                if (responseUnidadAdministrativa is null || !responseUnidadAdministrativa.Success || responseUnidadAdministrativa.Result is null)
                {
                    return ResultOperation.FailureWarningResponse<int>("No se pudo validar la unidad administrativa");
                }

                var responseUnidadAdministrativaSolicita = await _apiService.GetResultOperationAsync<AdministracionResponse>($"{_catologosEnpoints.RouteAdministracion}/{entity.idUnidadAdministrativaSolicitaCump}");
                if (responseUnidadAdministrativaSolicita is null || !responseUnidadAdministrativaSolicita.Success || responseUnidadAdministrativaSolicita.Result is null)
                {
                    return ResultOperation.FailureWarningResponse<int>("No se pudo validar la unidad administrativa");
                }

                if (entity.plazoCumplimentar == 1 || entity.plazoCumplimentar == 2)
                {
                    if (entity.plazoCumplimentar == 1)
                    {
                        var responseFechaVencimiento = await _apiService.GetResultOperationAsync<string>($"{_proxyEnpoints.RouteFechasAutoCalculadas}?idModule={EnumModulosSicoj.CONSULTAS.GetHashCode()}&baseDate={entity.fecha_firmeza!.Value.ToString("yyyy-MM-dd")}&addDays={122}&nextDay={true}");
                        if (responseFechaVencimiento is null || !responseFechaVencimiento.Success || string.IsNullOrEmpty(responseFechaVencimiento.Result))
                        {
                            return ResultOperation.FailureWarningResponse<int>("No se pudo calcular la fecha de vencimiento");

                        }

                        if (!DateTime.TryParse(responseFechaVencimiento.Result, out DateTime fechaVencimiento))
                        {
                            return ResultOperation.FailureWarningResponse<int>("La fecha de vencimiento no tiene el formato correcto.");

                        }

                        entity.fecha_vencimiento = fechaVencimiento;
                    }
                    else if (entity.plazoCumplimentar == 2)
                    {
                        var responseFechaVencimiento = await _apiService.GetResultOperationAsync<string>($"{_proxyEnpoints.RouteFechasAutoCalculadas}?idModule={EnumModulosSicoj.CONSULTAS.GetHashCode()}&baseDate={entity.fecha_firmeza!.Value.ToString("yyyy-MM-dd")}&addDays={30}&nextDay={true}");
                        if (responseFechaVencimiento is null || !responseFechaVencimiento.Success || string.IsNullOrEmpty(responseFechaVencimiento.Result))
                        {
                            return ResultOperation.FailureWarningResponse<int>("No se pudo calcular la fecha de vencimiento");

                        }

                        if (!DateTime.TryParse(responseFechaVencimiento.Result, out DateTime fechaVencimiento))
                        {
                            return ResultOperation.FailureWarningResponse<int>("La fecha de vencimiento no tiene el formato correcto.");

                        }

                        entity.fecha_vencimiento = fechaVencimiento;
                    }
                }

                var result = await _repository.UpdateCumplimentacionRepository(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<Cumplimentacion> GetByAllServiceCumplimentacion(int id) =>
        await _repository.GetByIdAllAsyncRepositoryCumplimentacion(id);

        public async Task<ResultOperation> GetByNoAsuntoCumplimentacion(string noAsunto, UserInformationView userInformationView, int contadorvisitas)
        {
            try
            {
                var result = await _repository.GetByNoAsuntoConsulta(noAsunto);
                if (result is null)
                {
                    // return ResultOperation.FailureWarningResponse<ResponseConsultaList>("No se encontraron resultados.");
                    return ResultOperation<int?>.SuccessResponse(contadorvisitas, "No se encontraron resultados.");
                }

                if (result.idEstadoProcesal != 8 && result.idEstadoProcesal != 7)
                {
                    return ResultOperation<int?>.SuccessResponse(result, "No se puede cumplimentar el número de asunto " + result.no_asunto + " debido a que el estado procesal es diferente a “Concluido notificado” o “Resuelto notificado” y no puede continuar el registro");
                }


                var responseUnidadAdministrativa = await _apiService.GetResultOperationAsync<AdministracionResponse>($"{_catologosEnpoints.RouteAdministracion}/{result.idAdministracion}");
                if (responseUnidadAdministrativa is null || !responseUnidadAdministrativa.Success || responseUnidadAdministrativa.Result is null)
                {
                    return ResultOperation.FailureWarningResponse<int>("No se pudo validar la unidad administrativa");
                }

                if (userInformationView.IdAdministracion != result.idAdministracion)
                {
                    //return ResultOperation.FailureWarningResponse<int>("El asunto pertenece a otra administracion diferente ¿Está seguro de que desea realizar la cumplimentación?");
                    // return ResultOperation<int?>.SuccessResponse(result,"El asunto pertenece a otra administracion diferente ¿Está seguro de que desea realizar la cumplimentación?");
                    result.perteneceOtraUnidad = true;
                }

                var result2 = await _repository.GetCumplimentacionById(result.no_asunto!);
                if (result2 is not null)
                {
                    result.intentos = contadorvisitas;
                    result.cumplimentar = false;
                    //return ResultOperation.FailureWarningResponse<ResponseConsultaList>("El asunto capturado ya fue cumplimentado con el " + result2.no_asunto + " por lo que no puede registrar otra cumplimentación");
                    return ResultOperation<int?>.SuccessResponse(result, "El asunto capturado ya fue cumplimentado con el " + result2.no_asunto + " por lo que no puede registrar otra cumplimentación");
                }

                var resultOperation = ResultOperation.SuccessResponseNoMessage(result);

                if (result.idAdministracion > 0)
                {
                    var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.Administracion, result.idAdministracion.ToString());
                    if (!string.IsNullOrEmpty(catalogValue))
                    {
                        resultOperation.Result.administracion = catalogValue;
                    }
                    else
                        resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la unidad administrativa/subadminsitración.");

                }
                resultOperation.Result.intentos = contadorvisitas;
                resultOperation.Result.cumplimentar = true;
                resultOperation.Result.idAdministracionCentral = 0;
                resultOperation.Result.administracionCentral = "";
                resultOperation.Result.idAdministracion = 0;
                resultOperation.Result.administracion = "";
                resultOperation.Result.idSubadministracion = 0;
                resultOperation.Result.subadministracion = "";
                resultOperation.Result.idEstadoProcesal = 0;
                resultOperation.Result.estadoProcesal = "";
                resultOperation.Result.idEstadoTarea = 0;
                resultOperation.Result.estadoTarea = "";
               

                return resultOperation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Envio email
        public async Task<ResultOperation> EnvioEmail(Email entity)

        {
            try
            {
                using var httpClient = new HttpClient();

                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(string.Join(",", entity.To)), "To");
                formData.Add(new StringContent(string.Join(",", entity.Cc)), "Cc");
                formData.Add(new StringContent(entity.Subject), "Subject");
                formData.Add(new StringContent(entity.Body), "Body");
                formData.Add(new StringContent(entity.IsHtml.ToString()), "IsHtml");
                formData.Add(new StringContent(entity.Priority?.ToString() ?? string.Empty), "Priority");


                var response = await httpClient.PostAsync($"{_proxyEnpoints.RouteEmail}", formData);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Correo enviado exitosamente.");
                }
                else
                {
                    Console.WriteLine($"Error al enviar correo: {response.StatusCode}");
                }


                return ResultOperation.SuccessResponseNoMessage(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}