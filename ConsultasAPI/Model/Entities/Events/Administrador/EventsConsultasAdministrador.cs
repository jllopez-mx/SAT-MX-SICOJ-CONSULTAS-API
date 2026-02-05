
using Sicoj.Utils;
using ConsultasAPI.Model.DTO;
using Sicoj.Utils.Models;
using ConsultasAPI.Model.ViewModels.Enums;
using ClosedXML.Excel;
using ConsultasAPI.Model.DTO.Response.Administrador;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;

namespace ConsultasAPI.Model.Entities.Events.Administrador
{
    public class EventsConsultasAdministrador
    {
        public static ArchivoConsulta CreateDocumento(
            int id_consulta,
            int? id_tipo_documento,
            int id_seccion,
            int id_administracion,
            string file_name,
            string path_file,
            string content_type,
            string owner_name,
            string usuario_creacion,
            string size

        )
        {

            ArchivoConsulta entity = new()
            {
                id_rol = EnumRol.Administrador.GetHashCode(),
                id_consulta = id_consulta,
                id_tipo_documento = id_tipo_documento,
                file_name = file_name,
                path_file = path_file,
                content_type = content_type,
                owner_name = owner_name,
                usuario_creacion = usuario_creacion,
                id_seccion = id_seccion,
                size = size,
                estatus = true,
                id_administracion = id_administracion
            };
            return entity;
        }
        public static void UpdateModalidaConsultaAdministradorComercioExterior(ref Consulta entity,
                    bool promoventeEsContribuyente,
                   string? rfcContribuyente,
                   string? contribuyente,
                   string fechaPresentacion,
                   string fechaRecepcion,
                   int? id_tipoAsunto,
                   int? id_tipoModalidad,
                   int idsubadministracion
               )
        {
            Guard.ValidateStringRfc(ref rfcContribuyente!, "RFC");
            Guard.ValidateStringAlphanumeric(ref contribuyente!, "Promovente");


            entity.promovente_es_contribuyente = promoventeEsContribuyente;
            entity.rfc_contribuyente = rfcContribuyente;
            entity.contribuyente = contribuyente;
            entity.fecha_presentacion = Convert.ToDateTime(fechaPresentacion);
            entity.fecha_recepcion = Convert.ToDateTime(fechaRecepcion);
            entity.id_tipo_asunto = id_tipoAsunto;
            entity.id_tipo_modalidad = id_tipoModalidad;
            entity.id_Subadministracion = idsubadministracion;
        }

        public static void UpdateModalidaConsultaAdministradorImpuestosInternos(ref Consulta entity,
                 bool promoventeEsContribuyente,
                string? rfcContribuyente,
                string? contribuyente,
                string? fechaRecepcion,
                string despachoAutorizado,
                string domicilioPromovente,
                string domicilioNotificaciones,
                int? id_tema,
                decimal monto,
                string usuarioasigno,
                int? id_tipoAsunto,
                int? id_tipoModalidad,
                int idsubadministracion,
                bool montoDeterminado
            )
        {
            Guard.CatalogValue(ref id_tema, "tema", true);
            Guard.ValidateStringRfc(ref rfcContribuyente!, "RFC");
            Guard.ValidateStringAlphanumeric(ref contribuyente!, "Promovente");
            Guard.ValidateStringAlphanumeric(ref despachoAutorizado!, "despacho Autorizado");
            Guard.ValidateStringAlphanumeric(ref usuarioasigno!, "usuario");
            Guard.ValidateDecimal(monto, "Monto", false);

            entity.promovente_es_contribuyente = promoventeEsContribuyente;
            entity.rfc_contribuyente = rfcContribuyente;
            entity.contribuyente = contribuyente;
            entity.fecha_recepcion = Convert.ToDateTime(fechaRecepcion);
            entity.despacho_autorizado = despachoAutorizado;
            entity.domicilio_promovente = domicilioPromovente;
            entity.domicilio_notificaciones = domicilioNotificaciones;
            entity.id_tema = id_tema;
            entity.monto = monto;
            entity.usuario_asigno = usuarioasigno;
            entity.id_tipo_asunto = id_tipoAsunto;
            entity.id_tipo_modalidad = id_tipoModalidad;
            entity.id_Subadministracion = idsubadministracion;
            entity.monto_determinado = montoDeterminado;

        }

        public static PersonasAutorizadas CreatePersonasAutorizadas(
            int idConsulta,
            string nombre,
            string rfc,
            string telefono,
            string email
        )
        {
            Guard.ValidateStringEmpty(ref nombre!, "Nombre");
            Guard.ValidateStringRfc(ref rfc!, "RFC");

            PersonasAutorizadas entity = new()
            {
                id_consulta = idConsulta,
                nombre = nombre,
                rfc = rfc,
                telefono = telefono,
                email = email,
                id_rol = EnumRol.Administrador.GetHashCode()
            };
            return entity;
        }

        public static void UpdatePersonasAutorizadas(ref PersonasAutorizadas entity,
            string rfc,
            string nombre,
            string? telefono,
            string? email,
            int id_consulta
        )
        {
            Guard.ValidateStringAlphanumeric(ref nombre!, "Nombre");
            Guard.ValidateStringRfc(ref rfc!, "RFC");


            entity.rfc = rfc;
            entity.nombre = nombre;
            entity.telefono = telefono;
            entity.email = email;
            entity.id_consulta = id_consulta;
        }

        public static void DeletePersonasAutorizadas(ref PersonasAutorizadas entity, int id

        )
        {
            entity.id = id;

        }

        public static Remision RemisionModalidaConsultaAdministrador(
                        int idConsulta,
                        int? AdministracionRemite,
                        string noOficiRemison,
                        int? tipoAutoridad,
                       DateTime fechaOficio,
                       string rfcValue,
                       string usuario
                   )
        {
            Guard.CatalogValue(ref AdministracionRemite, "Unidad Administrativa que Remite", true);
            Guard.CatalogValue(ref tipoAutoridad, "Tipo de Autoridad", true);
            Guard.ValidateStringAlphanumeric(ref noOficiRemison!, "Nomero de oficio");


            Remision entity = new()
            {
                id_consulta = idConsulta,
                id_administracion_remite = AdministracionRemite,
                no_oficio_remision = noOficiRemison,
                id_tipo_autoridad = tipoAutoridad,
                fecha_oficio = fechaOficio,
                usuario = usuario,
                id_rol = EnumRol.Administrador.GetHashCode()
            };
            return entity;
        }

        public static PersonasAutorizadas GetPersonasAutorizadasAdministrador()
        {
            PersonasAutorizadas entity = new()
            {
                id_rol = EnumRol.Administrador.GetHashCode()
            };
            return entity;
        }

        public static Remision GetRemisionAdministrador()
        {
            Remision entity = new()
            {
                id_rol = EnumRol.Administrador.GetHashCode()
            };
            return entity;
        }

        public static void DeleteModalidaArchivoConsulta(ref ResponseArchivosConsulta entity

        )
        {

            entity.id = entity.id;


        }

        public static void AsignarAdministrador(ref Consulta entity,
            int id_Consulta,
            string? id_abogado,
            string? usuario_asigno
        )
        {

            entity.id_abogado = id_abogado;
            entity.usuario_asigno = usuario_asigno;
        }

        public static Requerimientos CreateRequerimiento(
            int id_consulta,
            string? no_oficio,
            DateTime fecha_requerimiento
        )
        {
            Guard.ValidateStringAlphanumeric(ref no_oficio!, "Numero de oficio");

            Requerimientos entity = new()
            {
                id_consulta = id_consulta,
                id_rol = EnumRol.Administrador.GetHashCode(),
                no_oficio = no_oficio,
                fecha_requerimiento = fecha_requerimiento
            };

            return entity;
        }
        public static Consulta SolicitaRequerimiento(
        int id_consulta,
        bool solicitaRequerimiento
    )
        {

            Consulta entity = new()
            {
                id = id_consulta,
                solicita_requerimiento = solicitaRequerimiento
            };

            return entity;
        }

        public static void UpdateRequerimientos(ref Requerimientos entity,
            int id_consulta,
            bool? atendio,
            DateTime fecha_notificacion,
            DateTime? fecha_atencion,
            string? no_oficio,
            DateTime fecha_requerimiento
        )
        {


            entity.id_consulta = id_consulta;
            entity.atendio = atendio;
            entity.fecha_notificacion = fecha_notificacion;
            entity.fecha_atencion = fecha_atencion;
            entity.no_oficio = no_oficio;
            entity.fecha_requerimiento = fecha_requerimiento;
            entity.id_rol = EnumRol.Administrador.GetHashCode();
        }

        //Solicitud Información

        public static SolicitudInformacion CreateSolicitudInformacion(
            int id_consulta,
            int? id_unidad_administrativa,
            string? no_oficio_solicitud,
            string? no_oficio_respuesta,
            bool atendio_solicitud,
            DateTime fecha_oficio_solicitud,
            DateTime fecha_oficio_respuesta,
            DateTime fecha_recepion,
            bool unidadEsInterna,
            string unidadAdministrativaExterna
        )

        {



            SolicitudInformacion entity = new()
            {
                id_consulta = id_consulta,
                id_unidad_administrativa = id_unidad_administrativa,
                no_oficio_solicitud = no_oficio_solicitud,
                no_oficio_respuesta = no_oficio_respuesta,
                id_rol = EnumRol.Administrador.GetHashCode(),
                atendio_solicitud = atendio_solicitud,
                fecha_oficio_solicitud = fecha_oficio_solicitud,
                fecha_oficio_respuesta = fecha_oficio_respuesta,
                fecha_recepcion = fecha_recepion,
                unidadEsInterna = unidadEsInterna,
                unidad_Administrativa_Externa = unidadAdministrativaExterna

            };
            return entity;


        }

        public static SolicitudInformacion CreateSolicitudInformacionSinAtencion(
           int id_consulta,
           int? id_unidad_administrativa,
           string? no_oficio_solicitud,
           string? no_oficio_respuesta,
           bool atendio_solicitud,
           bool unidadEsInterna,
           string unidadAdministrativaExterna
       )

        {


            SolicitudInformacion entity = new()
            {
                id_consulta = id_consulta,
                id_unidad_administrativa = id_unidad_administrativa,
                no_oficio_solicitud = no_oficio_solicitud,
                no_oficio_respuesta = no_oficio_respuesta,
                id_rol = EnumRol.Administrador.GetHashCode(),
                atendio_solicitud = atendio_solicitud,
                unidadEsInterna = unidadEsInterna,
                unidad_Administrativa_Externa = unidadAdministrativaExterna

            };
            return entity;


        }

        public static void RequestUpdateSolicitudInformacion(ref SolicitudInformacion entity,
          int id_consulta,
          int idUnidadAdministrativa,
          string? noOficioSolicitud,
          DateTime fechaOficioSolicitud,
          bool atendioSolicitud,
          string? noOficioRespuesta,
          DateTime fechaOficioRespuesta,
          DateTime fechaRecepcion,
          bool unidadEsInterna,
          string unidadAdministrativaExterna
      )
        {
            Guard.ValidateStringAlphanumeric(ref noOficioSolicitud!, "Numero de oficio de solicitud");
            Guard.ValidateStringAlphanumeric(ref noOficioRespuesta!, "Numero de oficio de respuesta");

            entity.id_consulta = id_consulta;
            entity.id_unidad_administrativa = idUnidadAdministrativa;
            entity.no_oficio_solicitud = noOficioSolicitud;
            entity.fecha_oficio_solicitud = fechaOficioSolicitud;
            entity.atendio_solicitud = atendioSolicitud;
            entity.no_oficio_respuesta = noOficioRespuesta;
            entity.fecha_recepcion = fechaRecepcion;
            entity.unidadEsInterna = unidadEsInterna;
            entity.unidad_Administrativa_Externa = unidadAdministrativaExterna;
        }

        public static void RequestUpdateSolicitudInformacionSinAtencion(ref SolicitudInformacion entity,
             int id_consulta,
           int idUnidadAdministrativa,
           string? noOficioSolicitud,
           DateTime fechaOficioSolicitud,
           bool atendioSolicitud,
           string? noOficioRespuesta,
           bool unidadEsInterna,
           string unidadAdministrativaExterna
       )

        {

            entity.id_consulta = id_consulta;
            entity.id_unidad_administrativa = idUnidadAdministrativa;
            entity.no_oficio_solicitud = noOficioSolicitud;
            entity.fecha_oficio_solicitud = fechaOficioSolicitud;
            entity.atendio_solicitud = atendioSolicitud;
            entity.no_oficio_respuesta = noOficioRespuesta;
            entity.unidadEsInterna = unidadEsInterna;
            entity.unidad_Administrativa_Externa = unidadAdministrativaExterna;
        }

        public static Resolucion CreateResolucion(
           int id_consulta,
           string? no_oficio,
           DateTime fecha_resolucion,
           int id_sentido
       )
        {
            Guard.ValidateString(ref no_oficio!, "Numero de oficio");

            Resolucion entity = new()
            {
                id_consulta = id_consulta,
                id_rol = EnumRol.Administrador.GetHashCode(),
                no_oficio = no_oficio,
                fecha_resolucion = fecha_resolucion,
                id_sentido = id_sentido
            };

            return entity;
        }

        public static void UpdateResolucion(ref Resolucion entity,
            int id_consulta,
            string? no_oficio,
            DateTime fecha_resolucion,
            DateTime fecha_notificacion,
            int id_sentido
        )
        {
            Guard.ValidateString(ref no_oficio!, "Numero de oficio");

            entity.id_consulta = id_consulta;
            entity.no_oficio = no_oficio;
            entity.fecha_notificacion = fecha_notificacion;
            entity.fecha_resolucion = fecha_resolucion;
            entity.id_sentido = id_sentido;
        }

        public static void ConcluirResolucion(ref Resolucion entity,
         int idConsulta
     )
        {

            entity.id_consulta = idConsulta;
        }

        public static RequerimientosProdecon CreateRequerimientoProdecon(
            int id_consulta,
            string? no_oficio,
            string? no_expediente,
            DateTime fecha_oficio,
            DateTime fecha_ingreso,
            string? atencion,
            bool accionAdicional
        )
        {
            Guard.ValidateStringAlphanumeric(ref no_oficio!, "Numero de oficio");
            Guard.ValidateStringAlphanumeric(ref no_expediente!, "Numero de oficio");

            RequerimientosProdecon entity = new()
            {
                id_consulta = id_consulta,
                id_rol = EnumRol.Administrador.GetHashCode(),
                no_oficio = no_oficio,
                no_expediente = no_expediente,
                fecha_ingreso = fecha_ingreso,
                fecha_oficio = fecha_oficio,
                atencion = atencion,
                accionAdicional = accionAdicional
            };

            return entity;
        }

        public static void UpdateRequerimientosProdecon(ref RequerimientosProdecon entity,
            int id_consulta,
            string? no_oficio,
            string? no_expediente,
            DateTime fecha_oficio,
            DateTime fecha_ingreso,
            string? atencion,
            bool accionAdicional
        )
        {
            Guard.ValidateStringAlphanumeric(ref no_oficio!, "Numero de oficio");
            Guard.ValidateStringAlphanumeric(ref no_expediente!, "Numero de oficio");

            entity.id_consulta = id_consulta;
            entity.no_expediente = no_expediente;
            entity.no_oficio = no_oficio;
            entity.fecha_oficio = fecha_oficio;
            entity.fecha_ingreso = fecha_ingreso;
            entity.atencion = atencion;
            entity.accionAdicional = accionAdicional;
        }

        public static Reasignar UpdateReasignar(ref Consulta entity,
           string? usuarioModificacion,
           string? rfcNuevoAbogado,
           string? rfcAntiguoAbogado,
           int? idUnidadAdministrativaReasignador,
           int? idSubadministrativaReasignador,
           int? idUnidadAdministrativaReasignado,
           int? idSubadministracioReasignado
       )
        {
            switch (entity.id_estado_procesal)
            {
                case EnumEstadoProcesalCons.Concluido_Notificado:
                    throw new Exception("La autorización no puede ser reasingnada por que tiene el estado de procesal: concluido notificado.");
                case EnumEstadoProcesalCons.Concluido_Remitido:
                    throw new Exception("La autorización no puede ser reasingnada por que tiene el estado de procesal: concluido remitido.");
                case EnumEstadoProcesalCons.Aviso_Concluido:
                    throw new Exception("La autorización no puede ser reasingnada por que tiene el estado de procesal: aviso concluido.");
                default:
                    break;
            }

            switch (entity.id_estado_tarea)
            {
                case EnumEstadoTareaCons.Pendiente_de_registrar:
                    throw new Exception("La autorización no puede ser reasingnada por que tiene el estado de tarea: pendiente de registrar.");
                case EnumEstadoTareaCons.Pendiente_de_turnar:
                    throw new Exception("La autorización no puede ser reasingnada por que tiene el estado de tarea: pendiente de turnar.");
                case EnumEstadoTareaCons.Pendiente_de_asignar:
                    throw new Exception("La autorización no puede ser reasingnada por que tiene el estado de tarea: pendiente de asignar.");
                case EnumEstadoTareaCons.Concluido_Remitido:
                    throw new Exception("La autorización no puede ser reasingnada por que tiene el estado de tarea: concluido remitido.");
                default:
                    break;
            }

            if (string.IsNullOrEmpty(rfcAntiguoAbogado))
                throw new Exception("El asunto no se tiene asignado un abogado.");

            if (rfcAntiguoAbogado == rfcNuevoAbogado)
                throw new Exception("El asunto no se puede asignar al mismo abogado.");

            entity.id_estado_tarea = EnumEstadoTareaCons.Reasingado;
            entity.usuario_modificacion = usuarioModificacion;

            Reasignar entityReasignacion = new()
            {
                id_consulta = entity.id,
                rfc_funcionario_reasignador = usuarioModificacion!,
                rfc_funcionario_reasignado = rfcNuevoAbogado!,
                rfc_funcionario_retirado = rfcAntiguoAbogado!,
                id_unidad_administrativa_reasingador = idUnidadAdministrativaReasignador.GetValueOrDefault(),
                id_subadministracion_reasignador = idSubadministrativaReasignador.GetValueOrDefault(),
                id_unidad_administrativa_reasignado = idUnidadAdministrativaReasignado.GetValueOrDefault(),
                id_subadministracion_reasignado = idSubadministracioReasignado.GetValueOrDefault(),
                id_estado_procesal_previo = entity.id_estado_procesal
            };

            return entityReasignacion;
        }

        public static AvisosComunicados CreateAvisosYComunicados(
            int id_consulta,
            int id_tipo_aviso,
            string? folio,
            DateTime fecha_ingreso,
            string? observaciones,
            bool tieneFolio
        )
        {
            if (tieneFolio)
            {
                Guard.ValidateStringAlphanumeric(ref folio!, "Numero de oficio");
            }
            AvisosComunicados entity = new()
            {
                id_consulta = id_consulta,
                id_rol = EnumRol.Administrador.GetHashCode(),
                id_tipo_aviso = id_tipo_aviso,
                folio = folio!,
                fecha_ingreso = fecha_ingreso,
                observaciones = observaciones!,
                tiene_folio = tieneFolio
            };
            

            return entity;
        }
        public static void UpdateAvisosYComunicados(ref AvisosComunicados entity,
           int id_consulta,
           bool atencionAdicional,
           string? atencion

       )
        {

            entity.id_consulta = id_consulta;
            entity.atencion_adicional = atencionAdicional;
            entity.descripcion_atencion = atencion!;

        }

        public static SolicitudTransparencia CreateModalidadSolicitudTransparencia(
            int idConsulta,
            string noSolicitud,
            DateTime fechaSolicitud

        )
        {

            SolicitudTransparencia entity = new()
            {
                id_rol = EnumRol.Administrador.GetHashCode(),
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

        internal static Reasignar UpdateReasignar(ref Consulta entity, string? rfc1, string? rfc2, string? id_abogado, int? idAdministracion1, int? idSubadministracion1, int? idAdministracion2, int? idSubadministracion2, object estesisepuede)
        {
            throw new NotImplementedException();
        }

        public static void AsignarCumplimentacionAdministrador(ref Cumplimentacion entity,
            int id_Cumplimentacion,
            string? id_abogado,
            int id_Administracion,
            int id_Subadministracion
        )
        {
            entity.id = id_Cumplimentacion;
            entity.id_abogado = id_abogado;
            entity.id_administracion = id_Administracion;
            entity.id_Subadministracion = id_Subadministracion;
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

        public static ResolucionCumplimentacion CreateResolucionCumplimentacion(
            int id_cumplimentacion,
            string? no_oficio,
            DateTime fecha_resolucion,
            int id_sentido
        )
        {
            Guard.ValidateString(ref no_oficio!, "Numero de oficio");

            ResolucionCumplimentacion entity = new()
            {
                id_cumplimentacion = id_cumplimentacion,
                id_rol = EnumRol.Administrador.GetHashCode(),
                no_oficio = no_oficio,
                fecha_resolucion = fecha_resolucion,
                id_sentido = id_sentido
            };

            return entity;
        }

        public static void UpdateResolucionCumplimentacion(ref ResolucionCumplimentacion entity,
            int id_cumplimentacion,
            string? no_oficio,
            DateTime fecha_resolucion,
            DateTime fecha_notificacion,
            int id_sentido
        )
        {
            Guard.ValidateString(ref no_oficio!, "Numero de oficio");

            entity.id_cumplimentacion = id_cumplimentacion;
            entity.no_oficio = no_oficio;
            entity.fecha_notificacion = fecha_notificacion;
            entity.fecha_resolucion = fecha_resolucion;
            entity.id_sentido = id_sentido;
        }


        public static void concluirResolucionCumplimentacion(ref ResolucionCumplimentacion entity,
            int idCumplimentacion
        )
        {

            entity.id_cumplimentacion = idCumplimentacion;
        }

        public static XLWorkbook GenerarExcelClosedXmlConsulta(List<ResponseReporteGlobalConsultas> datos)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Reporte");

            // Crear encabezados agrupados (filas 1 y 2)
            ws.Cell("A1").Value = "Datos Generales";
            ws.Cell("A1").Style.Fill.BackgroundColor = XLColor.FromName("PowderBlue");
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Range("A1:O1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("P1").Value = "Asignación";
            ws.Cell("P1").Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            ws.Cell("P1").Style.Font.Bold = true;

            ws.Cell("Q1").Value = "Remisión";
            ws.Cell("Q1").Style.Fill.BackgroundColor = XLColor.FromName("PowderBlue");
            ws.Cell("Q1").Style.Font.Bold = true;
            ws.Range("Q1:S1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("T1").Value = "Requerimiento";
            ws.Cell("T1").Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            ws.Cell("T1").Style.Font.Bold = true;
            ws.Range("T1:Y1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("Z1").Value = "Solicitud Opinión Información";
            ws.Cell("Z1").Style.Fill.BackgroundColor = XLColor.FromName("PowderBlue");
            ws.Cell("Z1").Style.Font.Bold = true;
            ws.Range("Z1:AF1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("AG1").Value = "Emisión Resolución";
            ws.Cell("AG1").Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            ws.Cell("AG1").Style.Font.Bold = true;
            ws.Range("AG1:AJ1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("AK1").Value = "Requerimientos Prodecon";
            ws.Cell("AK1").Style.Fill.BackgroundColor = XLColor.FromName("PowderBlue");
            ws.Cell("AK1").Style.Font.Bold = true;
            ws.Range("AK1:AO1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("AP1").Value = "Solicitud Transparencia";
            ws.Cell("AP1").Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            ws.Cell("AP1").Style.Font.Bold = true;
            ws.Range("AP1:AQ1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Encabezados individuales (fila 2)
            string[] headers = new string[]
            {
            "Unidad que atiende el asunto (Central/ADJ)",
            "Subadministración",
            "Número de asunto",
            "RFC",
            "Promovente",
            "RFC Contribuyente",
            "Contribuyente",
            "Despacho Autorizado",
            "Fecha de presentación en el SAT",
            "Tipo Asunto",
            "Tema (Subtipo)",
            "Monto objeto de la promoción",
            "Fecha de recepción en Oficialia de Partes",
            "Fecha de vencimiento",
            "Estado procesal",

            "Nombre del abogado",

            "Unidad a la que se remite el asunto",
            "Número de oficio de remisión",
            "Fecha Oficio ",

            "Oficio Requerimiento",
            "Fecha Oficio del Requerimiento",
            "Fecha de notificación del oficio de requerimiento",
            "Fecha de Vencimiento Requerimiento",
            "Atendió el Requerimiento",
            "Fecha de Atención del Requerimiento",

            "Unidad administrativa a la que se le solicita la solicitud de opinión e información",
            "Oficio de solicitud de opinión e información",
            "Fecha del oficio de solicitud de opinión e información",
            "¿Atendió solicitud  de opinión e información?",
            "Número de oficio de respuesta  de opinión e información",
            "Fecha de oficio de respuesta  de opinión e información",
            "Fecha de recepción  de opinión e información",

            "Oficio de resolución",
            "Fecha del oficio de resolución",
            "Sentido de la resolución",
            "Fecha de notificación del oficio de resolución",

            "Número de oficio PRODECON",
            "Número de expediente PRODECON",
            "Fecha de oficio de requerimiento PRODECON",
            "Fecha de ingreso en el SAT de requerimiento PRODECON",
            "Descripción de la atención requerimiento PRODECON",

            "Número de solicitud de transparencia",
            "Fecha de solicitud de transparencia"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cell(2, i + 1).Value = headers[i];
                ws.Cell(2, i + 1).Style.Font.Bold = true;
            }

            // Cargar datos (desde fila 3)
            int row = 3;
            foreach (var item in datos)
            {
                ws.Cell(row, 1).Value = item.administracion;
                ws.Cell(row, 2).Value = item.subadministracion;
                ws.Cell(row, 3).Value = item.no_asunto;
                ws.Cell(row, 4).Value = item.rfc;
                ws.Cell(row, 5).Value = item.promovente;
                ws.Cell(row, 6).Value = item.rfc_contribuyente;
                ws.Cell(row, 7).Value = item.contribuyente;
                ws.Cell(row, 8).Value = item.despacho_autorizado;
                ws.Cell(row, 9).Value = item.fecha_presentacion;
                ws.Cell(row, 10).Value = item.tipoAsunto;
                ws.Cell(row, 11).Value = item.tema;
                ws.Cell(row, 12).Value = item.monto;
                ws.Cell(row, 13).Value = item.fecha_recepcion;
                ws.Cell(row, 14).Value = item.fecha_vencimiento;
                ws.Cell(row, 15).Value = item.estadoProcesal;

                ws.Cell(row, 16).Value = item.abogado;

                ws.Cell(row, 17).Value = item.administracion_remite;
                ws.Cell(row, 18).Value = item.no_oficio_remision;
                ws.Cell(row, 19).Value = item.fecha_oficio_remision;

                ws.Cell(row, 20).Value = item.no_oficio_Requerimiento;
                ws.Cell(row, 21).Value = item.fecha_oficio_requerimiento;
                ws.Cell(row, 22).Value = item.fecha_notificacion_requerimiento;
                ws.Cell(row, 23).Value = item.fecha_vencimiento_Requerimiento;
                ws.Cell(row, 24).Value = item.atendio_requerimiento.HasValue ? (item.atendio_requerimiento.Value ? "Sí" : "No") : "";
                ws.Cell(row, 25).Value = item.fecha_atencion_requerimiento;

                ws.Cell(row, 26).Value = item.unidadAdministrativaSolicitudInformacion;
                ws.Cell(row, 27).Value = item.noOficioSolicitudInformacion;
                ws.Cell(row, 28).Value = item.fechaOficioSolicitudInformacion;
                ws.Cell(row, 29).Value = item.atendioSolicitudInformacion ? "Sí" : "No";
                ws.Cell(row, 30).Value = item.noOficioRespuestaSolicitudInformacion;
                ws.Cell(row, 31).Value = item.fechaOficioRespuestaSolicitudInformacion;
                ws.Cell(row, 32).Value = item.fechaRecepcionSolicitudInformacion;

                ws.Cell(row, 33).Value = item.noOficioResolucion;
                ws.Cell(row, 34).Value = item.fechaOficionResolucion;
                ws.Cell(row, 35).Value = item.sentido;
                ws.Cell(row, 36).Value = item.fechaOficioNotificacion;

                ws.Cell(row, 37).Value = item.noOficioProdecon;
                ws.Cell(row, 38).Value = item.noExpedienteProdecon;
                ws.Cell(row, 39).Value = item.fechaOficioProdecon;
                ws.Cell(row, 40).Value = item.fechaIngresoProdecon;
                ws.Cell(row, 41).Value = item.atencionProdecon;

                ws.Cell(row, 42).Value = item.noSolicitudTrasparencia;
                ws.Cell(row, 43).Value = item.fechaSolicitudTransparencia;

                row++;
            }

            ws.Columns().AdjustToContents();

            return wb;
        }
        public static XLWorkbook GenerarExcelClosedXmlCumplimentacion(List<ResponseReporteGlobalCumplimentacion> datos)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Reporte");

            // Crear encabezados agrupados (filas 1 y 2)
            ws.Cell("A1").Value = "Datos Generales";
            ws.Cell("A1").Style.Fill.BackgroundColor = XLColor.FromName("PowderBlue");
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Range("A1:L1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("M1").Value = "Asignación";
            ws.Cell("M1").Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            ws.Cell("M1").Style.Font.Bold = true;


            ws.Cell("N1").Value = "Emisión Resolución";
            ws.Cell("N1").Style.Fill.BackgroundColor = XLColor.FromName("PowderBlue");
            ws.Cell("N1").Style.Font.Bold = true;
            ws.Range("N1:P1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("Q1").Value = "Información de la Cumplimnetación";
            ws.Cell("Q1").Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            ws.Cell("Q1").Style.Font.Bold = true;
            ws.Range("Q1:V1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Encabezados individuales (fila 2)
            string[] headers = new string[]
            {
            "Unidad que atiende el asunto (Central/ADJ)	",
            "Subadministración",
            "Número de asunto de la cumplimentación",
            "RFC de la cumplimentación",
            "Promovente de la cumplimentación",
            "RFC del Contribuyente de la cumplimentación",
            "Contribuyente de la cumplimentación",
            "Fecha de la recepción de la solicitud",
            "Tipo Asunto",
            "Número de asunto a cumplimentar",
            "Fecha de vencimiento de la cumplimentación",
            "Estado Procesal",
            "Nombre del abogado de la cumplimentación",

            "Abogado",

            "Oficio de resolución en cumplimentación",
            "Fecha de oficio de resolución en cumplimiento",
            "Sentido de la resolución en cumplimiento",
            "Fecha de notificación del oficio de resolución en cumplimiento",

            "Número de Juicio/Recurso/Amparo de la cumplimentación",
            "Fecha de firmeza de la resolución/sentencia",
            "Órgano Jurisdiccional/Autoridad de la cumplimentación",
            "Unidad administrativa que solicita el cumplimiento",
            "Oficio de resolución impugnada/recurrida de la cumplimentación",
            "Fecha del oficio de resolución impugnada/recurrida de la cumplimentación"
            };



            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cell(2, i + 1).Value = headers[i];
                ws.Cell(2, i + 1).Style.Font.Bold = true;
            }

            // Cargar datos (desde fila 3)
            int row = 3;
            foreach (var item in datos)
            {

                //Datos Generales
                ws.Cell(row, 1).Value = item.administracion;
                ws.Cell(row, 2).Value = item.subadministracion;
                ws.Cell(row, 3).Value = item.no_asunto;
                ws.Cell(row, 4).Value = item.rfc;
                ws.Cell(row, 5).Value = item.promovente;
                ws.Cell(row, 6).Value = item.rfc_contribuyente;
                ws.Cell(row, 7).Value = item.contribuyente;
                ws.Cell(row, 8).Value = item.fecha_recepcion;
                ws.Cell(row, 9).Value = item.tipoAsunto;
                ws.Cell(row, 10).Value = item.no_asunto_cumplimentar;
                ws.Cell(row, 11).Value = item.fecha_vencimiento;
                ws.Cell(row, 12).Value = item.estadoProcesal;
                //Asignación
                ws.Cell(row, 13).Value = item.abogado;
                //Emisión Resolución
                ws.Cell(row, 14).Value = item.noOficioResolucion;
                ws.Cell(row, 15).Value = item.fechaOficionResolucion;
                ws.Cell(row, 16).Value = item.sentido;
                ws.Cell(row, 17).Value = item.fechaOficioNotificacion;
                //Datos de la cumplimentación
                ws.Cell(row, 18).Value = item.numeroJuicioCumplimentacion;
                ws.Cell(row, 19).Value = item.fecha_firmeza_cumplimnetacion;
                ws.Cell(row, 20).Value = item.organoJurisdiccionalCumplimnetacion;
                ws.Cell(row, 21).Value = item.UnidadAdministrativaSolicitaCump;
                ws.Cell(row, 22).Value = item.oficioResolucionCumplimnetacion;
                ws.Cell(row, 23).Value = item.fechaOficioResolucionCumplimentacion;

                row++;
            }

            ws.Columns().AdjustToContents();

            return wb;
        }

         public static XLWorkbook GenerarExcelClosedXmlGeneral(List<ResponseReporteGeneral> datos)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Reporte");

            // Crear encabezados agrupados (filas 1 y 2)
            ws.Cell("A1").Value = "Datos Generales";
            ws.Cell("A1").Style.Fill.BackgroundColor = XLColor.FromName("PowderBlue");
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Range("A1:P1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("Q1").Value = "Asignación";
            ws.Cell("Q1").Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            ws.Cell("Q1").Style.Font.Bold = true;

            ws.Cell("R1").Value = "Remisión";
            ws.Cell("R1").Style.Fill.BackgroundColor = XLColor.FromName("PowderBlue");
            ws.Cell("R1").Style.Font.Bold = true;
            ws.Range("R1:T1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("U1").Value = "Requerimiento";
            ws.Cell("U1").Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            ws.Cell("U1").Style.Font.Bold = true;
            ws.Range("U1:Z1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("AA1").Value = "Solicitud Opinión Información";
            ws.Cell("AA1").Style.Fill.BackgroundColor = XLColor.FromName("PowderBlue");
            ws.Cell("AA1").Style.Font.Bold = true;
            ws.Range("AA1:AG1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("AH1").Value = "Emisión Resolución";
            ws.Cell("AH1").Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            ws.Cell("AH1").Style.Font.Bold = true;
            ws.Range("AH1:AK1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("AL1").Value = "Requerimientos Prodecon";
            ws.Cell("AL1").Style.Fill.BackgroundColor = XLColor.FromName("PowderBlue");
            ws.Cell("AL1").Style.Font.Bold = true;
            ws.Range("AL1:AP1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("AQ1").Value = "Solicitud Transparencia";
            ws.Cell("AQ1").Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            ws.Cell("AQ1").Style.Font.Bold = true;
            ws.Range("AP1:AR1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("AS1").Value = "Información de la Cumplimnetación";
            ws.Cell("AS1").Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            ws.Cell("AS1").Style.Font.Bold = true;
            ws.Range("AS1:AX1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Encabezados individuales (fila 2)
            string[] headers = new string[]
            {
            "Unidad que atiende el asunto (Central/ADJ)",
            "Subadministración",
            "Número de asunto",
            "RFC",
            "Promovente",
            "RFC Contribuyente",
            "Contribuyente",
            "Despacho Autorizado",
            "Fecha de presentación en el SAT",
            "Tipo Asunto",
            "Tema (Subtipo)",
            "No Asunto a Cumplimentar",
            "Monto objeto de la promoción",
            "Fecha de recepción en Oficialia de Partes",
            "Fecha de vencimiento",
            "Estado procesal",

            "Nombre del abogado",

            "Unidad a la que se remite el asunto",
            "Número de oficio de remisión",
            "Fecha Oficio ",

            "Oficio Requerimiento",
            "Fecha Oficio del Requerimiento",
            "Fecha de notificación del oficio de requerimiento",
            "Fecha de Vencimiento Requerimiento",
            "Atendió el Requerimiento",
            "Fecha de Atención del Requerimiento",

            "Unidad administrativa a la que se le solicita la solicitud de opinión e información",
            "Oficio de solicitud de opinión e información",
            "Fecha del oficio de solicitud de opinión e información",
            "¿Atendió solicitud  de opinión e información?",
            "Número de oficio de respuesta  de opinión e información",
            "Fecha de oficio de respuesta  de opinión e información",
            "Fecha de recepción  de opinión e información",

            "Oficio de resolución",
            "Fecha del oficio de resolución",
            "Sentido de la resolución",
            "Fecha de notificación del oficio de resolución",

            "Número de oficio PRODECON",
            "Número de expediente PRODECON",
            "Fecha de oficio de requerimiento PRODECON",
            "Fecha de ingreso en el SAT de requerimiento PRODECON",
            "Descripción de la atención requerimiento PRODECON",

            "Número de solicitud de transparencia",
            "Fecha de solicitud de transparencia",

            "Número de Juicio/Recurso/Amparo de la cumplimentación",
            "Fecha de firmeza de la resolución/sentencia",
            "Órgano Jurisdiccional/Autoridad de la cumplimentación",
            "Unidad administrativa que solicita el cumplimiento",
            "Oficio de resolución impugnada/recurrida de la cumplimentación",
            "Fecha del oficio de resolución impugnada/recurrida de la cumplimentación"


            };

            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cell(2, i + 1).Value = headers[i];
                ws.Cell(2, i + 1).Style.Font.Bold = true;
            }

            // Cargar datos (desde fila 3)
            int row = 3;
            foreach (var item in datos)
            {
                ws.Cell(row, 1).Value = item.administracion;
                ws.Cell(row, 2).Value = item.subadministracion;
                ws.Cell(row, 3).Value = item.no_asunto;
                ws.Cell(row, 4).Value = item.rfc;
                ws.Cell(row, 5).Value = item.promovente;
                ws.Cell(row, 6).Value = item.rfc_contribuyente;
                ws.Cell(row, 7).Value = item.contribuyente;
                ws.Cell(row, 8).Value = item.despacho_autorizado;
                ws.Cell(row, 9).Value = item.fecha_presentacion;
                ws.Cell(row, 10).Value = item.tipoAsunto;
                ws.Cell(row, 11).Value = item.tema;
                ws.Cell(row, 12).Value = item.no_asunto_cumplimentar;
                ws.Cell(row, 13).Value = item.monto;
                ws.Cell(row, 14).Value = item.fecha_recepcion;
                ws.Cell(row, 15).Value = item.fecha_vencimiento;
                ws.Cell(row, 16).Value = item.estadoProcesal;

                ws.Cell(row, 17).Value = item.abogado;

                ws.Cell(row, 18).Value = item.administracion_remite;
                ws.Cell(row, 19).Value = item.no_oficio_remision;
                ws.Cell(row, 20).Value = item.fecha_oficio_remision;

                ws.Cell(row, 21).Value = item.no_oficio_Requerimiento;
                ws.Cell(row, 22).Value = item.fecha_oficio_requerimiento;
                ws.Cell(row, 23).Value = item.fecha_notificacion_requerimiento;
                ws.Cell(row, 24).Value = item.fecha_vencimiento_Requerimiento;
                ws.Cell(row, 25).Value = item.atendio_requerimiento.HasValue ? (item.atendio_requerimiento.Value ? "Sí" : "No") : "";
                ws.Cell(row, 26).Value = item.fecha_atencion_requerimiento;

                ws.Cell(row, 27).Value = item.unidadAdministrativaSolicitudInformacion;
                ws.Cell(row, 28).Value = item.noOficioSolicitudInformacion;
                ws.Cell(row, 29).Value = item.fechaOficioSolicitudInformacion;
                ws.Cell(row, 30).Value = item.atendioSolicitudInformacion ? "Sí" : "No";
                ws.Cell(row, 31).Value = item.noOficioRespuestaSolicitudInformacion;
                ws.Cell(row, 32).Value = item.fechaOficioRespuestaSolicitudInformacion;
                ws.Cell(row, 33).Value = item.fechaRecepcionSolicitudInformacion;

                ws.Cell(row, 34).Value = item.noOficioResolucion;
                ws.Cell(row, 35).Value = item.fechaOficionResolucion;
                ws.Cell(row, 36).Value = item.sentido;
                ws.Cell(row, 37).Value = item.fechaOficioNotificacion;

                ws.Cell(row, 38).Value = item.noOficioProdecon;
                ws.Cell(row, 39).Value = item.noExpedienteProdecon;
                ws.Cell(row, 40).Value = item.fechaOficioProdecon;
                ws.Cell(row, 41).Value = item.fechaIngresoProdecon;
                ws.Cell(row, 42).Value = item.atencionProdecon;

                ws.Cell(row, 43).Value = item.noSolicitudTrasparencia;
                ws.Cell(row, 44).Value = item.fechaSolicitudTransparencia;

                 //Datos de la cumplimentación
                ws.Cell(row, 45).Value = item.numeroJuicioCumplimentacion;
                ws.Cell(row, 46).Value = item.fecha_firmeza_cumplimnetacion;
                ws.Cell(row, 47).Value = item.organoJurisdiccionalCumplimnetacion;
                ws.Cell(row, 48).Value = item.UnidadAdministrativaSolicitaCump;
                ws.Cell(row, 49).Value = item.oficioResolucionCumplimnetacion;
                ws.Cell(row, 50).Value = item.fechaOficioResolucionCumplimentacion;

                row++;
            }

            ws.Columns().AdjustToContents();

            return wb;
        }

        public static byte[] GenerarPDF(List<Response_Resolucion_PDF> data)
        {
            using var stream = new MemoryStream();
            using var document = new PdfDocument();

            var font = new XFont("Arial", 12, XFontStyle.Regular);
            var boldFont = new XFont("Arial", 12, XFontStyle.Bold);

            const int marginLeft = 40;
            const int marginTop = 110;
            const int pageHeightLimit = 800;

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DibujarEncabezado(gfx, page, boldFont);

            int y = marginTop;

            // Función para agregar nueva página cuando sea necesario
            void CheckPageBreak()
            {
                if (y >= pageHeightLimit)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    DibujarEncabezado(gfx, page, boldFont);
                    y = marginTop;
                }
            }

            // Función para dibujar etiqueta y valor en una línea
            void DrawLabelValue(string label, string value)
            {
                CheckPageBreak();

                // Reemplaza null por texto vacío o un marcador
                value ??= "N/A"; // o simplemente: value = value ?? "";

                gfx.DrawString(label, boldFont, XBrushes.Black, new XPoint(marginLeft, y));
                double labelWidth = gfx.MeasureString(label, boldFont).Width;
                gfx.DrawString(value, font, XBrushes.Black, new XPoint(marginLeft + labelWidth, y));
                y += 20;
            }

            gfx.DrawString("SICOJ", boldFont, XBrushes.Black, new XPoint(250, y));
            y += 20;
            gfx.DrawString("ACUSE DE CONCLUSIÓN DE CAPTURA", boldFont, XBrushes.Black, new XPoint(180, y));
            y += 40;

            foreach (var item in data)
            {
                DrawLabelValue("Número de asunto: ", item.no_asunto!);
                DrawLabelValue("Número de expediente: ", item.no_oficio!);
                y += 10;

                CheckPageBreak();
                gfx.DrawString("Motivo de conclusión:", boldFont, XBrushes.Black, new XPoint(marginLeft, y));
                y += 20;

                // Si se desea mostrar motivos:
                // foreach (var motivo in item.MotivosConclusion)
                // {
                //     CheckPageBreak();
                //     gfx.DrawString("• " + motivo, font, XBrushes.Black, new XPoint(marginLeft + 20, y));
                //     y += 20;
                // }

                y += 20;

                DrawLabelValue("Promovente: ", item.promovente!);
                DrawLabelValue("Contribuyente: ", item.contribuyente!);
                DrawLabelValue("Fecha de generación: ", DateTime.Now.ToString("dd/MM/yyyy"));
                DrawLabelValue("Unidad Administrativa: ", item.administracion);
                DrawLabelValue("Abogado: ", item.abogado);

                y += 20; // Espacio adicional entre registros
            }

            document.Save(stream, false);
            return stream.ToArray();

        }


        static void DibujarEncabezado(XGraphics gfx, PdfPage page, XFont boldFont)
        {
            var imgLeft = XImage.FromFile("Resources/logo_hacienda.png");
            var imgCenter = XImage.FromFile("Resources/logo_sat.png");

            double pageWidth = page.Width.Point;
            double imgHeight = 50;
            double imgWidth = 50;

            // Dibuja imagen izquierda
            gfx.DrawImage(imgLeft, 40, 20, imgWidth, imgHeight);

            // Dibuja imagen centrada
            gfx.DrawImage(imgCenter, (pageWidth - imgWidth) / 2, 20, imgWidth, imgHeight);

            // Texto largo alineado a la derecha
            string fullText = "Administración General Jurídica" +
                     "Administración Central de Asuntos Penales" +
                     "y Especiales";

            double maxWidth = 200; // Máximo ancho permitido para el texto a la derecha
            double startX = pageWidth - 40; // Margen derecho
            double startY = 20; // Parte superior del encabezado



            // Divide el texto en líneas
            List<string> lineas = DividirTextoEnLineas(fullText, boldFont, gfx, maxWidth);

            // Dibuja cada línea alineada a la derecha
            foreach (var linea in lineas)
            {
                XSize size = gfx.MeasureString(linea, boldFont);
                double x = startX - size.Width;
                gfx.DrawString(linea, boldFont, XBrushes.Black, new XPoint(x, startY + 15));
                startY += 15; // Espacio entre líneas
            }
        }

        static List<string> DividirTextoEnLineas(string texto, XFont font, XGraphics gfx, double maxWidth)
        {
            var palabras = texto.Split(' ');
            var lineas = new List<string>();
            string lineaActual = "";

            foreach (var palabra in palabras)
            {
                string prueba = (lineaActual.Length == 0) ? palabra : lineaActual + " " + palabra;
                XSize size = gfx.MeasureString(prueba, font);

                if (size.Width <= maxWidth)
                {
                    lineaActual = prueba;
                }
                else
                {
                    if (!string.IsNullOrEmpty(lineaActual))
                        lineas.Add(lineaActual);
                    lineaActual = palabra;
                }
            }

            if (!string.IsNullOrEmpty(lineaActual))
                lineas.Add(lineaActual);

            return lineas;
        }
    }
}