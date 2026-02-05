using ConsultasAPI.Model.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.Entities.Events.AdministradorGlobal
{
    public class EventsConsultasAdministradorGlobal
    {

        #region Requerimientos
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
            entity.id_rol = EnumRol.Administrador_Global.GetHashCode();
        }
        #endregion
        
        #region Reactivar

        public static void Reactivar(ref Consulta entity,
            int id_Consulta           
        )
        {
            
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

        #endregion

        #region Personas Autorizadas
        public static PersonasAutorizadas GetPersonasAutorizadasAdministrador()
        {
            PersonasAutorizadas entity = new()
            {
                id_rol = EnumRol.Administrador.GetHashCode()
            };
            return entity;
        }
        #endregion
    }
}
