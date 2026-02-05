using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO.Contracts.Abogado
{
    public class RequestAsignarAbogado
    {
        [Required]
        public int id_Consulta { get; set; }
        [Required]
        public string idAbogado { get; set; } = null!;



    }
}