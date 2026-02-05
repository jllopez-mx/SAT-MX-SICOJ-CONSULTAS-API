using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestCreateCumplimentacion
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

        [Required]
        public string fechaRecepcion { get; set; } =null!;

         [Required]
         public int idTipoAsunto { get; set; } 

         [Required]
         public int idTipoModalidad { get; set; } 

        public int? idAdministracion { get; set; }
        public int? idAdministracionSolicita { get; set; }
        [Required]
        public string? noAsuntoConsulta { get; set; }

        //[Required]
        public string? noJuicio { get; set; }

        //[Required]
        public string? fechaFirmeza { get; set; } =null!;

        //[Required]
        public int? idOrganoJurisdiccional { get; set; }

        //[Required]
        public int? plazoCumplimentar { get; set; }

        public string? fechaVencimiento { get; set; }


        public IFormFile? documento { get; set; }
         public string? noFolio { get; set; } =null!;
        public int? idTipoArchivo { get; set; }
        public int? idSeccion { get; set; }

    }
       
}