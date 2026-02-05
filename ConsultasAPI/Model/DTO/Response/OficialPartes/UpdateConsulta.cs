
namespace ConsultasAPI.Model.DTO
{
    public class UpdateConsulta
    {
     
        public int id { get; set; }
        public string rfc { get; set; } = null!;     
        public string promovente { get; set; } = null!;       
        public Boolean promovente_es_contribuyente { get; set; }
        public string? rfc_contribuyente { get; set; }
        public string? contribuyente { get; set; } 
        public string? despacho_autorizado { get; set; }       
        public DateTime fecha_presentacion { get; set; }     
        public DateTime fecha_recepcion { get; set; }      
        public int? id_tipo_asunto { get; set; } = new();       
        public int? id_tipo_modalidad { get; set; } = new();
    }
}