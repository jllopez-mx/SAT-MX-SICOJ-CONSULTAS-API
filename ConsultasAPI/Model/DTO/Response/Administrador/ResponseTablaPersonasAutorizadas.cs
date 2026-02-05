
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class ResponseTablaPersonasAutorizadas
    {
        public int id { get; set; }
        public int idRol { get; set; } 
        public string rol { get; set; } = null!;
        public string? usuario { get; set; }
        public string? nombre { get; set; }
        public string? rfc { get; set; } = null!;
        public string? telefono { get; set; }
        public string? email { get; set; }
        public string? fechaRegistro { get; set; }
        public string? fechaModificacion { get; set; }
    }
}