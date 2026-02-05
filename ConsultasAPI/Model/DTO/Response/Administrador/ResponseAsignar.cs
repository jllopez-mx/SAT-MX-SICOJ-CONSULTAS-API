using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.DTO.Response.Administrador
{
    public class ResponseAsignar
    {
        public string noAsunto { get; set; } = null!;
        public string abogado { get; set; } = null!;
    }
}