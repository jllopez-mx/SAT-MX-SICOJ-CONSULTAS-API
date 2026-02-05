using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestCreateRequerimiento
    {
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public string noOficioRequerimiento { get; set; }=string.Empty;

        [Required]
        public string fechaRequerimiento { get; set; } =null!;

        public IFormFile? documento { get; set; }
         public string noFolio { get; set; } =null!;
        public int? idTipoArchivo { get; set; }
        public int? idSeccion { get; set; }
    }
       
}