using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.Entities
{
    public class Consulta
    {
        public int id { get; set; }
        public string? no_asunto { get; set; } = null!;
        public string? rfc { get; set; } = null!;
        public string? promovente { get; set; } = null!;
        public Boolean promovente_es_contribuyente { get; set; }
        public string? rfc_contribuyente { get; set; }
        public string? contribuyente { get; set; }
        public int? id_tipo_asunto { get; set; }
        public string tipo_asunto { get; set; } = string.Empty;
        public int? id_tipo_modalidad { get; set; }
        public string tipo_modalidad { get; set; } = string.Empty;
        public string? despacho_autorizado { get; set; } = string.Empty;
        public DateTime fecha_presentacion { get; set; }
        public DateTime fecha_recepcion { get; set; }
        public DateTime fecha_registro { get; set; }
        public DateTime? fecha_vencimiento { get; set; }
        public Boolean turnado { get; set; }
        public int? id_estado_tarea { get; set; }
        public string estado_tarea { get; set; } = string.Empty;
        public int id_estado_procesal { get; set; }
        public string estado_procesal { get; set; } = string.Empty;
        public string? numero_empleado { get; set; } = string.Empty;
        public DateTime fecha_turnado { get; set; }
        public Boolean status { get; set; }
        public Boolean remitido { get; set; }
        public DateTime fecha_actualizacion { get; set; }
        public DateTime fecha_asignacion { get; set; }
        public int? idRol { get; set; }
        public int? idUsuario { get; set; }
        public int? id_administracion { get; set; }
        public string Administracion { get; set; } = string.Empty;
        public int? id_Subadministracion { get; set; }
        public string Subadministracion { get; set; } = string.Empty;
        public int? id_administracion_central { get; set; }
        public string administracion_central { get; set; } = string.Empty;
        public string? usuario_creacion { get; set; } = string.Empty;
        public string? usuario_modificacion { get; set; } = string.Empty;
        public string? usuario_asigno { get; set; } = string.Empty;
        public string? id_abogado { get; set; }
        public string?  abogado { get; set; }
        public string estatus_operacion { get; set; } = string.Empty;
        public string domicilio_promovente { get; set; } = string.Empty;
        public string domicilio_notificaciones { get; set; } = string.Empty;
        public int? id_tema { get; set; }
        public string tema { get; set; } = string.Empty;
        public decimal? monto { get; set; }
       public Boolean monto_determinado { get; set; }
       public Boolean activo { get; set; }
       public Boolean asignar { get; set; }
       public Boolean concluido { get; set; }
       public Boolean solicita_requerimiento { get; set; }

    }
}