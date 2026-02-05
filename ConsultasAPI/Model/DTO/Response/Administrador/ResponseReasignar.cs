using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.DTO.Response.Administrador
{
    public class ResponseReasignar
    {        
        public int ReasignacionesExitosas { get; set; }
        public int ReasignacionesIncorrectas { get; set; }
        public string abogado { get; set; } = null!;
    }
}