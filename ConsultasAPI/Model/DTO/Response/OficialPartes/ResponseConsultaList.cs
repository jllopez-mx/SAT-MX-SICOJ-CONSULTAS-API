using Sicoj.Utils.Redis;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
  public class ResponseConsultaList: PortadorClass
  {

    //public int id { get; set; }
    public string? no_asunto { get; set; } = null!;
    public string? rfc { get; set; } = null!;
    public string? promovente { get; set; } = null!;
    public string? rfc_contribuyente { get; set; }
    public Boolean promovente_es_contribuyente { get; set; }
    public string? contribuyente { get; set; } = null!;
    public int idTipoAsunto { get; set; }
    public string tipoAsunto { get; set; } = string.Empty;
    public int idTipoModalidad { get; set; }
    public string tipoModalidad { get; set; } = string.Empty;
    public string? despacho_autorizado { get; set; } = null!;
    public string? fecha_presentacion { get; set; }
    public string? fecha_recepcion { get; set; }
    public string? fecha_registro { get; set; }
    public string? fecha_vencimiento { get; set; }
    public Boolean turnado { get; set; }
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
    public string? fecha_turnado { get; set; }
    public Boolean remitido { get; set; }
    public Boolean activo { get; set; }
    public int idTema { get; set; }
    public string tema { get; set; } = string.Empty;
    public string domicilioPromovente { get; set; } = string.Empty;
    public string domicilioNotificaciones { get; set; } = string.Empty;
    public string? idAbogado { get; set; }
    public string? abogado { get; set; } = string.Empty;
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

    public int intentos { get; set; }
    public Boolean cumplimentar { get; set; }
    public Boolean perteneceOtraUnidad { get; set; }
    public int idSeccionDescartar { get; set; }
    public int idSeccionModificar { get; set; }

    public string? semaforo { get; set; }
    public int idSemaforo { get; set; }


    public int idOrganoJurisdiccional { get; set; }
    public string? organoJurisdiccional { get; set; } = null!;
    public string? numeroJuicio { get; set; } = null!;
    public int tipo_recurso { get; set; }
      public string? noRecurso { get; set; } = null!;
    public string? fecha_firmeza { get; set; }
    public string? fecha_asignacion { get; set; }
    public string? recurso { get; set; } = null!;
    public int id_plazo_cumplimentar { get; set; }
    public string? plazo_cumplimentar { get; set; } = null!;
    public string? oficioResolucion { get; set; } = null!;
    public string? fechaOficioResolucion { get; set; }
    public int horas { get; set; }    
    public int? idUnidadAdministrativaSolicitaCump { get; set; }
    public string UnidadAdministrativaSolicitaCump { get; set; } = string.Empty;
    public Boolean asigno { get; set; }
    public string usuario_asigno { get; set; } = string.Empty;
  }

}