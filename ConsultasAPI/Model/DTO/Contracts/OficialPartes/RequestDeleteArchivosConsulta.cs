using System.ComponentModel.DataAnnotations;


namespace ConsultasAPI.Model.DTO
{
    public class RequestDeleteArchivosConsulta
    {
        [Required]
        public int idConsulta { get; set; }

         [Required]
        public List<int> id { get; set; } = null!;
    }
}