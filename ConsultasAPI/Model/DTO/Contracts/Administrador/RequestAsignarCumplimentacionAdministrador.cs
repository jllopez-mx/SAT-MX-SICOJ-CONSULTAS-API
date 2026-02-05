using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO.Contracts.Administrador
{
    public class RequestAsignarCumplimentacionAdministrador
    {
        [Required]
        public int idCumplimentacion { get; set; }
        
        [Required]
        public string idAbogado { get; set; } = null!;

        [Required]
        public int idAdministracion { get; set; }

        [Required]
        public int idSubadministracion { get; set; }



    }
}