using ConsultasAPI.Model.ViewModels.Enums;
using Sicoj.Utils;

namespace ConsultasAPI.Model.Entities.Events.AdministradorUnidadAdministrativa
{
    public class EventsConsultasAdministradorUnidadAdministrativa
    {
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

        #region Asignar

        public static void AsignarAdministrador(ref Consulta entity,
            int id_Consulta,
            string? id_abogado,
            string? usuario_asigno
        )
        {

            entity.id_abogado = id_abogado;
            entity.usuario_asigno = usuario_asigno;
        }

        #endregion
        
        #region Remitir

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

        #endregion
    }
}