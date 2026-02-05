using System.ComponentModel.DataAnnotations;

namespace ConsultasAPI.Model.DTO.Contracts.Administrador
{
    public class RequestCreateAvisosYComunicados
    {
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public int idTipoAviso { get; set; }

        
        public string folio { get; set; }=string.Empty;

        [Required]
        public Boolean tieneFolio { get; set; }

        [Required]
        public string fechaIngreso { get; set; } =null!;

      
        public string observaciones { get; set; } =null!;
      

    }
}