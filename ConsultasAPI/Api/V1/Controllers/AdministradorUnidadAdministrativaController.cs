using ConsultasAPI.Model.IDAO.IRepository;
using ConsultasAPI.Model.IDAO.IServiceDAO;
using ConsultasAPI.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Sicoj.Utils.Files;
using Sicoj.Utils.Redis;
using Sicoj.Utils.Models;
using Sicoj.Utils.Middleware;
using Sicoj.Utils.Enums;
using ConsultasAPI.Model.Entities.Events.AdministradorGlobal;
using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.DTO.Contracts.Administrador;
using ConsultasAPI.Model.ViewModels.Enums;
using ConsultasAPI.Model.Entities;
using FluentValidation;
using ConsultasAPI.Model.Entities.Events.Administrador;
using ConsultasAPI.Model.Entities.Events.AdministradorUnidadAdministrativa;
using Sicoj.Utils.Extentions;
using Mapster;

namespace ConsultasAPI.Api.V1.Controllers
{
    [ApiController]
    [Route("sicoj/consultas/api/v1/administradorunidadadministrativa/consultas")]
    public class AdministradorUnidadAdministrativaController : ControllerBase
    {
        private readonly ILogger<AdministradorUnidadAdministrativaController> _logger;
        private readonly IConsultasAdministradorUnidadAdministrativaService _consultasAdministradorUnidadAdministrativaService;
        private static object _lock = new object();
        private readonly IFileSystemService _fileSystemService;
        private readonly IRedisClient _redisClient;
        private readonly IValidator<RequestAsignarAdministrador> _validatorAsignarAdministrador;
        private readonly IValidator<RequestRemision> _validatorRequestRemision;



        public AdministradorUnidadAdministrativaController(ILogger<AdministradorUnidadAdministrativaController> logger, IConsultasAdministradorUnidadAdministrativaService consultasAdministradorUnidadAdministrativaService, IFileSystemService fileSystemService, IRedisClient redisClient, IValidator<RequestAsignarAdministrador> validatorAsignarAdministrador, IValidator<RequestRemision> validatorRequestRemision)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consultasAdministradorUnidadAdministrativaService = consultasAdministradorUnidadAdministrativaService ?? throw new ArgumentNullException(nameof(consultasAdministradorUnidadAdministrativaService));
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(redisClient));
            _validatorAsignarAdministrador = validatorAsignarAdministrador ?? throw new ArgumentNullException(nameof(validatorAsignarAdministrador));
            _validatorRequestRemision = validatorRequestRemision ?? throw new ArgumentNullException(nameof(validatorRequestRemision));
        }


        #region  Historico de Asuntos


        /// <summary>
        /// Bandeja de Historico : Administrador Unidad Administrativa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<ResponseConsultaByFiltersAdministrador>), 200)]
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


                RequestFiltrosConsultaAdministrador filters = new();
                Filters.MapFilters(request, filters);


                if (!Filters.MapSort<EnumOrderColumnConsultasByFiltrosAdministrador>(request, null!, false, out string orderByColumn, out bool orderDesc))
                {
                    return Ok(ResultOperation.FailureWarningResponse<List<RequestFiltrosConsultaAdministrador>>("La columna de ordenamiento no es válida.")
                        );
                }

                var result = await _consultasAdministradorUnidadAdministrativaService.GetBandejaHistoricoByFiltersService(
                     request.fetch,
                    request.page,
                    orderByColumn,
                    orderDesc,
                    Filters.GetStringValue(filters!.ByNoAsunto.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaPresentacionDesde.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaPresentacionHasta.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaVencimientoDesde.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaVencimientoHasta.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaCSSJDesde.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaCSSJHasta.FirstOrDefault()),
                    Filters.GetStringValue(filters.ByRfcPromovente.FirstOrDefault()),
                    Filters.GetStringValue(filters.ByPromovente.FirstOrDefault()),
                    filters.ByIdTipoAsunto.Adapt<List<int>>(),
                    Filters.GetIntValue(filters!.ByIdAdministracion.FirstOrDefault()),
                    Filters.GetIntValue(filters!.ByIdSubadministracion.FirstOrDefault()),
                    Filters.GetStringValue(filters.ByIdAbogadoAsigno.FirstOrDefault()),
                    filters.ByIdEstadoTarea.Adapt<List<int>>(),
                    filters.ByIdTipoModalidad.Adapt<List<int>>(),
                    filters.ByIdEstadoProcesal.Adapt<List<int>>(),
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


        #endregion

        #region  Reasignar

        /// <summary>
        /// Asignar abogado a consulta : AUA 
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
                var entityExists = await _consultasAdministradorUnidadAdministrativaService.GetByIdServiceAdministrador(request.idConsulta);
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
                    EventsConsultasAdministradorUnidadAdministrativa.AsignarAdministrador(ref entityExists,
                            request.idConsulta,
                            request.idAbogado,
                            sessionInformation.UserInformation.Rfc
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorUnidadAdministrativaService.AsignarConsultaService(entityExists);
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
        /// Reasignar : Administrador Unidad Administrativa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("modificar-reasignar")]
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
                    var result = await _consultasAdministradorUnidadAdministrativaService.ReAsignarComercioExteriorAsync(request.idList.ToArray(), request.idAbogado, sessionInformation.UserInformation);
                    return Ok(result);
                }
                else if (request.idTipoAsunto == EnumTipoAsunto.IMPUESTOS_INTERNOS.GetHashCode())
                {
                    var result = await _consultasAdministradorUnidadAdministrativaService.ReAsignarImpuestosInternosAsync(request.idList.ToArray(), request.idAbogado, sessionInformation.UserInformation);
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

        #endregion

        #region  Requerimientos

        /// <summary>
        /// Requerimiento  por Id  : Administrador Unidad Administrativa
        /// </summary>
        /// <param name="id">Id Requerimiento</param>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseRequerimientoList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("requerimiento/{Id}")]
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



                var result = await _consultasAdministradorUnidadAdministrativaService.GetRequerimientoById(id);
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
        /// Tabla de Requerimientos : Administrador Unidad Administrativa
        ///  ///<param name="idConsulta">Id Requerimiento</param>
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


                var result = await _consultasAdministradorUnidadAdministrativaService.GetTablaRequerimientosService(idConsulta);

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

        #region  Resolución

        /// <summary>
        /// Obtienes Resolucion  por Id  : Administrador Unidad Administrativa
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


                var result = await _consultasAdministradorUnidadAdministrativaService.GetResolucionById(id);
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
        /// Tabla de Resoluciones : Administrador UA
        /// </summary>
        ///<param name="idConsulta">Id Consulta</param>
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


                var result = await _consultasAdministradorUnidadAdministrativaService.GetTablaResolucionService(idConsulta);

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

        #region  Consultas

        /// <summary>
        /// Consultas  por Id  : Administrador Unidad Administrativa
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

                var result = await _consultasAdministradorUnidadAdministrativaService.GetByIdDisconnected(id);
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

        #region Tomar-soltar

        /// <summary>
        ///  Bloquea el registro por Id : Administrador Unidad Administrativa
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

                var entityExists = await _consultasAdministradorUnidadAdministrativaService.GetByIdServiceAdministrador(id);
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
        /// Libera el registro  : Administrador Unidad Administrativa
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

                var entityExists = await _consultasAdministradorUnidadAdministrativaService.GetByIdServiceAdministrador(id);
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

        #endregion

        #region Archivos
        /// <summary>
        /// Lista los documentos por filtros : Administrador Unidad Administrativa
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

                var result = await _consultasAdministradorUnidadAdministrativaService.GetArchivoByFiltersService(
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


        /// <summary>
        /// Visualiza documento de la consulta : Administrador Unidad Administrativa
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



                var entityExists = await _consultasAdministradorUnidadAdministrativaService.GetByIdArchivoService(id);
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

        #endregion

        #region Requrtimeintos prodecon
        /// <summary>
        /// Tabla de Requerimientos Prodecon : Administrador Unidad Administrativa
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


                var result = await _consultasAdministradorUnidadAdministrativaService.GetTablaRequerimientosProdeconService(idConsulta);

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
        /// Tabla de Solicitud de Información : Administrador Unidad Administrativa
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



                var result = await _consultasAdministradorUnidadAdministrativaService.GetTablaSolicitudInformacionService(idConsulta);

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
        /// Tabla de Avisos Y Comunicados : Administrador Unidad Administrativa
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

                var result = await _consultasAdministradorUnidadAdministrativaService.GetTablaAvisosYComunicados(idConsulta);

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

        #region  Solicitud de transparencia

        /// <summary>
        /// Solicitud Transparencia  por Id  : Administrador Unidad Administrativa
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

                var result = await _consultasAdministradorUnidadAdministrativaService.GetByIdSolicitudTransparenciaServices(id);
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
        /// Tabla de Solicitud Transparencia : Administrador Unidad Administrativa
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


                var result = await _consultasAdministradorUnidadAdministrativaService.GetTablaSolicitudTransparenciaService(idConsulta);

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

        #region  Personas Autorizaddas
        /// <summary>
        /// Tabla de personas autorizadas :  Administrador Unidad Administrativa
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


                var result = await _consultasAdministradorUnidadAdministrativaService.GetTablaPersonasAutorizadasService(
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

        #endregion

        #region Remision

        /// <summary>
        /// Remitir consultas  : AUA
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
                    entity = EventsConsultasAdministradorUnidadAdministrativa.RemisionModalidaConsultaAdministrador(
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

                var result = await _consultasAdministradorUnidadAdministrativaService.AddRemisionAdministradorService(entity, entityDocumento, dataFile!);
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
        /// Tabla de consultas remitidas  : AUA
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


                var result = await _consultasAdministradorUnidadAdministrativaService.GetTablaRemisionAdministradorService(
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


        #endregion
        #region 
        /// <summary>
        /// Exportar : AUA
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


                if (filters!.ByIdTipoAsunto.Contains(EnumTipoAsunto.COMERCIO_EXTERIOR.GetHashCode().ToString()) || filters!.ByIdTipoAsunto.Contains(EnumTipoAsunto.IMPUESTOS_INTERNOS.GetHashCode().ToString()))
                {


                    var datos = await _consultasAdministradorUnidadAdministrativaService.ExportarConsulta(
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

                    var datos = await _consultasAdministradorUnidadAdministrativaService.ExportarCumplimentacion(
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

                    var datos = await _consultasAdministradorUnidadAdministrativaService.ExportarReporteGeneral(
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

        /// <summary>
        /// Tabla de Medios de defensa : Administrador UA
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseMediosDefensa>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("medios-defensa-general")]
        public async Task<IActionResult> Medios_Defensa_Proxy([FromQuery] List<string> noAsunto)
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


                var result = await _consultasAdministradorUnidadAdministrativaService.GetTablaMediosDefensaGeneral(noAsunto);

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