using ConsultasAPI.Model.Entities;
using Sicoj.Utils.Models;
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.DTO.Response.Administrador;

namespace ConsultasAPI.Model.IDAO.IServiceDAO
{
    public interface IConsultasAbogadoService
    {
        Task<ResultOperation> AddRemisionAbogadoService(Remision entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<ResultOperation> AddPersonasAutorizadas(PersonasAutorizadas entity);
        Task<PersonasAutorizadas> GetByAllServicePersonaAutorizadas(int id);
        Task<ResultOperation> UpdatePersonasAutorizadas(PersonasAutorizadas entity);
        Task<ResultOperation> DeletePersonasAutorizadas(PersonasAutorizadas entity);
        Task<ResultOperation> GetTablaPersonasAutorizadasService(int Fetch, int Page, int idConsulta);
        Task<ResultOperation> GetTablaRemisionAbogadoService(int Fetch, int Page);

        Task<Consulta> GetByAllService(int id);
        Task<ResultOperation<int>> AddArchivosAsyncService(ArchivoConsulta entity, DataFile dataFile);
        Task<ResponseArchivosConsulta> GetByIdArchivoDeleteService(int id);
        Task<ResultOperation> DeleteArchivoConsultaService(ResponseArchivosConsulta entity);
        Task<List<ResponseArchivosConsulta>> GetAllArchivoService(int id_remision);
        Task<ResponseArchivosConsulta> GetByIdArchivoService(int id);

        Task<ArchivoConsulta> GetIdArchivoConsultaService(int id);
        Task<ResultOperation<int>> UpdateArchivoService(ArchivoConsulta entity, DataFile dataFile);

        Task<Consulta> GetByIdServiceAbogado(int id);
        Task<ResultOperation> UpdateConsultaServiceAbogadoImpuestosInternos(Consulta entity);
        Task<ResultOperation> UpdateConsultaServiceAbogadoComercioExterior(Consulta entity);
        Task<ResultOperation<ResponseAsignar>> AsignarConsultaService(Consulta entity);

        #region Historico
        Task<ResultOperation> GetBandejaHistoricoByFiltersService(
            int Fetch, int
            Page, string?
            OrderByColumn,
            bool OrderDesc,
             string? noAsunto,
            DateTime? fechaPresentacionDesde,
            DateTime? fechaPresentacionHasta,
            DateTime? fechaVencimientoDesde,
            DateTime? fechaVencimientoHasta,
            string? rfcPromovente,
            string? promovente,
            List<int>? tipoAsuntoList,
            List<int>? estadoTareaList,
            List<int>? tipoModalidadList,
            List<int>? estadoProcesalList,
            List<int>? alertaList,
            UserInformationView userInformationView);
        #endregion
        #region  Bandeja de pendientes
        Task<ResultOperation> GetBandejaPendientes_AbogadoService(
            int Fetch,
            int Page,
            string? OrderByColumn,
            bool OrderDesc,
            string? noAsunto,
            DateTime? fechaPresentacionDesde,
            DateTime? fechaPresentacionHasta,
            DateTime? fechaVencimientoDesde,
            DateTime? fechaVencimientoHasta,
            string? rfcPromovente,
            string? promovente,
            List<int>? tipoAsuntoList,
            List<int>? estadoTareaList,
            List<int>? tipoModalidadList,
            List<int>? estadoProcesalList,
            UserInformationView userInformationView);
        #endregion
        Task<ResultOperation> GetByIdAbogadoService(int id);

        Task<ResultOperation> GetByIdDisconnected(int id);

        Task<ResultOperation> AddRequerimientoService(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<ResultOperation> SolicitaRequerimientoService(Consulta entity);
        Task<ResultOperation> GetRequerimientoById(int id);
        Task<ResultOperation> GetTablaRequerimientosService(int idConsulta);
        Task<Requerimientos> GetByAllServiceRequerimientos(int id);
        Task<ResultOperation> UpdateRequerimientos(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile);

        Task<ResultOperation> AddResolucionService(Resolucion entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<Resolucion> GetByAllServiceResolucion(int idConsulta);
        Task<ResultOperation> GetResolucionById(int id);
        Task<ResultOperation> UpdateResolucion(Resolucion entity);
        Task<ResultOperation> ConcluirResolucion(Resolucion entity, ArchivoConsulta entityDocumento, DataFile dataFile);//

        Task<ResultOperation> AddRequerimientoProdeconService(RequerimientosProdecon entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<ResultOperation> GetRequerimientoProdeconById(int id);
        Task<ResultOperation> GetTablaRequerimientosProdeconService(int idConsulta);
        Task<RequerimientosProdecon> GetByAllServiceRequerimientosProdecon(int id);
        Task<ResultOperation> UpdateRequerimientosProdecon(RequerimientosProdecon entity);

        Task<ResultOperation> AddSolicitudInformacionService(SolicitudInformacion entity);
        Task<ResultOperation> GetTablaSolicitudInformacionService(int idConsulta);
        Task<ResultOperation> GetSolicitudInformacionById(int id);
        Task<SolicitudInformacion> GetByAllServiceSolicitudInformacion(int id);
        Task<ResultOperation> UpdateSolicitudInformacion(SolicitudInformacion entity);

        Task<ResultOperation<ResponseReasignar>> ReAsignarComercioExteriorAsync(int[] idList, string rfcAbogado, UserInformationView userInformationView);
        Task<ResultOperation<ResponseReasignar>> ReAsignarImpuestosInternosAsync(int[] idList, string rfcAbogado, UserInformationView userInformationView);

        //Avisos y Comunicados
        Task<ResultOperation> AddAvisosYComunicadosService(AvisosComunicados entity);

        Task<AvisosComunicados> GetByServiceAvisosYComunicados(int id);

        Task<ResultOperation> UpdateAvisosYComunicados(AvisosComunicados entity);

        Task<ResultOperation> GetTablaAvisosYComunicados(int idConsulta);

        Task<ResultOperation> GetArchivoByFiltersService
        (
           int Fetch,
           int Page,
           string? OrderByColumn,
           bool OrderDesc,
           int? idRol,
           string? folio,
           int? idSeccion,
           int? idConsulta,
           int? idRemision,
           bool? estatus,
           DateTime? fechaCreacionDesde,
           DateTime? fechaCreacionHasta,
           bool? remplazable,
           bool? permanente,
           UserInformationView userInformationView,
        int? idDocumentoSeccion
       );

        #region Solicitud Transparencia

        Task<ResultOperation> AddSolicitudTransparenciaService(SolicitudTransparencia entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<SolicitudTransparencia> GetByIdSolicitudTransparenciaService(int id);
        Task<ResultOperation> UpdateSolicitudTransparenciaService(SolicitudTransparencia entity);
        Task<ResultOperation> GetByIdSolicitudTransparenciaServices(int id);
        Task<ResultOperation> GetTablaSolicitudTransparenciaService(int idConsulta);
        Task<ResultOperation> DeleteSolicitudTransparenciaService(SolicitudTransparencia entity);

        #endregion

        #region Cumplimentacion

        Task<ResultOperation> AddResolucionCumplimentacionService(ResolucionCumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<ResolucionCumplimentacion> GetByAllServiceResolucionCumplimentacion(int id_cumplimentacion);
        Task<ResultOperation> UpdateResolucionCumplimentacion(ResolucionCumplimentacion entity);
        Task<ResultOperation> ConcluirResolucionCumplimentacion(ResolucionCumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile);

        Task<ResultOperation> GetTablaResolucionService(int idConsulta);
        Task<Cumplimentacion> GetByAllServiceCumplimentacion(int id);
        Task<ResultOperation> UpdateCumplimentacionService(Cumplimentacion entity, UserInformationView userInformationView);

        #endregion
          Task<ResultOperation> GetTablaMediosDefensaGeneral(List<string> noAsunto);
               
    }
}