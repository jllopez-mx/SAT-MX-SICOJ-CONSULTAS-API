using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestUpdateConsultaAbogadoImpuestosInternos
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
        public string? domicilioPromovente { get; set; }

        
        public string? domicilioNotificaciones { get; set; }

        [Required]
        public int idTema { get; set; }

        [Required]
        public decimal monto { get; set; }

        [Required]
        public string fechaRecepcion { get; set; } = null!;

        [Required]
        public string? despachoAutorizado { get; set; } = string.Empty;

        [Required]
        public int idTipoAsunto { get; set; } 

        [Required]
        public int idTipoModalidad { get; set; } 

        public int idSubadministracion { get; set; }

        
    }

}