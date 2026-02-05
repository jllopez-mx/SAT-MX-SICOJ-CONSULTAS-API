using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.DTO.Contracts.Abogado
{
    public class RequestFiltrosConsultaAbogado
    {
        public List<string> ByNoAsunto { get; set; } = new()!;
        public List<string> ByFechaPresentacionDesde { get; set; } = new()!;
        public List<string> ByFechaPresentacionHasta { get; set; } = new()!;
        public List<string> ByFechaVencimientoDesde { get; set; } = new()!;
        public List<string> ByFechaVencimientonHasta { get; set; } = new()!;
        public List<string> ByRfc { get; set; } = new()!;
        public List<string> ByPromovente { get; set; } = new()!;
        public List<string> ByIdTipoModalidad { get; set; } = new()!;
        public List<string> ByIdEstadoProcesal { get; set; } = new()!;
        public List<string> ByIdEstadoTarea { get; set; } = new()!;
        public List<string> ByIdTipoAsunto { get; set; } = new()!;
    }
}