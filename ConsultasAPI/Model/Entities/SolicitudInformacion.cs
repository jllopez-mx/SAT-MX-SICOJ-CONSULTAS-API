using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.Entities
{
    public class SolicitudInformacion
    {       
        public int id { get; set; }
        public int id_rol { get; set; }
        public int id_consulta {get; set;}      
        public int? id_unidad_administrativa { get; set; }
        public Boolean unidadEsInterna { get; set; }
        public string? unidad_Administrativa_Externa { get; set; }  
        public string unidadAdministrativa { get; set; }=string.Empty;
        public string? no_oficio_solicitud { get; set; } = null!;
        public DateTime fecha_oficio_solicitud { get; set; }
        public Boolean atendio_solicitud { get; set; }    
        public string? no_oficio_respuesta { get; set; } = null!;        
        public DateTime fecha_oficio_respuesta { get; set; }
        public DateTime fecha_recepcion { get; set; }
        public DateTime fecha_registro { get; set; }
        public DateTime fecha_modificacion { get; set; }
    }
}

