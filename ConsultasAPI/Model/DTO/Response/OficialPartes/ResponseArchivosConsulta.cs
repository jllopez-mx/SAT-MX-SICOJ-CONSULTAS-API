using Sicoj.Utils.ViewModels;
namespace ConsultasAPI.Model.DTO
{
    public class ResponseArchivosConsulta
    {
        public int id { get; set; }
        public int idRol { get; set; }
        public string rol { get; set; } = string.Empty;
        public int id_consulta { get; set; }
        public int id_remision { get; set; }
        public string? folio { get; set; }
        public int idSeccion { get; set; }
        public string seccion { get; set; } = string.Empty;
        public string? nombre { get; set; } = null!;
        public string? path_file { get; set; } = null!;
        public int idTipoDocumento { get; set; }
        public string tipoDocumento { get; set; } = string.Empty;
        public string? owner_name { get; set; } = null!;
        public bool estatus { get; set; }
        public string? fecha_creacion { get; set; }
        public string? fecha_modificacion { get; set; }
        public bool activo { get; set; }
        public string? tamanoDocumento { get; set; }
        public int id_administracion { get; set; }
        public string administracion { get; set; } = string.Empty;
        public bool permanente  { get; set; }
        public bool remplazable {get;set;}
        public string? usuario_modificacion { get; set; }
        public string? usuario_creacion { get; set; }
        public int idDocumentoSeccion { get; set; }
    }
}