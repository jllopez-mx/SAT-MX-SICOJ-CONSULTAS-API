using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestTurnarCumplimentacion
    {
        [Required]
        public int id { get; set; }   

        [Required]
        public int idAdministracion { get; set; }     

    }
       
}