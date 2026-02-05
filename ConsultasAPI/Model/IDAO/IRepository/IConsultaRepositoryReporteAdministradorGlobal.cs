using ConsultasAPI.Model.DTO.Response.Administrador;

namespace ConsultasAPI.Model.IDAO.IRepository
{
    public interface IConsultaRepositoryReporteAdministradorGlobal
    {
        #region Exportar
        Task<List<ResponseReporteGlobalConsultas>> ExportarConsultaAsyncRepository(
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
        Task<List<ResponseReporteGlobalCumplimentacion>> ExportarCumplimentacionAsyncRepository(
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
        Task<List<ResponseReporteGeneral>> ExportarReporteGeneralAsyncRepository(
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