using Sicoj.Utils.Redis;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class ResponseConsultaByFilters:PortadorClass


    {
        //public int id { get; set; }
        public string? noAsunto { get; set; }
        public string? rfc { get; set; }
        public string? promovente { get; set; }
        public Boolean promoventeEsContribuyente { get; set; }
        public string? rfcContribuyente { get; set; }
        public string? contribuyente { get; set; }
        public string? despachoAutorizado { get; set; }
        public int idTipoAsunto { get; set; }
        public string tipoAsunto { get; set; } = string.Empty;
        public int idEstadoTarea { get; set; } = new();
        public string estadoTarea { get; set; } = string.Empty;
        public int idTipoModalidad { get; set; } = new();
        public string tipoModalidad { get; set; } = string.Empty;
        public int idEstadoProcesal { get; set; } = new();
        public string estadoProcesal { get; set; } = string.Empty;
        public int idAdministracionCentral { get; set; }
        public string administracionCentral { get; set; } = string.Empty;
        public int idAdministracion { get; set; }
        public string administracion { get; set; } = string.Empty;
        public int idsubadministracion { get; set; }
        public string subadministracion { get; set; } = string.Empty;
        public string? fechaRecepcion { get; set; }
        public string? fechaPresentacion { get; set; }
        public string? fechaVencimiento { get; set; }
        public string? fechaTurnado { get; set; }
        public string? idEmpleado { get; set; }
        public int idAlerta { get; set; }
        public string alerta { get; set; } = string.Empty;
        public string? idAbogado { get; set; }
        public string? abogado { get; set; } = string.Empty;
        public string? domicilio { get; set; }
        public string? domicilioNotificaciones { get; set; }
        public int idTema { get; set; }
        public string tema { get; set; } = string.Empty;
        public decimal monto { get; set; }
        public Boolean remitido { get; set; }
         public Boolean asignado { get; set; }
         public int idSeccionModificar { get; set; }
         public int idColorFechacssj { get; set; }
         public string colorFechacssj { get; set; } = string.Empty;
        public int idAlertaDG { get; set; }
        public string alertaDG { get; set; } = string.Empty;
        public int registroVence { get; set; }

        public int idSemaforo { get; set; }
        public string semaforo { get; set; } = string.Empty;

        public int idOrganoJurisdiccional { get; set; }
        public string? organoJurisdiccional { get; set; } = null!;
        public string? numeroJuicio { get; set; } = null!;
        public int tiporecurso { get; set; }
        public string? noRecurso { get; set; } = null!;
        public string? fechaFirmeza { get; set; }
        public string? fechaAsignacion { get; set; }
        public string? recurso { get; set; } = null!;
        public int idPlazoCumplimentar { get; set; }
        public string? plazoCumplimentar { get; set; } = null!;
        public string? oficioResolucion { get; set; } = null!;
        public string? fechaOficioResolucion { get; set; }
        public int horas { get; set; }    
        public int? idUnidadAdministrativaSolicitaCump { get; set; }
        public string UnidadAdministrativaSolicitaCump { get; set; } = string.Empty;


    }

}