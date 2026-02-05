using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestCreateResolucion
    {
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public string noOficioResolucion { get; set; }=string.Empty;

        [Required]
         public int idSentido { get; set; } 

        [Required]
        public string fechaResolucion { get; set; } =null!;

        [Required]
        public IFormFile? documento { get; set; }

        public string noFolio { get; set; } =null!;

        [Required]
        public int? idTipoArchivo { get; set; }

        [Required]
        public int? idSeccion { get; set; }

    }
       
}
