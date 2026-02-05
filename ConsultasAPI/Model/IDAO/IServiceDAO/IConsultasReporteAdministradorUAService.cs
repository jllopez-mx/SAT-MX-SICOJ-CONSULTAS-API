using ConsultasAPI.Model.DTO.Response.Administrador;

namespace ConsultasAPI.Model.IDAO.IServiceDAO
{
  public interface IConsultasReporteAdministradorUAService
  {
    #region Exportar
    Task<List<ResponseReporteGlobalConsultas>> ExportarConsulta
  (
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


 );
    Task<List<ResponseReporteGlobalCumplimentacion>> ExportarCumplimentacion
   (
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


  );
   Task<List<ResponseReporteGeneral>> ExportarReporteGeneral
   (
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


  );
    #endregion
  }
}