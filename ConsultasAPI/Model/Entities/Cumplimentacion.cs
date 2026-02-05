using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.Entities
{
    public class Cumplimentacion
    {
        public int id { get; set; }
        public string? rfc { get; set; } = null!;
        public string? promovente { get; set; } = null!;
        public Boolean promovente_es_contribuyente { get; set; }
        public string? rfc_contribuyente { get; set; }
        public string? contribuyente { get; set; }
        public int? id_tipo_asunto { get; set; }
        public string tipo_asunto { get; set; } = string.Empty;
        public int? id_tipo_modalidad { get; set; }
        public string tipo_modalidad { get; set; } = string.Empty;
        public int id_consulta {get; set;}
        public string? no_asunto { get; set; } = null!;
        public DateTime fecha_recepcion { get; set; }
        public DateTime fecha_registro { get; set; }
        public DateTime? fecha_vencimiento { get; set; }
        public DateTime? fechaVencimientoCump { get; set; }
        public Boolean turnado { get; set; }
        public DateTime fecha_turnado { get; set; }
        public int? id_administracion { get; set; }
        public string Administracion { get; set; } = string.Empty;
        public int? id_Subadministracion { get; set; }
        public string Subadministracion { get; set; } = string.Empty;
        public string? id_abogado { get; set; }
        public string? numero_empleado { get; set; } = string.Empty;
        public Boolean remitido { get; set; }
        public Boolean activo { get; set; }
        public DateTime fecha_asignacion { get; set; }
        public Boolean asignado { get; set; }
        public string? numero_juicio { get; set; } = null!;
        public DateTime? fecha_firmeza { get; set; }
        public int? plazoCumplimentar { get; set; }
        public string? no_asunto_consulta { get; set; } = null!;
        public string? usuario_creacion { get; set; } = string.Empty;
        public string? despacho_autorizado { get; set; } = null!;
        public DateTime fecha_presentacion { get; set; }
        public int ?idAdministracionCentral { get; set; }
        public string administracionCentral { get; set; } = string.Empty;
        public int idEstadoTarea { get; set; }
        public string estadoTarea { get; set; } = string.Empty;
        public int idEstadoProcesal { get; set; }
        public string estadoProcesal { get; set; } = string.Empty;
        public string? id_empleado { get; set; } = string.Empty;
        public int idColorFechacssj { get; set; }
        public string colorFechacssj { get; set; } = string.Empty;
        public int idAlertaDG { get; set; }
        public string alertaDG { get; set; } = string.Empty;
        public int registroVence { get; set; }
        public int? id_organo_jurisdiccional { get; set; }
        public string? organoJurisdiccional { get; set; } = null!;
        public int tipo_recurso { get; set; }
        public int? idUnidadAdministativaCump { get; set; }
        public string unidadAdministativaCump { get; set; } = string.Empty;
        public int? idUnidadAdministrativaSolicitaCump { get; set; }
        public string UnidadAdministrativaSolicitaCump { get; set; } = string.Empty;

    }
}