using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{

    public class RequestCrearArchivoRemision
    {
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public IFormFile fileConsultas { get; set; } = null!;

        [Required]
        public string? noFolio { get; set; }

     
        public int idRemision { get; set; }

        [Required]
        public int idTipoDocumento { get; set; }

        [Required]
        public int idSeccion { get; set; }

        public int? idDocumentoSeccion { get; set; } 

    }


}
