using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;


namespace ConsultasAPI.Model.DTO.Contracts.OficialPartes
{
    public class RequestCreateSolicitudTransparencia
    {
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public string noSolicitud { get; set; }   =null!;     

       [Required]
        public string fechaSolicitud { get; set; } =null!;
        public IFormFile? documento { get; set; }
        public string noFolio { get; set; } =null!;
        public int? idTipoArchivo { get; set; }
        public int? idSeccion { get; set; }

    }
}
