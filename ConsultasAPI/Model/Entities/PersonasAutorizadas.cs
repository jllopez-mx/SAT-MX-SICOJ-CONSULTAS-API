namespace ConsultasAPI.Model.Entities
{
    public class PersonasAutorizadas
    {
        public int id { get; set; }
        public int id_consulta { get; set; }
        public int id_rol { get; set; }
        public string? nombre_rol { get; set; } = null!;
        public string? usuario { get; set; } = null!;
        public string? nombre { get; set; } = null!;
        public string? rfc { get; set; } = null!;
        public string? telefono { get; set; } = null!;
        public string? email { get; set; } = null!;
        public string? acciones { get; set; } = null!;
        public DateTime? fecha_registro { get; set; }
        public DateTime? fecha_modificacion { get; set; }
    }
}