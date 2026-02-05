using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.DTO.Response.Administrador
{
    public class ResponseSolicitudInformacionList
    {

        public int id { get; set; }
        public int idRol { get; set; }
        public string? nombreRol { get; set; } = null!;
        public int idConsulta { get; set; }
        public Boolean unidadEsInterna { get; set; }
        public int idUnidadAdministrativa { get; set; }
        public string unidadAdministrativaExterna { get; set; }=string.Empty;
        public string unidadAdministrativa { get; set; }=string.Empty;
        public string noOficioSolicitud { get; set; }=string.Empty;
        public string noOficioRespuesta { get; set; }=string.Empty;
        public Boolean atendioSolicitud { get; set; }
        public string? fechaOficioSolicitud { get; set; }
        public string? fechaOficioRespuesta { get; set; } 
        public string? fechaRecepcion { get; set; }
        
    }
}