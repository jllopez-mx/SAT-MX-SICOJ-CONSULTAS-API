using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsultasAPI.Model.ViewModels;
using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.Entities
{
    public class Remision
    {
        public int id { get; set; }
        public int id_consulta { get; set; }
        public int id_rol { get; set; }
        public string rol { get; set; } = string.Empty;
        public int? id_administracion_remite { get; set; }
        public string administracion_remite { get; set; } = string.Empty;
        public int? id_administracion_recibe { get; set; }
        public string administracion_recibe { get; set; } = string.Empty;
        public int? id_tipo_autoridad { get; set; }
        public string? no_oficio_remision { get; set; }
        public int id_estado_tarea { get; set; }
        public int id_estado_procesal { get; set; }
        public string? usuario { get; set; }
        public DateTime fecha_oficio { get; set; }
        public DateTime fecha_actualización { get; set; }
        public DateTime fecha_remision { get; set; }

    }
}