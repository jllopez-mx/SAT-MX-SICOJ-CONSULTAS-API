namespace ConsultasAPI.Model.DTO.Response.Administrador
{
    public class ResponseReporteGeneral
    {

        //Datos Generales

        public string administracion { get; set; } = string.Empty;
        public string subadministracion { get; set; } = string.Empty;
        public string? no_asunto { get; set; } = null!;
        public string? rfc { get; set; } = null!;
        public string? promovente { get; set; } = null!;
        public string? rfc_contribuyente { get; set; }
        public string? contribuyente { get; set; } = null!;
        public string? despacho_autorizado { get; set; } = null!;
        public string? fecha_presentacion { get; set; }
        public string tipoAsunto { get; set; } = string.Empty;
        public string tema { get; set; } = string.Empty;
        public string? no_asunto_cumplimentar { get; set; } = null!; //Cumplimentacion
        public decimal? monto { get; set; }
        public string? fecha_recepcion { get; set; }
        public string? fecha_vencimiento { get; set; }
        public string estadoProcesal { get; set; } = string.Empty;


        //Asignación                
        public string? abogado { get; set; } = string.Empty;


        //Remisión

        public string? administracion_remite { get; set; } = string.Empty;
        public string? no_oficio_remision { get; set; } = null!;
        public string? fecha_oficio_remision { get; set; }

        //Requerimiento

        public string? no_oficio_Requerimiento { get; set; } = null!;
        public string? fecha_oficio_requerimiento { get; set; }
        public string? fecha_notificacion_requerimiento { get; set; }
        public string? fecha_vencimiento_Requerimiento { get; set; }
        public Boolean? atendio_requerimiento { get; set; }
        public string? fecha_atencion_requerimiento { get; set; }


        //Solicitud de opinion de la información

        public string unidadAdministrativaSolicitudInformacion { get; set; } = string.Empty;
        public string noOficioSolicitudInformacion { get; set; } = string.Empty;
        public string? fechaOficioSolicitudInformacion { get; set; }
        public Boolean atendioSolicitudInformacion { get; set; }
        public string noOficioRespuestaSolicitudInformacion { get; set; } = string.Empty;
        public string? fechaOficioRespuestaSolicitudInformacion { get; set; }
        public string? fechaRecepcionSolicitudInformacion { get; set; }


        //Emisión de la resolución

        public string? noOficioResolucion { get; set; } = null!;
        public string fechaOficionResolucion { get; set; } = null!;
        public string? sentido { get; set; } = null!;
        public string fechaOficioNotificacion { get; set; } = null!;


        //Requerimientos prodecon

        public string? noOficioProdecon { get; set; } = null!;
        public string? noExpedienteProdecon { get; set; } = null!;
        public string? fechaOficioProdecon { get; set; }
        public string? fechaIngresoProdecon { get; set; }
        public string? atencionProdecon { get; set; } = null!;


        //Solicitud Transparencia


        public string? noSolicitudTrasparencia { get; set; } = null!;
        public string? fechaSolicitudTransparencia { get; set; }


        //Información de la cumplimentación

        public string? numeroJuicioCumplimentacion { get; set; } = null!;
        public string? fecha_firmeza_cumplimnetacion { get; set; }
        public string? organoJurisdiccionalCumplimnetacion { get; set; } = null!;
        public string UnidadAdministrativaSolicitaCump { get; set; } = string.Empty;
        public string? oficioResolucionCumplimnetacion { get; set; } = null!;
        public string? fechaOficioResolucionCumplimentacion { get; set; }



        
    }
}