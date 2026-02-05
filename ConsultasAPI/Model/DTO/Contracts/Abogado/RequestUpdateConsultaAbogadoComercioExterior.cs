using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;


namespace ConsultasAPI.Model.DTO.Contracts.Abogado
{
    public class RequestUpdateConsultaAbogadoComercioExterior
    {
        [Required]
        public int id { get; set; }

        [Required]
        public Boolean promoventeEsContribuyente { get; set; }

        [Required]
        public string? rfcContribuyente { get; set; }

        [Required]
        public string? contribuyente { get; set; }

        [Required]
        public string fechaPresentacion { get; set; } = null!;

        [Required]
        public string fechaRecepcion { get; set; } = null!;

        [Required]
        public int idTipoAsunto { get; set; } 

        [Required]
        public int idTipoModalidad { get; set; }

        public int idSubadministracion { get; set; }

    }
}