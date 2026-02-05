using Sicoj.Utils;
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.ViewModels.Enums;
namespace ConsultasAPI.Model.Entities.Events.OficialPartes
{
    public class EventsConsultasOficialPartes
    {
        public static Consulta CreateModalidadConsulta(
          string rfc,
          string promovente,
          bool promoventeEsContribuyente,
          string? rfcContribuyente,
          string? contribuyente,
          int? id_tipoAsunto,
          int? id_tipoModalidad,
          string despachoAutorizado,
          DateTime fechaPresentacion,
          DateTime fechaRecepcion,
          int? IdAdministracionCentral,
          string usuarioCreacion,
          int idAdministracion

      )
        {
            Guard.ValidateStringRfc(ref rfc!, "RFC");
            Guard.ValidateStringAlphanumeric(ref promovente!, "Promovente");
            Guard.ValidateStringRfc(ref rfcContribuyente, "RFC contribuyente", promoventeEsContribuyente);
            Guard.ValidateStringAlphanumeric(ref contribuyente, "Contribuyente", promoventeEsContribuyente);
            Guard.CatalogValue(ref id_tipoAsunto, "Tipo Asunto", true);
            Guard.CatalogValue(ref id_tipoModalidad, "Tipo Modalidad", true);
            Guard.CatalogValue(ref IdAdministracionCentral, "Unidad Administrativa Central", false);


            Consulta entity = new()
            {
                id_tipo_asunto = id_tipoAsunto,
                id_tipo_modalidad = id_tipoModalidad,
                rfc = rfc,
                promovente = promovente,
                promovente_es_contribuyente = promoventeEsContribuyente,
                rfc_contribuyente = rfcContribuyente,
                contribuyente = contribuyente,
                despacho_autorizado = despachoAutorizado,
                fecha_presentacion = fechaPresentacion,
                fecha_recepcion = fechaRecepcion,
                usuario_creacion = usuarioCreacion,
                id_administracion_central = IdAdministracionCentral,
                id_administracion = idAdministracion,
                id_Subadministracion = idAdministracion

            };
            return entity;
        }
        public static void UpdateModalidaConsulta(ref Consulta entity,
                   string rfc,
                   string promovente,
                   bool promoventeEsContribuyente,
                   string? rfcContribuyente,
                   string? contribuyente,
                   string despachoAutorizado,
                   string fechaPresentacion,
                   string fechaRecepcion,
                   int? id_tipoAsunto,
                   int? id_tipoModalidad,
                   string usuariomodificacion,
                    int? idAdministracion
               )
        {
            Guard.ValidateStringRfc(ref rfc!, "RFC");
            Guard.ValidateStringAlphanumeric(ref promovente!, "Promovente");
            Guard.ValidateStringAlphanumeric(ref despachoAutorizado!, "Despacho o Autorizados");
            Guard.CatalogValue(ref idAdministracion, "Unidad administrativa", false);

            entity.rfc = rfc;
            entity.promovente = promovente;
            entity.promovente_es_contribuyente = promoventeEsContribuyente;
            entity.rfc_contribuyente = rfcContribuyente;
            entity.contribuyente = contribuyente;
            entity.despacho_autorizado = despachoAutorizado;
            entity.fecha_presentacion = Convert.ToDateTime(fechaPresentacion);
            entity.fecha_recepcion = Convert.ToDateTime(fechaRecepcion);
            entity.id_tipo_asunto = id_tipoAsunto;
            entity.id_tipo_modalidad = id_tipoModalidad;
            entity.usuario_modificacion = usuariomodificacion;
            entity.Administracion = Convert.ToString(idAdministracion)!;


        }


        public static void TurnarModalidaConsulta(ref Consulta entity,
                   int id,
                   string numeroEmpleado
               )


        {


            entity.id = id;
            entity.numero_empleado = numeroEmpleado;

        }
        public static void DeleteModalidaConsulta(ref Consulta entity, int id

        )
        {
            entity.id = id;

        }
        public static void DeleteModalidaArchivoConsulta(ref ResponseArchivosConsulta entity

        )
        {

            entity.id = entity.id;


        }

        public static SolicitudTransparencia CreateModalidadSolicitudTransparencia(
            int idConsulta,
            string noSolicitud,
            DateTime fechaSolicitud

        )
        {

            SolicitudTransparencia entity = new()
            {
                id_rol = EnumRol.Oficial_De_Partes.GetHashCode(),
                id_consulta = idConsulta,
                noSolicitud = noSolicitud,
                fechaSolicitud = fechaSolicitud

            };
            return entity;
        }

        public static void UpdateSolicitudTransparencia(ref SolicitudTransparencia entity,
            int id,
            int idConsulta,
            string noSolicitud,
            DateTime fechaSolicitud
        )
        {
            entity.id = id;
            entity.id_consulta = idConsulta;
            entity.noSolicitud = noSolicitud;
            entity.fechaSolicitud = fechaSolicitud;
        }

        public static void DeleteSolicitudTransparencia(ref SolicitudTransparencia entity, int id

        )
        {
            entity.id = id;

        }

        #region Cumplimentación

        public static Cumplimentacion CreateCumplimentacion(
            string rfc,
            string promovente,
            bool promoventeEsContribuyente,
            string rfcContribuyente,
            string contribuyente,
            int idTipoAsunto,
            int idTipoModalidad,
            string noAsuntoConsulta,
            string noJuicio,
            DateTime fechaRecepcion,
            DateTime? fechaFirmeza,
            DateTime? fechaVencimiento,
            int? idOrganoJurisdiccional,
            int? idAdministracion,
            int? idAdministracionSolicita,
            int? plazoCumplimentar,
            string usuarioCreacion,
            int? IdAdministracionCentral
        )
        {
            Cumplimentacion entity = new()
            {
                rfc = rfc,
                promovente = promovente,
                promovente_es_contribuyente = promoventeEsContribuyente,
                rfc_contribuyente = rfcContribuyente,
                contribuyente = contribuyente,
                id_tipo_asunto = idTipoAsunto,
                id_tipo_modalidad = idTipoModalidad,
                no_asunto_consulta = noAsuntoConsulta,
                numero_juicio = noJuicio,
                fecha_recepcion = fechaRecepcion,
                fecha_firmeza = fechaFirmeza,
                fecha_vencimiento = fechaVencimiento,
                id_organo_jurisdiccional = idOrganoJurisdiccional,
                id_administracion = idAdministracion,
                id_Subadministracion = idAdministracion,
                idAdministracionCentral = IdAdministracionCentral,
                idUnidadAdministrativaSolicitaCump = idAdministracionSolicita,
                plazoCumplimentar = plazoCumplimentar,
                usuario_creacion = usuarioCreacion
            };
            return entity;
        }

        public static void TurnarCumplimentacion(ref Cumplimentacion entity,
            int id,
            int idAdministracion,
            string numeroEmpleado
        )
        {


            entity.id = id;
            entity.id_administracion = idAdministracion;
            entity.numero_empleado = numeroEmpleado;

        }

        public static void UpdateCumplimentacion(ref Cumplimentacion entity,
            string noJuicio,
            DateTime fechaRecepcion,
            DateTime? fechaFirmeza,
            DateTime? fechaVencimiento,
            int? idOrganoJurisdiccional,
            int? idAdministracion,
            int? idAdministracionSolicita,
            int plazoCumplimentar,
              int idsubadministracion
        )
        {
            entity.numero_juicio = noJuicio;
            entity.fecha_recepcion = Convert.ToDateTime(fechaRecepcion);
            entity.fecha_firmeza = Convert.ToDateTime(fechaFirmeza);
            entity.fecha_vencimiento = Convert.ToDateTime(fechaVencimiento);
            entity.id_organo_jurisdiccional = idOrganoJurisdiccional;
            entity.id_administracion = idAdministracion;
            entity.idUnidadAdministrativaSolicitaCump = idAdministracionSolicita;
            entity.plazoCumplimentar = plazoCumplimentar;
            entity.id_Subadministracion = idsubadministracion;
        }

        #endregion

        #region Email

        public static Email CreaCorreo(
        List<string> to,
         List<string> Cc,
         string Subject,
         bool IsHtml,
         string Body

     )
        {

            Email entity = new()
            {
                To = to,
                Cc = Cc,
                Subject = Subject,
                IsHtml = IsHtml,
                Body = Body,
                Priority = (System.Net.Mail.MailPriority?)1,
            };


            return entity;
        }

        #endregion
    }
}