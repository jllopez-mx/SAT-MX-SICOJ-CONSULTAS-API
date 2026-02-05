using System.ComponentModel.DataAnnotations;

namespace ConsultasAPI.Model.DTO
{
    public class RequestUpdateRequerimiento
    {
        [Required]
        public int id { get; set; }
        
        [Required]
        public int idConsulta { get; set; }

        public string noOficioRequerimiento { get; set; }=string.Empty;

        public string fechaRequerimiento { get; set; } =null!;
      
        public Boolean? atendio { get; set; }=null!;

        public string fechaNotificacion { get; set; } =null!;

        public string fechaAtencion { get; set; } =null!;

         public IFormFile? documento { get; set; }

        public string noFolio { get; set; } =null!;
         
        public int? idTipoArchivo { get; set; }
        public int? idSeccion { get; set; }

    }
       
}