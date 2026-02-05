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
using Sicoj.Utils.Extentions;
using Sicoj.Utils.ViewModels;
using Mapster;

namespace ConsultasAPI.Api.V1.Controllers
{
    [Route("sicoj/consultas/api/v1/administradorglobal/consultas")]
    public class AdministradorGlobalController : ControllerBase
    {
        #region Variables

        private static object _lock = new object();
        private readonly IFileSystemService _fileSystemService;
        private readonly ILogger<AdministradorController> _logger;
        private readonly IRedisClient _redisClient;
        private readonly IConsultasAdministradorGlobalService _consultasAdministradorGlobalService;
        private readonly IConsultaRepositoryAdministrador _consultaRepositoryAdministradorGlobal;
        private readonly IValidator<RequestUpdateRequerimiento> _validatorRequestUpdateRequerimiento;
        private readonly IValidator<RequestUpdateResolucion> _validatorRequestUpdateResolucion;
        private readonly IValidator<RequestUpdateConsultaAdministradorImpuestosInternos> _validatorUpdateConsultaAdministadorImpuestosInternos;
        private readonly IValidator<RequestUpdateConsultaAdministradorComercioExterior> _validatorUpdateConsultaAdministadorComercioExterior;

        public AdministradorGlobalController(IFileSystemService fileSystemService, ILogger<AdministradorController> logger, IRedisClient redisClient, IConsultasAdministradorGlobalService consultasAdministradorGlobalService, IConsultaRepositoryAdministrador consultaRepositoryAdministradorGlobal, IValidator<RequestUpdateRequerimiento> validatorRequestUpdateRequerimiento, IValidator<RequestUpdateResolucion> validatorRequestUpdateResolucion, IValidator<RequestUpdateConsultaAdministradorImpuestosInternos> validatorUpdateConsultaAdministadorImpuestosInternos, IValidator<RequestUpdateConsultaAdministradorComercioExterior> validatorUpdateConsultaAdministadorComercioExterior)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(redisClient));
            _consultasAdministradorGlobalService = consultasAdministradorGlobalService ?? throw new ArgumentNullException(nameof(consultasAdministradorGlobalService));
            _consultaRepositoryAdministradorGlobal = consultaRepositoryAdministradorGlobal ?? throw new ArgumentNullException(nameof(consultaRepositoryAdministradorGlobal));
            _validatorRequestUpdateRequerimiento = validatorRequestUpdateRequerimiento ?? throw new ArgumentNullException(nameof(validatorRequestUpdateRequerimiento));
            _validatorRequestUpdateResolucion = validatorRequestUpdateResolucion ?? throw new ArgumentNullException(nameof(validatorRequestUpdateResolucion));
            _validatorUpdateConsultaAdministadorImpuestosInternos = validatorUpdateConsultaAdministadorImpuestosInternos ?? throw new ArgumentNullException(nameof(validatorUpdateConsultaAdministadorImpuestosInternos));
            _validatorUpdateConsultaAdministadorComercioExterior = validatorUpdateConsultaAdministadorComercioExterior ?? throw new ArgumentNullException(nameof(validatorUpdateConsultaAdministadorComercioExterior));
        }

        #endregion

        #region  Historico de Asuntos

        /// <summary>
        /// Bandeja de Historico : Administrador Global
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

                var result = await _consultasAdministradorGlobalService.GetBandejaHistoricoByFiltersService(
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

        #region Reactivar

        /// <summary>
        /// Reactivar : Administrador Global
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("Reactivar")]
        public async Task<IActionResult> PatchImpustosInternos(
          [FromForm] RequestUpdateReactivar request
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

                var entityExists = await _consultasAdministradorGlobalService.GetByIdConsultaService(request.idConsulta);
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
                    EventsConsultasAdministradorGlobal.Reactivar(ref entityExists,
                            request.idConsulta
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasAdministradorGlobalService.UpdateReactivarService(request.idConsulta);
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

        #region Requerimiento


        /// <summary>
        /// Requerimiento  por Id  : Administrador Global
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



                var result = await _consultasAdministradorGlobalService.GetRequerimientoById(id);
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
        /// Tabla de Requerimientos : Administrador Global
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


                var result = await _consultasAdministradorGlobalService.GetTablaRequerimientosService(idConsulta);

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
        /// Descartar Requerimientos : Administrador Global
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("descartar-requerimientos")]
        public async Task<IActionResult> PatchDescartarRequerimientos(
          [FromForm] RequestUpdateDescartarRequerimientos request
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

                var result = await _consultasAdministradorGlobalService.UpdateDescartarRequerimientosService(request.idConsulta, request.idSeccion, request.descartarUltimoRegistro);
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
        /// Modificar Requerimiento: Administrador Global
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("modificar-requerimiento")]
        public async Task<IActionResult> PatchModificarRequerimiento(
          [FromForm] RequestUpdateEstadoConsulta request
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

                var result = await _consultasAdministradorGlobalService.UpdateModificarRequerimientoService(request.idConsulta);
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
        /// REasignar : Administrador Global
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
                    var result = await _consultasAdministradorGlobalService.ReAsignarComercioExteriorAsync(request.idList.ToArray(), request.idAbogado, sessionInformation.UserInformation);
                    return Ok(result);
                }
                else if (request.idTipoAsunto == EnumTipoAsunto.IMPUESTOS_INTERNOS.GetHashCode())
                {
                    var result = await _consultasAdministradorGlobalService.ReAsignarImpuestosInternosAsync(request.idList.ToArray(), request.idAbogado, sessionInformation.UserInformation);
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

        #region  Emisión Resolución


        /// <summary>
        /// Obtienes Resolucion  por Id  : Administrador Global
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


                var result = await _consultasAdministradorGlobalService.GetResolucionById(id);
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
        /// Modificar Resolucion: Administrador Global
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("modificar-resolucion")]
        public async Task<IActionResult> PatchModificarResolucion(
          [FromForm] RequestUpdateEstadoConsulta request
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

                var result = await _consultasAdministradorGlobalService.UpdateModificarResoluciuonService(request.idConsulta);
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
        /// Descartar Requerimientos : Administrador Global
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("descartar-resolucion")]
        public async Task<IActionResult> PatchDescartarResolucion(
          [FromForm] RequestUpdateDescartarResolucion request
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

                var result = await _consultasAdministradorGlobalService.UpdateDescartarResolucionService(request.idConsulta);
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
        /// Tabla de Resoluciones : Administrador Global
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


                var result = await _consultasAdministradorGlobalService.GetTablaResolucionService(idConsulta);

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

        #region Consultas

        /// <summary>
        /// Consultas  por Id  : Administrador Global
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

                var result = await _consultasAdministradorGlobalService.GetByIdDisconnected(id);
                return Ok(result);
            }
            catch (Exception _e)
            {
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        $"Ah ocurrido un error inesperado => {_e.Message}"
                    )
                );
            }
        }



        #endregion

        #region Tomar-soltar

        /// <summary>
        ///  Bloquea el registro por Id : Administrador Global
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


                var entityExists = await _consultasAdministradorGlobalService.GetByIdServiceAdministrador(id);
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
        /// Libera el registro  : Administrador Global
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

                var entityExists = await _consultasAdministradorGlobalService.GetByIdServiceAdministrador(id);
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

        #region Datos Generales

        /// <summary>
        /// Descartar Datos Generales: Administrador Global
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("descartar-datos-generales")]
        public async Task<IActionResult> PatchDescartar(
          [FromForm] RequestUpdateEstadoConsulta request
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

                var result = await _consultasAdministradorGlobalService.UpdateDescartarDatosGeneralesService(request.idConsulta);
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
        /// Modificar Datos Generales: Administrador Global
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("modificar-datos-generales")]
        public async Task<IActionResult> PatchModificar(
          [FromForm] RequestUpdateEstadoConsulta request
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

                var result = await _consultasAdministradorGlobalService.UpdateModificarAsuntoService(request.idConsulta);
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

        #region Archivos
        /// <summary>
        /// Lista los documentos por filtros : Administrador  Global
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

                var result = await _consultasAdministradorGlobalService.GetArchivoByFiltersService(
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
        /// Visualiza documento de la consulta : Administrador Global
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



                var entityExists = await _consultasAdministradorGlobalService.GetByIdArchivoService(id);
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

        #region Requerimientos prodecon
        /// <summary>
        /// Tabla de Requerimientos Prodecon : Administrador Global
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


                var result = await _consultasAdministradorGlobalService.GetTablaRequerimientosProdeconService(idConsulta);

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
        /// Tabla de Solicitud de Información : Administrador Global
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



                var result = await _consultasAdministradorGlobalService.GetTablaSolicitudInformacionService(idConsulta);

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
        /// Tabla de Avisos Y Comunicados : Administrador Global
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

                var result = await _consultasAdministradorGlobalService.GetTablaAvisosYComunicados(idConsulta);

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
        /// Solicitud Transparencia  por Id  : Administrador Global
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

                var result = await _consultasAdministradorGlobalService.GetByIdSolicitudTransparenciaServices(id);
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
        /// Tabla de Solicitud Transparencia : Administrador Global
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


                var result = await _consultasAdministradorGlobalService.GetTablaSolicitudTransparenciaService(idConsulta);

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
        /// Tabla de personas autorizadas : Administrador Global
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


                var result = await _consultasAdministradorGlobalService.GetTablaPersonasAutorizadasService(
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

        #region 
        /// <summary>
        /// Exportar : Administrador Global
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


                    var datos = await _consultasAdministradorGlobalService.ExportarConsulta(
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

                    var datos = await _consultasAdministradorGlobalService.ExportarCumplimentacion(
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

                    var datos = await _consultasAdministradorGlobalService.ExportarReporteGeneral(
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
        /// Tabla de Medios de defensa : Administrador Global
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


                var result = await _consultasAdministradorGlobalService.GetTablaMediosDefensaGeneral(noAsunto);

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
