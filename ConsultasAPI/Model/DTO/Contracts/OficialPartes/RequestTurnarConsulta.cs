using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestTurnarConsulta
    {
        [Required]
        public int id { get; set; }        

    }
       
}