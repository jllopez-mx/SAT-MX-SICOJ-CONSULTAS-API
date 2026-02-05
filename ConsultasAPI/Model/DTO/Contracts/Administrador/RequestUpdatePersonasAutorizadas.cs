using System.ComponentModel.DataAnnotations;


namespace ConsultasAPI.Model.DTO
{
    public class RequestUpdatePersonasAutorizadas
    {
        [Required]
        public int id { get; set; }

        [Required]
        public int idConsulta { get; set; }
        
        [Required]
        public string nombre { get; set; } = null!;
        
        [Required]
        public string rfc { get; set; } = null!;

        public string? telefono { get; set; }

        public string? email { get; set; }
    }
       
}