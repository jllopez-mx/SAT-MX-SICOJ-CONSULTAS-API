namespace ConsultasAPI.Model.DTO.Response.Administrador
{
    public class Response_Resolucion_PDF
    {
        public string? no_asunto { get; set; } = null!;
        public string? rfc { get; set; } = null!;
        public string? promovente { get; set; } = null!;
        public string? rfc_contribuyente { get; set; }
        public Boolean promovente_es_contribuyente { get; set; }
        public string? contribuyente { get; set; } = null!;
        public string administracionCentral { get; set; } = string.Empty;
        public int idAdministracion { get; set; }
        public string administracion { get; set; } = string.Empty;
        public int idSubadministracion { get; set; }

        public string? no_oficio { get; set; } = null!;
        public string fecha_notificacion { get; set; } = null!;
        public string fecha_resolucion { get; set; } = null!;
        public int? id_sentido { get; set; }
        public string? sentido { get; set; } = null!;
         public string abogado { get; set; } = null!;

    }
}