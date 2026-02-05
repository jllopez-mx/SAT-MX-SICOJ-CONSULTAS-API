using System.ComponentModel.DataAnnotations;

namespace ConsultasAPI.Model.DTO
{
    public class RequestUpdateDescartarResolucion
    {

        [Required]
        public int idConsulta { get; set; }

    }

}