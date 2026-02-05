
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.IDAO.IRepository;
using ConsultasAPI.Model.IDAO.IServiceDAO;
using ConsultasAPI.Model.ViewModels.Enums;
using Sicoj.Utils.Enums;
using Sicoj.Utils.Models;
using Sicoj.Utils.Redis;
using Sicoj.Utils.ViewModels;
using Sicoj.Utils.Extentions;
using ConsultasAPI.Model.Entities;
using Microsoft.Extensions.Options;

namespace ConsultasAPI.Model.DAO.ServicesDAO
{
    public class ConsultasSupervisorService : IConsultasSupervisorService
    {

        private readonly IConsultaRepositorySupervisor _repositorySupervisor;
        private readonly IRedisClient _redisClient;
        private readonly IApiService _apiService;
        private readonly string _routeInfoUsuario = null!;
        private readonly ProxyEnpoints _proxyEnpoints;

        public ConsultasSupervisorService(IConsultaRepositorySupervisor repositorySupervisor, IRedisClient redisClient, IApiService apiService, IConfiguration configuration, IOptions<ProxyEnpoints> proxyEnpoints)
        {
            _repositorySupervisor = repositorySupervisor ?? throw new ArgumentNullException(nameof(repositorySupervisor));
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(redisClient));
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _routeInfoUsuario = configuration.GetValue<string>("CatalogsEndpoints:RouteInfoUsuario")!;
            _proxyEnpoints = proxyEnpoints.Value ?? throw new ArgumentNullException(nameof(proxyEnpoints));
        }


        #region Historico de Asuntos
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
            DateTime? fechaCSSJDesde,
            DateTime? fechaCSSJHasta,
            string? rfcPromovente,
            string? promovente,
            List<int>? TipoAsunto,
            int? idAdministracion,
            int? idSubadministracion,
            string? idAbogadoAsigno,
            List<int>? EstadoTarea,
            List<int>? TipoModalidad,
            List<int>? EstadoProcesal,
            UserInformationView userInformationView
        )
        {
            try
            {
                var countResult = await _repositorySupervisor.GetAllByFiltersCountAsyncRepository(
                        noAsunto,
                        fechaPresentacionDesde,
                        fechaPresentacionHasta,
                        fechaVencimientoDesde,
                        fechaVencimientoHasta,
                        fechaCSSJDesde,
                        fechaCSSJHasta,
                        rfcPromovente,
                        promovente,
                        TipoAsunto,
                        idAdministracion,
                        idSubadministracion,
                        idAbogadoAsigno,
                        EstadoTarea,
                        TipoModalidad,
                        EstadoProcesal,
                        userInformationView.IdAdministracionCentral,
                        userInformationView.IdAdministracion
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseConsultaByFiltersAdministrador>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }

                var result = await _repositorySupervisor.GetAllByFiltersAsyncRepository(
                    Fetch,
                    Page,
                    OrderByColumn,
                    OrderDesc,
                    noAsunto,
                    fechaPresentacionDesde,
                    fechaPresentacionHasta,
                    fechaVencimientoDesde,
                    fechaVencimientoHasta,
                    fechaCSSJDesde,
                    fechaCSSJHasta,
                    rfcPromovente,
                    promovente,
                    TipoAsunto,
                    idAdministracion,
                    idSubadministracion,
                    idAbogadoAsigno,
                    EstadoTarea,
                    TipoModalidad,
                    EstadoProcesal,
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

        #region Consulta

        public async Task<ResultOperation> GetByIdDisconnected(int id)
        {
            try
            {
                int idAlerta = EnumAlertas.GRIS.GetHashCode();
                string alerta = EnumAlertas.GRIS.ToString();

                var result = await _repositorySupervisor.GetTablaConsultaRepository(id);
                if (result is null)
                {
                    return ResultOperation.FailureWarningResponse<ResponseConsultaList>("No se encontraron resultados.");
                }
                _redisClient.ValidateTakeList(ref result, EnumModulosRedis.CONSULTAS);
                var resultOperation = ResultOperation.SuccessResponseNoMessage(result);

                foreach (var item in result)
                {
                    if (item.fecha_CSSJ != null)
                    {
                        int diasHabiles = 0;

                        for (DateTime fecha = Convert.ToDateTime(item.fecha_CSSJ); fecha <= DateTime.Now; fecha = fecha.AddDays(1))
                        {
                            if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                            {
                                diasHabiles++;
                            }
                        }

                        if (diasHabiles <= 2)
                        {
                            idAlerta = EnumAlertas.VERDE.GetHashCode();
                            alerta = EnumAlertas.VERDE.ToString();
                        }
                        else
                        {
                            idAlerta = EnumAlertas.ROJO.GetHashCode();
                            alerta = EnumAlertas.ROJO.ToString();
                        }
                    }

                    if (item.idTipoAsunto > 0)
                    {
                        var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.TipoAsuntoConsulta, item.idTipoAsunto.ToString());
                        if (!string.IsNullOrEmpty(catalogValue))
                        {
                            item.tipoAsunto = catalogValue;
                        }
                        else
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre del tipo asunto.");

                    }

                    if (item.idTipoModalidad > 0)
                    {

                        var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.TipoEntradaConsulta, item.idTipoModalidad.ToString());
                        if (!string.IsNullOrEmpty(catalogValue))
                        {
                            item.tipoModalidad = catalogValue;
                        }
                        else
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre del tipo de modalidad.");
                    }

                    if (item.idAdministracion > 0)
                    {

                        var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.Administracion, item.idAdministracion.ToString());
                        if (!string.IsNullOrEmpty(catalogValue))
                        {
                            item.administracion = catalogValue;
                        }
                        else
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la unidad administrativa/subadminsitración.");

                    }


                    if (item.idEstadoProcesal > 0)
                    {
                        var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.EstadoProcesalConsulta, item.idEstadoProcesal.ToString());
                        if (!string.IsNullOrEmpty(catalogValue))
                        {
                            item.estadoProcesal = catalogValue;
                        }
                        else
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre del estado procesal.");

                    }

                    if (item.idEstadoTarea > 0)
                    {
                        var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.TareaConsulta, item.idEstadoTarea.ToString());
                        if (!string.IsNullOrEmpty(catalogValue))
                        {
                            item.estadoTarea = catalogValue;
                        }
                        else
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la tarea.");
                    }

                    if (item is not null && !string.IsNullOrEmpty(item.idAbogado) && !item.remitido)
                    {
                        var response = await _apiService.GetResultOperationAsync<UserInformationView>($"{_routeInfoUsuario}/{item.idAbogado}");
                        if (response is not null && response.Success && response.Result is not null)
                        {
                            item.idAbogado = response.Result.Rfc;
                            item.abogado = response.Result.Nombre;
                            item.abogadoActivo = true;
                        }
                        else
                        {
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre del abogado.");
                            item.abogadoActivo = false;
                        }
                    }
                }

                return ResultOperation.SuccessResponse(
                   new DataTableViewAlerta<ResponseConsultaList>(new(idAlerta, alerta), result),
                   string.Join("\n", resultOperation.Messages.Select(m => m.detailMessage))
                   );
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Solicitud de transparencia
        public async Task<ResultOperation> GetTablaSolicitudTransparenciaService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertas.VERDE.GetHashCode();
                string alerta = EnumAlertas.VERDE.ToString();


                var countResult = await _repositorySupervisor.GetTablaSolicitudTransparenciaCountRepository(idConsulta);

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
                var result = await _repositorySupervisor.GetTablaSolicitudTransparenciaRepository(idConsulta);

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
        #endregion
        #region Archivos

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
                var countResult = await _repositorySupervisor.GetArchivosByFiltersCountAsyncRepository(
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

                var result = await _repositorySupervisor.GetArchivoByFiltersAsyncRepository(
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

        public async Task<ResponseArchivosConsulta> GetByIdArchivoService(int id) =>
        await _repositorySupervisor.GetByIdArchivoAsyncRepository(id);

        #endregion
        #region Requerimiento

        public async Task<ResultOperation> GetTablaRequerimientosService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var consultaDetails = await _repositorySupervisor.GetByIdDisconnected(idConsulta);
                if (consultaDetails.solicita_requerimiento == false)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                       null!)
                   );
                }


                var countResult = await _repositorySupervisor.GetTablaRequerimientosCountAsyncRepository(idConsulta);



                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositorySupervisor.GetTablaAlertaRequerimientosCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }
                var result = await _repositorySupervisor.GetTablaRequerimientosAsyncRepository(idConsulta);
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
        #endregion
        #region Solicitud de opinion de la información
        public async Task<ResultOperation> GetTablaSolicitudInformacionService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var countResult = await _repositorySupervisor.GetTablaSolicitudInformacionCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseSolicitudInformacionList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositorySupervisor.GetTablaAlertaSolicitudInformacionCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }
                var result = await _repositorySupervisor.GetTablaSolicitudInformacionAsyncRepository(idConsulta);
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
        #endregion
        #region  Emisión resolución
        public async Task<ResultOperation> GetTablaResolucionService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var consultaDetails = await _repositorySupervisor.GetByIdDisconnected(idConsulta);

                var result = await _repositorySupervisor.GetTablaResolucionAsyncRepository(idConsulta);
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

        public async Task<ResultOperation> GetResolucionById(int id)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                //var consultaDetails = await _repositorySupervisor.GetByIdDisconnected(idConsulta);

                var result = await _repositorySupervisor.GetByIdAllAsyncRepositoryResolucion(id);
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
        #endregion
        #region Avisos Y Comunicados
        public async Task<ResultOperation> GetTablaAvisosYComunicados(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var countResult = await _repositorySupervisor.GetTablaAvisosYComunicadosCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseAvisosComunicadosList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositorySupervisor.GetAlertaAvisosYComunicadosCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }

                var result = await _repositorySupervisor.GetTablaAvisosYComunicadosAsyncRepository(idConsulta);
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
        #endregion
        #region  Requerimientos procedon 

        public async Task<ResultOperation> GetTablaRequerimientosProdeconService(int idConsulta)
        {

            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var countResult = await _repositorySupervisor.GetTablaRequerimientosProdeconCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseRequerimientoProdeconList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var result = await _repositorySupervisor.GetTablaRequerimientosProdeconAsyncRepository(idConsulta);
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

        #endregion
        #region Medios defensa
        public async Task<ResultOperation> GetTablaMediosDefensa(string noAsunto)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();


                var result = await _repositorySupervisor.GetTablaMediosDefensaAsyncRepository(noAsunto);
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

        public async Task<ResultOperation> GetTablaMediosDefensaGeneral(string noAsunto)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();


                var result = await _apiService.GetResultOperationAsync<List<ResponseMediosDefensa>>($"{_proxyEnpoints.RouteMediosDefensa}?idModule={EnumModulosSicoj.CONSULTAS.GetHashCode()}&noAsunto={noAsunto}");
                if (result is not null)
                {

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseMediosDefensa>(new(idAlerta, alerta),
                        result.Result)
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
                       result.Result)
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
                await _repositorySupervisor.ExportarConsultaAsyncRepository(
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
     await _repositorySupervisor.ExportarCumplimentacionAsyncRepository(
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
                     await _repositorySupervisor.ExportarReporteGeneralAsyncRepository(
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