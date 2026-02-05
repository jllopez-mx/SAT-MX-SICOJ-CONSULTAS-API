namespace ConsultasAPI.Model.Entities
{
    public class RequerimientosProdecon
    {
        public int id { get; set; }
        public int id_consulta {get; set;}
        public int id_rol { get; set; }
        public string? nombre_rol { get; set; } = null!;
        public Boolean accionAdicional { get; set; }
        public string? no_oficio { get; set; } = null!;
        public string? no_expediente { get; set; } = null!;
        public DateTime fecha_oficio { get; set; }
        public DateTime fecha_ingreso { get; set; }
        public string? atencion { get; set; } = null!;
    }
}