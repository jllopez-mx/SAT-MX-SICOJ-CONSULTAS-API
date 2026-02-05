using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class ResponseTablaRemision
    {
        public int id { get; set; }
        public int idRol { get; set; }
        public string rol { get; set; } = null!;
        public int idAdministracionRemite { get; set; }
        public string administracionRemite { get; set; } = null!;
        public int idAdministracionRecibe { get; set; }
        public string administracionRecibe { get; set; } = null!;
        public int idTipoAutoridad { get; set; }
        public string tipoAutoridad { get; set; } = null!;
        public string? noOficioRemision { get; set; }
        public int idEstadoTarea { get; set; }
        public string estadoTarea { get; set; } = null!;
        public int idEstadoProcesal { get; set; }
        public string estadoProcesal { get; set; } = null!;
        public string? usuario { get; set; }
        public DateTime? fechaOficio { get; set; }
        public DateTime? fechaActualizacion { get; set; }
        public DateTime? fecharemision { get; set; }

    }
}