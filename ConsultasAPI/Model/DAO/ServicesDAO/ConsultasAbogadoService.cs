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
using ConsultasAPI.Model.IDAO.IRepository;
using Microsoft.Extensions.Configuration;
using Sicoj.Utils.Extentions;
using ConsultasAPI.Model.DTO.CatalogsContracts;
using Sicoj.Utils.Enums;
using ConsultasAPI.Model.DTO.Contracts.Abogado;
using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.Entities.Events.Abogado;
using Microsoft.Extensions.Configuration;
using Sicoj.Utils.Middleware;
using ConsultasAPI.Model.ViewModels.Enums;
using Sicoj.Utils.Redis;
using Microsoft.Extensions.Options;


namespace ConsultasAPI.Model.DAO.ServicesDAO
{
    public class ConsultasAbogadoService : IConsultasAbogadoService
    {
        private readonly IConsultaRepositoryAbogado _repositoryAbogado;
        private readonly IApiService _apiService;
        private readonly IRedisClient _redisClient;
        // private readonly string _routeTipoAsunto = null!;
        // private readonly string _routeTipoModalidad = null!;
        // private readonly string _routeEstadoTarea = null!;
        // private readonly string _routeEstadoProcesal = null!;
        // private readonly string _routeUnidadAdministrativa = null!;
        // private readonly string _routeSubadministracion = null!;
        // private readonly string _routeUsuarioInfoDetalle = null!;
        private readonly string _routeInfoUsuario = null!;
        private readonly CatalogosEnpoints _catologosEnpoints;
        private readonly ProxyEnpoints _proxyEnpoints;
        public ConsultasAbogadoService(IConfiguration configuration, IConsultaRepositoryAbogado repositoryAbogado, IApiService apiService, IRedisClient redisClient, IOptions<CatalogosEnpoints> catologosEnpoints, IOptions<ProxyEnpoints> proxyEnpoints)
        {
            _repositoryAbogado = repositoryAbogado ?? throw new ArgumentNullException(nameof(repositoryAbogado));
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(_redisClient));
            // _routeTipoAsunto = configuration.GetValue<string>("CatalogsEndpoints:RouteTipoAsunto")!;
            // _routeTipoModalidad = configuration.GetValue<string>("CatalogsEndpoints:RouteTipoModalidad")!;
            // _routeEstadoTarea = configuration.GetValue<string>("CatalogsEndpoints:RouteEstadoTarea")!;
            // _routeEstadoProcesal = configuration.GetValue<string>("CatalogsEndpoints:RouteEstadoProcesal")!;
            // _routeUnidadAdministrativa = configuration.GetValue<string>("CatalogsEndpoints:RouteAdministracion")!;
            // _routeSubadministracion = configuration.GetValue<string>("CatalogsEndpoints:RouteAdministracionCentral")!;
            // _routeUsuarioInfoDetalle = configuration.GetValue<string>("CatalogsEndpoints:RouteUsuarioInfoDetalle")!;
            _routeInfoUsuario = configuration.GetValue<string>("CatalogsEndpoints:RouteInfoUsuario")!;
            _catologosEnpoints = catologosEnpoints.Value ?? throw new ArgumentNullException(nameof(catologosEnpoints));
            _proxyEnpoints = proxyEnpoints.Value ?? throw new ArgumentNullException(nameof(proxyEnpoints));
            _redisClient = redisClient;
        }

        public async Task<ResultOperation> AddRemisionAbogadoService(Remision entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var result2 = await _repositoryAbogado.GetByIdDisconnected(entity.id_consulta);

                if (result2.idAdministracion == entity.id_administracion_remite)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se puede remitir a la misma administracion en la que se encuentra el registro.");
                }

                DateTime dateTime = DateTime.Now;

                if (entity.fecha_oficio.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de oficio no puede ser mayor a la fecha actual.");
                }

                var result = await _repositoryAbogado.AddRemisionAbogadoRepository(entity, entityDocumento, dataFile);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> AddPersonasAutorizadas(PersonasAutorizadas entity)
        {
            try
            {
                var result = await _repositoryAbogado.AddPersonasAutorizadas(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PersonasAutorizadas> GetByAllServicePersonaAutorizadas(int id) =>
        await _repositoryAbogado.GetByIdAllAsyncRepositoryPersonasAutorizadas(id);

        public async Task<ResultOperation> UpdatePersonasAutorizadas(PersonasAutorizadas entity)
        {
            try
            {
                var result = await _repositoryAbogado.UpdateAsyncPersonasAutorizadas(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> DeletePersonasAutorizadas(PersonasAutorizadas entity)
        {
            try
            {

                var result = await _repositoryAbogado.DeleteAsyncPersonasAutorizadas(entity.id);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> GetTablaPersonasAutorizadasService(int Fetch, int Page, int idConsulta)
        {
            try
            {
                var countResult = await _repositoryAbogado.GetTablaPersonasAutorizadasCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseTablaPersonasAutorizadas>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }
                var result = await _repositoryAbogado.GetTablaPersonasAutorizadasAsyncRepository(
                    Fetch,
                    Page,
                    idConsulta
                );

                if (result is null)
                {
                    return ResultOperation<List<ResponseTablaPersonasAutorizadas>>.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponse(
                    new DataTableView<ResponseTablaPersonasAutorizadas>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> GetTablaRemisionAbogadoService(int Fetch, int Page)
        {
            try
            {
                var countResult = await _repositoryAbogado.GetTablaRemisionAbogadoCountAsyncRepository();

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseTablaRemision>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }
                var result = await _repositoryAbogado.GetTablaRemisionAbogadoAsyncRepository(
                    Fetch,
                    Page
                );

                if (result is null)
                {
                    return ResultOperation<List<ResponseTablaRemision>>.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponse(
                    new DataTableView<ResponseTablaRemision>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Consulta> GetByAllService(int id) =>
        await _repositoryAbogado.GetByIdAllAsyncRepository(id);

        public async Task<ResultOperation<int>> AddArchivosAsyncService(ArchivoConsulta entity, DataFile dataFile)
        {

            try
            {

                var result = await _repositoryAbogado.AddFileAsyncRepository(entity, dataFile);

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
        await _repositoryAbogado.GetByIdArchivoAsyncRepository(id);

        public async Task<ResultOperation> DeleteArchivoConsultaService(ResponseArchivosConsulta entity)
        {
            try
            {

                var result = await _repositoryAbogado.DeleteByIdArchivoAsyncRepository(entity.id);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);//, "El documento se ha eliminado exitosamente.");
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<ArchivoConsulta> GetIdArchivoConsultaService(int id) =>
       await _repositoryAbogado.GetIdArchivoConsultaAsyncRepository(id);

        public async Task<ResultOperation<int>> UpdateArchivoService(ArchivoConsulta entity, DataFile dataFile)
        {
            var result = await _repositoryAbogado.UpdateDocumentoAsync(entity, dataFile);
            if (!result.Success)
                return ResultOperation.FailureErrorResponse<int>($"{result.MsgError!}:{result.DetailError}");

            return ResultOperation.SuccessResponseNoMessage(result.Result.GetValueOrDefault());
        }

        public async Task<List<ResponseArchivosConsulta>> GetAllArchivoService(int id_remision) =>
        await _repositoryAbogado.GetArchivosByIdRegistroAsync_Repository(id_remision);

        public async Task<ResponseArchivosConsulta> GetByIdArchivoService(int id) =>
        await _repositoryAbogado.GetByIdArchivoAsyncRepository(id);

        public async Task<Consulta> GetByIdServiceAbogado(int id) =>
       await _repositoryAbogado.GetByIdAsyncRepositoryAbogado(id);

        public async Task<ResultOperation> UpdateConsultaServiceAbogadoImpuestosInternos(Consulta entity)
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
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de presentación no puede ser mayor a la fecha actual.");
                }

                if (entity.fecha_recepcion.Date < entity.fecha_recepcion.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse("La fecha de presentación no puede ser menor a la fecha de recepción.");
                }


                var result = await _repositoryAbogado.UpdateAsyncRepositoryAbogadoImpuestosInternos(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> UpdateConsultaServiceAbogadoComercioExterior(Consulta entity)
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
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de presentación no puede ser mayor a la fecha actual.");
                }

                if (entity.fecha_recepcion.Date < entity.fecha_recepcion.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse("La fecha de presentación no puede ser menor a la fecha de recepción.");
                }


                var result = await _repositoryAbogado.UpdateAsyncRepositoryAbogadoComercioExterior(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation<ResponseAsignar>> AsignarConsultaService(Consulta entity)
        {
            try
            {

                var result = await _repositoryAbogado.AsignarConsultaRepository(entity);
                if (!result.Success)
                    return ResultOperation.FailureErrorResponse<ResponseAsignar>($"{result.MsgError!}:{result.DetailError}");

                var resultOperation = ResultOperation.SuccessResponseNoMessage(new ResponseAsignar());

                resultOperation.Result.noAsunto = entity.no_asunto!;
                var response = await _apiService.GetResultOperationAsync<UserInformationView>($"{_routeInfoUsuario}/{entity.id_abogado}");
                if (response is not null && response.Success && response.Result is not null)
                {
                    resultOperation.Result.abogado = response.Result.Nombre!;
                }
                else
                    resultOperation.AddWarningMessage("No se pudo recuperar el nombre del abogado.");

                return resultOperation;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> GetByIdDisconnected(int id)
        {
            try
            {
                var result = await _repositoryAbogado.GetByIdDisconnected(id);
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

        #region  Historico
        public async Task<ResultOperation> GetBandejaHistoricoByFiltersService(
            int Fetch,
            int Page,
            string? OrderByColumn,
            bool OrderDesc,
            string? noAsunto,
            DateTime? fechaPresentacionDesde,
            DateTime? fechaPresentacionHasta,
            DateTime? fechaVencimientoDesde,
            DateTime? fechaVencimientoHasta,
            string? rfcPromovente,
            string? promovente,
            List<int>? tipoAsuntoList,
            List<int>? estadoTareaList,
            List<int>? tipoModalidadList,
            List<int>? estadoProcesalList,
            List<int>? alertaList,
            UserInformationView userInformationView)
        {
            try
            {
                var countResult = await _repositoryAbogado.GetHistoricoCountAsyncRepository(
                        noAsunto,
                        fechaPresentacionDesde,
                        fechaPresentacionHasta,
                        fechaVencimientoDesde,
                        fechaVencimientoHasta,
                        rfcPromovente,
                        promovente,
                        tipoAsuntoList,
                        estadoTareaList,
                        tipoModalidadList,
                        estadoProcesalList,
                        alertaList,
                        userInformationView.IdAdministracionCentral,
                        userInformationView.IdAdministracion
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseConsultaByFilters>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }

                var result = await _repositoryAbogado.GetHistoricosAsyncRepository(
                    Fetch,
                    Page,
                    OrderByColumn,
                    OrderDesc,
                     noAsunto,
                    fechaPresentacionDesde,
                    fechaPresentacionHasta,
                    fechaVencimientoDesde,
                    fechaVencimientoHasta,
                    rfcPromovente,
                    promovente,
                    tipoAsuntoList,
                    estadoTareaList,
                    tipoModalidadList,
                    estadoProcesalList,
                    alertaList,
                    userInformationView.IdAdministracionCentral,
                    userInformationView.IdAdministracion
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

        #region Bandeja de pendientes
        public async Task<ResultOperation> GetBandejaPendientes_AbogadoService(
             int Fetch,
            int Page,
            string? OrderByColumn,
            bool OrderDesc,
            string? noAsunto,
            DateTime? fechaPresentacionDesde,
            DateTime? fechaPresentacionHasta,
            DateTime? fechaVencimientoDesde,
            DateTime? fechaVencimientoHasta,
            string? rfcPromovente,
            string? promovente,
            List<int>? tipoAsuntoList,
            List<int>? estadoTareaList,
            List<int>? tipoModalidadList,
            List<int>? estadoProcesalList,
            UserInformationView userInformationView)

        {
            try
            {
                var countResult = await _repositoryAbogado.GetPendientesCountAsyncRepositoryAbogado(
                        noAsunto,
                        fechaPresentacionDesde,
                        fechaPresentacionHasta,
                        fechaVencimientoDesde,
                        fechaVencimientoHasta,
                        rfcPromovente,
                        promovente,
                        tipoAsuntoList,
                        estadoTareaList,
                        tipoModalidadList,
                        estadoProcesalList,
                        userInformationView.IdAdministracionCentral,
                        userInformationView.IdAdministracion,
                        userInformationView.IdSubadministracion,
                        userInformationView.Rfc
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseConsultaByFiltersAbogado>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }
                var result = await _repositoryAbogado.GetBandejaPendientesAsyncRepositoryAbogado(
                    Fetch,
                    Page,
                    OrderByColumn,
                    OrderDesc,
                    noAsunto,
                    fechaPresentacionDesde,
                    fechaPresentacionHasta,
                    fechaVencimientoDesde,
                    fechaVencimientoHasta,
                    rfcPromovente,
                    promovente,
                    tipoAsuntoList,
                    estadoTareaList,
                    tipoModalidadList,
                    estadoProcesalList,
                    userInformationView.IdAdministracionCentral,
                    userInformationView.IdAdministracion,
                    userInformationView.IdSubadministracion,
                    userInformationView.Rfc
                );

                var bandejaList = result.ToList();
                if (bandejaList.Any())
                {
                    _redisClient.ValidateTakeList(ref bandejaList, EnumModulosRedis.CONSULTAS);
                }

                if (result is null)
                {
                    return ResultOperation<List<ResponseConsultaByFiltersAbogado>>.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableView<ResponseConsultaByFiltersAbogado>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public async Task<ResultOperation> GetByIdAbogadoService(int id)
        {
            try
            {
                var result = await _repositoryAbogado.GetByIdRepositoryAbogado(id);
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

        public async Task<ResultOperation> AddRequerimientoService(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                if (entity.fecha_requerimiento.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de requerimiento no puede ser menor a la fecha de presentación en el SAT.");
                }



                var result = await _repositoryAbogado.AddAsyncRequerimiento(entity, entityDocumento, dataFile);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResultOperation> SolicitaRequerimientoService(Consulta entity)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }


                var result = await _repositoryAbogado.SolicitaAsyncRequerimiento(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> GetRequerimientoById(int id)
        {
            try
            {
                var result = await _repositoryAbogado.GetByIdAsyncRepositoryRequerimientos(id);
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

        public async Task<ResultOperation> GetTablaRequerimientosService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();


                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(idConsulta);
                if (consultaDetails.solicita_requerimiento == false)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                       null!)
                   );
                }


                var countResult = await _repositoryAbogado.GetTablaRequerimientosCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositoryAbogado.GetTablaAlertaRequerimientosCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }
                var result = await _repositoryAbogado.GetTablaRequerimientosAsyncRepository(idConsulta);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                    null!)
                );
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                       result)
                   );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Requerimientos> GetByAllServiceRequerimientos(int id) =>
        await _repositoryAbogado.GetByIdAllAsyncRepositoryRequerimientos(id);

        public async Task<ResultOperation> UpdateRequerimientos(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                if (entity.fecha_requerimiento.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de requerimiento no puede ser menor a la fecha de presentación en el SAT.");
                }

                var result = await _repositoryAbogado.UpdateAsyncRequerimientos(entity, entityDocumento, dataFile);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> AddResolucionService(Resolucion entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                int validacion = await _repositoryAbogado.GetValidacionResolucion(entity.id_consulta);

                if (validacion == 0)
                {
                    if (entity.fecha_resolucion.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                    {
                        return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de resolucion no puede ser menor a la fecha de presentación en el SAT.");
                    }

                    var result = await _repositoryAbogado.AddAsyncResolucion(entity, entityDocumento, dataFile);
                    if (!result.Success)
                        return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                    return ResultOperation.SuccessResponseNoMessage(result.Result);
                }
                else if (validacion == 1)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se encontro documentacion obligatoria adjunta");
                }
                else if (validacion == 2)
                {
                    return ResultOperation<int?>.FailureErrorResponse("Los Requerimientos para esta consulta no estan concluidos");
                }
                else
                {
                    return ResultOperation<int?>.FailureErrorResponse("Ah ocurrido un error inesperado al guardar el registro");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Resolucion> GetByAllServiceResolucion(int idConsulta) =>
        await _repositoryAbogado.GetByIdAllAsyncRepositoryResolucionT(idConsulta);

        public async Task<ResultOperation> GetResolucionById(int id)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                //var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(id);

                var result = await _repositoryAbogado.GetByIdAllAsyncRepositoryResolucion(id);
                if (result is not null && result.Any())
                {
                    var resolucion = result.First();
                    if (resolucion.concluido == true)
                    {
                        return ResultOperation.SuccessResponseNoMessage(
                            new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                            result)
                        );
                    }
                    else
                    {
                        idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                        alerta = EnumAlertaSeccion.EN_PROCESO.ToString();

                    }



                    if (result is null)
                    {
                        idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                        alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                        return ResultOperation.SuccessResponseNoMessage(
                            new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                            null!)
                        );
                    }
                }
                else
                {
                    return ResultOperation.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                    result)
                );

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> UpdateResolucion(Resolucion entity)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                if (entity.fecha_resolucion.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de resolucion no puede ser menor a la fecha de presentación en el SAT.");
                }

                if (entity.fecha_notificacion.Date < entity.fecha_resolucion.Date || entity.fecha_notificacion.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de notificación del oficio del requerimiento no puede ser menor a la fecha del oficio del requerimiento.");
                }

                var result = await _repositoryAbogado.UpdateAsyncResolucion(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<ResultOperation> ConcluirResolucion(Resolucion entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }


                var result = await _repositoryAbogado.ConcluirAsyncResolucion(entity, entityDocumento, dataFile);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<ResultOperation> AddRequerimientoProdeconService(RequerimientosProdecon entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                if (entity.fecha_oficio.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de requerimiento prodecon no puede ser menor a la fecha de presentación en el SAT.");
                }

                if (entity.fecha_ingreso.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de atención no puede ser menor a la fecha de presentación en el SAT.");
                }

                var result = await _repositoryAbogado.AddAsyncRequerimientoProdecon(entity, entityDocumento, dataFile);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RequerimientosProdecon> GetByAllServiceRequerimientosProdecon(int id) =>
        await _repositoryAbogado.GetByIdAllAsyncRepositoryRequerimientosProdecon(id);

        public async Task<ResultOperation> UpdateRequerimientosProdecon(RequerimientosProdecon entity)
        {
            try
            {
                var result = await _repositoryAbogado.UpdateAsyncRequerimientosProdecon(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> GetRequerimientoProdeconById(int id)
        {
            try
            {
                var result = await _repositoryAbogado.GetByIdAllAsyncRepositoryRequerimientosProdecon(id);
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

        public async Task<ResultOperation> GetTablaRequerimientosProdeconService(int idConsulta)
        {

            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var countResult = await _repositoryAbogado.GetTablaRequerimientosProdeconCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseRequerimientoProdeconList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var result = await _repositoryAbogado.GetTablaRequerimientosProdeconAsyncRepository(idConsulta);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseRequerimientoProdeconList>(new(idAlerta, alerta),
                    null!)
                );
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseRequerimientoProdeconList>(new(idAlerta, alerta),
                       result)
                   );
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> AddSolicitudInformacionService(SolicitudInformacion entity)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                var consultaDetails = await _repositoryAbogado.GetByIdAsyncRepositoryAbogado(entity.id_consulta);

                if (entity.fecha_oficio_solicitud.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de oficio de solicitud no puede ser mayor a la fecha actual.");
                }

                if (entity.atendio_solicitud)
                {


                    if (entity.fecha_oficio_solicitud.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                    {
                        return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de oficio de solicitud debe  ser mayor  o igual a la fecha de presentación en el SAT.");
                    }

                    if (entity.fecha_oficio_respuesta.Date < Convert.ToDateTime(entity.fecha_oficio_solicitud.Date))
                    {
                        return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de oficio de respuesta  debe  ser mayor  a la fecha de oficio de solicitud.");
                    }

                    if (entity.fecha_oficio_respuesta.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                    {
                        return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de oficio de respuesta debe  ser mayor  o igual a la fecha de presentación en el SAT.");
                    }

                    if (entity.fecha_recepcion.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                    {
                        return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de oficio de recepcion debe  ser mayor  a la fecha de presentación en el SAT.");
                    }

                    if (entity.fecha_recepcion.Date < Convert.ToDateTime(entity.fecha_oficio_respuesta))
                    {
                        return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de oficio de recepcion debe  ser mayor o igual  a la fecha de respuesta en el SAT.");
                    }

                }




                var result = await _repositoryAbogado.AddAsyncRepositorySolicitudInformacion(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> GetTablaSolicitudInformacionService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var countResult = await _repositoryAbogado.GetTablaSolicitudInformacionCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseSolicitudInformacionList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositoryAbogado.GetTablaAlertaSolicitudInformacionCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }
                var result = await _repositoryAbogado.GetTablaSolicitudInformacionAsyncRepository(idConsulta);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseSolicitudInformacionList>(new(idAlerta, alerta),
                    null!)
                );
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseSolicitudInformacionList>(new(idAlerta, alerta),
                       result)
                   );
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<ResultOperation> GetSolicitudInformacionById(int id)
        {
            try
            {
                var result = await _repositoryAbogado.GetByIdAsyncRepositorySolicitudInformacion(id);
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

        public async Task<SolicitudInformacion> GetByAllServiceSolicitudInformacion(int id) =>
        await _repositoryAbogado.GetByIdAllAsyncRepositorySolicitudInformacion(id);

        public async Task<ResultOperation> UpdateSolicitudInformacion(SolicitudInformacion entity)
        {
            try
            {
                var result = await _repositoryAbogado.UpdateAsyncSolicitudInformacion(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation<ResponseReasignar>> ReAsignarComercioExteriorAsync(int[] idList, string rfcAbogado, UserInformationView userInformationView)
        {
            var responseAbogado = await _apiService.GetResultOperationAsync<UserInformationView>($"{_routeInfoUsuario}/{rfcAbogado}");
            if (responseAbogado is null || !responseAbogado.Success || responseAbogado.Result is null)
            {
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"No se pudo realizar la validación para verificar que el abogado seleccionado pertenezca a la administracion/subadministración.");
            }

            List<Consulta> entityList = await _repositoryAbogado.GetListByIdsAsync(idList);
            if (entityList is null || !entityList.Any())
            {
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"No existen las autorizaciones.");
            }

            var resultOperation = ResultOperation.SuccessResponseNoMessage(new ResponseReasignar());

            List<int> listInvalidos = new List<int>();
            List<Reasignar> listReasignacion = new();
            foreach (var item in idList)
            {
                var entity = entityList.FirstOrDefault(c => c.id == item && c.activo);
                if (entity is null)
                {
                    resultOperation.AddWarningMessage($"La autorización con el identificador {item} no existe o se eliminó.");
                    listInvalidos.Add(item);
                    continue;
                }

                try
                {
                    Reasignar entityReasignar = EventsConsultasAbogado.UpdateReasignar(ref entity,
                        userInformationView.Rfc,
                        responseAbogado.Result.Rfc,
                        entity.id_abogado,
                        userInformationView.IdAdministracion,
                        userInformationView.IdSubadministracion,
                        responseAbogado.Result.IdAdministracion,
                        responseAbogado.Result.IdSubadministracion);

                    if (!UserSession.ValidateUser(responseAbogado!.Result!, EnumRolesSicoj.ABOGADO, EnumModulosSicoj.CONSULTAS, out string message, false, null!, true, entity.id_administracion_central, true, entity.id_administracion, true, entityReasignar.id_subadministracion_reasignado))
                    {
                        resultOperation.AddWarningMessage($"La consulta de Comercio Exterior con el número de asunto {entity.no_asunto}: {message}");
                    }

                    listReasignacion.Add(entityReasignar);
                }
                catch (Exception _e)
                {
                    resultOperation.AddWarningMessage($"La consulta de Comercio Exterior con el número de asunto {entity.no_asunto}: {_e.Message}");
                    listInvalidos.Add(item);
                    continue;
                }
            }

            resultOperation.Result.ReasignacionesExitosas = listReasignacion.Count;
            resultOperation.Result.ReasignacionesIncorrectas = listInvalidos.Count;
            resultOperation.Result.abogado = responseAbogado.Result.Nombre!;

            if (!listReasignacion.Any())
            {
                return resultOperation;
            }

            var result = await _repositoryAbogado.ReasignarAsync(listReasignacion);
            if (!result.Success)
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"{result.MsgError!}:{result.DetailError}");

            return resultOperation;
        }

        public async Task<ResultOperation<ResponseReasignar>> ReAsignarImpuestosInternosAsync(int[] idList, string rfcAbogado, UserInformationView userInformationView)
        {
            var responseAbogado = await _apiService.GetResultOperationAsync<UserInformationView>($"{_routeInfoUsuario}/{rfcAbogado}");
            if (responseAbogado is null || !responseAbogado.Success || responseAbogado.Result is null)
            {
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"No se pudo realizar la validación para verificar que el abogado seleccionado pertenezca a la administracion/subadministración.");
            }

            List<Consulta> entityList = await _repositoryAbogado.GetListByIdsAsync(idList);
            if (entityList is null || !entityList.Any())
            {
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"No existen las autorizaciones.");
            }

            var resultOperation = ResultOperation.SuccessResponseNoMessage(new ResponseReasignar());

            List<int> listInvalidos = new List<int>();
            List<Reasignar> listReasignacion = new();
            foreach (var item in idList)
            {
                var entity = entityList.FirstOrDefault(c => c.id == item && c.activo);
                if (entity is null)
                {
                    resultOperation.AddWarningMessage($"La autorización con el identificador {item} no existe o se eliminó.");
                    listInvalidos.Add(item);
                    continue;
                }

                try
                {
                    Reasignar entityReasignar = EventsConsultasAbogado.UpdateReasignar(ref entity,
                        userInformationView.Rfc,
                        responseAbogado.Result.Rfc,
                        entity.id_abogado,
                        userInformationView.IdAdministracion,
                        userInformationView.IdSubadministracion,
                        responseAbogado.Result.IdAdministracion,
                        responseAbogado.Result.IdSubadministracion);

                    if (!UserSession.ValidateUser(responseAbogado!.Result!, EnumRolesSicoj.ABOGADO, EnumModulosSicoj.CONSULTAS, out string message, false, null!, true, entity.id_administracion_central, true, entity.id_administracion, true, entityReasignar.id_subadministracion_reasignado))
                    {
                        resultOperation.AddWarningMessage($"La consulta de Impuestos Internos con el número de asunto {entity.no_asunto}: {message}");
                    }

                    listReasignacion.Add(entityReasignar);
                }
                catch (Exception _e)
                {
                    resultOperation.AddWarningMessage($"La consulta de Impuestos Internos con el número de asunto {entity.no_asunto}: {_e.Message}");
                    listInvalidos.Add(item);
                    continue;
                }
            }

            resultOperation.Result.ReasignacionesExitosas = listReasignacion.Count;
            resultOperation.Result.ReasignacionesIncorrectas = listInvalidos.Count;
            resultOperation.Result.abogado = responseAbogado.Result.Nombre!;

            if (!listReasignacion.Any())
            {
                return resultOperation;
            }

            var result = await _repositoryAbogado.ReasignarAsync(listReasignacion);
            if (!result.Success)
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"{result.MsgError!}:{result.DetailError}");

            return resultOperation;
        }


        //Avisos Y Comunicados
        public async Task<ResultOperation> AddAvisosYComunicadosService(AvisosComunicados entity)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }


                if (entity.fecha_ingreso.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de atención no puede ser menor a la fecha de presentación en el SAT.");
                }

                var result = await _repositoryAbogado.AddAsyncAvisosYComunicadosRepository(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<AvisosComunicados> GetByServiceAvisosYComunicados(int id) =>
        await _repositoryAbogado.GetByIdAsyncRepositoryAvisosComunicados(id);

        public async Task<ResultOperation> UpdateAvisosYComunicados(AvisosComunicados entity)
        {
            try
            {
                var result = await _repositoryAbogado.UpdateAsyncAvisosYComunicadosRepository(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> GetTablaAvisosYComunicados(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var countResult = await _repositoryAbogado.GetTablaAvisosYComunicadosCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseAvisosComunicadosList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositoryAbogado.GetAlertaAvisosYComunicadosCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }

                var result = await _repositoryAbogado.GetTablaAvisosYComunicadosAsyncRepository(idConsulta);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseAvisosComunicadosList>(new(idAlerta, alerta),
                    null!)
                );
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseAvisosComunicadosList>(new(idAlerta, alerta),
                       result)
                   );
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
         UserInformationView userInformationView,
       int? idDocumentoSeccion
      )
        {
            try
            {
                var countResult = await _repositoryAbogado.GetArchivosByFiltersCountAsyncRepository(
                        idRol,
                        folio,
                        idSeccion,
                        idConsulta,
                        idRemision,
                        estatus,
                        fechaCreacionDesde,
                        fechaCreacionHasta,
                        remplazable,
                        permanente,
                        idDocumentoSeccion
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseArchivosConsulta>>.SuccessResponseNoMessage("No hay elementos que mostrar");//.FailureWarningResponse("No se encontraron resultados"); ;
                }

                var result = await _repositoryAbogado.GetArchivoByFiltersAsyncRepository(
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
                    permanente,
                    idDocumentoSeccion
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
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                DateTime dateTime = DateTime.Now;
                if (entity.fechaSolicitud.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de solicitud no debe ser mayor a la fecha actual.");
                }


                var result = await _repositoryAbogado.AddAsyncSolicitudTransparenciaService(entity, entityDocumento, dataFile);
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
        await _repositoryAbogado.GetByIdSolicitudTransparenciaRepository(id);

        public async Task<ResultOperation> UpdateSolicitudTransparenciaService(SolicitudTransparencia entity)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                DateTime dateTime = DateTime.Now;
                if (entity.fechaSolicitud.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de solicitud no debe ser mayor a la fecha actual.");
                }

                var result = await _repositoryAbogado.UpdateSolicitudTransparenciaRepository(entity);
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
                var result = await _repositoryAbogado.GetByIdSolicitudTransparenciaRepositorys(id);
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


                var countResult = await _repositoryAbogado.GetTablaSolicitudTransparenciaCountRepository(idConsulta);

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
                var result = await _repositoryAbogado.GetTablaSolicitudTransparenciaRepository(idConsulta);

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

                var result = await _repositoryAbogado.DeleteSolicitudTransparenciaRepository(entity.id);
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

        public async Task<ResultOperation> AddResolucionCumplimentacionService(ResolucionCumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_cumplimentacion);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                int validacion = await _repositoryAbogado.GetValidacionResolucion(entity.id_cumplimentacion);

                if (validacion == 0)
                {
                    if (entity.fecha_resolucion.Date < Convert.ToDateTime(consultaDetails.fecha_firmeza))
                    {
                        return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de resolucion no puede ser menor a la fecha de firmeza");
                    }

                    var result = await _repositoryAbogado.AddAsyncResolucionCumplimentacion(entity, entityDocumento, dataFile);
                    if (!result.Success)
                        return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                    return ResultOperation.SuccessResponseNoMessage(result.Result);
                }
                else if (validacion == 1)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se encontro documentacion obligatoria adjunta");
                }
                else if (validacion == 2)
                {
                    return ResultOperation<int?>.FailureErrorResponse("Los Requerimientos para esta consulta no estan concluidos");
                }
                else
                {
                    return ResultOperation<int?>.FailureErrorResponse("Ah ocurrido un error inesperado al guardar el registro");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResolucionCumplimentacion> GetByAllServiceResolucionCumplimentacion(int id_cumplimentacion) =>
        await _repositoryAbogado.GetByIdAllAsyncRepositoryResolucionCumplimentacionT(id_cumplimentacion);

        public async Task<ResultOperation> UpdateResolucionCumplimentacion(ResolucionCumplimentacion entity)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_cumplimentacion);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                if (entity.fecha_resolucion.Date < Convert.ToDateTime(consultaDetails.fecha_firmeza))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de resolucion no puede ser menor a la fecha de firmeza");
                }

                if (entity.fecha_notificacion.Date < entity.fecha_resolucion.Date || entity.fecha_notificacion.Date < Convert.ToDateTime(consultaDetails.fecha_firmeza))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de notificación del oficio del requerimiento no puede ser menor a la fecha del oficio del requerimiento.");
                }

                var result = await _repositoryAbogado.UpdateAsyncResolucionCumplimentacion(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> ConcluirResolucionCumplimentacion(ResolucionCumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(entity.id_cumplimentacion);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }


                var result = await _repositoryAbogado.ConcluirAsyncResolucionCumplimentacion(entity, entityDocumento, dataFile);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }


        public async Task<ResultOperation> GetTablaResolucionService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var consultaDetails = await _repositoryAbogado.GetByIdDisconnected(idConsulta);

                var result = await _repositoryAbogado.GetTablaResolucionAsyncRepository(idConsulta);
                if (consultaDetails is not null)
                {
                    if (consultaDetails.concluido == true)
                    {
                        return ResultOperation.SuccessResponseNoMessage(
                            new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                            result)
                        );
                    }
                    else
                    {
                        idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                        alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                    }

                    if (result is null)
                    {
                        idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                        alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                        return ResultOperation.SuccessResponseNoMessage(
                            new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                            null!)
                        );
                    }
                }
                else
                {
                    return ResultOperation.SuccessResponseNoMessage("No se encontraron resultados");
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                       result)
                   );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Cumplimentacion> GetByAllServiceCumplimentacion(int id) =>
        await _repositoryAbogado.GetByIdAllAsyncRepositoryCumplimentacion(id);

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

                var result = await _repositoryAbogado.UpdateCumplimentacionRepository(entity);
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
    
      public async Task<ResultOperation> GetTablaMediosDefensaGeneral(List<string> noAsunto)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();



                var request = new RequestMediosDefensa
                {
                    idModulo = EnumModulosSicoj.CONSULTAS.GetHashCode(),
                    dato = noAsunto
                };

                var wrapper = await _apiService.PostAsync<WrapperProxyMediosDefensa>(
                    _proxyEnpoints.RouteMediosDefensa,
                    request
                );

                var result = wrapper?.Result?.mediosDefensa;

                if (result is not null)
                {

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseMediosDefensa>(new(idAlerta, alerta),
                        result)
                    );
                }

                if (result is null)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseMediosDefensa>(new(idAlerta, alerta),
                        null!)
                    );
                }


                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseMediosDefensa>(new(idAlerta, alerta),
                       result)
                   );
            }
            catch (Exception)
            {
                throw;
            }
        }
    
    }
}