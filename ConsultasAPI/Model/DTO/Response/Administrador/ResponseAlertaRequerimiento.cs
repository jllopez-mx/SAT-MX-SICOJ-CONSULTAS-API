using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.DTO.Response.Administrador
{
    public class ResponseAlertaRequerimiento
    {
        public int idAlerta { get; set; }
        public string alerta { get; set; } = string.Empty;
    }
}