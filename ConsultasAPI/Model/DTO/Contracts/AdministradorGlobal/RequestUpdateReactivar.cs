using System.ComponentModel.DataAnnotations;

namespace ConsultasAPI.Model.DTO
{
    public class RequestUpdateReactivar
    {

        [Required]
        public int idConsulta { get; set; }

    }

}
