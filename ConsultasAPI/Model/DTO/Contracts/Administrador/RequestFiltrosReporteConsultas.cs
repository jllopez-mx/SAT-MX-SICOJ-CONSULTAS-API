namespace ConsultasAPI.Model.DTO.Contracts.Administrador
{
    public class RequestFiltrosReporteConsultas
    {

        public List<string> ByNoAsunto { get; set; } = new()!;
        public List<string> ByRfcPromovente { get; set; } = new()!;
        public List<string> ByPromovente { get; set; } = new()!;
        public List<string> ByIdAdministracion { get; set; } = new()!;
        public List<string> ByIdSubadministracion { get; set; } = new()!;
        public List<string> ByIdAbogadoAsigno { get; set; } = new()!;
        public List<string> ByIdTipoEntrada { get; set; } = new()!;
        public List<string> ByIdTipoAsunto { get; set; } = new()!;
        public List<string> ByIdEstadoProcesal { get; set; } = new()!;
        public List<string> ByIdTema { get; set; } = new()!;
        public List<string> ByFechaPresentacionDesde { get; set; } = new()!;
        public List<string> ByFechaPresentacionHasta { get; set; } = new()!;
        public List<string> ByFechaVencimientoDesde { get; set; } = new()!;
        public List<string> ByFechaVencimientoHasta { get; set; } = new()!;
        public List<string> ByFechaConclucionDesde { get; set; } = new()!;
        public List<string> ByFechaConclucionHasta { get; set; } = new()!;
        public List<string> ByFechaOficioNotificacionResolucionDesde { get; set; } = new()!;
        public List<string> ByFechaOficioNotificacionResolucionHasta { get; set; } = new()!;
        public List<string> ByFechaIngresoProdeconDesde { get; set; } = new()!;
        public List<string> ByFechaIngresoProdeconHasta { get; set; } = new()!;
        public List<string> ByFechaSolicituTransparenciaDesde { get; set; } = new()!;
        public List<string> ByFechaSolicituTransparenciaHasta { get; set; } = new()!;

    }
}