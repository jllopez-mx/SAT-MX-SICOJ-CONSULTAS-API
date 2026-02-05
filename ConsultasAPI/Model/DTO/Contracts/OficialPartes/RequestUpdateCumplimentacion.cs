using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestUpdateCumplimentacion
    {

        [Required]
        public int id { get; set; }

        //[Required]
        public string? noJuicio { get; set; }

        [Required]
        public string fechaRecepcion { get; set; } =null!;

        //[Required]
        public string? fechaFirmeza { get; set; } =null!;

        //[Required]
        public int? idOrganoJurisdiccional { get; set; }

        [Required]
        public int plazoCumplimentar { get; set; }

        public string? fechaVencimiento { get; set; }

        public int? idAdministracion { get; set; }
        public int? idAdministracionSolicita { get; set; }

        public int idSubadministracion  { get; set; }

    }

}