using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestUpdateRequerimientoProdecon
    {
        [Required]
        public int id { get; set; }
        
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public string noOficioRequerimiento { get; set; }=string.Empty;

        [Required]
        public string noExpedienteRequerimiento { get; set; }=string.Empty;

        [Required]
        public string fechaOficio { get; set; } =null!;

        [Required]
        public string fechaIngreso { get; set; } =null!;

        [Required]
        public Boolean accionAdicional { get; set; }

        public string atencion { get; set; } =null!;
    }
       
}