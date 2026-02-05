using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.Entities
{
    public class AvisosComunicados
    {
        public int id { get; set; }
        public int id_consulta { get; set; }
        public int id_rol { get; set; }
        public string? nombre_rol { get; set; } = null!;
        public int id_tipo_aviso { get; set; }
         public string? tipo_aviso { get; set; } = null!;
        public string folio { get; set; }=string.Empty;
        public Boolean tiene_folio { get; set; }
        public DateTime fecha_ingreso { get; set; } 
        public string observaciones { get; set; } =null!;      
        public Boolean atencion_adicional { get; set; }
        public string descripcion_atencion { get; set; } =null!;
    }
}