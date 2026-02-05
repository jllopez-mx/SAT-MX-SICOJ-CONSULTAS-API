using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.DTO.Contracts.Administrador
{
    public class RequestFiltrosConsultaAdministrador
    {
        public List<string> ByNoAsunto { get; set; } = new()!;
        public List<string> ByFechaPresentacionDesde { get; set; } = new()!;
        public List<string> ByFechaPresentacionHasta { get; set; } = new()!;
        public List<string> ByFechaVencimientoDesde { get; set; } = new()!;
        public List<string> ByFechaVencimientoHasta { get; set; } = new()!;
         public List<string> ByFechaCSSJDesde { get; set; } = new()!;
        public List<string> ByFechaCSSJHasta { get; set; } = new()!;        
        public List<string> ByRfcPromovente { get; set; } = new()!;
        public List<string> ByPromovente { get; set; } = new()!;
        public List<string> ByIdTipoAsunto { get; set; } = new()!;
        public List<string> ByIdAdministracion { get; set; } = new()!;
        public List<string> ByIdSubadministracion { get; set; } = new()!;
        public List<string> ByIdAbogadoAsigno { get; set; } = new()!;
        public List<string> ByIdEstadoTarea { get; set; } = new()!;
        public List<string> ByIdTipoModalidad { get; set; } = new()!;
        public List<string> ByIdEstadoProcesal { get; set; } = new()!;
    }
}