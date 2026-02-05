using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.DTO.Contracts.Administrador
{
    public class RequestFiltrosArchivoAdministrador
    {
        public List<string> ByIdRol { get; set; } = new()!;
        public List<string> ByFolio { get; set; } = new()!;
        public List<string> ByIdSeccion { get; set; } = new()!;
        public List<string> ByIdConsulta { get; set; } = new()!;
        public List<string> ByIdRemision { get; set; } = new()!;
        public List<string> ByEstatus { get; set; } = new()!;
        public List<string> ByFechaCreacionDesde { get; set; } = new()!;
        public List<string> ByFechaCreacionHasta { get; set; } = new()!;
        public List<string> ByRemplazable { get; set; } = new()!;
        public List<string> ByPermanente { get; set; } = new()!;
        public List<string> ByIdDocumentoSeccion { get; set; } = new()!;

    }
}