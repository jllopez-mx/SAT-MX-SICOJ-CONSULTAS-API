
using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{

    public class RequestCreaArchivoConsulta
    {

        [Required]
        public IFormFile fileConsultas { get; set; } = null!;

        [Required]
        public string? noFolio { get; set; }

        [Required]
        public int idConsulta { get; set; }

        [Required]
        public int idTipoDocumento { get; set; }

        [Required]
        public int idSeccion { get; set; } 
        
        public int? idDocumentoSeccion { get; set; } 




    }


}
