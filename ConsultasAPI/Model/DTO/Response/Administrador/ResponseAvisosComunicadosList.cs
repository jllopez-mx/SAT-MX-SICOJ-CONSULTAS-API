using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.DTO.Response.Administrador
{
    public class ResponseAvisosComunicadosList
    {

         public int id { get; set; }
        public int idConsulta { get; set; }

        public int idRol { get; set; }
        public string? rol { get; set; } = null!;

        public int idTipoAviso { get; set; }
         public string? tipo_aviso { get; set; } = null!;
        
        public string folio { get; set; }=string.Empty;

        public Boolean tieneFolio { get; set; }

        public string fechaIngreso { get; set; } =null!;
      
        public string observaciones { get; set; } =null!;

        public Boolean atencionAdicional { get; set; }

        public string descripcionAtencion { get; set; } =null!;

        public string fechaRegistro { get; set; } =null!;

        
    }
}