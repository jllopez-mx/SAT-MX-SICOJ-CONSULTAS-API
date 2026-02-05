using System.ComponentModel.DataAnnotations;

namespace ConsultasAPI.Model.DTO.Contracts.Administrador
{
    public class RequestUpdateAvisosYComunicados
    {
        [Required]
        public int id { get; set; }

        [Required]
        public int idConsulta { get; set; }

       [Required]
        public Boolean atencionAdicional { get; set; }

        public string descripcionAtencion { get; set; } =null!;


    }
}