using System.ComponentModel.DataAnnotations;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestUpdateResolucionCumplimentacion
    {
        [Required]
        public int id { get; set; }

        [Required]
        public int idCumplimentacion { get; set; }

        
        public string noOficioResolucion { get; set; }=string.Empty;

     
         public int idSentido { get; set; } 

       
        public string fechaResolucion { get; set; } =null!;

       
        public string fechaNotificacion { get; set; } =null!;        
      

    }
       
}
