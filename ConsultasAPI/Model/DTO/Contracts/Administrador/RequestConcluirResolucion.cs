using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestConcluirResolucion
    {
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public IFormFile? documento { get; set; }

        public string noFolio { get; set; } =null!;

        [Required]
        public int? idTipoArchivo { get; set; }

        [Required]
        public int? idSeccion { get; set; }

    }
       
}
