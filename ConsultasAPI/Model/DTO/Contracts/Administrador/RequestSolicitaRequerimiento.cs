using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestSolicitaRequerimiento
    {
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public Boolean solicitaRequerimiento { get; set; }
       
    }
       
}