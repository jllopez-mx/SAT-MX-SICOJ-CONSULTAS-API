using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.DTO.Response.Administrador
{
    public class ResponseResolucion
    {
        public int id { get; set; }
        public int id_consulta {get; set;}
        public int id_rol { get; set; }
        public string? nombre_rol { get; set; } = null!;
        public string? no_oficio { get; set; } = null!;
        public string fecha_notificacion { get; set; }= null!;
        public string fecha_resolucion { get; set; }= null!;
        public int? id_sentido { get; set; }
        public string? sentido { get; set; } = null!;
        public string fecha_vencimiento { get; set; }= null!;
        public Boolean concluido { get; set; }
    }
}