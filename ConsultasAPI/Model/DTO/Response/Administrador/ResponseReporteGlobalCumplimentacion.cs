namespace ConsultasAPI.Model.DTO.Response.Administrador
{
    public class ResponseReporteGlobalCumplimentacion
    {

        //Datos Generales

        public string administracion { get; set; } = string.Empty;
        public string subadministracion { get; set; } = string.Empty;
        public string? no_asunto { get; set; } = null!;
        public string? rfc { get; set; } = null!;
        public string? promovente { get; set; } = null!;
        public string? rfc_contribuyente { get; set; }
        public string? contribuyente { get; set; } = null!;
        public string? fecha_recepcion { get; set; }
        public string tipoAsunto { get; set; } = string.Empty;
        public string tema { get; set; } = string.Empty;
        public string? no_asunto_cumplimentar { get; set; } = null!;
        public string? fecha_vencimiento { get; set; }
        public string estadoProcesal { get; set; } = string.Empty;


        //Asignación                
        public string? abogado { get; set; } = string.Empty;



        //Emisión de la resolución

        public string? noOficioResolucion { get; set; } = null!;
        public string fechaOficionResolucion { get; set; } = null!;
        public string? sentido { get; set; } = null!;
        public string fechaOficioNotificacion { get; set; } = null!;


        //Información de la cumplimentación

        public string? numeroJuicioCumplimentacion { get; set; } = null!;
        public string? fecha_firmeza_cumplimnetacion { get; set; }
        public string? organoJurisdiccionalCumplimnetacion { get; set; } = null!;
        public string UnidadAdministrativaSolicitaCump { get; set; } = string.Empty;
        public string? oficioResolucionCumplimnetacion { get; set; } = null!;
        public string? fechaOficioResolucionCumplimentacion { get; set; }


    }
}