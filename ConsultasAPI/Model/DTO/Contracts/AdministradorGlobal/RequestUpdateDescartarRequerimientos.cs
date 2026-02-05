using System.ComponentModel.DataAnnotations;

namespace ConsultasAPI.Model.DTO
{
    public class RequestUpdateDescartarRequerimientos
    {

        [Required]
        public int idConsulta { get; set; }

        [Required]
        public int idSeccion { get; set; }

        [Required]
        public bool descartarUltimoRegistro { get; set; }

    }

}