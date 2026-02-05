using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestRemision
    {
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public int idAdministracionRemite { get; set; }


        [Required]
        public int idTipoAutoridad { get; set; }

        [Required]
        public string noOficioRemison { get; set; } = string.Empty;

        [Required]
        public string fechaOficio { get; set; } = string.Empty;

        [Required]
        public IFormFile? documento { get; set; }

        [Required]
        public int? idTipoArchivo { get; set; }

        [Required]
        public int? idSeccion { get; set; }


    }
}