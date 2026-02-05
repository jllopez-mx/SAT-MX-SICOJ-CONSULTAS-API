using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.Entities;
using ConsultasAPI.Model.Entities.Events.OficialPartes;
using ConsultasAPI.Model.IDAO;
using ConsultasAPI.Model.IDAO.IServiceDAO;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sicoj.Utils.Models;
using Sicoj.Utils.Redis;
using ConsultasAPI.Model.ViewModels.Enums;
using Sicoj.Utils.Files;
using Sicoj.Utils.Middleware;
using Sicoj.Utils.Enums;
using ConsultasAPI.Model.DTO.Contracts.OficialPartes;
using ConsultasAPI.Model.DTO.Contracts.Administrador;
using Sicoj.Utils.Extentions;
using Microsoft.Extensions.Options;
using ConsultasAPI.Model.DTO.ContractsValidations;
using StackExchange.Redis;
using Sicoj.Utils.Models.Requests;
using ConsultasAPI.Model.DTO.Contracts.Email;

namespace ConsultasAPI.Api.V1.Controllers
{
    [ApiController]
    [Route("sicoj/consultas/api/v1/oficial-partes/consultas")]
    public class OficialPartesController : ControllerBase
    {
        #region Variables
        private readonly IConsultaRepositoryOficialPartes _repository;
        private readonly IValidator<RequestCreateConsulta> _validatorConsulta;
        private readonly IValidator<RequestUpdateConsulta> _validatorUpdateConsultas;
        private readonly IValidator<RequestTurnarConsulta> _validatorturnar;
        private readonly IValidator<RequestTurnarCumplimentacion> _validatorturnarCumplimentacion;
        private readonly IValidator<RequestBuscaConsultaRfc> _validaorrfc;
        private readonly IValidator<RequestDeleteArchivosConsulta> _validatorDeleteArchivos;
        private readonly IValidator<RequestCreateConsulta> _validatorRequestCreateConsulta;
        private readonly IConsultasOficialPartesService _consultasOficialPartesService;
        private readonly ILogger<OficialPartesController> _logger;
        private readonly IRedisClient _redisClient;
        private readonly CatalogosEnpoints _catologosEnpoints;
        private readonly ProxyEnpoints _proxyEnpoints;
        
        private static object _lock = new object();
        private readonly IFileSystemService _fileSystemService;
        private readonly IValidator<RequestDocumentoUpdate> _requestDocumentoUpdateValidator;

         private readonly IValidator<RequestCreateSolicitudTransparencia> _validatorRequestCreateSolicitudTransparencia;
         private readonly IValidator<RequestUpdateSolicitudTransparencia> _validatorRequestUpdateSolicitudTransparencia;
         private readonly IValidator<RequestCreateCumplimentacion> _validatorRequestCreateCumplimentacion;
         private readonly IValidator<RequestUpdateCumplimentacion> _validatorUpdateCumplimentacion;

        public OficialPartesController(IConsultaRepositoryOficialPartes repository,

        IValidator<RequestCreateConsulta> validatorConsulta,

        IValidator<RequestUpdateConsulta> validatorUpdateConsultas,

        IValidator<RequestTurnarConsulta> validatorturnar,

        IValidator<RequestBuscaConsultaRfc> validaorrfc,

        IValidator<RequestCreateConsulta> validatorRequestCreateConsulta,

        IConsultasOficialPartesService consultasOficialPartesService,

        ILogger<OficialPartesController> logger,

        IRedisClient redisClient,

        IValidator<RequestDeleteArchivosConsulta> validatorDeleteArchivos,

        IFileSystemService fileSystemService,

        IValidator<RequestCreateSolicitudTransparencia> validatorRequestCreateSolicitudTransparencia,

        IValidator<RequestUpdateSolicitudTransparencia> validatorUpdateSolicitudTransparencia,

        IValidator<RequestDocumentoUpdate> requestDocumentoUpdateValidator,

        IOptions<CatalogosEnpoints> catologosEnpoints,
        IOptions<ProxyEnpoints> proxyEnpoints,
        IValidator<RequestCreateCumplimentacion> validatorRequestCreateCumplimentacion,
        IValidator<RequestTurnarCumplimentacion> validatorturnarCumplimentacion,
        IValidator<RequestUpdateCumplimentacion> validatorUpdateCumplimentacion

        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validatorConsulta = validatorConsulta ?? throw new ArgumentNullException(nameof(validatorConsulta));
            _validatorUpdateConsultas = validatorUpdateConsultas ?? throw new ArgumentNullException(nameof(validatorUpdateConsultas));
            _validatorturnar = validatorturnar ?? throw new ArgumentNullException(nameof(validatorturnar));
            _validaorrfc = validaorrfc ?? throw new ArgumentNullException(nameof(validaorrfc));
            _validatorRequestCreateConsulta = validatorRequestCreateConsulta ?? throw new ArgumentNullException(nameof(validatorRequestCreateConsulta));
            _consultasOficialPartesService = consultasOficialPartesService ?? throw new ArgumentNullException(nameof(consultasOficialPartesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(redisClient));
            _validatorDeleteArchivos = validatorDeleteArchivos ?? throw new ArgumentNullException(nameof(validatorDeleteArchivos));
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _validatorRequestCreateSolicitudTransparencia = validatorRequestCreateSolicitudTransparencia ?? throw new ArgumentNullException(nameof(validatorRequestCreateSolicitudTransparencia));
            _validatorRequestUpdateSolicitudTransparencia = validatorUpdateSolicitudTransparencia ?? throw new ArgumentNullException(nameof(validatorUpdateSolicitudTransparencia));
            _requestDocumentoUpdateValidator = requestDocumentoUpdateValidator ?? throw new ArgumentNullException(nameof(requestDocumentoUpdateValidator));
            _catologosEnpoints = catologosEnpoints.Value ?? throw new ArgumentNullException(nameof(catologosEnpoints));
            _proxyEnpoints = proxyEnpoints.Value ?? throw new ArgumentNullException(nameof(proxyEnpoints));
            _validatorRequestCreateCumplimentacion = validatorRequestCreateCumplimentacion ?? throw new ArgumentNullException(nameof(validatorRequestCreateCumplimentacion));
            _validatorturnarCumplimentacion = validatorturnarCumplimentacion ?? throw new ArgumentNullException(nameof(validatorturnarCumplimentacion));
            _validatorUpdateCumplimentacion = validatorUpdateCumplimentacion ?? throw new ArgumentNullException(nameof(validatorUpdateCumplimentacion));
            
        }

        #endregion

        


         /// <summary>
        /// Bandeja de Pendientes : Oficial de Partes
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


                var result = await _consultasOficialPartesService.GetBandejaPendientesService(
                    request.fetch,
                    request.page,
                    orderByColumn,
                    orderDesc,
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
        /// Bandeja de Historico : Oficial de Partes
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


                if (!Filters.MapSort<EnumOrderColumnConsultasByFiltros>(request, null!, false,out string orderByColumn, out bool orderDesc))
                {
                    return Ok(ResultOperation.FailureWarningResponse<List<RequestFiltrosConsulta>>("La columna de ordenamiento no es válida.")
                        );
                }

                var result = await _consultasOficialPartesService.GetBandejaHistoricoByFiltersService(
                     request.fetch,
                    request.page,
                    orderByColumn,
                    orderDesc,
                    Filters.GetStringValue(filters!.ByNoAsunto.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaRecepcionDesde.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaRecepcionHasta.FirstOrDefault()),
                    Filters.GetStringValue(filters.ByRfc.FirstOrDefault()),
                    Filters.GetStringValue(filters.ByPromovente.FirstOrDefault()),
                    filters!.ByIdTipoAsunto.Adapt<List<int>>(),
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


        /// <summary>
        /// Crea consultas (Comercio Exterior / Impuestos Internos) : Oficial de Partes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("")]
        public async Task<IActionResult> PostValidaciones(
         [FromForm] RequestCreateConsulta request
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

                var validationResult = await _validatorRequestCreateConsulta.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }

                Consulta entity = null!;
                try
                {
                    entity = EventsConsultasOficialPartes.CreateModalidadConsulta(
                        request.rfc,
                        request.promovente,
                        request.promoventeEsContribuyente,
                        request.rfcContribuyente,
                        request.contribuyente,
                        request.idTipoAsunto,
                        request.idTipoModalidad,
                        request.despachoAutorizado!,
                        DateTime.Parse(request.fechaPresentacion!),
                        DateTime.Parse(request.fechaRecepcion!),
                        sessionInformation.UserInformation.IdAdministracionCentral,
                        sessionInformation.UserInformation.Rfc!,
                        request.idAdministracion
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasOficialPartesService.AddConsultaService(entity,sessionInformation.UserInformation);
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
        /// Edita consultas (Comercio Exterior / Impuestos Internos) : Oficial de Partes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("")]
        public async Task<IActionResult> PatchConsultas(
          [FromForm] RequestUpdateConsulta request
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

                if (request.idTipoModalidad == EnumTipoModalidad.FISICO.GetHashCode() || request.idTipoModalidad== EnumTipoModalidad.LINEA.GetHashCode())
                {
                    var validationResult = await _validatorUpdateConsultas.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                    }
                    var entityExists = await _consultasOficialPartesService.GetByAllService(request.id);
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
                        EventsConsultasOficialPartes.UpdateModalidaConsulta(ref entityExists,
                                request.rfc,
                                request.promovente,
                                request.promoventeEsContribuyente,
                                request.rfcContribuyente,
                                request.contribuyente,
                                request.despachoAutorizado!,
                                request.fechaPresentacion!,
                                request.fechaRecepcion!,
                                request.idTipoAsunto,
                                request.idTipoModalidad,
                                sessionInformation.UserInformation.Rfc!,
                                request.idAdministracion
                        );
                    }
                    catch (Exception _ex)
                    {
                        return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                    }
                    var result = await _consultasOficialPartesService.UpdateConsultaService(entityExists,sessionInformation.UserInformation);
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
        /// Turna consultas : Oficial de Partes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("turnar")]
        public async Task<IActionResult> TurnarConsultas(
          [FromForm] RequestTurnarConsulta request
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

                var validationResult = await _validatorturnar.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasOficialPartesService.GetByAllService(request.id);
                
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "El registro ingresado para turnar no existe."
                        )
                    );
                }
                  if (entityExists is not  null)
                {
                    if(entityExists.turnado)
                    {
                             return Ok(
                        ResultOperation.FailureErrorResponse(
                            "El registro ya se encuentra turnado"
                        )
                    );
                    }
                   
                }
                
                try
                {
                    EventsConsultasOficialPartes.TurnarModalidaConsulta(ref entityExists!,
                            request.id,
                            sessionInformation.TokenInfomation.workforceID  
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasOficialPartesService.TurnarConsultaService(entityExists);
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
        /// Elimina consultas : Oficial de Partes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpDelete("")]
        public async Task<IActionResult> DeleteConsultas(
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

                
                var entityExists = await _consultasOficialPartesService.GetByAllService(id);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
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

                try
                {
                    EventsConsultasOficialPartes.DeleteModalidaConsulta(ref entityExists,
                    id
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasOficialPartesService.DeleteConsultaService(entityExists);
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
        /// Consultas por Id (Descontinuado)  : Oficial de Partes
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
        public async Task<IActionResult> GetById(int id)
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

              

                var result = await _consultasOficialPartesService.GetByIdService(id);

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
        /// Consultas  por Id  : Oficial de Partes
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
        public async Task<IActionResult> GetByIdDisconnected( int id)
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


                var result = await _consultasOficialPartesService.GetByIdDisconnected(id);
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
        /// Bloquea el registro por Id : Oficial de Partes
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

                var entityExists = await _consultasOficialPartesService.GetByIdService(id);
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
        /// Libera el registro  : Oficial de Partes
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

                var entityExists = await _consultasOficialPartesService.GetByIdService(id);
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
        /// Carga  documento a la consulta : Oficial de Partes
        /// </summary>
        /// <param name="request">Datos del archivo y del registro de Consultas </param>
        /// <returns>Id del registro de archivo agregado</returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("archivo")]
        public async Task<IActionResult> PostFile(
            [FromForm] RequestCreaArchivoConsulta request
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

                 
                var entityExists = await _consultasOficialPartesService.GetByAllService(request.idConsulta);
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "La consulta no existe."
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

                  _fileSystemService.FileTryOut(
                        request.fileConsultas, 
                        Path.Combine("CONSULTAS", "COMERCIO EXTERIOR", request.idConsulta.ToString()), 
                        out var dataFile);

                ArchivoConsulta entity = null!;
                

                try
                {
                    entity = EventsArchivosConsultasOficialPartesEvents.Create(
                        request.idConsulta,
                        request.idTipoDocumento,
                        request.fileConsultas.FileName,               
                        dataFile.FilePath,
                        dataFile.File.FileName,
                        sessionInformation.UserInformation.Rfc!,
                        sessionInformation.UserInformation.Rfc!,
                        request.noFolio!,
                        request.idSeccion,
                        _fileSystemService.ConvertBytesToMegaBytesString(dataFile.File.Length),
                        request.idDocumentoSeccion
                        
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasOficialPartesService.AddArchivosAsyncService(entity, dataFile);
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
        /// Método para actualizar un documento : Oficial de Partes
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

                //string key = null!;
                var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>($"{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}");

             //   var keyExists = await _redisClient.GetValueAsync<UserBlockingRegistration>(key);
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

                var entityExists = await _consultasOficialPartesService.GetByAllService(request.idConsulta);
                    if (entityExists is null)
                    {
                        return Ok(
                            ResultOperation.FailureErrorResponse(
                                "La consulta no existe."
                            )
                        );
                    }
                    ArchivoConsulta entityDocumento = await _consultasOficialPartesService.GetIdArchivoConsultaService(request.id);
                   


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
                            EventsArchivosConsultasOficialPartesEvents.UpdateWithFile(
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
                            EventsArchivosConsultasOficialPartesEvents.Update(
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

                    ResultOperation<int> result = await _consultasOficialPartesService.UpdateArchivoService(entityDocumento!, dataFile!);
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
        /// Borra documento de la consulta : Oficial de Partes
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
                    var entityExists = await _consultasOficialPartesService.GetByIdArchivoDeleteService(item);
                    if (entityExists is not null)
                    {
                        try
                        {
                            EventsConsultasOficialPartes.DeleteModalidaArchivoConsulta(ref entityExists
                            );
                        }
                        catch (Exception _ex)
                        {
                            return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                        }
                        var result = await _consultasOficialPartesService.DeleteArchivoConsultaService(entityExists);
                    }
                }
                return Ok(ResultOperation.SuccessResponse(true));
            }
            catch (Exception _e)
            {
                _logger.LogError(_e, "Ah ocurrido un error al eliminar el registro.");
                return BadRequest(
                    ResultOperation.FailureErrorResponse(
                        "Ah ocurrido un error inesperado al intentar eliminar el registro."
                    )
                );
            }
        }


        /// <summary>
        /// Lista los documentos asociados a la consulta : Oficial de Partes
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

                
                var result = await _consultasOficialPartesService.GetAllArchivoService(id);
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
        /// Visualiza documento de la consulta : Oficial de Partes
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

                 
                var entityExists = await _consultasOficialPartesService.GetByIdArchivoService(id);
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
                var contentType = EventsArchivosConsultasOficialPartesEvents.GetArchivoExt(fileName);

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
        /// Obtiene rfc ampliado : Oficial de Partes
        /// </summary>
        /// <param name="nombre">Id de  RFC Ampliado</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<ResponseConsultaRfc>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("rfc")]
        public async Task<IActionResult> GetRfcAmpliado(string nombre)
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

                 
                var result = await _consultasOficialPartesService.GetRfcampliadosyncService(nombre);
                if (result is null)
                {
                    return Ok(
                        ResultOperation.FailureWarningResponse("No se encontraron resultados")
                    );
                }
                return Ok(
                    ResultOperation.SuccessResponse(
                        result.Adapt<List<ResponseConsultaRfc>>()
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
        /// Lista los documentos por filtros : Oficial Partes
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

                var result = await _consultasOficialPartesService.GetArchivoByFiltersService(
                     request.fetch,
                    request.page,
                    orderByColumn,
                    orderDesc,
                    Filters.GetIntValue(filters.ByIdRol.FirstOrDefault()),
                    Filters.GetStringValue(filters!.ByFolio.FirstOrDefault()),
                    Filters.GetIntValue(filters.ByIdSeccion.FirstOrDefault()),
                    Filters.GetIntValue(filters.ByIdConsulta.FirstOrDefault()),
                    Filters.GetIntValue(filters.ByIdRemision.FirstOrDefault()),
                    Filters.GetBoolValue(filters.ByEstatus.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaCreacionDesde.FirstOrDefault()),
                    Filters.GetDateTimeValue(filters.ByFechaCreacionHasta.FirstOrDefault()),                   
                    Filters.GetBoolValue(filters.ByRemplazable.FirstOrDefault()),
                    Filters.GetBoolValue(filters.ByPermanente.FirstOrDefault()),                   
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

        #region Solicitud-Transparencia
        /// <summary>
        /// Crea consultas (Comercio Exterior / Impuestos Internos) : Oficial de Partes
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

                     entity = EventsConsultasOficialPartes.CreateModalidadSolicitudTransparencia(
                        request.idConsulta,
                        request.noSolicitud,                  
                        DateTime.Parse(request.fechaSolicitud!)
                    );

                     if (dataFile is not null)
                        {
                            entityDocumento =EventsArchivosConsultasOficialPartesEvents.CreateDocumento(
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


                var result = await _consultasOficialPartesService.AddSolicitudTransparenciaService(entity,entityDocumento,dataFile!);
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
        /// Edita solicitud transparencia : Oficial Partes
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
                    var entityExists = await _consultasOficialPartesService.GetByIdSolicitudTransparenciaService(request.id);
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
                        EventsConsultasOficialPartes.UpdateSolicitudTransparencia(ref entityExists,
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
                    var result = await _consultasOficialPartesService.UpdateSolicitudTransparenciaService(entityExists);
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
        /// Solicitud Transparencia  por Id  : Oficial Partes
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

                var result = await _consultasOficialPartesService.GetByIdSolicitudTransparenciaServices(id);
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
        /// Tabla de Solicitud Transparencia : Oficial Partes
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

                 
                var result = await _consultasOficialPartesService.GetTablaSolicitudTransparenciaService(idConsulta);

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

                var entityExists = await _consultasOficialPartesService.GetByIdSolicitudTransparenciaService(id);
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
                    EventsConsultasOficialPartes.DeleteSolicitudTransparencia(ref entityExists,
                    id
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasOficialPartesService.DeleteSolicitudTransparenciaService(entityExists);
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
    
        #region Cumplimentación

        /// <summary>
        /// Crea Cumplimentación : Oficial de Partes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("cumplimentacion")]
        public async Task<IActionResult> PostCumplimentacion(
         [FromForm] RequestCreateCumplimentacion request
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
                       
           

                var validationResult = await _validatorRequestCreateCumplimentacion.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" -- ")));
                }

                DataFile dataFile = null!;
                if (request.documento is not null)
                {
                    _fileSystemService.FileTryOut(
                        request.documento,
                        Path.Combine("CONSULTAS", "CUMPLIMENTACION", request.noAsuntoConsulta!.ToString()),
                        out dataFile);
                }

                ArchivoConsulta entityDocumento = null!;
                Cumplimentacion entity = null!;
                try
                {
                    entity = EventsConsultasOficialPartes.CreateCumplimentacion(
                        request.rfc!,
                        request.promovente!,
                        request.promoventeEsContribuyente!,
                        request.rfcContribuyente!,
                        request.contribuyente!,
                        request.idTipoAsunto,
                        request.idTipoModalidad!,
                        request.noAsuntoConsulta!,
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
                        sessionInformation.UserInformation.Rfc!,
                         sessionInformation.UserInformation.IdAdministracionCentral
                    );

                    if (dataFile is not null)
                        {
                            entityDocumento =EventsArchivosConsultasOficialPartesEvents.CreateDocumentoCumplimentacion(
                                request.noAsuntoConsulta!,
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

                var result = await _consultasOficialPartesService.AddCumplimentacionService(entity,entityDocumento, dataFile!);
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
        /// Turna cumplimentacion : Oficial de Partes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPatch("turnar-cumplimentacion")]
        public async Task<IActionResult> TurnarCumplimentacion(
          [FromForm] RequestTurnarCumplimentacion request
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

                var validationResult = await _validatorturnarCumplimentacion.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Ok(ResultOperation.FailureWarningResponse(validationResult.ToString(" - ")));
                }
                var entityExists = await _consultasOficialPartesService.GetByAllServiceCumplimentacion(request.id);
                
                if (entityExists is null)
                {
                    return Ok(
                        ResultOperation.FailureErrorResponse(
                            "El registro ingresado para turnar no existe."
                        )
                    );
                }
                  if (entityExists is not  null)
                {
                    if(entityExists.turnado)
                    {
                             return Ok(
                        ResultOperation.FailureErrorResponse(
                            "El registro ya se encuentra turnado"
                        )
                    );
                    }
                   
                }
                
                try
                {
                    EventsConsultasOficialPartes.TurnarCumplimentacion(ref entityExists!,
                            request.id,
                            request.idAdministracion,
                            sessionInformation.TokenInfomation.workforceID  
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }
                var result = await _consultasOficialPartesService.TurnarCumplimentacionService(entityExists);
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
        /// Edita cumplimentacion (Comercio Exterior / Impuestos Internos) : Oficial de Partes
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
                    var entityExists = await _consultasOficialPartesService.GetByAllServiceCumplimentacion(request.id);
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
                        EventsConsultasOficialPartes.UpdateCumplimentacion(ref entityExists,
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
                    var result = await _consultasOficialPartesService.UpdateCumplimentacionService(entityExists,sessionInformation.UserInformation);
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
        /// Consultas  por Id  : Oficial de Partes
        /// </summary>
        /// <param name="noAsunto">Número de asunto para la consulta de cumplimiento</param>
        /// <returns></returns>
        [ProducesResponseType(
            typeof(ResultOperation<ResponseCumplimentacionList>),
            200
        )]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpGet("buscar-cumplimentacion/{noAsunto}")]
        public async Task<IActionResult> GetByAsuntoCumplimentacion(string noAsunto)
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

                  
                await _redisClient.IncrementeContador("{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}{request.noJuicio}");

                var contadorvisitas = await _redisClient.GetValueContador("{EnumModulosRedis.CONSULTAS.ToStringValue()}{request.idConsulta}{request.noJuicio}");
              


                var result = await _consultasOficialPartesService.GetByNoAsuntoCumplimentacion(noAsunto,sessionInformation.UserInformation,contadorvisitas);
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

        #region  Envio correo
        /// <summary>
        /// Envió de correos : Oficial de Partes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResultOperation<int>), 200)]
        [ProducesResponseType(typeof(ResultOperation), 400)]
        [Authorize(EnumRoles.RR_OP)]
        [HttpPost("Email")]
        public async Task<IActionResult> Email(
         [FromForm] RequestEmailMessage request
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

              
                Email entity = null!;
                try
                {
                    entity = EventsConsultasOficialPartes.CreaCorreo(  
                        request.To,
                        request.Cc,
                        request.Subject,                      
                        request.IsHtml,
                        request.Body
                        
                    );
                }
                catch (Exception _ex)
                {
                    return Ok(ResultOperation.FailureWarningResponse(_ex.Message));
                }

                var result = await _consultasOficialPartesService.EnvioEmail(entity);
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
        #endregion

    }
}