using Microsoft.AspNetCore.Mvc;
using Sicoj.Utils.Models;
using Sicoj.Utils.Middleware;
using Sicoj.Utils.Enums;
using ConsultasAPI.Model.DTO.Contracts.Administrador;
using ConsultasAPI.Model.ViewModels.Enums;
using Sicoj.Utils.Extentions;
using Mapster;
using ConsultasAPI.Model.Entities.Events.Administrador;
using Sicoj.Utils.Redis;
using ConsultasAPI.Model.IDAO.IServiceDAO;

namespace ConsultasAPI.Api.V1.Controllers

{
    [ApiController]
    [Route("sicoj/consultas/api/v1/reporte-administrador-global/consultas")]
    public class ReporteAdministradorGlobalController : ControllerBase
    {
        private readonly ILogger<OficialPartesController> _logger;
        private readonly IRedisClient _redisClient;
        private readonly IConsultasReporteAdministradorGlobalService _consultasReporteAdministradorGlobalService;

        public ReporteAdministradorGlobalController(ILogger<OficialPartesController> logger, IRedisClient redisClient, IConsultasReporteAdministradorGlobalService consultasReporteAdministradorGlobalService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(redisClient));
            _consultasReporteAdministradorGlobalService = consultasReporteAdministradorGlobalService ?? throw new ArgumentNullException(nameof(consultasReporteAdministradorGlobalService));
        }
        #region 
        /// <summary>
        /// Exportar : Reporte Administrador Global
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


                    var datos = await _consultasReporteAdministradorGlobalService.ExportarConsulta(
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

                    var datos = await _consultasReporteAdministradorGlobalService.ExportarCumplimentacion(
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

                    var datos = await _consultasReporteAdministradorGlobalService.ExportarReporteGeneral(
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

    }
}
