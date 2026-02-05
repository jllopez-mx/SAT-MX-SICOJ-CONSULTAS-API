using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsultasAPI.Model.Entities;
using ConsultasAPI.Model.ViewModels.Enums;

namespace ConsultasAPI.Model.Entities.Events.Administrador
{
    public class EventsArchivosConsultasAdministrador
    {
         public static ArchivoConsulta Create(
            int id_remision,
            int? id_tipo_documento,
            string file_name,
            string path_file,
            string content_type,
            string owner_name,
            string usuario_creacion,
            string? noFolio,
            int? id_seccion,
            int id_consulta,
            string size,
            int? idDocumentoSeccion
        )
        {
            if (idDocumentoSeccion is null)
            {
                idDocumentoSeccion=0;
            }

            ArchivoConsulta entity = new()
            {
                id_rol = EnumRol.Administrador.GetHashCode(),
                id_remision = id_remision,
                id_consulta = id_consulta,
                id_tipo_documento = id_tipo_documento,
                file_name = file_name,
                path_file = path_file,
                content_type = content_type,
                owner_name = owner_name,
                usuario_creacion = usuario_creacion,
                no_folio=noFolio,
                id_seccion=id_seccion,
                size=size,
                estatus=true,
                id_documento_seccion=idDocumentoSeccion
            };
            return entity;
        }
          public static void UpdateWithFile(
            ref ArchivoConsulta entity,
            string? folio,
            int id_tipo_documento,
            string file_name,
            string file_path,
            string content_type,
            string file_size,
            string owner_name,
            string usuarioModificacion
        )
        {
            entity.no_folio = folio;
            entity.id_tipo_documento = id_tipo_documento;
            entity.file_name = file_name;
            entity.path_file = file_path;
            entity.size = file_size;
            entity.owner_name = owner_name;
            entity.usuario_creacion = usuarioModificacion;
        }

        public static void Update(
            ref ArchivoConsulta entity,
            string? folio,
            int id_tipo_documento,
            string owner_name,
            string usuarioModificacion
        )
        {
            entity.no_folio = folio;
            entity.id_tipo_documento = id_tipo_documento;
            entity.owner_name = owner_name;
            entity.usuario_modificacion = usuarioModificacion;
        }


        public static string GetPathFile(ArchivoConsulta entity
        )
        {
            return Path.Combine(entity.path_file!, $"{entity.id}{entity.file_name}");
        }

        public static ArchivoConsulta GetRolAdministrador()
        {
            ArchivoConsulta entity = new()
            {
                id_rol = EnumRol.Administrador.GetHashCode()
            };
            return entity;
        }

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
            string size,
            string noFolio

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
                id_administracion=id_administracion,
                no_folio=noFolio
            
            };
            return entity;
        }

        public static string GetArchivoExt(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }
    }
}