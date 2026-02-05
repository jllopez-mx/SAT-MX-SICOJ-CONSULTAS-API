using System.ComponentModel.DataAnnotations;

namespace ConsultasAPI.Model.DTO
{
    public class RequestUpdateEstadoConsulta
    {

        [Required]
        public int idConsulta { get; set; }

    }

}
