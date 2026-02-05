using System.ComponentModel.DataAnnotations;

namespace ConsultasAPI.Model.DTO.Contracts.Administrador
{
    public class RequestCreateSolicitudInformacion
    {
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public Boolean unidadEsInterna { get; set; }
  
        public int idUnidadAdministrativa { get; set; }

        public string unidadAdministrativaExterna { get; set; }=string.Empty;
        
        [Required]
        public string noOficioSolicitud { get; set; }=string.Empty;

     
        public string noOficioRespuesta { get; set; }=string.Empty;

        [Required]
        public Boolean atendioSolicitud { get; set; }

       [Required]
        public string fechaOficioSolicitud { get; set; } =null!;

        public string fechaOficioRespuesta { get; set; } =null!;

      
        public string fechaRecepcion { get; set; } =null!;
    }
}