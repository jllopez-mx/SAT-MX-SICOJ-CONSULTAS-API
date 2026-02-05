namespace ConsultasAPI.Model.Entities
{
    public class Resolucion
    {
        public int id { get; set; }
        public int id_consulta {get; set;}
        public int id_rol { get; set; }
        public string? nombre_rol { get; set; } = null!;
        public string? no_oficio { get; set; } = null!;
        public DateTime fecha_notificacion { get; set; }
        public DateTime fecha_resolucion { get; set; }
        public int? id_sentido { get; set; }
        public string? sentido { get; set; } = null!;
         public string? fecha_vencimiento { get; set; }
    }
}
