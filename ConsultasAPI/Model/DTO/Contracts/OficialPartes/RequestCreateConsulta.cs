using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestCreateConsulta
    {
        [Required]
        public string rfc { get; set; } = null!;
        
        [Required]
        public string promovente { get; set; } = null!;

        [Required]
        public Boolean promoventeEsContribuyente { get; set; }

        [Required]
        public string? rfcContribuyente { get; set; } 

        [Required]
        public string? contribuyente { get; set; } 
        
        public string? despachoAutorizado { get; set; }

        [Required]
        public string fechaPresentacion { get; set; } =null!;

        [Required]
        public string fechaRecepcion { get; set; } =null!;

         [Required]
         public int idTipoAsunto { get; set; } 

         [Required]
         public int idTipoModalidad { get; set; } 

   
        public int idAdministracion { get; set; }

    }
       
}