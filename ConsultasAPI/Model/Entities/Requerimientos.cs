namespace ConsultasAPI.Model.Entities
{
    public class Requerimientos
    {
        public int id { get; set; }
        public int id_consulta {get; set;}
        public int id_rol { get; set; }
        public string? nombre_rol { get; set; } = null!;
        public Boolean? atendio { get; set; } = null!;
        public string? no_oficio { get; set; } = null!;
        public DateTime fecha_notificacion { get; set; }
        public DateTime fecha_requerimiento { get; set; }
        public DateTime fecha_vencimiento { get; set; }
        public DateTime? fecha_atencion { get; set; }
    }
}