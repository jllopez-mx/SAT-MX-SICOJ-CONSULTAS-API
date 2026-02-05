using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.Entities;
using ConsultasAPI.Model.IDAO.IServiceDAO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sicoj.Utils.Models;
using Sicoj.Utils.Redis;
using ConsultasAPI.Model.IDAO.IRepository;
using ConsultasAPI.Model.Entities.Events.Abogado;
using Sicoj.Utils.Middleware;
using Sicoj.Utils.Enums;
using ConsultasAPI.Model.ViewModels.Enums;
using Sicoj.Utils.Files;
using Mapster;
using ConsultasAPI.Model.DTO.Contracts.Abogado;
using ConsultasAPI.Model.DTO.Contracts.Administrador;
using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.DTO.Contracts.OficialPartes;
using Sicoj.Utils.Extentions;


namespace ConsultasAPI.Api.V1.Controllers
{
    [Route("sicoj/consultas/api/v1/abogado/consultas")]
    public class AbogadoController : ControllerBase
    {

        private readonly ILogger<AdministradorController> _logger;
        private readonly IRedisClient _redisClient;
        private readonly IConsultasAbogadoService _consultasAbogadoService;
        private readonly IConsultaRepositoryAbogado _consultaRepositoryAbogado;
        private readonly IValidator<RequestRemision> _validatorRequestRemision;
        private readonly IValidator<RequestCreatePersonasAutorizadas> _validatorRequestCreatePersonasAutorizadas;
        private readonly IValidator<RequestUpdatePersonasAutorizadas> _validatorUpdatePersonasAutorizadas;
        private readonly IValidator<RequestDeleteArchivosConsulta> _validatorDeleteArchivos;
        private readonly IValidator<RequestUpdateConsultaAbogadoImpuestosInternos> _validatorRequesUpdateConsultaAbogadoImpuestosInternos;
        private readonly IValidator<RequestUpdateConsultaAbogadoComercioExterior> _validatorRequestUpdateConsultaAbogadoComercioExterior;
        private readonly IValidator<RequestCreateSolicitudInformacion> _validatorRequestCreateSolicitudInformacion;
        private readonly IValidator<RequestUpdateSolicitudInformacion> _validatorRequestUpdateSolicitudInformacion;
        private readonly IValidator<RequestCreateRequerimiento> _validatorRequestCreateRequerimiento;
        private readonly IValidator<RequestSolicitaRequerimiento> _validatorRequestSolicitaRequerimiento;
        private readonly IValidator<RequestUpdateRequerimiento> _validatorRequestUpdateRequerimiento;
        private readonly IValidator<RequestCreateResolucion> _validatorRequestCreateResolucion;
        private readonly IValidator<RequestCreateResolucionCumplimentacion> _validatorRequestCreateResolucionCumplimentacion;
        private readonly IValidator<RequestUpdateResolucion> _validatorRequestUpdateResolucion;
        private readonly IValidator<RequestUpdateResolucionCumplimentacion> _validatorRequestUpdateResolucionCumplimentacion;
        private readonly IValidator<RequestConcluirResolucion> _validatorRequestConcluirResolucion;
        private readonly IValidator<RequestConcluirResolucionCumplimentacion> _validatorRequestConcluirResolucionCumplimentacion;
        private readonly IValidator<RequestCreateRequerimientoProdecon> _validatorRequestCreateRequerimientoProdecon;
        private readonly IValidator<RequestUpdateRequerimientoProdecon> _validatorRequestUpdateRequerimientoProdecon;

        private readonly IValidator<RequestCreateAvisosYComunicados> _validatorRequestAvisosYComunicados;
        private readonly IValidator<RequestUpdateAvisosYComunicados> _validatorRequestUpdateAvisosYComunicados;

        private readonly IFileSystemService _fileSystemService;
        private readonly IValidator<RequestDocumentoUpdate> _requestDocumentoUpdateValidator;
        private static object _lock = new object();
        private readonly IValidator<RequestAsignarAbogado> _validatorAsignarAbogado;
        private readonly IValidator<RequestCreateSolicitudTransparencia> _validatorRequestCreateSolicitudTransparencia;
        private readonly IValidator<RequestUpdateSolicitudTransparencia> _validatorRequestUpdateSolicitudTransparencia;
        private readonly IValidator<RequestUpdateCumplimentacion> _validatorUpdateCumplimentacion;

        public AbogadoController(ILogger<AdministradorController> logger, IRedisClient redisClient, IConsultasAbogadoService consultasAbogadoService, IConsultaRepositoryAbogado consultaRepositoryAbogado, IValidator<RequestRemision> validatorRequestRemision, IValidator<RequestCreatePersonasAutorizadas> validatorPersonasAutorizadas, IValidator<RequestUpdatePersonasAutorizadas> validatorUpdatePersonasAutorizadas, IValidator<RequestDeleteArchivosConsulta> validatorDeleteArchivos, IValidator<RequestUpdateConsultaAbogadoImpuestosInternos> validatorRequesUpdateConsultaAbogadoImpuestosInternos, IValidator<RequestUpdateConsultaAbogadoComercioExterior> validatorRequestUpdateConsultaAbogadoComercioExterior, IValidator<RequestAsignarAbogado> validatorAsignarAbogado, IFileSystemService fileSystemService, IValidator<RequestCreateRequerimiento> validatorRequerimiento, IValidator<RequestUpdateRequerimiento> validatorUpdateRequerimiento, IValidator<RequestCreateResolucion> validatorResolucion, IValidator<RequestUpdateResolucion> validatorUpdateResolucion, IValidator<RequestCreateRequerimientoProdecon> validatorRequerimientoProdecon, IValidator<RequestUpdateRequerimientoProdecon> validatorUpdateRequerimientoProdecon, IValidator<RequestCreateSolicitudInformacion> validatorRequestCreateSolicitudInformacion, IValidator<RequestUpdateSolicitudInformacion> validatorRequestUpdateSolicitudInformacion, IValidator<RequestCreateAvisosYComunicados> validatorRequestAvisosYComunicados, IValidator<RequestUpdateAvisosYComunicados> validatorRequestUpdateAvisosYComunicados, IValidator<RequestCreateSolicitudTransparencia> validatorRequestCreateSolicitudTransparencia, IValidator<RequestUpdateSolicitudTransparencia> validatorUpdateSolicitudTransparencia, IValidator<RequestDocumentoUpdate> requestDocumentoUpdateValidator, IValidator<RequestSolicitaRequerimiento> validatorRequestSolicitaRequerimiento, IValidator<RequestConcluirResolucion> validatorRequestConcluirResolucion, IValidator<RequestCreateResolucionCumplimentacion> validatorResolucionCumplimentacion, IValidator<RequestUpdateResolucionCumplimentacion> validatorUpdateResolucionCumplimentacion, IValidator<RequestConcluirResolucionCumplimentacion> validatorRequestConcluirResolucionCumplimentacion, IValidator<RequestUpdateCumplimentacion> validatorUpdateCumplimentacion)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(redisClient));
            _consultasAbogadoService = consultasAbogadoService ?? throw new ArgumentNullException(nameof(consultasAbogadoService));
            _consultaRepositoryAbogado = consultaRepositoryAbogado ?? throw new ArgumentNullException(nameof(consultaRepositoryAbogado));
            _validatorRequestRemision = validatorRequestRemision ?? throw new ArgumentNullException(nameof(validatorRequestRemision));
            _validatorRequestCreatePersonasAutorizadas = validatorPersonasAutorizadas ?? throw new ArgumentNullException(nameof(validatorPersonasAutorizadas));
            _validatorUpdatePersonasAutorizadas = validatorUpdatePersonasAutorizadas ?? throw new ArgumentNullException(nameof(validatorUpdatePersonasAutorizadas));
            _validatorDeleteArchivos = validatorDeleteArchivos ?? throw new ArgumentNullException(nameof(validatorDeleteArchivos));
            _validatorRequesUpdateConsultaAbogadoImpuestosInternos = validatorRequesUpdateConsultaAbogadoImpuestosInternos ?? throw new ArgumentNullException(nameof(validatorRequesUpdateConsultaAbogadoImpuestosInternos));
            _validatorRequestUpdateConsultaAbogadoComercioExterior = validatorRequestUpdateConsultaAbogadoComercioExterior ?? throw new ArgumentNullException(nameof(_validatorRequestUpdateConsultaAbogadoComercioExterior));
            _validatorAsignarAbogado = validatorAsignarAbogado ?? throw new ArgumentNullException(nameof(_validatorAsignarAbogado));
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _validatorRequestCreateRequerimiento = validatorRequerimiento ?? throw new ArgumentNullException(nameof(validatorRequerimiento));
            _validatorRequestUpdateRequerimiento = validatorUpdateRequerimiento ?? throw new ArgumentNullException(nameof(validatorUpdateRequerimiento));
            _validatorRequestCreateResolucion = validatorResolucion ?? throw new ArgumentNullException(nameof(validatorResolucion));
            _validatorRequestUpdateResolucion = validatorUpdateResolucion ?? throw new ArgumentNullException(nameof(validatorUpdateResolucion));
            _validatorRequestCreateRequerimientoProdecon = validatorRequerimientoProdecon ?? throw new ArgumentNullException(nameof(validatorRequerimientoProdecon));
            _validatorRequestUpdateRequerimientoProdecon = validatorUpdateRequerimientoProdecon ?? throw new ArgumentNullException(nameof(validatorUpdateRequerimientoProdecon));
            _validatorRequestCreateSolicitudInformacion = validatorRequestCreateSolicitudInformacion ?? throw new ArgumentNullException(nameof(validatorRequestCreateSolicitudInformacion));
            _validatorRequestUpdateSolicitudInformacion = validatorRequestUpdateSolicitudInformacion ?? throw new ArgumentNullException(nameof(validatorRequestUpdateSolicitudInformacion));
            _validatorRequestAvisosYComunicados = validatorRequestAvisosYComunicados ?? throw new ArgumentNullException(nameof(validatorRequestAvisosYComunicados));
            _validatorRequestUpdateAvisosYComunicados = validatorRequestUpdateAvisosYComunicados ?? throw new ArgumentNullException(nameof(validatorRequestUpdateAvisosYComunicados));
            _validatorRequestCreateSolicitudTransparencia = validatorRequestCreateSolicitudTransparencia ?? throw new ArgumentNullException(nameof(validatorRequestCreateSolicitudTransparencia));
            _validatorRequestUpdateSolicitudTransparencia = validatorUpdateSolicitudTransparencia ?? throw new ArgumentNullException(nameof(validatorUpdateSolicitudTransparencia));
            _requestDocumentoUpdateValidator = requestDocumentoUpdateValidator ?? throw new ArgumentNullException(nameof(requestDocumentoUpdateValidator));
            _validatorRequestSolicitaRequerimiento = validatorRequestSolicitaRequerimiento ?? throw new ArgumentNullException(nameof(validatorRequestSolicitaRequerimiento));
            _validatorRequestConcluirResolucion = validatorRequestConcluirResolucion ?? throw new ArgumentNullException(nameof(validatorRequestConcluirResolucion));
            _validatorRequestCreateResolucionCumplimentacion = validatorResolucionCumplimentacion ?? throw new ArgumentNullException(nameof(validatorResolucionCumplimentacion));
            _validatorRequestUpdateResolucionCumplimentacion = validatorUpdateResolucionCumplimentacion ?? throw new ArgumentNullException(nameof(validatorUpdateResolucionCumplimentacion));
            _validatorRequestConcluirResolucionCumplimentacion = validatorRequestConcluirResolucionCumplimentacion ?? throw new ArgumentNullException(nameof(validatorRequestConcluirResolucionCumplimentacion));
            _validatorUpdateCumplimentacion = validatorUpdateCumplimentacion ?? throw new ArgumentNullException(nameof(validatorUpdateCumplimentacion));
        }

        /// <summary>
        /// Bandeja de Historico : Abogado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<ResponseConsultaByFilters>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("bandeja-historico")]
        public async Task<IActionResult> BandejaHistoricoByFilters(
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


                RequestFiltrosConsulta filters = new();
                Filters.MapFilters(request, filters);


                if (!Filters.MapSort<EnumOrderColumnConsultasByFiltros>(request, null!, false, out string orderByColumn, out bool orderDesc))
                {
                    return Ok(ResultOperation.FailureWarningResponse<List<RequestFiltrosConsulta>>("La columna de ordenamiento no es válida.")
                        );
                }

                var result = await _consultasAbogadoService.GetBandejaHistoricoByFiltersService(
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
                    filters.ByIdEstadoTarea.Adapt<List<int>>(),
                    filters.ByIdTipoModalidad.Adapt<List<int>>(),
                    filters.ByIdEstadoProcesal.Adapt<List<int>>(),
                    filters.ByAlerta.Adapt<List<int>>(),
                    sessionInformation.UserInformation
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
        /// Bandeja de Pendientes : Abogado
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseConsultaByFiltersAbogado>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("bandeja-pendientes")]
        public async Task<IActionResult> GetBandejapendientes([FromQuery] PagerQueryFilters request)
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

                RequestFiltrosConsultaAbogado filters = new();
                Filters.MapFilters(request, filters);


                if (!Filters.MapSort<EnumOrderColumnConsultasByFiltrosAbogado>(request, null!, false, out string orderByColumn, out bool orderDesc))
                {
                    return Ok(ResultOperation.FailureWarningResponse<List<ResponseConsultaByFiltersAbogado>>("La columna de ordenamiento no es válida.")
                        );
                }

                var result = await _consultasAbogadoService.GetBandejaPendientes_AbogadoService(
                     request.fetch,
                    request.page,
                    orderByColumn,
                    orderDesc,
                    Filters.GetStringValue(filters!.ByNoAsunto.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaPresentacionDesde.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaPresentacionHasta.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaVencimientoDesde.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaVencimientonHasta.FirstOrDefault()),
                    Filters.GetStringValue(filters.ByRfc.FirstOrDefault()),
                    Filters.GetStringValue(filters.ByPromovente.FirstOrDefault()),
                    filters.ByIdTipoAsunto.Adapt<List<int>>(),
                    filters.ByIdEstadoTarea.Adapt<List<int>>(),
                    filters.ByIdTipoModalidad.Adapt<List<int>>(),
                    filters.ByIdEstadoProcesal.Adapt<List<int>>(),
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
        /// Crea personas autorizadas : Abogado
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
                    entity = EventsConsultasAbogado.CreatePersonasAutorizadas(
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

                var result = await _consultasAbogadoService.AddPersonasAutorizadas(entity);
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
        /// Edita personas autorizadas : Abogado
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
                var entityExists = await _consultasAbogadoService.GetByAllServicePersonaAutorizadas(request.id);
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
                    EventsConsultasAbogado.UpdatePersonasAutorizadas(ref entityExists,
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
                var result = await _consultasAbogadoService.UpdatePersonasAutorizadas(entityExists);
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
        /// Elimina personas autorizadas : Abogado
        /// </summary>
        /// <param name="idConsulta"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpDelete("personas-autorizadas")]
        public async Task<IActionResult> DeletePersonasAutorizadas(
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

                var entityExists = await _consultasAbogadoService.GetByAllServicePersonaAutorizadas(id);
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
                    EventsConsultasAbogado.DeletePersonasAutorizadas(ref entityExists,
                    id
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAbogadoService.DeletePersonasAutorizadas(entityExists);
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
        /// Tabla de personas autorizadas : Abogado
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


                var result = await _consultasAbogadoService.GetTablaPersonasAutorizadasService(
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
        /// Carga  documento a la consulta : Abogado 
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




                var entityExists = await _consultasAbogadoService.GetByAllService(request.idConsulta);
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
                      Path.Combine("CONSULTAS", "REMISION", request.idRemision.ToString()),
                      out var dataFile);

                ArchivoConsulta entity = null!;

                try
                {
                    entity = EventsArchivosConsultasAbogado.Create(
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
                var result = await _consultasAbogadoService.AddArchivosAsyncService(entity, dataFile);
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
        /// Método para actualizar un documento : Abogado
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

                var entityExists = await _consultasAbogadoService.GetByAllService(request.idConsulta);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
                        )
                    );
                }
                ArchivoConsulta entityDocumento = await _consultasAbogadoService.GetIdArchivoConsultaService(request.id);



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
                        EventsArchivosConsultasAbogado.UpdateWithFile(
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
                        EventsArchivosConsultasAbogado.Update(
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

                ResultOperation<int> result = await _consultasAbogadoService.UpdateArchivoService(entityDocumento!, dataFile!);
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
        /// Borra documento de la consulta : Abogado
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
                    var entityExists = await _consultasAbogadoService.GetByIdArchivoDeleteService(item);
                    if (entityExists is not null)
                    {
                        try
                        {
                            EventsConsultasAbogado.DeleteModalidaArchivoConsulta(ref entityExists
                            );
                        }
                        catch (Exception _ex)
                        {
                            return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                        }
                        var result = await _consultasAbogadoService.DeleteArchivoConsultaService(entityExists);
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
        /// Lista los documentos asociados a la consulta : Abogado
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



                var result = await _consultasAbogadoService.GetAllArchivoService(id);
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
        /// Visualiza documento de la consulta : Abogado
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



                var entityExists = await _consultasAbogadoService.GetByIdArchivoService(id);
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
                var contentType = EventsArchivosConsultasAbogado.GetArchivoExt(fileName);

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
        /// Edita consultas  para impuestos internos : Abogado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("impuestos-internos")]
        public async Task<IActionResult> PatchConsultasAbogadoImpustosInternos(
          [FromForm] RequestUpdateConsultaAbogadoImpuestosInternos request
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
                    var validationResult = await _validatorRequesUpdateConsultaAbogadoImpuestosInternos.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                    }
                    var entityExists = await _consultasAbogadoService.GetByIdServiceAbogado(request.id);
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
                        EventsConsultasAbogado.UpdateModalidaConsultaAbogadoImpuestosInternos(ref entityExists,

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
                                request.idSubadministracion
                        );
                    }
                    catch (Exception _ex)
                    {
                        return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                    }
                    var result = await _consultasAbogadoService.UpdateConsultaServiceAbogadoImpuestosInternos(entityExists);
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
        /// Edita consultas  para comercio exterior : Abogado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("comercio-exterior")]
        public async Task<IActionResult> PatchConsultasAbogadoComercioExterior(
          [FromForm] RequestUpdateConsultaAbogadoComercioExterior request
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
                    var validationResult = await _validatorRequestUpdateConsultaAbogadoComercioExterior.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                    }
                    Consulta entityExists = await _consultasAbogadoService.GetByIdServiceAbogado(request.id);
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
                        EventsConsultasAbogado.UpdateModalidaConsultaAbogadoComercioExterior(ref entityExists,

                                request.promoventeEsContribuyente,
                                request.rfcContribuyente,
                                request.contribuyente,
                                request.fechaPresentacion!,
                                request.fechaRecepcion!,
                                sessionInformation.UserInformation.Rfc,
                                request.idTipoAsunto,
                                request.idTipoModalidad,
                                request.idSubadministracion
                        );
                    }
                    catch (Exception _ex)
                    {
                        return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                    }
                    var result = await _consultasAbogadoService.UpdateConsultaServiceAbogadoComercioExterior(entityExists);
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
        /// Asignar abogado a consulta : Abogado 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("asignar")]
        public async Task<IActionResult> PatchConsultasAdministradorAsignar(
          [FromForm] RequestAsignarAbogado request
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



                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.id_Consulta}");
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


                var validationResult = await _validatorAsignarAbogado.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasAbogadoService.GetByIdServiceAbogado(request.id_Consulta);
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
                    EventsConsultasAbogado.AsignarAbogado(ref entityExists,
                            request.id_Consulta,
                            request.idAbogado,
                            sessionInformation.UserInformation.Rfc
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAbogadoService.AsignarConsultaService(entityExists);
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
        /// Bloquea el registro por Id : Abogado
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

                var entityExists = await _consultasAbogadoService.GetByIdAbogadoService(id);
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
        ///Libera el registro por Id : Abogado
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

                var entityExists = await _consultasAbogadoService.GetByIdAbogadoService(id);
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
        /// Consultas  por Id  : Abogado
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


                var result = await _consultasAbogadoService.GetByIdDisconnected(id);
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
        /// Activa requerimeinto: Abogado
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

                    entity = EventsConsultasAbogado.SolicitaRequerimiento(
                        request.idConsulta,
                        request.solicitaRequerimiento
                    );

                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAbogadoService.SolicitaRequerimientoService(entity);
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
        /// Crea Requerimiento : Abogado
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
                    entity = EventsConsultasAbogado.CreateRequerimiento(
                        request.idConsulta,
                        request.noOficioRequerimiento,
                        DateTime.Parse(request.fechaRequerimiento!)
                    );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAbogado.CreateDocumento(
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

                var result = await _consultasAbogadoService.AddRequerimientoService(entity, entityDocumento, dataFile!);
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
        /// Edita requerimientos : Abogado
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
                var entityExists = await _consultasAbogadoService.GetByAllServiceRequerimientos(request.id);
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

                    EventsConsultasAbogado.UpdateRequerimientos(ref entityExists,
                    request.idConsulta,
                    request.atendio,
                    DateTime.Parse(request.fechaNotificacion!),
                    fechaAtencionParsed,
                    request.noOficioRequerimiento,
                DateTime.Parse(request.fechaRequerimiento!)
                );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAbogado.CreateDocumento(
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
                var result = await _consultasAbogadoService.UpdateRequerimientos(entityExists, entityDocumento, dataFile!);
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
        /// Requerimiento  por Id  : Abogado
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

                var result = await _consultasAbogadoService.GetRequerimientoById(id);
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
        /// Tabla de Requerimientos : Abogado
        /// <param name="idConsulta">Id Requerimiento</param>
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


                var result = await _consultasAbogadoService.GetTablaRequerimientosService(idConsulta);

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

                    entity = EventsConsultasAbogado.CreateResolucion(
                    request.idConsulta,
                    request.noOficioResolucion,
                    DateTime.Parse(request.fechaResolucion!),
                    request.idSentido
                );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAbogado.CreateDocumento(
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

                var result = await _consultasAbogadoService.AddResolucionService(entity, entityDocumento, dataFile!);
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
                var entityExists = await _consultasAbogadoService.GetByAllServiceResolucion(request.idConsulta);
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

                    EventsConsultasAbogado.UpdateResolucion(ref entityExists,
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

                var result = await _consultasAbogadoService.UpdateResolucion(entityExists);
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
                var entityExists = await _consultasAbogadoService.GetByAllServiceResolucion(request.idConsulta);
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

                    EventsConsultasAbogado.ConcluirResolucion(ref entityExists,
                    request.idConsulta
                    );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAbogado.CreateDocumento(
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

                var result = await _consultasAbogadoService.ConcluirResolucion(entityExists, entityDocumento, dataFile!);
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
        /// Resolucion  por Id  : Abogado
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

                var result = await _consultasAbogadoService.GetResolucionById(id);
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
        /// Crea Requerimiento Prodecon : Abogado
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

                    entity = EventsConsultasAbogado.CreateRequerimientoProdecon(
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
                        entityDocumento = EventsArchivosConsultasAbogado.CreateDocumento(
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

                var result = await _consultasAbogadoService.AddRequerimientoProdeconService(entity, entityDocumento, dataFile!);
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
        /// Edita requerimientos prodecon : Abogado
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
                var entityExists = await _consultasAbogadoService.GetByAllServiceRequerimientosProdecon(request.id);
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
                    EventsConsultasAbogado.UpdateRequerimientosProdecon(ref entityExists,
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
                var result = await _consultasAbogadoService.UpdateRequerimientosProdecon(entityExists);
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
        /// Requerimiento Prodecon  por Id  : Abogado
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



                var result = await _consultasAbogadoService.GetRequerimientoProdeconById(id);
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
        /// Tabla de Requerimientos Prodecon : Abogado
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


                var result = await _consultasAbogadoService.GetTablaRequerimientosProdeconService(idConsulta);

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
        /// Crea solicitud de información : Abogado
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
                        entity = EventsConsultasAbogado.CreateSolicitudInformacion(
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
                        entity = EventsConsultasAbogado.CreateSolicitudInformacionSinAtencion(
                       request.idConsulta,
                       request.idUnidadAdministrativa,
                       request.noOficioSolicitud,
                       request.noOficioRespuesta,
                       request.atendioSolicitud,
                       request.unidadEsInterna,
                       request.unidadAdministrativaExterna,
                        DateTime.Parse(request.fechaOficioSolicitud!)
                   );
                    }
                }


                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAbogadoService.AddSolicitudInformacionService(entity);
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
        /// Edita solicitud de informacion : Abogado
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
                var entityExists = await _consultasAbogadoService.GetByAllServiceSolicitudInformacion(request.id);
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
                        EventsConsultasAbogado.RequestUpdateSolicitudInformacion(ref entityExists,
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
                        EventsConsultasAbogado.RequestUpdateSolicitudInformacionSinAtencion(ref entityExists,
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
                var result = await _consultasAbogadoService.UpdateSolicitudInformacion(entityExists);
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
        /// Tabla de Solicitud de Información : Abogado
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



                var result = await _consultasAbogadoService.GetTablaSolicitudInformacionService(idConsulta);

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
        /// Solicitud de Información  por Id  : Abogado
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


                var result = await _consultasAbogadoService.GetSolicitudInformacionById(id);
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
            RequestReasignar request
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
                    var result = await _consultasAbogadoService.ReAsignarComercioExteriorAsync(request.idList.ToArray(), request.idAbogado, sessionInformation.UserInformation);
                    return Ok(result);
                }
                else if (request.idTipoAsunto == EnumTipoAsunto.IMPUESTOS_INTERNOS.GetHashCode())
                {
                    var result = await _consultasAbogadoService.ReAsignarImpuestosInternosAsync(request.idList.ToArray(), request.idAbogado, sessionInformation.UserInformation);
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

        //Avisos y comunicados

        /// <summary>
        /// Crea Avisos Y Comunicados : Abogado
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
                    entity = EventsConsultasAbogado.CreateAvisosYComunicados(
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

                var result = await _consultasAbogadoService.AddAvisosYComunicadosService(entity);
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
        /// Edita avisos y comunicados : Abogado
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
                var entityExists = await _consultasAbogadoService.GetByServiceAvisosYComunicados(request.id);
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
                    EventsConsultasAbogado.UpdateAvisosYComunicados(ref entityExists,
                        request.idConsulta,
                        request.atencionAdicional,
                        request.descripcionAtencion
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAbogadoService.UpdateAvisosYComunicados(entityExists);
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
        /// Avisos Y comunicados por Id  : Abogado
        /// </summary>
        /// <param name="id">Id Requerimiento</param>
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



                var result = await _consultasAbogadoService.GetByServiceAvisosYComunicados(id);
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
        /// Tabla de Avisos Y Comunicados : Abogado
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

                var result = await _consultasAbogadoService.GetTablaAvisosYComunicados(idConsulta);

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
        /// Lista los documentos por filtros : Abogado
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

                var result = await _consultasAbogadoService.GetArchivoByFiltersService(
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
        /// Crea Solicitud Transparencia : Abogado
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

                    entity = EventsConsultasAbogado.CreateModalidadSolicitudTransparencia(
                       request.idConsulta,
                       request.noSolicitud,
                       DateTime.Parse(request.fechaSolicitud!)
                   );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAbogado.CreateDocumento(
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


                var result = await _consultasAbogadoService.AddSolicitudTransparenciaService(entity, entityDocumento, dataFile!);
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
        /// Edita solicitud transparencia : Abogado
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
                var entityExists = await _consultasAbogadoService.GetByIdSolicitudTransparenciaService(request.id);
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
                    EventsConsultasAbogado.UpdateSolicitudTransparencia(ref entityExists,
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
                var result = await _consultasAbogadoService.UpdateSolicitudTransparenciaService(entityExists);
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
        /// Solicitud Transparencia  por Id  : Abogado
        /// </summary>
        /// <param name="id">Id Requerimiento</param>
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

                var result = await _consultasAbogadoService.GetByIdSolicitudTransparenciaServices(id);
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
        /// Tabla de Solicitud Transparencia : Abogado
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


                var result = await _consultasAbogadoService.GetTablaSolicitudTransparenciaService(idConsulta);

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
        /// Elimina solicitud Transparencia : Abogado
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

                var entityExists = await _consultasAbogadoService.GetByIdSolicitudTransparenciaService(id);
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
                    EventsConsultasAbogado.DeleteSolicitudTransparencia(ref entityExists,
                    id
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasAbogadoService.DeleteSolicitudTransparenciaService(entityExists);
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

                    entity = EventsConsultasAbogado.CreateResolucionCumplimentacion(
                    request.idCumplimentacion,
                    request.noOficioResolucion,
                    DateTime.Parse(request.fechaResolucion!),
                    request.idSentido
                );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAbogado.CreateDocumento(
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

                var result = await _consultasAbogadoService.AddResolucionCumplimentacionService(entity, entityDocumento, dataFile!);
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
                var entityExists = await _consultasAbogadoService.GetByAllServiceResolucionCumplimentacion(request.idCumplimentacion);
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

                    EventsConsultasAbogado.UpdateResolucionCumplimentacion(ref entityExists,
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

                var result = await _consultasAbogadoService.UpdateResolucionCumplimentacion(entityExists);
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
                var entityExists = await _consultasAbogadoService.GetByAllServiceResolucionCumplimentacion(request.idCumplimentacion);
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

                    EventsConsultasAbogado.concluirResolucionCumplimentacion(ref entityExists,
                    request.idCumplimentacion
                    );

                    if (dataFile is not null)
                    {
                        entityDocumento = EventsArchivosConsultasAbogado.CreateDocumento(
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

                var result = await _consultasAbogadoService.ConcluirResolucionCumplimentacion(entityExists, entityDocumento, dataFile!);
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
        /// Tabla de Resoluciones : Abogado
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


                var result = await _consultasAbogadoService.GetTablaResolucionService(idConsulta);

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
                var entityExists = await _consultasAbogadoService.GetByAllServiceCumplimentacion(request.id);
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
                    EventsConsultasAbogado.UpdateCumplimentacion(ref entityExists,
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
                var result = await _consultasAbogadoService.UpdateCumplimentacionService(entityExists, sessionInformation.UserInformation);
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


                var result = await _consultasAbogadoService.GetTablaMediosDefensaGeneral(noAsunto);

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

    }
}