using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;


namespace ConsultasAPI.Model.DTO.Contracts.OficialPartes
{
    public class RequestUpdateSolicitudTransparencia
    {
        [Required]
        public int id { get; set; }

        [Required]
        public int idConsulta { get; set; }

        [Required]
        public string noSolicitud { get; set; }   =null!;     

       [Required]
        public string fechaSolicitud { get; set; } =null!;

    }
}
