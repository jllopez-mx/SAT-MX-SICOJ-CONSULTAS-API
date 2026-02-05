using Sicoj.Utils;
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.ViewModels.Enums;

namespace ConsultasAPI.Model.Entities.Events.Abogado
{
    public class EventsConsultasAbogado
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
                id_seccion=id_seccion,
                size=size,
                estatus=true,
                id_administracion=id_administracion
            };
            return entity;
        }
  
            public static PersonasAutorizadas CreatePersonasAutorizadas(
            int id_consulta,
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
                nombre = nombre,
                rfc = rfc,
                telefono = telefono,
                email = email,
                id_rol = EnumRol.Abogado.GetHashCode(),
                id_consulta=id_consulta
            };
            return entity;
        }
        public static void ConcluirResolucion(ref Resolucion entity,
            int idConsulta
        )
        {         

            entity.id_consulta = idConsulta;
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
            Guard.ValidateEmail(ref email,"Email");
            Guard.ValidateTelefono(ref telefono,"Telefono");
            

                entity.rfc = rfc;
                entity.nombre = nombre;
                entity.telefono = telefono;
                entity.email = email;      
                entity.id_consulta=id_consulta;      
        }

        public static void DeletePersonasAutorizadas(ref PersonasAutorizadas entity, int id
            
        )
        {
                entity.id = id;

        }

        public static PersonasAutorizadas GetPersonasAutorizadasAbogado()
        {
            PersonasAutorizadas entity = new()
            {
                id_rol = EnumRol.Abogado.GetHashCode()
            };
            return entity;
        }

        public static Remision GetRemisionAbogado()
        {
            Remision entity = new()
            {
                id_rol = EnumRol.Abogado.GetHashCode()
            };
            return entity;
        }

        public static void DeleteModalidaArchivoConsulta(ref ResponseArchivosConsulta entity
            
        )
        {
         
                entity.id = entity.id;

            
        }

        public static void UpdateModalidaConsultaAbogadoImpuestosInternos(ref Consulta entity,
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
                    int idsubadministracion
               )
        {
            Guard.CatalogValue(ref id_tema, "tema", true);
            Guard.ValidateStringRfc(ref rfcContribuyente!, "RFC");
            Guard.ValidateStringAlphanumeric(ref contribuyente!, "Promovente");
            Guard.ValidateStringAlphanumeric(ref despachoAutorizado!, "despacho Autorizado");
            Guard.ValidateStringAlphanumeric(ref domicilioPromovente!, "Domicilio Promovente");
            Guard.ValidateStringAlphanumeric(ref domicilioNotificaciones!, "Domicilio Notificaciones");
            Guard.ValidateStringAlphanumeric(ref usuarioasigno!, "usuario");
            Guard.ValidateDecimal(monto, "Monto",false);

            entity.promovente_es_contribuyente = promoventeEsContribuyente;
            entity.rfc_contribuyente = rfcContribuyente;
            entity.contribuyente = contribuyente;
            entity.fecha_recepcion = Convert.ToDateTime(fechaRecepcion);
            entity.despacho_autorizado=despachoAutorizado;
            entity.domicilio_promovente = domicilioPromovente;
            entity.domicilio_notificaciones = domicilioNotificaciones;
            entity.id_tema = id_tema;
            entity.monto=monto;
            entity.usuario_asigno=usuarioasigno;
            entity.id_tipo_asunto=id_tipoAsunto;
            entity.id_tipo_modalidad=id_tipoModalidad;
            entity.id_Subadministracion=entity.id_Subadministracion;

        }

        public static void UpdateModalidaConsultaAbogadoComercioExterior(ref Consulta entity,
                    bool promoventeEsContribuyente,
                   string? rfcContribuyente,
                   string? contribuyente,
                   string fechaPresentacion,
                   string fechaRecepcion,
                   string usuarioasigno,
                   int? id_tipoAsunto,
                   int? id_tipoModalidad,
                   int idsubadministracion
               )
        {
            
            Guard.ValidateStringRfc(ref rfcContribuyente!, "RFC");
            Guard.ValidateStringAlphanumeric(ref contribuyente!, "Promovente");
            Guard.ValidateStringAlphanumeric(ref usuarioasigno!, "usuario");
            

            entity.promovente_es_contribuyente = promoventeEsContribuyente;
            entity.rfc_contribuyente = rfcContribuyente;
            entity.contribuyente = contribuyente;
            entity.fecha_presentacion = Convert.ToDateTime(fechaPresentacion);
            entity.fecha_recepcion = Convert.ToDateTime(fechaRecepcion);
            entity.usuario_asigno = usuarioasigno;
            entity.id_tipo_asunto=id_tipoAsunto;
            entity.id_tipo_modalidad=id_tipoModalidad;
            entity.id_Subadministracion=idsubadministracion;


        }

        public static void AsignarAbogado(ref Consulta entity,
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
                id_rol = EnumRol.Abogado.GetHashCode(),
                no_oficio = no_oficio,
                fecha_requerimiento = fecha_requerimiento,
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
                solicita_requerimiento=solicitaRequerimiento
            };

            return entity;
        }


        public static void UpdateRequerimientos(ref Requerimientos entity,
            int id_consulta,
            bool? atendio,
            DateTime fecha_notificacion,   
           DateTime? fecha_atencion ,
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
                id_rol = EnumRol.Abogado.GetHashCode(),
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
                id_rol = EnumRol.Abogado.GetHashCode(),
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

        public static SolicitudInformacion CreateSolicitudInformacion(
            int id_consulta,
            int? id_unidad_administrativa,
            string? no_oficio_solicitud,
            string? no_oficio_respuesta,
            bool atendio_solicitud,
            DateTime fecha_oficio_solicitud,   
            DateTime fecha_oficio_respuesta, 
            DateTime fecha_recepion ,
            bool unidadEsInterna,
            string unidadAdministrativaExterna      
        )

        {

        

            SolicitudInformacion entity = new()
            {
                id_consulta = id_consulta,
                id_unidad_administrativa = id_unidad_administrativa,
                no_oficio_solicitud=no_oficio_solicitud,
                no_oficio_respuesta=no_oficio_respuesta,
                id_rol = EnumRol.Abogado.GetHashCode(),
                atendio_solicitud = atendio_solicitud,
                fecha_oficio_solicitud = fecha_oficio_solicitud,
                fecha_oficio_respuesta = fecha_oficio_respuesta,
                fecha_recepcion = fecha_recepion,
                unidadEsInterna=unidadEsInterna,
                unidad_Administrativa_Externa=unidadAdministrativaExterna
                
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
            string unidadAdministrativaExterna,
            DateTime fechaOficioSolicitud
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
                unidad_Administrativa_Externa = unidadAdministrativaExterna,
                fecha_oficio_solicitud=fechaOficioSolicitud,
                

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
            entity.id_unidad_administrativa=idUnidadAdministrativa;
            entity.no_oficio_solicitud = noOficioSolicitud;
            entity.fecha_oficio_solicitud = fechaOficioSolicitud;
            entity.atendio_solicitud = atendioSolicitud;
            entity.no_oficio_respuesta = noOficioRespuesta;
            entity.fecha_recepcion = fechaRecepcion;
            entity.unidadEsInterna = unidadEsInterna;
            entity.unidad_Administrativa_Externa = unidadAdministrativaExterna;
            entity.fecha_oficio_respuesta=fechaOficioRespuesta;
        }

        public static void  RequestUpdateSolicitudInformacionSinAtencion(ref SolicitudInformacion entity,
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
            entity.id_unidad_administrativa=idUnidadAdministrativa;
            entity.no_oficio_solicitud = noOficioSolicitud;
            entity.fecha_oficio_solicitud = fechaOficioSolicitud;
            entity.atendio_solicitud = atendioSolicitud;
            entity.no_oficio_respuesta = noOficioRespuesta;
            entity.unidadEsInterna = unidadEsInterna;
            entity.unidad_Administrativa_Externa = unidadAdministrativaExterna;
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

        //Avisos y comunicados
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
                id_rol = EnumRol.Abogado.GetHashCode(),
                id_tipo_aviso=id_tipo_aviso,
                folio = folio!,
                fecha_ingreso = fecha_ingreso,
                observaciones = observaciones!,
                tiene_folio=tieneFolio
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
                id_rol = EnumRol.Abogado.GetHashCode(),
                id_consulta  =  idConsulta,
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
                id_rol = EnumRol.Abogado.GetHashCode(),
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

        public static void UpdateCumplimentacion(ref Cumplimentacion entity,
            string noJuicio,
            DateTime fechaRecepcion,
            DateTime? fechaFirmeza,
            DateTime? fechaVencimiento,
            int? idOrganoJurisdiccional,
            int? idAdministracion,
            int? idAdministracionSolicita,
            int plazoCumplimentar ,
            int idsubadministracion
        )
        {
                entity.numero_juicio = noJuicio;
                entity.fecha_recepcion = Convert.ToDateTime(fechaRecepcion);
                entity.fecha_firmeza = Convert.ToDateTime(fechaFirmeza);
                entity.fecha_vencimiento = Convert.ToDateTime(fechaVencimiento);
                entity.id_organo_jurisdiccional = idOrganoJurisdiccional;
                entity.id_administracion=idAdministracion;
                entity.idUnidadAdministrativaSolicitaCump = idAdministracionSolicita;
                entity.plazoCumplimentar = plazoCumplimentar;
                entity.id_Subadministracion=idsubadministracion;
        }

        }
    }

