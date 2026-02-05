
using System.ComponentModel.DataAnnotations;

namespace ConsultasAPI.Model.DTO.Contracts.OficialPartes
{
    public class RequestDocumentoUpdate
    {
        public IFormFile documento { get; set; } = null!;
        
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public int idTipoDocumento { get; set; }
        
        [Required]
        public int id { get; set; }
        
        public int idTipoAsunto { get; set; }
        public string numeroFolio { get; set; } = null!;
    }
}