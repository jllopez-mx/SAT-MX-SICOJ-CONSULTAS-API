using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO.Contracts.Administrador
{
    public class RequestAsignarAdministrador
    {
        [Required]
        public int idConsulta { get; set; }
        
        [Required]
        public string idAbogado { get; set; } = null!;



    }
}