using Sicoj.Utils.Redis;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
  public class ResponseCumplimentacionList: PortadorClass
  {

    //public int id { get; set; }
        public string? rfc { get; set; } = null!;
        public string? promovente { get; set; } = null!;
        public Boolean promovente_es_contribuyente { get; set; }
        public string? rfc_contribuyente { get; set; }
        public string? contribuyente { get; set; }
        public int? id_tipo_asunto { get; set; }
        public string tipo_asunto { get; set; } = string.Empty;
        public int? id_tipo_modalidad { get; set; }
        public string tipo_modalidad { get; set; } = string.Empty;
        public int id_rol { get; set; }
        public string? nombre_rol { get; set; } = null!;
        public int id_consulta {get; set;}
        public string? no_asunto { get; set; } = null!;
        public string? fecha_recepcion { get; set; }
        public string? fecha_registro { get; set; }
        public string? fecha_vencimiento { get; set; }
        public string? fechaVencimientoCump { get; set; }
        public Boolean turnado { get; set; }
        public string? fecha_turnado { get; set; }
        public int? id_administracion { get; set; }
        public string Administracion { get; set; } = string.Empty;
        public int? id_Subadministracion { get; set; }
        public string Subadministracion { get; set; } = string.Empty;
        public string? id_abogado { get; set; }
        public string?  abogado { get; set; }
        public string? numero_empleado { get; set; } = string.Empty;
        public Boolean remitido { get; set; }
        public Boolean activo { get; set; }
        public string? fecha_modificacion { get; set; }
        public string? fecha_asignacion { get; set; }
        public Boolean asignado { get; set; }
        public string? numero_juicio { get; set; } = null!;
        public string? fecha_firmeza { get; set; }
        public int? plazoCumplimentar { get; set; }
        public string? no_asunto_consulta { get; set; } = null!;
        public string? usuario_creacion { get; set; } = string.Empty;


        //public string? usuario_creacion { get; set; } = string.Empty;
        //public string? usuario_modificacion { get; set; } = string.Empty;
        //public string? usuario_asigno { get; set; } = string.Empty;

    public int idTipoAsunto { get; set; }
    public string tipoAsunto { get; set; } = string.Empty;
    public int idTipoModalidad { get; set; }
    public string tipoModalidad { get; set; } = string.Empty;
    public string? despacho_autorizado { get; set; } = null!;
    public string? fecha_presentacion { get; set; }
    public int idAdministracionCentral { get; set; }
    public string administracionCentral { get; set; } = string.Empty;
    public int idAdministracion { get; set; }
    public string administracion { get; set; } = string.Empty;
    public int idSubadministracion { get; set; }
    public string subadministracion { get; set; } = string.Empty;
    public int idEstadoTarea { get; set; }
    public string estadoTarea { get; set; } = string.Empty;
    public int idEstadoProcesal { get; set; }
    public string estadoProcesal { get; set; } = string.Empty;
    public string? id_empleado { get; set; } = string.Empty;
    public int idTema { get; set; }
    public string tema { get; set; } = string.Empty;
    public string domicilioPromovente { get; set; } = string.Empty;
    public string domicilioNotificaciones { get; set; } = string.Empty;
    public string? idAbogado { get; set; }
     public Boolean abogadoActivo { get; set; }
    public decimal? monto { get; set; }
     public Boolean montoDeterminado { get; set; }
     public Boolean? solicita_requerimiento { get; set; }
     public Boolean concluido { get; set; }
    public string? fecha_CSSJ { get; set; }
    public int idColorFechacssj { get; set; }
    public string colorFechacssj { get; set; } = string.Empty;
    public int idAlerta { get; set; }
    public string alerta { get; set; } = string.Empty;
    public int idAlertaDG { get; set; }
    public string alertaDG { get; set; } = string.Empty;
    public int registroVence { get; set; }
    public int? id_organo_jurisdiccional { get; set; }
    public string? organoJurisdiccional { get; set; } = null!;
    public string? numeroJuicio { get; set; } = null!;
    public int tipo_recurso { get; set; }
    public int intentos { get; set; }
    public Boolean cumplimentar { get; set; }
    public int? idUnidadAdministativaCump { get; set; }
    public string unidadAdministativaCump { get; set; } = string.Empty;
    public int? idUnidadAdministrativaSolicitaCump { get; set; }
    public string UnidadAdministrativaSolicitaCump { get; set; } = string.Empty;
  }

}