using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.DTO.Contracts.Administrador
{
    public class RequestReasignar
    {
        public List<int> idList { get; set; } = null!;
        public string idAbogado { get; set; } = null!;
        public int idTipoAsunto { get; set; }
    }
}