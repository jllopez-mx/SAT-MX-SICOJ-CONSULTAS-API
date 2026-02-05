using Sicoj.Utils.Models;
using Sicoj.Utils.ViewModels;
using ConsultasAPI.Model.Entities;
using ConsultasAPI.Model.IDAO.IServiceDAO;
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.IDAO.IRepository;
using Sicoj.Utils.Extentions;
using ConsultasAPI.Model.DTO.CatalogsContracts;
using Sicoj.Utils.Enums;
using ConsultasAPI.Model.DTO.Response.Administrador;
using Sicoj.Utils.Middleware;
using ConsultasAPI.Model.Entities.Events.Administrador;
using ConsultasAPI.Model.ViewModels.Enums;
using Sicoj.Utils.Redis;
using Microsoft.Extensions.Options;
using ClosedXML.Excel;
using System.Data;


namespace ConsultasAPI.Model.DAO.ServicesDAO
{
    public class ConsultasAdministradorService : IConsultasAdministradorService
    {
        private readonly IConsultaRepositoryAdministrador _repositoryAdministrador;
        private readonly IApiService _apiService;
        private readonly IRedisClient _redisClient;
        private readonly string _routeInfoUsuario = null!;
        private readonly CatalogosEnpoints _catologosEnpoints;
        private readonly ProxyEnpoints _proxyEnpoints;


        public ConsultasAdministradorService(IConfiguration configuration, IConsultaRepositoryAdministrador repositoryAdministrador,
        IApiService apiService,
        IRedisClient redisClient,
        IOptions<CatalogosEnpoints> catologosEnpoints,
        IOptions<ProxyEnpoints> proxyEnpoints)
        {
            _repositoryAdministrador = repositoryAdministrador ?? throw new ArgumentNullException(nameof(repositoryAdministrador));
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _routeInfoUsuario = configuration.GetValue<string>("CatalogsEndpoints:RouteInfoUsuario")!;
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(redisClient));
            _catologosEnpoints = catologosEnpoints.Value ?? throw new ArgumentNullException(nameof(catologosEnpoints));
            _proxyEnpoints = proxyEnpoints.Value ?? throw new ArgumentNullException(nameof(proxyEnpoints));
        }

        public async Task<ResultOperation> GetBandejaPendientes_AdministradorService(int Fetch, int Page, string? OrderByColumn, bool OrderDesc, UserInformationView userInformationView)
        {
            try
            {
                var countResult = await _repositoryAdministrador.GetAllByFiltersCountAsyncRepositoryAdministrador(
                        null,
                        userInformationView.IdAdministracionCentral,
                        userInformationView.IdAdministracion
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseConsultaByFilters>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }
                var result = await _repositoryAdministrador.GetBandejaPendientesAsyncRepositoryAdministrador(
                    Fetch,
                    Page,
                    OrderByColumn,
                    OrderDesc,
                    userInformationView.IdAdministracionCentral,
                    userInformationView.IdAdministracion
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

        public async Task<Consulta> GetByIdServiceAdministrador(int id) =>
       await _repositoryAdministrador.GetByIdAsyncRepositoryAdministrador(id);

        public async Task<ResultOperation> UpdateConsultaServiceAdministradorComercioExterior(Consulta entity)
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


                var result = await _repositoryAdministrador.UpdateAsyncRepositoryAdministradorComercioExterior(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<ResultOperation> UpdateConsultaServiceAdministradorImpuestosInternos(Consulta entity)
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


                var result = await _repositoryAdministrador.UpdateAsyncRepositoryAdministradorImpuestosInternos(entity);
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
                var result = await _repositoryAdministrador.AddPersonasAutorizadas(entity);
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
        await _repositoryAdministrador.GetByIdAllAsyncRepositoryPersonasAutorizadas(id);

        public async Task<ResultOperation> UpdatePersonasAutorizadas(PersonasAutorizadas entity)
        {
            try
            {
                var result = await _repositoryAdministrador.UpdateAsyncPersonasAutorizadas(entity);
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

                var result = await _repositoryAdministrador.DeleteAsyncPersonasAutorizadas(entity.id);
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
                var countResult = await _repositoryAdministrador.GetTablaPersonasAutorizadasCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseTablaPersonasAutorizadas>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }
                var result = await _repositoryAdministrador.GetTablaPersonasAutorizadasAsyncRepository(
                    Fetch,
                    Page,
                    idConsulta
                );

                if (result is null)
                {
                    return ResultOperation<List<ResponseTablaPersonasAutorizadas>>.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableView<ResponseTablaPersonasAutorizadas>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> GetByIdAdministradorService(int id)
        {
            try
            {
                var result = await _repositoryAdministrador.GetByIdRepositoryAdministrador(id);
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

        public async Task<ResultOperation> AddRemisionAdministradorService(Remision entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {

                var result2 = await _repositoryAdministrador.GetByIdDisconnected(entity.id_consulta);

                if (result2.idAdministracion == entity.id_administracion_remite)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se puede remitir a la misma administracion en la que se encuentra el registro.");
                }

                DateTime dateTime = DateTime.Now;

                if (entity.fecha_oficio.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de oficio no puede ser mayor a la fecha actual.");
                }

                var result = await _repositoryAdministrador.AddRemisionAdministradorRepository(entity, entityDocumento, dataFile);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> GetTablaRemisionAdministradorService(int Fetch, int Page)
        {
            try
            {
                var countResult = await _repositoryAdministrador.GetTablaRemisionAdministradorCountAsyncRepository();

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseTablaRemision>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }
                var result = await _repositoryAdministrador.GetTablaRemisionAdministradorAsyncRepository(
                    Fetch,
                    Page
                );

                if (result is null)
                {
                    return ResultOperation<List<ResponseTablaRemision>>.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
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
        await _repositoryAdministrador.GetByIdAllAsyncRepository(id);

        public async Task<ResultOperation<int>> AddArchivosAsyncService(ArchivoConsulta entity, DataFile dataFile)
        {

            try
            {
                var result = await _repositoryAdministrador.AddFileAsyncRepository(entity, dataFile);
                if (!result.Success)
                    return ResultOperation.FailureErrorResponse<int>($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result.GetValueOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ArchivoConsulta> GetIdArchivoConsultaService(int id) =>
        await _repositoryAdministrador.GetIdArchivoConsultaAsyncRepository(id);

        public async Task<ResultOperation<int>> UpdateArchivoService(ArchivoConsulta entity, DataFile dataFile)
        {
            var result = await _repositoryAdministrador.UpdateDocumentoAsync(entity, dataFile);
            if (!result.Success)
                return ResultOperation.FailureErrorResponse<int>($"{result.MsgError!}:{result.DetailError}");

            return ResultOperation.SuccessResponseNoMessage(result.Result.GetValueOrDefault());
        }

        public async Task<ResponseArchivosConsulta> GetByIdArchivoDeleteService(int id) =>
        await _repositoryAdministrador.GetByIdArchivoAsyncRepository(id);

        public async Task<ResultOperation> DeleteArchivoConsultaService(ResponseArchivosConsulta entity)
        {
            try
            {

                var result = await _repositoryAdministrador.DeleteByIdArchivoAsyncRepository(entity.id);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);//, "El documento se ha eliminado exitosamente.");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<ResponseArchivosConsulta>> GetAllArchivoService(int id_registro) =>
        await _repositoryAdministrador.GetArchivosByIdRegistroAsync_Repository(id_registro);

        public async Task<ResponseArchivosConsulta> GetByIdArchivoService(int id) =>
        await _repositoryAdministrador.GetByIdArchivoAsyncRepository(id);

        public async Task<ResultOperation<ResponseAsignar>> AsignarConsultaService(Consulta entity)
        {
            try
            {
                var result = await _repositoryAdministrador.AsignarConsultaRepository(entity);
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
                var result = await _repositoryAdministrador.GetByIdDisconnected(id);
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

                if (result is not null && !string.IsNullOrEmpty(result.idAbogado) && !result.remitido)
                {

                    var response = await _apiService.GetResultOperationAsync<UserInformationView>($"{_routeInfoUsuario}/{resultOperation.Result.idAbogado}");
                    if (response is not null && response.Success && response.Result is not null)
                    {
                        resultOperation.Result.idAbogado = response.Result.Rfc;
                        resultOperation.Result.abogado = response.Result.Nombre;
                    }
                    else
                        resultOperation.AddWarningMessage("No se pudo recuperar el nombre del abogado.");
                }

                return resultOperation;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region  Historico

        public async Task<ResultOperation> GetBandejaHistoricoByFiltersService
        (
            int Fetch,
            int Page,
            string?
            OrderByColumn,
            bool OrderDesc,
            string? noAsunto,
            DateTime? fechaPresentacionDesde,
            DateTime? fechaPresentacionHasta,
            DateTime? fechaVencimientoDesde,
            DateTime? fechaVencimientoHasta,
            string? rfcPromovente,
            string? promovente,
             List<int>? TipoAsunto,
            int? idAdministracion,
            int? idSubadministracion,
            string? idAbogadoAsigno,
            List<int>? EstadoTarea,
            List<int>? TipoModalidad,
            List<int>? EstadoProcesal,
            UserInformationView userInformationView,
            bool reasignarAsuntos
        )
        {
            try
            {
                var countResult = await _repositoryAdministrador.GetHistoricoCountAsyncRepository(
                        noAsunto,
                        fechaPresentacionDesde,
                        fechaPresentacionHasta,
                        fechaVencimientoDesde,
                        fechaVencimientoHasta,
                        rfcPromovente,
                        promovente,
                        TipoAsunto,
                        idAdministracion,
                        idSubadministracion,
                        idAbogadoAsigno,
                        EstadoTarea,
                        TipoModalidad,
                        EstadoProcesal,
                        reasignarAsuntos,
                        userInformationView.IdAdministracionCentral,
                        userInformationView.IdAdministracion
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseConsultaByFiltersAdministrador>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }

                var result = await _repositoryAdministrador.GetHistoricoAsyncRepository(
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
                    TipoAsunto,
                    idAdministracion,
                    idSubadministracion,
                    idAbogadoAsigno,
                    EstadoTarea,
                    TipoModalidad,
                    EstadoProcesal,
                    reasignarAsuntos,
                    userInformationView.IdAdministracionCentral,
                    userInformationView.IdAdministracion
                );

                if (result is null)
                {
                    return ResultOperation<List<ResponseConsultaByFiltersAdministrador>>.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableView<ResponseConsultaByFiltersAdministrador>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public async Task<ResultOperation> AddRequerimientoService(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                if (entity.fecha_requerimiento.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de requerimiento no puede ser menor a la fecha de presentación en el SAT.");
                }


                var result = await _repositoryAdministrador.AddAsyncRequerimiento(entity, entityDocumento, dataFile);
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
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }


                var result = await _repositoryAdministrador.SolicitaAsyncRequerimiento(entity);
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
                var result = await _repositoryAdministrador.GetByIdAsyncRepositoryRequerimientos(id);
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

                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(idConsulta);
                if (consultaDetails.solicita_requerimiento == false)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                       null!)
                   );
                }


                var countResult = await _repositoryAdministrador.GetTablaRequerimientosCountAsyncRepository(idConsulta);



                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositoryAdministrador.GetTablaAlertaRequerimientosCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }
                var result = await _repositoryAdministrador.GetTablaRequerimientosAsyncRepository(idConsulta);
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
        await _repositoryAdministrador.GetByIdAllAsyncRepositoryRequerimientos(id);

        public async Task<ResultOperation> UpdateRequerimientos(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                if (entity.fecha_requerimiento.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de requerimiento no puede ser menor a la fecha de presentación en el SAT.");
                }

                var result = await _repositoryAdministrador.UpdateAsyncRequerimientos(entity, entityDocumento, dataFile);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }
        // Solicitud Información
        public async Task<ResultOperation> AddSolicitudInformacionService(SolicitudInformacion entity)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                var consultaDetails = await _repositoryAdministrador.GetByIdAsyncRepositoryAdministrador(entity.id_consulta);

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



                var result = await _repositoryAdministrador.AddAsyncRepositorySolicitudInformacion(entity);
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

                var countResult = await _repositoryAdministrador.GetTablaSolicitudInformacionCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseSolicitudInformacionList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositoryAdministrador.GetTablaAlertaSolicitudInformacionCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }
                var result = await _repositoryAdministrador.GetTablaSolicitudInformacionAsyncRepository(idConsulta);
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
                var result = await _repositoryAdministrador.GetByIdAsyncRepositorySolicitudInformacion(id);
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
        await _repositoryAdministrador.GetByIdAllAsyncRepositorySolicitudInformacion(id);

        public async Task<ResultOperation> UpdateSolicitudInformacion(SolicitudInformacion entity)
        {
            try
            {
                var result = await _repositoryAdministrador.UpdateAsyncSolicitudInformacion(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<ResultOperation> AddResolucionService(Resolucion entity, ArchivoConsulta entityDocumento, DataFile dataFile) //Aqui
        {
            try
            {
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                int validacion = await _repositoryAdministrador.GetValidacionResolucion(entity.id_consulta);

                if (validacion == 0)
                {
                    if (entity.fecha_resolucion.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                    {
                        return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de resolucion no puede ser menor a la fecha de presentación en el SAT.");
                    }

                    var result = await _repositoryAdministrador.AddAsyncResolucion(entity, entityDocumento, dataFile);
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
        await _repositoryAdministrador.GetByIdAllAsyncRepositoryResolucionT(idConsulta);

        public async Task<ResultOperation> GetResolucionById(int id)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                //var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(idConsulta);

                var result = await _repositoryAdministrador.GetByIdAllAsyncRepositoryResolucion(id);
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

        public async Task<ResultOperation> UpdateResolucion(Resolucion entity)//XXX
        {
            try
            {
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_consulta);
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

                var result = await _repositoryAdministrador.UpdateAsyncResolucion(entity);
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
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }


                var result = await _repositoryAdministrador.ConcluirAsyncResolucion(entity, entityDocumento, dataFile);
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
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_consulta);
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

                var result = await _repositoryAdministrador.AddAsyncRequerimientoProdecon(entity, entityDocumento, dataFile);
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
        await _repositoryAdministrador.GetByIdAllAsyncRepositoryRequerimientosProdecon(id);

        public async Task<ResultOperation> UpdateRequerimientosProdecon(RequerimientosProdecon entity)
        {
            try
            {
                var result = await _repositoryAdministrador.UpdateAsyncRequerimientosProdecon(entity);
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
                var result = await _repositoryAdministrador.GetByIdAllAsyncRepositoryRequerimientosProdecon(id);
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

                var countResult = await _repositoryAdministrador.GetTablaRequerimientosProdeconCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseRequerimientoProdeconList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var result = await _repositoryAdministrador.GetTablaRequerimientosProdeconAsyncRepository(idConsulta);
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

        public async Task<ResultOperation<ResponseReasignar>> ReAsignarComercioExteriorAsync(int[] idList, string rfcAbogado, UserInformationView userInformationView)
        {
            var responseAbogado = await _apiService.GetResultOperationAsync<UserInformationView>($"{_routeInfoUsuario}/{rfcAbogado}");
            if (responseAbogado is null || !responseAbogado.Success || responseAbogado.Result is null)
            {
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"No se pudo realizar la validación para verificar que el abogado seleccionado pertenezca a la administracion/subadministración.");
            }

            List<Consulta> entityList = await _repositoryAdministrador.GetListByIdsAsync(idList);
            if (entityList is null || !entityList.Any())
            {
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"No existen las consultas.");
            }

            var resultOperation = ResultOperation.SuccessResponseNoMessage(new ResponseReasignar());

            List<int> listInvalidos = new List<int>();
            List<Reasignar> listReasignacion = new();
            foreach (var item in idList)
            {
                var entity = entityList.FirstOrDefault(c => c.id == item && c.activo);
                if (entity is null)
                {
                    resultOperation.AddWarningMessage($"La consulta con el identificador {item} no existe o se eliminó.");
                    listInvalidos.Add(item);
                    continue;
                }

                var entityabogado = entityList.FirstOrDefault(c => c.id == item && c.id_abogado == rfcAbogado);
                if (entityabogado is not null)
                {
                    resultOperation.AddWarningMessage($"La consulta con el identificador {item} ya tiene asignado este abogado.");
                    listInvalidos.Add(item);
                    continue;
                }

                try
                {
                    Reasignar entityReasignar = EventsConsultasAdministrador.UpdateReasignar(ref entity,
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

            var result = await _repositoryAdministrador.ReasignarAsync(listReasignacion);
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

            List<Consulta> entityList = await _repositoryAdministrador.GetListByIdsAsync(idList);
            if (entityList is null || !entityList.Any())
            {
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"No existen las consultas.");
            }

            var resultOperation = ResultOperation.SuccessResponseNoMessage(new ResponseReasignar());

            List<int> listInvalidos = new List<int>();
            List<Reasignar> listReasignacion = new();
            foreach (var item in idList)
            {
                var entity = entityList.FirstOrDefault(c => c.id == item && c.activo);
                if (entity is null)
                {
                    resultOperation.AddWarningMessage($"La consulta con el identificador {item} no existe o se eliminó.");
                    listInvalidos.Add(item);
                    continue;
                }

                try
                {

                    Reasignar entityReasignar = EventsConsultasAdministrador.UpdateReasignar(ref entity,
                        userInformationView.Rfc,
                        responseAbogado.Result.Rfc,
                        entity.id_abogado,
                        userInformationView.IdAdministracion,
                        userInformationView.IdSubadministracion,
                        responseAbogado.Result.IdAdministracion,
                        responseAbogado.Result.IdSubadministracion
                        );

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

            var result = await _repositoryAdministrador.ReasignarAsync(listReasignacion);
            if (!result.Success)
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"{result.MsgError!}:{result.DetailError}");

            return resultOperation;
        }

        //Avisos Y Comunicados
        public async Task<ResultOperation> AddAvisosYComunicadosService(AvisosComunicados entity)
        {
            try
            {
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }


                if (entity.fecha_ingreso.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de atención no puede ser menor a la fecha de presentación en el SAT.");
                }

                var result = await _repositoryAdministrador.AddAsyncAvisosYComunicadosRepository(entity);
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
        await _repositoryAdministrador.GetByIdAsyncRepositoryAvisosComunicados(id);

        public async Task<ResultOperation> UpdateAvisosYComunicados(AvisosComunicados entity)
        {
            try
            {
                var result = await _repositoryAdministrador.UpdateAsyncAvisosYComunicadosRepository(entity);
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

                var countResult = await _repositoryAdministrador.GetTablaAvisosYComunicadosCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseAvisosComunicadosList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositoryAdministrador.GetAlertaAvisosYComunicadosCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }

                var result = await _repositoryAdministrador.GetTablaAvisosYComunicadosAsyncRepository(idConsulta);
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
        //Archivos
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
                var countResult = await _repositoryAdministrador.GetArchivosByFiltersCountAsyncRepository(
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

                var result = await _repositoryAdministrador.GetArchivoByFiltersAsyncRepository(
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
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                DateTime dateTime = DateTime.Now;
                if (entity.fechaSolicitud.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de solicitud no debe ser mayor a la fecha actual.");
                }


                var result = await _repositoryAdministrador.AddAsyncSolicitudTransparenciaService(entity, entityDocumento, dataFile);
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
        await _repositoryAdministrador.GetByIdSolicitudTransparenciaRepository(id);

        public async Task<ResultOperation> UpdateSolicitudTransparenciaService(SolicitudTransparencia entity)
        {
            try
            {
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_consulta);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                DateTime dateTime = DateTime.Now;
                if (entity.fechaSolicitud.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de solicitud no debe ser mayor a la fecha actual.");
                }

                var result = await _repositoryAdministrador.UpdateSolicitudTransparenciaRepository(entity);
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
                var result = await _repositoryAdministrador.GetByIdSolicitudTransparenciaRepositorys(id);
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


                var countResult = await _repositoryAdministrador.GetTablaSolicitudTransparenciaCountRepository(idConsulta);

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
                var result = await _repositoryAdministrador.GetTablaSolicitudTransparenciaRepository(idConsulta);

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

                var result = await _repositoryAdministrador.DeleteSolicitudTransparenciaRepository(entity.id);
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

        public async Task<Cumplimentacion> GetByAllServiceCumplimentacion(int id) =>
        await _repositoryAdministrador.GetByIdAllAsyncRepositoryCumplimentacion(id);

        public async Task<ResultOperation<ResponseAsignarCumplimentacion>> AsignarConsultaCumplimentacionService(Cumplimentacion entity)
        {
            try
            {
                var result = await _repositoryAdministrador.AsignarCumplimentacionRepository(entity);
                if (!result.Success)
                    return ResultOperation.FailureErrorResponse<ResponseAsignarCumplimentacion>($"{result.MsgError!}:{result.DetailError}");

                var resultOperation = ResultOperation.SuccessResponseNoMessage(new ResponseAsignarCumplimentacion());

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

                var result = await _repositoryAdministrador.UpdateCumplimentacionRepository(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> AddResolucionCumplimentacionService(ResolucionCumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            try
            {
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_cumplimentacion);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                int validacion = await _repositoryAdministrador.GetValidacionResolucion(entity.id_cumplimentacion);

                if (validacion == 0)
                {
                    if (entity.fecha_resolucion.Date < Convert.ToDateTime(consultaDetails.fecha_firmeza))
                    {
                        return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de resolucion no puede ser menor a la fecha de firmeza");
                    }

                    var result = await _repositoryAdministrador.AddAsyncResolucionCumplimentacion(entity, entityDocumento, dataFile);
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

        public async Task<ResolucionCumplimentacion> GetByAllServiceResolucionCumplimentacion(int idCumplimentacion) =>
        await _repositoryAdministrador.GetByIdAllAsyncRepositoryResolucionCumplimentacionT(idCumplimentacion);

        public async Task<ResultOperation> UpdateResolucionCumplimentacion(ResolucionCumplimentacion entity)
        {
            try
            {
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_cumplimentacion);
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

                var result = await _repositoryAdministrador.UpdateAsyncResolucionCumplimentacion(entity);
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
                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(entity.id_cumplimentacion);
                if (consultaDetails == null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }


                var result = await _repositoryAdministrador.ConcluirAsyncResolucionCumplimentacion(entity, entityDocumento, dataFile);
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

                var consultaDetails = await _repositoryAdministrador.GetByIdDisconnected(idConsulta);

                var result = await _repositoryAdministrador.GetTablaResolucionAsyncRepository(idConsulta);
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

        #endregion


        #region Medios defensa
        public async Task<ResultOperation> GetTablaMediosDefensa(List<string> noAsunto)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();


                var result = await _repositoryAdministrador.GetTablaMediosDefensaAsyncRepository(noAsunto);
                if (result is not null)
                {


                    return ResultOperation.SuccessResponseNoMessage(result);
                }

                if (result is null)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(result);
                }


                return ResultOperation.SuccessResponseNoMessage(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

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
        #endregion


        #region  Exportar
        public async Task<List<ResponseReporteGlobalConsultas>> ExportarConsulta(
           string? noAsunto,
           string? rfcPromovente,
           string? promovente,
           int? idAdministracion,
           int? idSubadministracion,
           string? idAbogadoAsigno,
           List<int>? TipoEntrada,
           List<int>? TipoAsunto,
           List<int>? EstadoProcesal,
           int? idTema,
           DateTime? fechaPresentacionDesde,
           DateTime? fechaPresentacionHasta,
           DateTime? fechaVencimientoDesde,
           DateTime? fechaVencimientoHasta,
           DateTime? FechaConclucionDesde,
           DateTime? FechaConclucionHasta,
           DateTime? FechaOficioNotificacionResolucionDesde,
           DateTime? FechaOficioNotificacionResolucionHasta,
           DateTime? FechaIngresoProdeconDesde,
           DateTime? FechaIngresoProdeconHasta,
           DateTime? FechaSolicituTransparenciaDesde,
           DateTime? FechaSolicituTransparenciaHasta

        ) =>
                await _repositoryAdministrador.ExportarConsultaAsyncRepository(
                     noAsunto,
                     rfcPromovente,
                     promovente,
                     idAdministracion,
                     idSubadministracion,
                     idAbogadoAsigno,
                     TipoEntrada,
                     TipoAsunto,
                     EstadoProcesal,
                     idTema,
                     fechaPresentacionDesde,
                     fechaPresentacionHasta,
                     fechaVencimientoDesde,
                     fechaVencimientoHasta,
                     FechaConclucionDesde,
                     FechaConclucionHasta,
                     FechaOficioNotificacionResolucionDesde,
                     FechaOficioNotificacionResolucionHasta,
                     FechaIngresoProdeconDesde,
                     FechaIngresoProdeconHasta,
                     FechaSolicituTransparenciaDesde,
                     FechaSolicituTransparenciaHasta

                );

        public async Task<List<ResponseReporteGlobalCumplimentacion>> ExportarCumplimentacion(
string? noAsunto,
string? rfcPromovente,
string? promovente,
int? idAdministracion,
int? idSubadministracion,
string? idAbogadoAsigno,
List<int>? TipoEntrada,
List<int>? TipoAsunto,
List<int>? EstadoProcesal,
int? idTema,
DateTime? fechaPresentacionDesde,
DateTime? fechaPresentacionHasta,
DateTime? fechaVencimientoDesde,
DateTime? fechaVencimientoHasta,
DateTime? FechaConclucionDesde,
DateTime? FechaConclucionHasta,
DateTime? FechaOficioNotificacionResolucionDesde,
DateTime? FechaOficioNotificacionResolucionHasta,
DateTime? FechaIngresoProdeconDesde,
DateTime? FechaIngresoProdeconHasta,
DateTime? FechaSolicituTransparenciaDesde,
DateTime? FechaSolicituTransparenciaHasta

) =>
     await _repositoryAdministrador.ExportarCumplimentacionAsyncRepository(
          noAsunto,
          rfcPromovente,
          promovente,
          idAdministracion,
          idSubadministracion,
          idAbogadoAsigno,
          TipoEntrada,
          TipoAsunto,
          EstadoProcesal,
          idTema,
          fechaPresentacionDesde,
          fechaPresentacionHasta,
          fechaVencimientoDesde,
          fechaVencimientoHasta,
          FechaConclucionDesde,
          FechaConclucionHasta,
          FechaOficioNotificacionResolucionDesde,
          FechaOficioNotificacionResolucionHasta,
          FechaIngresoProdeconDesde,
          FechaIngresoProdeconHasta,
          FechaSolicituTransparenciaDesde,
          FechaSolicituTransparenciaHasta

     );

        public async Task<List<ResponseReporteGeneral>> ExportarReporteGeneral(
             string? noAsunto,
             string? rfcPromovente,
             string? promovente,
             int? idAdministracion,
             int? idSubadministracion,
             string? idAbogadoAsigno,
             List<int>? TipoEntrada,
             List<int>? TipoAsunto,
             List<int>? EstadoProcesal,
             int? idTema,
             DateTime? fechaPresentacionDesde,
             DateTime? fechaPresentacionHasta,
             DateTime? fechaVencimientoDesde,
             DateTime? fechaVencimientoHasta,
             DateTime? FechaConclucionDesde,
             DateTime? FechaConclucionHasta,
             DateTime? FechaOficioNotificacionResolucionDesde,
             DateTime? FechaOficioNotificacionResolucionHasta,
             DateTime? FechaIngresoProdeconDesde,
             DateTime? FechaIngresoProdeconHasta,
             DateTime? FechaSolicituTransparenciaDesde,
             DateTime? FechaSolicituTransparenciaHasta

          ) =>
                  await _repositoryAdministrador.ExportarReporteGeneralAsyncRepository(
            noAsunto,
            rfcPromovente,
            promovente,
            idAdministracion,
            idSubadministracion,
            idAbogadoAsigno,
            TipoEntrada,
            TipoAsunto,
            EstadoProcesal,
            idTema,
            fechaPresentacionDesde,
            fechaPresentacionHasta,
            fechaVencimientoDesde,
            fechaVencimientoHasta,
            FechaConclucionDesde,
            FechaConclucionHasta,
            FechaOficioNotificacionResolucionDesde,
            FechaOficioNotificacionResolucionHasta,
            FechaIngresoProdeconDesde,
            FechaIngresoProdeconHasta,
            FechaSolicituTransparenciaDesde,
            FechaSolicituTransparenciaHasta

       );
        #endregion
        public async Task<List<Response_Resolucion_PDF>> Exporta_PDF(string noAsunto) =>
       await _repositoryAdministrador.ExportarPDFAsyncRepository(noAsunto);
    }
}