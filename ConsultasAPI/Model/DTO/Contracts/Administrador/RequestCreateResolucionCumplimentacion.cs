using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestCreateResolucionCumplimentacion
    {
        [Required]
        public int idCumplimentacion { get; set; }

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
