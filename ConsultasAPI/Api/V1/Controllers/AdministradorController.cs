using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.Entities;
using ConsultasAPI.Model.IDAO.IServiceDAO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sicoj.Utils.Models;
using Sicoj.Utils.Redis;
using ConsultasAPI.Model.ViewModels.Enums;
using ConsultasAPI.Model.IDAO.IRepository;
using ConsultasAPI.Model.Entities.Events.Administrador;
using Sicoj.Utils.Middleware;
using Sicoj.Utils.Files;
using Sicoj.Utils.Enums;
using Mapster;
using ConsultasAPI.Model.DTO.Contracts.Administrador;
using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.DTO.Contracts.OficialPartes;
using Sicoj.Utils.Extentions;

namespace ConsultasAPI.Api.V1.Controllers
{
    [Route("sicoj/consultas/api/v1/administrador/consultas")]
    public class AdministradorController : ControllerBase
    {

        private readonly ILogger<AdministradorController> _logger;

        private readonly IRedisClient _redisClient;

        private static object _lock = new object();

        private readonly IConsultasAdministradorService _consultasAdministradorService;
        private readonly IConsultaRepositoryAdministrador _consultaRepositoryAdministrador;

        private readonly IValidator<RequestUpdateConsultaAdministradorComercioExterior> _validatorUpdateConsultaAdministadorComercioExterior;
        private readonly IValidator<RequestUpdateConsultaAdministradorImpuestosInternos> _validatorUpdateConsultaAdministadorImpuestosInternos;
        private readonly IValidator<RequestCreatePersonasAutorizadas> _validatorRequestCreatePersonasAutorizadas;
        private readonly IValidator<RequestUpdatePersonasAutorizadas> _validatorUpdatePersonasAutorizadas;
        private readonly IValidator<RequestRemision> _validatorRequestRemision;
        private readonly IValidator<RequestDeleteArchivosConsulta> _validatorDeleteArchivos;
        private readonly IValidator<RequestCreateRequerimiento> _validatorRequestCreateRequerimiento;
        private readonly IValidator<RequestSolicitaRequerimiento> _validatorRequestSolicitaRequerimiento;
        private readonly IValidator<RequestUpdateRequerimiento> _validatorRequestUpdateRequerimiento;
        private readonly IValidator<RequestAsignarAdministrador> _validatorAsignarAdministrador;
        private readonly IValidator<RequestCreateSolicitudInformacion> _validatorRequestCreateSolicitudInformacion;
        private readonly IValidator<RequestUpdateSolicitudInformacion> _validatorRequestUpdateSolicitudInformacion;
        private readonly IValidator<RequestCreateResolucion> _validatorRequestCreateResolucion;
        private readonly IValidator<RequestUpdateResolucion> _validatorRequestUpdateResolucion;
        private readonly IValidator<RequestConcluirResolucion> _validatorRequestConcluirResolucion;
        private readonly IValidator<RequestCreateRequerimientoProdecon> _validatorRequestCreateRequerimientoProdecon;
        private readonly IValidator<RequestUpdateRequerimientoProdecon> _validatorRequestUpdateRequerimientoProdecon;

        private readonly IValidator<RequestCreateAvisosYComunicados> _validatorRequestAvisosYComunicados;
        private readonly IValidator<RequestUpdateAvisosYComunicados> _validatorRequestUpdateAvisosYComunicados;


        private readonly IFileSystemService _fileSystemService;
        private readonly IValidator<RequestDocumentoUpdate> _requestDocumentoUpdateValidator;


        private readonly IValidator<RequestCreateSolicitudTransparencia> _validatorRequestCreateSolicitudTransparencia;
        private readonly IValidator<RequestUpdateSolicitudTransparencia> _validatorRequestUpdateSolicitudTransparencia;
        private readonly IValidator<RequestAsignarCumplimentacionAdministrador> _validatorAsignarCumplimentacionAdministrador;
        private readonly IValidator<RequestUpdateCumplimentacion> _validatorUpdateCumplimentacion;
        private readonly IValidator<RequestCreateResolucionCumplimentacion> _validatorRequestCreateResolucionCumplimentacion;
        private readonly IValidator<RequestUpdateResolucionCumplimentacion> _validatorRequestUpdateResolucionCumplimentacion;
        private readonly IValidator<RequestConcluirResolucionCumplimentacion> _validatorRequestConcluirResolucionCumplimentacion;

        public AdministradorController(
            ILogger<AdministradorController> logger,
            IRedisClient redisClient,
            IConsultasAdministradorService consultasAdministradorService,
            IConsultaRepositoryAdministrador consultaRepositoryAdministrador,
            IValidator<RequestUpdateConsultaAdministradorComercioExterior> validatorUpdateConsultaAdministadorComercioExterior,
            IValidator<RequestUpdateConsultaAdministradorImpuestosInternos> validatorUpdateConsultaAdministadorImpuestosInternos,
            IValidator<RequestCreatePersonasAutorizadas> validatorPersonasAutorizadas,
            IValidator<RequestUpdatePersonasAutorizadas> validatorUpdatePersonasAutorizadas,
            IValidator<RequestRemision> validatorRequestRemision, IValidator<RequestDeleteArchivosConsulta> validatorDeleteArchivos,
            IValidator<RequestAsignarAdministrador> validatorAsignarAdministrador,
            IFileSystemService fileSystemService, IValidator<RequestCreateRequerimiento> validatorRequerimiento,
            IValidator<RequestUpdateRequerimiento> validatorUpdateRequerimiento,
            IValidator<RequestCreateSolicitudInformacion> validatorRequestCreateSolicitudInformacion,
            IValidator<RequestUpdateSolicitudInformacion> validatorRequestUpdateSolicitudInformacion,
            IValidator<RequestCreateResolucion> validatorResolucion,
            IValidator<RequestUpdateResolucion> validatorUpdateResolucion,
            IValidator<RequestCreateRequerimientoProdecon> validatorRequerimientoProdecon,
            IValidator<RequestUpdateRequerimientoProdecon> validatorUpdateRequerimientoProdecon,
            IValidator<RequestCreateAvisosYComunicados> validatorRequestAvisosYComunicados,
            IValidator<RequestUpdateAvisosYComunicados> validatorRequestUpdateAvisosYComunicados,
            IValidator<RequestCreateSolicitudTransparencia> validatorRequestCreateSolicitudTransparencia,
            IValidator<RequestUpdateSolicitudTransparencia> validatorUpdateSolicitudTransparencia,
            IValidator<RequestDocumentoUpdate> requestDocumentoUpdateValidator,
            IValidator<RequestSolicitaRequerimiento> validatorRequestSolicitaRequerimiento,
            IValidator<RequestConcluirResolucion> validatorRequestConcluirResolucion,
            IValidator<RequestAsignarCumplimentacionAdministrador> validatorAsignarCumplimentacionAdministrador,
            IValidator<RequestUpdateCumplimentacion> validatorUpdateCumplimentacion,
            IValidator<RequestCreateResolucionCumplimentacion> validatorResolucionCumplimentacion,
            IValidator<RequestUpdateResolucionCumplimentacion> validatorUpdateResolucionCumplimentacion,
            IValidator<RequestConcluirResolucionCumplimentacion> validatorRequestConcluirResolucionCumplimentacion)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(redisClient));
            _consultasAdministradorService = consultasAdministradorService ?? throw new ArgumentNullException(nameof(consultasAdministradorService));
            _consultaRepositoryAdministrador = consultaRepositoryAdministrador ?? throw new ArgumentNullException(nameof(consultaRepositoryAdministrador));
            _validatorUpdateConsultaAdministadorComercioExterior = validatorUpdateConsultaAdministadorComercioExterior ?? throw new ArgumentNullException(nameof(validatorUpdateConsultaAdministadorComercioExterior));
            _validatorUpdateConsultaAdministadorImpuestosInternos = validatorUpdateConsultaAdministadorImpuestosInternos ?? throw new ArgumentNullException(nameof(validatorUpdateConsultaAdministadorImpuestosInternos));
            _validatorRequestCreatePersonasAutorizadas = validatorPersonasAutorizadas ?? throw new ArgumentNullException(nameof(validatorPersonasAutorizadas));
            _validatorUpdatePersonasAutorizadas = validatorUpdatePersonasAutorizadas ?? throw new ArgumentNullException(nameof(validatorUpdatePersonasAutorizadas));
            _validatorRequestRemision = validatorRequestRemision ?? throw new ArgumentNullException(nameof(validatorRequestRemision));
            _validatorDeleteArchivos = validatorDeleteArchivos ?? throw new ArgumentNullException(nameof(validatorDeleteArchivos));
            _validatorAsignarAdministrador = validatorAsignarAdministrador ?? throw new ArgumentNullException(nameof(validatorAsignarAdministrador));
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _validatorRequestCreateRequerimiento = validatorRequerimiento ?? throw new ArgumentNullException(nameof(validatorRequerimiento));
            _validatorRequestUpdateRequerimiento = validatorUpdateRequerimiento ?? throw new ArgumentNullException(nameof(validatorUpdateRequerimiento));
            _validatorRequestCreateSolicitudInformacion = validatorRequestCreateSolicitudInformacion ?? throw new ArgumentNullException(nameof(validatorRequestCreateSolicitudInformacion));
            _validatorRequestCreateResolucion = validatorResolucion ?? throw new ArgumentNullException(nameof(validatorResolucion));
            _validatorRequestUpdateResolucion = validatorUpdateResolucion ?? throw new ArgumentNullException(nameof(validatorUpdateResolucion));
            _validatorRequestUpdateSolicitudInformacion = validatorRequestUpdateSolicitudInformacion ?? throw new ArgumentNullException(nameof(validatorRequestUpdateSolicitudInformacion));
            _validatorRequestCreateRequerimientoProdecon = validatorRequerimientoProdecon ?? throw new ArgumentNullException(nameof(validatorRequerimientoProdecon));
            _validatorRequestUpdateRequerimientoProdecon = validatorUpdateRequerimientoProdecon ?? throw new ArgumentNullException(nameof(validatorUpdateRequerimientoProdecon));
            _validatorRequestAvisosYComunicados = validatorRequestAvisosYComunicados ?? throw new ArgumentNullException(nameof(validatorRequestAvisosYComunicados));
            _validatorRequestUpdateAvisosYComunicados = validatorRequestUpdateAvisosYComunicados ?? throw new ArgumentNullException(nameof(validatorRequestUpdateAvisosYComunicados));
            _validatorRequestCreateSolicitudTransparencia = validatorRequestCreateSolicitudTransparencia ?? throw new ArgumentNullException(nameof(validatorRequestCreateSolicitudTransparencia));
            _validatorRequestUpdateSolicitudTransparencia = validatorUpdateSolicitudTransparencia ?? throw new ArgumentNullException(nameof(validatorUpdateSolicitudTransparencia));
            _requestDocumentoUpdateValidator = requestDocumentoUpdateValidator ?? throw new ArgumentNullException(nameof(requestDocumentoUpdateValidator));
            _validatorRequestSolicitaRequerimiento = validatorRequestSolicitaRequerimiento ?? throw new ArgumentNullException(nameof(validatorRequestSolicitaRequerimiento));
            _validatorRequestConcluirResolucion = validatorRequestConcluirResolucion ?? throw new ArgumentNullException(nameof(validatorRequestConcluirResolucion));
            _validatorAsignarCumplimentacionAdministrador = validatorAsignarCumplimentacionAdministrador ?? throw new ArgumentNullException(nameof(validatorAsignarCumplimentacionAdministrador));
            _validatorUpdateCumplimentacion = validatorUpdateCumplimentacion ?? throw new ArgumentNullException(nameof(validatorUpdateCumplimentacion));
            _validatorRequestCreateResolucionCumplimentacion = validatorResolucionCumplimentacion ?? throw new ArgumentNullException(nameof(validatorResolucionCumplimentacion));
            _validatorRequestUpdateResolucionCumplimentacion = validatorUpdateResolucionCumplimentacion ?? throw new ArgumentNullException(nameof(validatorUpdateResolucionCumplimentacion));
            _validatorRequestConcluirResolucionCumplimentacion = validatorRequestConcluirResolucionCumplimentacion ?? throw new ArgumentNullException(nameof(validatorRequestConcluirResolucionCumplimentacion));
        }

        /// <summary>
        /// Bandeja de Historico : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reasignarAsuntos"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<ResponseConsultaByFiltersAdministrador>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("bandeja-historico")]
        public async Task<IActionResult> BandejaHistoricoByFilters(
            [FromQuery] PagerQueryFilters request, bool reasignarAsuntos
        )

        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                RequestFiltrosConsultaAdministrador filters = new();
                Filters.MapFilters(request, filters);


                if (!Filters.MapSort<EnumOrderColumnConsultasByFiltrosAdministrador>(request, null!, false, out string orderByColumn, out bool orderDesc))
                {
                    return Ok(ResultOperation.FailureWarningResponse<List<RequestFiltrosConsultaAdministrador>>("La columna de ordenamiento no es válida.")
                        );
                }

                var result = await _consultasAdministradorService.GetBandejaHistoricoByFiltersService(
                     request.fetch,
                    request.page,
                    orderByColumn,
                    orderDesc,
                    Filters.GetStringValue(filters!.ByNoAsunto.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaPresentacionDesde.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaPresentacionHasta.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaVencimientoDesde.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaVencimientoHasta.FirstOrDefault()),
                    Filters.GetStringValue(filters.ByRfcPromovente.FirstOrDefault()),
                    Filters.GetStringValue(filters.ByPromovente.FirstOrDefault()),
                    filters.ByIdTipoAsunto.Adapt<List<int>>(),
                    Filters.GetIntValue(filters!.ByIdAdministracion.FirstOrDefault()),
                    Filters.GetIntValue(filters!.ByIdSubadministracion.FirstOrDefault()),
                    Filters.GetStringValue(filters.ByIdAbogadoAsigno.FirstOrDefault()),
                    filters.ByIdEstadoTarea.Adapt<List<int>>(),
                    filters.ByIdTipoModalidad.Adapt<List<int>>(),
                    filters.ByIdEstadoProcesal.Adapt<List<int>>(),
                    sessionInformation.UserInformation,
                    reasignarAsuntos
                    );

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }


        /// <summary>
        /// Bandeja de Pendientes : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseConsultaList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("bandeja-pendientes")]
        public async Task<IActionResult> GetBandejapendientes([FromQuery] PagerQuery request)
        {


            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                if (!Filters.MapSort<EnumOrderColumnConsultasByFiltros>(request, null!, false, out string orderByColumn, out bool orderDesc))
                {
                    return Ok(ResultOperation.FailureWarningResponse<List<ResponseConsultaList>>("La columna de ordenamiento no es válida.")
                        );
                }
                var result = await _consultasAdministradorService.GetBandejaPendientes_AdministradorService(
                    request.fetch,
                    request.page,
                    orderByColumn,
                    orderDesc,
                    sessionInformation.UserInformation);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Edita consultas  para comercio exterior : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("comercio-exterior")]
        public async Task<IActionResult> PatchConsultasAdministradorComercioExterior(
          [FromForm] RequestUpdateConsultaAdministradorComercioExterior request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.id}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }


                if (request.idTipoModalidad == EnumTipoModalidad.FISICO.GetHashCode() || request.idTipoModalidad == EnumTipoModalidad.LINEA.GetHashCode())
                {
                    var validationResult = await _validatorUpdateConsultaAdministadorComercioExterior.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                    }
                    Consulta entityExists = await _consultasAdministradorService.GetByIdServiceAdministrador(request.id);
                    if (entityExists is null)
                    {
                        return Ok(
                            ResultOperation.FailureErrorResponse(
                                "La consulta no existe."
                            )
                        );
                    }




                    try
                    {
                        EventsConsultasAdministrador.UpdateModalidaConsultaAdministradorComercioExterior(ref entityExists,

                                request.promoventeEsContribuyente,
                                request.rfcContribuyente,
                                request.contribuyente,
                                request.fechaPresentacion!,
                                request.fechaRecepcion!,
                                request.idTipoAsunto,
                                request.idTipoModalidad,
                                request.idSubadministracion
                        );
                    }
                    catch (Exception _ex)
                    {
                        return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                    }
                    var result = await _consultasAdministradorService.UpdateConsultaServiceAdministradorComercioExterior(entityExists);
                    return Ok(result);
                }
                return Ok(ResultOperation.FailureWarningResponse("El tipo de modalidad no es válida."));
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Edita consultas  para impuestos internos : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("impuestos-internos")]
        public async Task<IActionResult> PatchConsultasAdministradorImpustosInternos(
          [FromForm] RequestUpdateConsultaAdministradorImpuestosInternos request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.id}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }



                if (request.idTipoModalidad == EnumTipoModalidad.FISICO.GetHashCode() || request.idTipoModalidad == EnumTipoModalidad.LINEA.GetHashCode())
                {
                    var validationResult = await _validatorUpdateConsultaAdministadorImpuestosInternos.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                    }
                    var entityExists = await _consultasAdministradorService.GetByIdServiceAdministrador(request.id);
                    if (entityExists is null)
                    {
                        return Ok(
                            ResultOperation.FailureErrorResponse(
                                "La consulta no existe."
                            )
                        );
                    }


                    try
                    {
                        EventsConsultasAdministrador.UpdateModalidaConsultaAdministradorImpuestosInternos(ref entityExists,

                                request.promoventeEsContribuyente,
                                request.rfcContribuyente,
                                request.contribuyente,
                                request.fechaRecepcion!,
                                request.despachoAutorizado!,
                                request.domicilioPromovente!,
                                request.domicilioNotificaciones!,
                                request.idTema,
                                request.monto,
                                sessionInformation.UserInformation.Rfc,
                                request.idTipoAsunto,
                                request.idTipoModalidad,
                                request.idSubadministracion,
                                request.montoDeterminado
                        );
                    }
                    catch (Exception _ex)
                    {
                        return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                    }
                    var result = await _consultasAdministradorService.UpdateConsultaServiceAdministradorImpuestosInternos(entityExists);
                    return Ok(result);
                }
                return Ok(ResultOperation.FailureWarningResponse("El tipo de modalidad no es válida."));
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Crea personas autorizadas : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("personas-autorizadas")]
        public async Task<IActionResult> PostValidacionesPersonasAutorizadas(
         [FromForm] RequestCreatePersonasAutorizadas request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }


                var validationResult = await _validatorRequestCreatePersonasAutorizadas.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }

                PersonasAutorizadas entity = null!;
                try
                {
                    entity = EventsConsultasAdministrador.CreatePersonasAutorizadas(
                        request.idConsulta,
                        request.nombre,
                        request.rfc,
                        request.telefono!,
                        request.email!
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.AddPersonasAutorizadas(entity);
                if (result.Success == false)
                {
                    return Ok(result.Messages);
                }

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ah ocurrrido un error al guardar el registro");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        "Ah ocurrido un error inesperado al guardar el registro"
                    )
                );
            }
        }

        /// <summary>
        /// Edita personas autorizadas : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("personas-autorizadas")]
        public async Task<IActionResult> PatchPersonasAutorizadas(
          [FromForm] RequestUpdatePersonasAutorizadas request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorUpdatePersonasAutorizadas.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByAllServicePersonaAutorizadas(request.id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }

                try
                {
                    EventsConsultasAdministrador.UpdatePersonasAutorizadas(ref entityExists,
                            request.rfc,
                            request.nombre,
                            request.telefono,
                            request.email,
                            request.idConsulta
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorService.UpdatePersonasAutorizadas(entityExists);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Elimina personas autorizadas : Administrador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpDelete("personas-autorizadas")]
        public async Task<IActionResult> DeletePersonasAutorizadas(
           int id
        )
        {
            try
            {

                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{id}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }


                var entityExists = await _consultasAdministradorService.GetByAllServicePersonaAutorizadas(id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "El registro no existe."
                        )
                    );
                }

                try
                {
                    EventsConsultasAdministrador.DeletePersonasAutorizadas(ref entityExists,
                    id
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorService.DeletePersonasAutorizadas(entityExists);
                return Ok(result);

            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Tabla de personas autorizadas : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseTablaPersonasAutorizadas>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("personas-autorizadas")]
        public async Task<IActionResult> GetPersonasAutorizadas([FromQuery] PagerQuery request, int idConsulta)
        {


            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var result = await _consultasAdministradorService.GetTablaPersonasAutorizadasService(
                    request.fetch,
                    request.page,
                    idConsulta);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Consultas por Id (Descontinuado)  : Administrador
        /// </summary>
        /// <param name="id">Id consulta</param>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseConsultaList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("Anterior/{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var result = await _consultasAdministradorService.GetByIdAdministradorService(id);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Remitir consultas  : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("remitir")]
        public async Task<IActionResult> PostRemision(
         [FromForm] RequestRemision request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestRemision.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }
                DataFile dataFile = null!;
                if (request.documento is not null)
                {
                    _fileSystemService.FileTryOut(
                        request.documento,
                        Path.Combine("CONSULTAS", "REMITIR", request.idConsulta.ToString()),
                        out dataFile);
                }

                Remision entity = null!;
                ArchivoConsulta entityDocumento = null!;

                try
                {
                    entity = EventsConsultasAdministrador.RemisionModalidaConsultaAdministrador(
                   request.idConsulta,
                   request.idAdministracionRemite,
                   request.noOficioRemison,
                   request.idTipoAutoridad,
                   DateTime.Parse(request.fechaOficio!),
                   sessionInformation.UserInformation.Rfc,
                   sessionInformation.UserInformation.Nombre!
               );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsConsultasAdministrador.CreateDocumento(
                            request.idConsulta!,
                            request.idTipoArchivo.GetValueOrDefault(),
                            request.idSeccion.GetValueOrDefault(),
                            sessionInformation.UserInformation.IdAdministracionCentral.GetValueOrDefault(),
                            dataFile.File.FileName,
                            dataFile.FilePath,
                            dataFile.File.ContentType,
                            sessionInformation.UserInformation.Rfc,
                            sessionInformation.UserInformation.Rfc,
                            _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length)
                        );
                    }
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.AddRemisionAdministradorService(entity, entityDocumento, dataFile!);
                if (result.Success == false)
                {
                    return Ok(result.Messages);
                }

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Tabla de consultas remitidas  : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseTablaRemision>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("remitir")]
        public async Task<IActionResult> Getremision([FromQuery] PagerQuery request)
        {


            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var result = await _consultasAdministradorService.GetTablaRemisionAdministradorService(
                    request.fetch,
                    request.page);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Carga  documento a la consulta : Administrador
        /// </summary>
        /// <param name="request">Datos del archivo y del registro de Consultas </param>
        /// <returns>Id del registro de archivo agregado</returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("archivo")]
        public async Task<IActionResult> PostFile(
            [FromForm] RequestCrearArchivoRemision request
        )

        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var entityExists = await _consultasAdministradorService.GetByAllService(request.idConsulta);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }


                _fileSystemService.FileTryOut(
                      request.fileConsultas,
                      Path.Combine("CONSULTAS", "REMISION", request.idConsulta.ToString()),
                      out var dataFile);

                ArchivoConsulta entity = null!;

                try
                {
                    entity = EventsArchivosConsultasAdministrador.Create(
                        request.idRemision,
                        request.idTipoDocumento,
                        request.fileConsultas.FileName,
                        dataFile.FilePath,
                        dataFile.File.FileName,
                        sessionInformation.UserInformation.Rfc!,
                        sessionInformation.UserInformation.Rfc!,
                        request.noFolio!,
                        request.idSeccion,
                        request.idConsulta,
                        _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length),
                        request.idDocumentoSeccion
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorService.AddArchivosAsyncService(entity, dataFile);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Método para actualizar un documento : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("archivo")]
        public async Task<IActionResult> PatchDocumento(
            [FromForm] RequestDocumentoUpdate request)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var validationResult = await _requestDocumentoUpdateValidator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse<int>(validationResult.ToString(" - ")));
                }

                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");

                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede usar ya que el usuario no ha tomado la consulta.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse<int>(
                            "La autorización ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                EnumFileType[] requiredExtentions = { EnumFileType.JPG, EnumFileType.JPEG, EnumFileType.PDF };

                var entityExists = await _consultasAdministradorService.GetByAllService(request.idConsulta);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }
                ArchivoConsulta entityDocumento = await _consultasAdministradorService.GetIdArchivoConsultaService(request.id);



                if (!_fileSystemService.FileTryOut(
                        request.documento,
                        Path.Combine("CONSULTAS", "COMERCIO EXTERIOR", request.idConsulta.ToString()),
                        out var dataFile, out string message, requiredExtentions, entityDocumento.path_file!))
                {
                    return Ok(
                            ResultOperation.FailureErrorResponse<int>(
                                message
                            )
                        );
                }

                if (entityDocumento is null || !entityDocumento.estatus)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>(
                            "No existe el documento o fue eliminado."
                        )
                    );
                }

                try
                {
                    if (dataFile is not null)
                    {
                        EventsArchivosConsultasAdministrador.UpdateWithFile(
                            ref entityDocumento,
                            request.numeroFolio!,
                            request.idTipoDocumento,
                            dataFile.File.FileName,
                            dataFile.FilePath,
                            dataFile.File!.ContentType,
                            _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length),
                            sessionInformation.UserInformation.Rfc!,
                            sessionInformation.UserInformation.Rfc!
                        );
                    }
                    else
                    {
                        EventsArchivosConsultasAdministrador.Update(
                            ref entityDocumento,
                            request.numeroFolio!,
                            request.idTipoDocumento,
                            sessionInformation.UserInformation.Rfc!,
                            sessionInformation.UserInformation.Rfc!
                        );
                    }
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse<int>(_ex.Message));
                }

                ResultOperation<int> result = await _consultasAdministradorService.UpdateArchivoService(entityDocumento!, dataFile!);
                return Ok(result);

            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Borra documento de la consulta : Administrador
        /// </summary>
        /// <param name="request">id del registro que se desea eliminar o inactivar</param>
        /// <returns>Result Operation con mensaje de operación exitosa</returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpDelete("archivo")]
        public async Task<IActionResult> DeleteArchivoConsultas(
           RequestDeleteArchivosConsulta request
        )
        {
            try
            {

                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }



                var validationResult = await _validatorDeleteArchivos.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }

                foreach (var item in request.id)
                {
                    var entityExists = await _consultasAdministradorService.GetByIdArchivoDeleteService(item);
                    if (entityExists is not null)
                    {
                        try
                        {
                            EventsConsultasAdministrador.DeleteModalidaArchivoConsulta(ref entityExists
                            );
                        }
                        catch (Exception _ex)
                        {
                            return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                        }
                        var result = await _consultasAdministradorService.DeleteArchivoConsultaService(entityExists);
                    }
                }
                return Ok(ResultOperation.SuccessResponse(true));
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Lista los documentos asociados a la consulta : Administrador
        /// </summary>
        /// <param name="id">Id Autorización Comercio Exterior</param>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseArchivosConsulta>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("list-archivos/{id}")]
        public async Task<IActionResult> GetAllArchivosAsync(int id)
        {

            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var result = await _consultasAdministradorService.GetAllArchivoService(id);
                if (result is null)
                {
                    return BadRequest(
                   ResultOperation.FailureErrorResponse(
                       "No existe el registro."
                   )
               );

                }

                return Ok(
                  ResultOperation.SuccessResponse(
                      result.Adapt<List<ResponseArchivosConsulta>>()
                  )
              );
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Visualiza documento de la consulta : Administrador
        /// </summary>
        /// <param name="id">Id del archivo registrado</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<ResponseArchivosConsulta>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("visualizar-archivos/{id}")]
        public async Task<IActionResult> Descargar(int id)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var entityExists = await _consultasAdministradorService.GetByIdArchivoService(id);
                if (entityExists is null)
                {
                    return BadRequest("No existe el registro.");
                }

                var response = await _fileSystemService.GetFileAsync(entityExists.path_file!);
                if (response is null)
                {
                    return BadRequest("No existe el documento.");
                }

                var fileName = Path.GetFileName(entityExists.path_file!);
                var contentType = EventsArchivosConsultasAdministrador.GetArchivoExt(fileName);

                var contentDisposition = new System.Net.Mime.ContentDisposition
                {
                    Inline = true,
                    FileName = fileName
                };

                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(response, contentType);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }

        }

        /// <summary>
        ///  Bloquea el registro por Id : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<bool>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("tomar")]
        public async Task<IActionResult> PatchTomar(
              int id
             )
        {
            try
            {

                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                EnumModulosRedis enumModulo = default!;
                enumModulo = EnumModulosRedis.CONSULTAS;


                var entityExists = await _consultasAdministradorService.GetByIdAdministradorService(id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<bool>(
                            "La consulta no existe."
                        )
                    );
                }


                var response = await _redisClient.Take(enumModulo, id, sessionInformation.UserInformation.Rfc!, sessionInformation.UserInformation.Nombre!);
                return Ok(response);

            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }
        /// <summary>
        /// Libera el registro  : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<bool>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("soltar")]
        public async Task<IActionResult> PatchSoltar(int id)
        {
            try
            {

                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                EnumModulosRedis enumModulo = default!;
                enumModulo = EnumModulosRedis.CONSULTAS;

                var entityExists = await _consultasAdministradorService.GetByIdAdministradorService(id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<bool>(
                            "La consulta no existe."
                        )
                    );
                }

                var response = await _redisClient.Drop(enumModulo, id, sessionInformation.UserInformation.Rfc!);
                return Ok(response);


            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Asignar abogado a consulta : Administrador 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("asignar")]
        public async Task<IActionResult> PatchConsultasAdministradorAsignar(
          [FromForm] RequestAsignarAdministrador request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }



                var validationResult = await _validatorAsignarAdministrador.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByIdServiceAdministrador(request.idConsulta);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }


                try
                {
                    EventsConsultasAdministrador.AsignarAdministrador(ref entityExists,
                            request.idConsulta,
                            request.idAbogado,
                            sessionInformation.UserInformation.Rfc
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorService.AsignarConsultaService(entityExists);
                return Ok(result);

            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }


        }

        /// <summary>
        /// Consultas  por Id  : Administrador
        /// </summary>
        /// <param name="id">Id consulta</param>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseConsultaList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdDisconnected(int id)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var result = await _consultasAdministradorService.GetByIdDisconnected(id);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Activa requerimeinto: Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("Solicita-requerimiento")]
        public async Task<IActionResult> SolicitaRequerimiento(
         [FromForm] RequestSolicitaRequerimiento request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestSolicitaRequerimiento.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }




                Consulta entity = null!;
                try
                {

                    entity = EventsConsultasAdministrador.SolicitaRequerimiento(
                        request.idConsulta,
                        request.solicitaRequerimiento
                    );

                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.SolicitaRequerimientoService(entity);
                if (result.Success == false)
                {
                    return Ok(result.Messages);
                }

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Crea Requerimiento : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("requerimiento")]
        public async Task<IActionResult> PostRequerimientos(
         [FromForm] RequestCreateRequerimiento request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestCreateRequerimiento.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }

                DataFile dataFile = null!;
                if (request.documento is not null)
                {
                    _fileSystemService.FileTryOut(
                        request.documento,
                        Path.Combine("CONSULTAS", "REQUERIMIENTO", request.idConsulta.ToString()),
                        out dataFile);
                }

                ArchivoConsulta entityDocumento = null!;
                Requerimientos entity = null!;
                try
                {

                    entity = EventsConsultasAdministrador.CreateRequerimiento(
                        request.idConsulta,
                        request.noOficioRequerimiento,
                        DateTime.Parse(request.fechaRequerimiento!)
                    );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAdministrador.CreateDocumento(
                            request.idConsulta!,
                            request.idTipoArchivo.GetValueOrDefault(),
                            request.idSeccion.GetValueOrDefault(),
                            sessionInformation.UserInformation.IdAdministracionCentral.GetValueOrDefault(),
                            dataFile.File.FileName,
                            dataFile.FilePath,
                            dataFile.File.ContentType,
                            sessionInformation.UserInformation.Rfc,
                            sessionInformation.UserInformation.Rfc,
                            _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length),
                            request.noFolio
                        );
                    }

                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.AddRequerimientoService(entity, entityDocumento, dataFile!);
                if (result.Success == false)
                {
                    return Ok(result.Messages);
                }

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Edita requerimientos : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("requerimiento")]
        public async Task<IActionResult> PatchRequerimientos(
          [FromForm] RequestUpdateRequerimiento request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestUpdateRequerimiento.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByAllServiceRequerimientos(request.id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }


                DataFile dataFile = null!;
                if (request.documento is not null)
                {
                    _fileSystemService.FileTryOut(
                        request.documento,
                        Path.Combine("CONSULTAS", "REQUERIMIENTO", request.idConsulta.ToString()),
                        out dataFile);
                }

                ArchivoConsulta entityDocumento = null!;
                try
                {
                    DateTime? fechaAtencionParsed = string.IsNullOrWhiteSpace(request.fechaAtencion)
                   ? (DateTime?)null
                   : DateTime.Parse(request.fechaAtencion);

                    EventsConsultasAdministrador.UpdateRequerimientos(ref entityExists,
                    request.idConsulta,
                    request.atendio,
                    DateTime.Parse(request.fechaNotificacion!),
                    fechaAtencionParsed,
                     request.noOficioRequerimiento,
                    DateTime.Parse(request.fechaRequerimiento!)
                    );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAdministrador.CreateDocumento(
                            request.idConsulta!,
                            request.idTipoArchivo.GetValueOrDefault(),
                            request.idSeccion.GetValueOrDefault(),
                            sessionInformation.UserInformation.IdAdministracionCentral.GetValueOrDefault(),
                            dataFile.File.FileName,
                            dataFile.FilePath,
                            dataFile.File.ContentType,
                            sessionInformation.UserInformation.Rfc,
                            sessionInformation.UserInformation.Rfc,
                            _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length),
                            request.noFolio
                        );
                    }
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorService.UpdateRequerimientos(entityExists, entityDocumento, dataFile!);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Requerimiento  por Id  : Administrador
        /// </summary>
        /// <param name="id">Id Requerimiento</param>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseRequerimientoList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("requerimiento/{id}")]
        public async Task<IActionResult> GetByIdRequerimiento(int id)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var result = await _consultasAdministradorService.GetRequerimientoById(id);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Tabla de Requerimientos : Administrador
        ///<param name="idConsulta">Id Requerimiento</param>
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseRequerimientoList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("list-requerimiento/{idConsulta}")]
        public async Task<IActionResult> GetRequerimientos(int idConsulta)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var result = await _consultasAdministradorService.GetTablaRequerimientosService(idConsulta);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Crea solicitud de información : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("solicitud-informacion")]
        public async Task<IActionResult> PostSolicitudInformacion(
         [FromForm] RequestCreateSolicitudInformacion request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestCreateSolicitudInformacion.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }

                SolicitudInformacion entity = null!;
                try
                {
                    if (request.atendioSolicitud)
                    {
                        entity = EventsConsultasAdministrador.CreateSolicitudInformacion(
                       request.idConsulta,
                       request.idUnidadAdministrativa,
                       request.noOficioSolicitud,
                       request.noOficioRespuesta,
                       request.atendioSolicitud,
                       DateTime.Parse(request.fechaOficioSolicitud!),
                       DateTime.Parse(request.fechaOficioRespuesta!),
                       DateTime.Parse(request.fechaRecepcion!),
                       request.unidadEsInterna,
                       request.unidadAdministrativaExterna
                   );
                    }
                    else
                    {
                        entity = EventsConsultasAdministrador.CreateSolicitudInformacionSinAtencion(
                       request.idConsulta,
                       request.idUnidadAdministrativa,
                       request.noOficioSolicitud,
                       request.noOficioRespuesta,
                       request.atendioSolicitud,
                       request.unidadEsInterna,
                       request.unidadAdministrativaExterna
                   );
                    }


                }


                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.AddSolicitudInformacionService(entity);
                if (result.Success == false)
                {
                    return Ok(result.Messages);
                }

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Edita solicitud de informacion : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("solicitud-informacion")]
        public async Task<IActionResult> PatchSolicitudInformacion(
          [FromForm] RequestUpdateSolicitudInformacion request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestUpdateSolicitudInformacion.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByAllServiceSolicitudInformacion(request.id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }

                try
                {

                    if (request.atendioSolicitud)
                    {
                        EventsConsultasAdministrador.RequestUpdateSolicitudInformacion(ref entityExists,
                    request.idConsulta,
                    request.idUnidadAdministrativa,
                    request.noOficioSolicitud,
                    DateTime.Parse(request.fechaOficioSolicitud!),
                    request.atendioSolicitud,
                    request.noOficioRespuesta,
                    DateTime.Parse(request.fechaOficioRespuesta!),
                    DateTime.Parse(request.fechaRecepcion!),
                    request.unidadEsInterna,
                    request.unidadAdministrativaExterna
                    );
                    }
                    else
                    {
                        EventsConsultasAdministrador.RequestUpdateSolicitudInformacionSinAtencion(ref entityExists,
                        request.idConsulta,
                        request.idUnidadAdministrativa,
                        request.noOficioSolicitud,
                        DateTime.Parse(request.fechaOficioSolicitud!),
                        request.atendioSolicitud,
                        request.noOficioRespuesta,
                        request.unidadEsInterna,
                        request.unidadAdministrativaExterna
                        );
                    }
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorService.UpdateSolicitudInformacion(entityExists);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }


        /// <summary>
        /// Tabla de Solicitud de Información : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseSolicitudInformacionList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("list-solicitud-informacion/{idConsulta}")]
        public async Task<IActionResult> GetSolicitudInformacion(int idConsulta)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var result = await _consultasAdministradorService.GetTablaSolicitudInformacionService(idConsulta);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Solicitud de Información  por Id  : Administrador
        /// </summary>
        /// <param name="id">Id solicitud</param>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseSolicitudInformacionList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("solicitud-informacion/{id}")]
        public async Task<IActionResult> GetByIdSolicitudInformacion(int id)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var result = await _consultasAdministradorService.GetSolicitudInformacionById(id);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Guardar Resolucion : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("guardar-resolucion")]
        public async Task<IActionResult> PostResolucion(
         [FromForm] RequestCreateResolucion request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestCreateResolucion.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }


                DataFile dataFile = null!;
                if (request.documento is not null)
                {
                    _fileSystemService.FileTryOut(
                        request.documento,
                        Path.Combine("CONSULTAS", "RESOLUCION", request.idConsulta.ToString()),
                        out dataFile);
                }

                ArchivoConsulta entityDocumento = null!;
                Resolucion entity = null!;
                try
                {

                    entity = EventsConsultasAdministrador.CreateResolucion(
                    request.idConsulta,
                    request.noOficioResolucion,
                    DateTime.Parse(request.fechaResolucion!),
                    request.idSentido
                );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAdministrador.CreateDocumento(
                            request.idConsulta!,
                            request.idTipoArchivo.GetValueOrDefault(),
                            request.idSeccion.GetValueOrDefault(),
                            sessionInformation.UserInformation.IdAdministracionCentral.GetValueOrDefault(),
                            dataFile.File.FileName,
                            dataFile.FilePath,
                            dataFile.File.ContentType,
                            sessionInformation.UserInformation.Rfc,
                            sessionInformation.UserInformation.Rfc,
                            _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length),
                            request.noFolio
                        );
                    }

                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                //entity,entityDocumento, dataFile!
                var result = await _consultasAdministradorService.AddResolucionService(entity, entityDocumento, dataFile!);
                if (result.Success == false)
                {
                    return Ok(result.Messages);
                }

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Edita resolucion : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("resolucion")]
        public async Task<IActionResult> PatchResolucion(
          [FromForm] RequestUpdateResolucion request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestUpdateResolucion.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByAllServiceResolucion(request.idConsulta);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }



                try
                {

                    EventsConsultasAdministrador.UpdateResolucion(ref entityExists,
                    request.idConsulta,
                    request.noOficioResolucion,
                    DateTime.Parse(request.fechaResolucion!),
                    DateTime.Parse(request.fechaNotificacion!),
                    request.idSentido
                    );

                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.UpdateResolucion(entityExists);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// concluir resolucion : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("concluir-resolucion")]
        public async Task<IActionResult> concluirResolucion(
          [FromForm] RequestConcluirResolucion request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestConcluirResolucion.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByAllServiceResolucion(request.idConsulta);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }


                DataFile dataFile = null!;
                if (request.documento is not null)
                {
                    _fileSystemService.FileTryOut(
                        request.documento,
                        Path.Combine("CONSULTAS", "CLONCLUIR RESOLUCION", request.idConsulta.ToString()),
                        out dataFile);
                }

                ArchivoConsulta entityDocumento = null!;

                try
                {

                    EventsConsultasAdministrador.ConcluirResolucion(ref entityExists,
                    request.idConsulta
                    );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAdministrador.CreateDocumento(
                            request.idConsulta!,
                            request.idTipoArchivo.GetValueOrDefault(),
                            request.idSeccion.GetValueOrDefault(),
                            sessionInformation.UserInformation.IdAdministracionCentral.GetValueOrDefault(),
                            dataFile.File.FileName,
                            dataFile.FilePath,
                            dataFile.File.ContentType,
                            sessionInformation.UserInformation.Rfc,
                            sessionInformation.UserInformation.Rfc,
                            _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length),
                            request.noFolio
                        );
                    }

                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.ConcluirResolucion(entityExists, entityDocumento, dataFile!);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Resolucion  por Id  : Administrador
        /// </summary>
        /// <param name="id">Id Resolucion</param>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseResolucion>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("resolucion/{id}")]
        public async Task<IActionResult> GetByIdResolucion(int id)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var result = await _consultasAdministradorService.GetResolucionById(id);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Crea Requerimiento Prodecon : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("requerimiento-prodecon")]
        public async Task<IActionResult> PostRequerimientosProdecon(
         [FromForm] RequestCreateRequerimientoProdecon request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestCreateRequerimientoProdecon.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }

                DataFile dataFile = null!;
                if (request.documento is not null)
                {
                    _fileSystemService.FileTryOut(
                        request.documento,
                        Path.Combine("CONSULTAS", "REQUERIMIENTO PRODECON", request.idConsulta.ToString()),
                        out dataFile);
                }

                ArchivoConsulta entityDocumento = null!;
                RequerimientosProdecon entity = null!;
                try
                {

                    entity = EventsConsultasAdministrador.CreateRequerimientoProdecon(
                                request.idConsulta,
                                request.noOficioRequerimiento,
                                request.noExpedienteRequerimiento,
                                DateTime.Parse(request.fechaOficio!),
                                DateTime.Parse(request.fechaIngreso!),
                                request.atencion,
                                request.accionAdicional
                            );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAdministrador.CreateDocumento(
                            request.idConsulta!,
                            request.idTipoArchivo.GetValueOrDefault(),
                            request.idSeccion.GetValueOrDefault(),
                            sessionInformation.UserInformation.IdAdministracionCentral.GetValueOrDefault(),
                            dataFile.File.FileName,
                            dataFile.FilePath,
                            dataFile.File.ContentType,
                            sessionInformation.UserInformation.Rfc,
                            sessionInformation.UserInformation.Rfc,
                            _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length),
                            request.noFolio
                        );
                    }

                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }


                var result = await _consultasAdministradorService.AddRequerimientoProdeconService(entity, entityDocumento, dataFile!);
                if (result.Success == false)
                {
                    return Ok(result.Messages);
                }

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Edita requerimientos prodecon : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("requerimiento-prodecon")]
        public async Task<IActionResult> PatchRequerimientosProdecon(
          [FromForm] RequestUpdateRequerimientoProdecon request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestUpdateRequerimientoProdecon.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByAllServiceRequerimientosProdecon(request.id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }

                try
                {
                    EventsConsultasAdministrador.UpdateRequerimientosProdecon(ref entityExists,
                        request.idConsulta,
                        request.noOficioRequerimiento,
                        request.noExpedienteRequerimiento,
                        DateTime.Parse(request.fechaOficio!),
                        DateTime.Parse(request.fechaIngreso!),
                        request.atencion,
                        request.accionAdicional
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorService.UpdateRequerimientosProdecon(entityExists);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Requerimiento Prodecon  por Id  : Administrador
        /// </summary>
        /// <param name="id">Id Requerimiento</param>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseRequerimientoProdeconList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("requerimiento-prodecon/{id}")]
        public async Task<IActionResult> GetByIdRequerimientoProdecon(int id)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var result = await _consultasAdministradorService.GetRequerimientoProdeconById(id);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Tabla de Requerimientos Prodecon : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseRequerimientoProdeconList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("list-requerimiento-prodecon/{idConsulta}")]
        public async Task<IActionResult> GetRequerimientosProdecon(int idConsulta)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var result = await _consultasAdministradorService.GetTablaRequerimientosProdeconService(idConsulta);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }


        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("reasignar")]
        public async Task<IActionResult> PatchReasignar(
          [FromForm] RequestReasignar request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                if (request.idTipoAsunto == EnumTipoAsunto.COMERCIO_EXTERIOR.GetHashCode())
                {
                    var result = await _consultasAdministradorService.ReAsignarComercioExteriorAsync(request.idList.ToArray(), request.idAbogado, sessionInformation.UserInformation);
                    return Ok(result);
                }
                else if (request.idTipoAsunto == EnumTipoAsunto.IMPUESTOS_INTERNOS.GetHashCode())
                {
                    var result = await _consultasAdministradorService.ReAsignarImpuestosInternosAsync(request.idList.ToArray(), request.idAbogado, sessionInformation.UserInformation);
                    return Ok(result);
                }
                return BadRequest("El tipo de asunto no es válido.");


            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Crea Avisos Y Comunicados : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("avisos-comunicados")]
        public async Task<IActionResult> PostAvisosYComunicados(
         [FromForm] RequestCreateAvisosYComunicados request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestAvisosYComunicados.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }

                AvisosComunicados entity = null!;
                try
                {
                    entity = EventsConsultasAdministrador.CreateAvisosYComunicados(
                        request.idConsulta,
                        request.idTipoAviso,
                        request.folio,
                        DateTime.Parse(request.fechaIngreso!),
                        request.observaciones,
                        request.tieneFolio
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.AddAvisosYComunicadosService(entity);
                if (result.Success == false)
                {
                    return Ok(result.Messages);
                }

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Edita avisos y comunicados : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("avisos-comunicados")]
        public async Task<IActionResult> PatchAvisosYComunicados(
          [FromForm] RequestUpdateAvisosYComunicados request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestUpdateAvisosYComunicados.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByServiceAvisosYComunicados(request.id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }

                try
                {
                    EventsConsultasAdministrador.UpdateAvisosYComunicados(ref entityExists,
                        request.idConsulta,
                        request.atencionAdicional,
                        request.descripcionAtencion
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorService.UpdateAvisosYComunicados(entityExists);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Avisos Y comunicados por Id  : Administrador
        /// </summary>
        /// <param name="id">Id Aviso y Comunicado</param>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseAvisosComunicadosList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("avisos-comunicados/{id}")]
        public async Task<IActionResult> GetByIdAvisosYComnunicados(int id)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var result = await _consultasAdministradorService.GetByServiceAvisosYComunicados(id);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Tabla de Avisos Y Comunicados : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseAvisosComunicadosList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("list-avisos-comunicados/{idConsulta}")]
        public async Task<IActionResult> GetTablaAvisosYComunicados(int idConsulta)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var result = await _consultasAdministradorService.GetTablaAvisosYComunicados(idConsulta);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }


        /// <summary>
        /// Lista los documentos por filtros : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<ResponseArchivosConsulta>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("list-filters-archivos")]
        public async Task<IActionResult> GetArchivosByFilters(
            [FromQuery] PagerQueryFilters request
        )

        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                RequestFiltrosArchivoAdministrador filters = new();
                Filters.MapFilters(request, filters);


                if (!Filters.MapSort<EnumOrderColumnArchivoByFiltrosAdministrador>(request, null!, false, out string orderByColumn, out bool orderDesc))
                {
                    return Ok(ResultOperation.FailureWarningResponse<List<RequestFiltrosArchivoAdministrador>>("La columna de ordenamiento no es válida.")
                        );
                }

                var result = await _consultasAdministradorService.GetArchivoByFiltersService(
                     request.fetch,
                    request.page,
                    orderByColumn,
                    orderDesc,
                    Filters.GetIntValue(filters!.ByIdRol.FirstOrDefault()),
                    Filters.GetStringValue(filters!.ByFolio.FirstOrDefault()),
                    Filters.GetIntValue(filters!.ByIdSeccion.FirstOrDefault()),
                    Filters.GetIntValue(filters!.ByIdConsulta.FirstOrDefault()),
                    Filters.GetIntValue(filters!.ByIdRemision.FirstOrDefault()),
                    Filters.GetBoolValue(filters.ByEstatus.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaCreacionDesde.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaCreacionHasta.FirstOrDefault()),
                    Filters.GetBoolValue(filters.ByRemplazable.FirstOrDefault()),
                    Filters.GetBoolValue(filters.ByPermanente.FirstOrDefault()),
                    sessionInformation.UserInformation,
                    Filters.GetIntValue(filters!.ByIdDocumentoSeccion.FirstOrDefault())
                    );

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        #region Solicitud-Transparencia
        /// <summary>
        /// Crea Solicitud Transparencia : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("solicitud-transparencia")]
        public async Task<IActionResult> PostSolicitudTransparencia(
         [FromForm] RequestCreateSolicitudTransparencia request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestCreateSolicitudTransparencia.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }

                DataFile dataFile = null!;
                if (request.documento is not null)
                {
                    _fileSystemService.FileTryOut(
                        request.documento,
                        Path.Combine("CONSULTAS", "solicitud-transparencia", request.idConsulta.ToString()),
                        out dataFile);
                }

                ArchivoConsulta entityDocumento = null!;
                SolicitudTransparencia entity = null!;
                try
                {

                    entity = EventsConsultasAdministrador.CreateModalidadSolicitudTransparencia(
                       request.idConsulta,
                       request.noSolicitud,
                       DateTime.Parse(request.fechaSolicitud!)
                   );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAdministrador.CreateDocumento(
                            request.idConsulta!,
                            request.idTipoArchivo.GetValueOrDefault(),
                            request.idSeccion.GetValueOrDefault(),
                            sessionInformation.UserInformation.IdAdministracionCentral.GetValueOrDefault(),
                            dataFile.File.FileName,
                            dataFile.FilePath,
                            dataFile.File.ContentType,
                            sessionInformation.UserInformation.Rfc!,
                            sessionInformation.UserInformation.Rfc!,
                            _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length),
                            request.noFolio
                        );
                    }

                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }


                var result = await _consultasAdministradorService.AddSolicitudTransparenciaService(entity, entityDocumento, dataFile!);
                if (result.Success == false)
                {
                    return Ok(result.Messages);
                }

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Edita solicitud transparencia : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("solicitud-transparencia")]
        public async Task<IActionResult> PatchSolicitudTransparencia(
          [FromForm] RequestUpdateSolicitudTransparencia request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestUpdateSolicitudTransparencia.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByIdSolicitudTransparenciaService(request.id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }

                try
                {
                    EventsConsultasAdministrador.UpdateSolicitudTransparencia(ref entityExists,
                    request.id,
                    request.idConsulta,
                    request.noSolicitud,
                    DateTime.Parse(request.fechaSolicitud!)
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorService.UpdateSolicitudTransparenciaService(entityExists);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }


        /// <summary>
        /// Solicitud Transparencia  por Id  : Administrador
        /// </summary>
        /// <param name="id">Id IdSolicitudTransparencia</param>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseSolicitudTransparenciaList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("solicitud-transparencia/{id}")]
        public async Task<IActionResult> GetByIdSolicitudTransparencia(int id)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var result = await _consultasAdministradorService.GetByIdSolicitudTransparenciaServices(id);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Tabla de Solicitud Transparencia : Administrador
        /// <param name="idConsulta">Id Id Consulta</param>
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseSolicitudTransparenciaList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("list-solicitud-transparencia/{idConsulta}")]
        public async Task<IActionResult> GetTablaSolicitudTransparencia(int idConsulta)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var result = await _consultasAdministradorService.GetTablaSolicitudTransparenciaService(idConsulta);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Elimina solicitud Transparencia : Oficial Partes
        /// </summary>
        /// <param name="idConsulta"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpDelete("solicitud-transparencia")]
        public async Task<IActionResult> DeleteSolicitudTransparencia(
           int idConsulta,
           int id
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{idConsulta}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var entityExists = await _consultasAdministradorService.GetByIdSolicitudTransparenciaService(id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "El registro no existe."
                        )
                    );
                }

                try
                {
                    EventsConsultasAdministrador.DeleteSolicitudTransparencia(ref entityExists,
                    id
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.DeleteSolicitudTransparenciaService(entityExists);
                return Ok(result);

            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        #endregion

        #region Cumplimentacion

        /// <summary>
        /// Edita cumplimentacion (Comercio Exterior / Impuestos Internos) : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("cumplimentacion")]
        public async Task<IActionResult> PatchCumplimentacion(
          [FromForm] RequestUpdateCumplimentacion request
        )
        {


            try
            {

                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.id}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorUpdateCumplimentacion.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByAllServiceCumplimentacion(request.id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }

                try
                {
                    EventsConsultasAdministrador.UpdateCumplimentacion(ref entityExists,
                        request.noJuicio!,
                        DateTime.Parse(request.fechaRecepcion!),
                        string.IsNullOrEmpty(request.fechaFirmeza)
                        ? (DateTime?)null
                        : DateTime.Parse(request.fechaFirmeza),
                        string.IsNullOrEmpty(request.fechaVencimiento)
                        ? (DateTime?)null
                        : DateTime.Parse(request.fechaVencimiento),
                        request.idOrganoJurisdiccional,
                        request.idAdministracion,
                        request.idAdministracionSolicita,
                        request.plazoCumplimentar,
                        request.idSubadministracion
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorService.UpdateCumplimentacionService(entityExists, sessionInformation.UserInformation);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Asignar abogado a cumplimentacion : Administrador 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("asignar-cumplimentacion")]
        public async Task<IActionResult> PatchCumplimentacionAdministradorAsignar(
          [FromForm] RequestAsignarCumplimentacionAdministrador request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idCumplimentacion}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }


                var validationResult = await _validatorAsignarCumplimentacionAdministrador.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }

                var entityExists = await _consultasAdministradorService.GetByAllServiceCumplimentacion(request.idCumplimentacion);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }


                try
                {
                    EventsConsultasAdministrador.AsignarCumplimentacionAdministrador(ref entityExists,
                            request.idCumplimentacion,
                            request.idAbogado,
                            request.idAdministracion,
                            request.idSubadministracion
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorService.AsignarConsultaCumplimentacionService(entityExists);
                return Ok(result);

            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }


        }

        /// <summary>
        /// Guardar Resolucion : Abogado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("guardar-resolucion-cumplimentacion")]
        public async Task<IActionResult> PostResolucionCumplimentacion(
         [FromForm] RequestCreateResolucionCumplimentacion request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idCumplimentacion}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestCreateResolucionCumplimentacion.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }

                DataFile dataFile = null!;
                if (request.documento is not null)
                {
                    _fileSystemService.FileTryOut(
                        request.documento,
                        Path.Combine("CONSULTAS", "RESOLUCION", request.idCumplimentacion.ToString()),
                        out dataFile);
                }

                ArchivoConsulta entityDocumento = null!;
                ResolucionCumplimentacion entity = null!;
                try
                {

                    entity = EventsConsultasAdministrador.CreateResolucionCumplimentacion(
                    request.idCumplimentacion,
                    request.noOficioResolucion,
                    DateTime.Parse(request.fechaResolucion!),
                    request.idSentido
                );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAdministrador.CreateDocumento(
                            request.idCumplimentacion!,
                            request.idTipoArchivo.GetValueOrDefault(),
                            request.idSeccion.GetValueOrDefault(),
                            sessionInformation.UserInformation.IdAdministracionCentral.GetValueOrDefault(),
                            dataFile.File.FileName,
                            dataFile.FilePath,
                            dataFile.File.ContentType,
                            sessionInformation.UserInformation.Rfc,
                            sessionInformation.UserInformation.Rfc,
                            _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length),
                            request.noFolio
                        );
                    }

                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.AddResolucionCumplimentacionService(entity, entityDocumento, dataFile!);
                if (result.Success == false)
                {
                    return Ok(result.Messages);
                }

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Edita resolucion : Abogado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("resolucion-cumplimentacion")]
        public async Task<IActionResult> PatchResolucionCumplimentacion(
          [FromForm] RequestUpdateResolucionCumplimentacion request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idCumplimentacion}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestUpdateResolucionCumplimentacion.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByAllServiceResolucionCumplimentacion(request.idCumplimentacion);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }





                try
                {

                    EventsConsultasAdministrador.UpdateResolucionCumplimentacion(ref entityExists,
                    request.idCumplimentacion,
                    request.noOficioResolucion,
                    DateTime.Parse(request.fechaResolucion!),
                    DateTime.Parse(request.fechaNotificacion!),
                    request.idSentido
                    );



                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.UpdateResolucionCumplimentacion(entityExists);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// concluir resolucion : Administrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("concluir-resolucion-cumplimentacion")]
        public async Task<IActionResult> concluirResolucionCumplimentacion(
          [FromForm] RequestConcluirResolucionCumplimentacion request
        )
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }



                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idCumplimentacion}");
                if (keyExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse<int>("El registro no se puede editar ya que el usuario no lo ha apartado.")
                    );
                }

                if (!keyExists.rfc.Equals(sessionInformation.UserInformation.Rfc))
                {
                    return Ok(
                        ResultOperation.FailureInformationResponse(
                            "El registro ya se encuentra en uso por otro usuario."
                        )
                    );
                }

                var validationResult = await _validatorRequestConcluirResolucionCumplimentacion.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAdministradorService.GetByAllServiceResolucionCumplimentacion(request.idCumplimentacion);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }


                DataFile dataFile = null!;
                if (request.documento is not null)
                {
                    _fileSystemService.FileTryOut(
                        request.documento,
                        Path.Combine("CONSULTAS", "CLONCLUIR RESOLUCION", request.idCumplimentacion.ToString()),
                        out dataFile);
                }

                ArchivoConsulta entityDocumento = null!;

                try
                {

                    EventsConsultasAdministrador.concluirResolucionCumplimentacion(ref entityExists,
                    request.idCumplimentacion
                    );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAdministrador.CreateDocumento(
                            request.idCumplimentacion!,
                            request.idTipoArchivo.GetValueOrDefault(),
                            request.idSeccion.GetValueOrDefault(),
                            sessionInformation.UserInformation.IdAdministracionCentral.GetValueOrDefault(),
                            dataFile.File.FileName,
                            dataFile.FilePath,
                            dataFile.File.ContentType,
                            sessionInformation.UserInformation.Rfc,
                            sessionInformation.UserInformation.Rfc,
                            _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length),
                            request.noFolio
                        );
                    }

                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAdministradorService.ConcluirResolucionCumplimentacion(entityExists, entityDocumento, dataFile!);
                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }



        /// <summary>
        /// Tabla de Resoluciones : Administrador
        ///<param name="idConsulta">Id Consulta</param>
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseResolucion>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("list-resolucion/{idConsulta}")]
        public async Task<IActionResult> GetResolucion(int idConsulta)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var result = await _consultasAdministradorService.GetTablaResolucionService(idConsulta);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }


        #endregion

        #region Medios de Defensa
        /// <summary>
        /// Tabla de Medios de defensa : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseMediosDefensa>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("medios-defensa")]
        public async Task<IActionResult> Medios_Defensa([FromBody] RequestMediosDefensa request)
        {
            try
            {
                List<String> noAsunto = request.dato;
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var result = await _consultasAdministradorService.GetTablaMediosDefensa(noAsunto);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        /// <summary>
        /// Tabla de Medios de defensa : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseMediosDefensa>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("medios-defensa-general")]
        public async Task<IActionResult> Medios_Defensa_Proxy(List<string> noAsunto)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var result = await _consultasAdministradorService.GetTablaMediosDefensaGeneral(noAsunto);

                return Ok(result);
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        #endregion

        #region 
        /// <summary>
        /// Exportar : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("exportar")]
        public async Task<IActionResult> Exportar_Excel([FromQuery] QueryFilters request)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }

                RequestFiltrosReporteConsultas filters = new();
                Filters.MapFiltersQuery(request, filters);


                if ( filters!.ByIdTipoAsunto.Contains(EnumTipoAsunto.COMERCIO_EXTERIOR.GetHashCode().ToString()) || filters!.ByIdTipoAsunto.Contains(EnumTipoAsunto.IMPUESTOS_INTERNOS.GetHashCode().ToString()))
                {


                    var datos = await _consultasAdministradorService.ExportarConsulta(
                        Filters.GetStringValue(filters!.ByNoAsunto.FirstOrDefault()),
                        Filters.GetStringValue(filters!.ByRfcPromovente.FirstOrDefault()),
                        Filters.GetStringValue(filters!.ByPromovente.FirstOrDefault()),
                        Filters.GetIntValue(filters!.ByIdAdministracion.FirstOrDefault()),
                        Filters.GetIntValue(filters!.ByIdSubadministracion.FirstOrDefault()),
                        Filters.GetStringValue(filters.ByIdAbogadoAsigno.FirstOrDefault()),
                        filters.ByIdTipoEntrada.Adapt<List<int>>(),
                        filters.ByIdTipoAsunto.Adapt<List<int>>(),
                        filters.ByIdEstadoProcesal.Adapt<List<int>>(),
                        Filters.GetIntValue(filters.ByIdTema.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaPresentacionDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaPresentacionHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaVencimientoDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaVencimientoHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaConclucionDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaConclucionHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaOficioNotificacionResolucionDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaOficioNotificacionResolucionHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaIngresoProdeconDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaIngresoProdeconHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaSolicituTransparenciaDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaSolicituTransparenciaHasta.FirstOrDefault())


                        );

                    if (datos is null)
                    {
                        return Ok(
                            ResultOperation.FailureErrorResponse(
                                "No se encontraron resultados"
                            )
                        );
                    }


                    using var workbook = EventsConsultasAdministrador.GenerarExcelClosedXmlConsulta(datos);
                    using var stream = new MemoryStream();
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteConsultas.xlsx");
                }
                else if (filters!.ByIdTipoAsunto.Contains(EnumTipoAsunto.Cumplimentacion.GetHashCode().ToString()))
                {

                    var datos = await _consultasAdministradorService.ExportarCumplimentacion(
                        Filters.GetStringValue(filters!.ByNoAsunto.FirstOrDefault()),
                        Filters.GetStringValue(filters!.ByRfcPromovente.FirstOrDefault()),
                        Filters.GetStringValue(filters!.ByPromovente.FirstOrDefault()),
                        Filters.GetIntValue(filters!.ByIdAdministracion.FirstOrDefault()),
                        Filters.GetIntValue(filters!.ByIdSubadministracion.FirstOrDefault()),
                        Filters.GetStringValue(filters.ByIdAbogadoAsigno.FirstOrDefault()),
                        filters.ByIdTipoEntrada.Adapt<List<int>>(),
                        filters.ByIdTipoAsunto.Adapt<List<int>>(),
                        filters.ByIdEstadoProcesal.Adapt<List<int>>(),
                        Filters.GetIntValue(filters.ByIdTema.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaPresentacionDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaPresentacionHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaVencimientoDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaVencimientoHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaConclucionDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaConclucionHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaOficioNotificacionResolucionDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaOficioNotificacionResolucionHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaIngresoProdeconDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaIngresoProdeconHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaSolicituTransparenciaDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaSolicituTransparenciaHasta.FirstOrDefault())


                        );
                    if (datos is null)
                    {
                        return Ok(
                            ResultOperation.FailureErrorResponse(
                                "No se encontraron resultados"
                            )
                        );
                    }

                    using var workbook = EventsConsultasAdministrador.GenerarExcelClosedXmlCumplimentacion(datos);
                    using var stream = new MemoryStream();
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteCumplimentacion.xlsx");

                }
                else
                { 

                    var datos = await _consultasAdministradorService.ExportarReporteGeneral(
                        Filters.GetStringValue(filters!.ByNoAsunto.FirstOrDefault()),
                        Filters.GetStringValue(filters!.ByRfcPromovente.FirstOrDefault()),
                        Filters.GetStringValue(filters!.ByPromovente.FirstOrDefault()),
                        Filters.GetIntValue(filters!.ByIdAdministracion.FirstOrDefault()),
                        Filters.GetIntValue(filters!.ByIdSubadministracion.FirstOrDefault()),
                        Filters.GetStringValue(filters.ByIdAbogadoAsigno.FirstOrDefault()),
                        filters.ByIdTipoEntrada.Adapt<List<int>>(),
                        filters.ByIdTipoAsunto.Adapt<List<int>>(),
                        filters.ByIdEstadoProcesal.Adapt<List<int>>(),
                        Filters.GetIntValue(filters.ByIdTema.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaPresentacionDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaPresentacionHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaVencimientoDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaVencimientoHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaConclucionDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaConclucionHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaOficioNotificacionResolucionDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaOficioNotificacionResolucionHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaIngresoProdeconDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaIngresoProdeconHasta.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaSolicituTransparenciaDesde.FirstOrDefault()),
                        Filters.GetDateTimeValue(filters.ByFechaSolicituTransparenciaHasta.FirstOrDefault())


                        );
                    if (datos is null)
                    {
                        return Ok(
                            ResultOperation.FailureErrorResponse(
                                "No se encontraron resultados"
                            )
                        );
                    }

                    using var workbook = EventsConsultasAdministrador.GenerarExcelClosedXmlGeneral(datos);
                    using var stream = new MemoryStream();
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteGlobal.xlsx");

                }                    

            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        #endregion

        #region Exporta PDF        

        /// <summary>
        /// Generar PDF : Administrador
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("generar-pdf")]

        public async Task<IActionResult> Generar_PDF_Resolucion(string noAsunto)
        {
            try
            {
                var sessionInformation = UserSession.GetValue(HttpContext);
                if (sessionInformation is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "No se pudo obtener la información del usuario."
                        )
                    );
                }


                var datos = await _consultasAdministradorService.Exporta_PDF(noAsunto);



                var pdfBytes = EventsConsultasAdministrador.GenerarPDF(datos);

                return File(pdfBytes, "application/pdf", "AcuseConclusion.pdf");
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ha ocurrrido un error.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        _e.ManageException()
                    )
                );
            }
        }

        #endregion
    }

}