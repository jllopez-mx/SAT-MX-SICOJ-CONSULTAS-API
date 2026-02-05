using ConsultasAPI.Model.Entities;
using ConsultasAPI.Model.DTO;
using Sicoj.Utils.Models;
using ConsultasAPI.Model.DTO.Contracts.Abogado;
using ConsultasAPI.Model.DTO.Response.Administrador;

namespace ConsultasAPI.Model.IDAO.IRepository
{
    public interface IConsultaRepositoryAbogado
    {
        Task<ResultTransaction> AddRemisionAbogadoRepository(Remision entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<ResultTransaction> AddPersonasAutorizadas(PersonasAutorizadas entity);
        Task<PersonasAutorizadas> GetByIdAllAsyncRepositoryPersonasAutorizadas(int id);
        Task<ResultTransaction> UpdateAsyncPersonasAutorizadas(PersonasAutorizadas entity);
        Task<ResultTransaction> DeleteAsyncPersonasAutorizadas(int id);
        Task<List<ResponseTablaPersonasAutorizadas>> GetTablaPersonasAutorizadasAsyncRepository(
        int pageSize,
        int page,
        int idConsulta
        );

        Task<int?> GetTablaPersonasAutorizadasCountAsyncRepository(int idConsulta);

        Task<List<ResponseTablaRemision>> GetTablaRemisionAbogadoAsyncRepository(
        int pageSize,
        int page
        );

        Task<int?> GetTablaRemisionAbogadoCountAsyncRepository();

        Task<Consulta> GetByIdAllAsyncRepository(int id);

        Task<Consulta> GetByIdAsyncRepositoryAbogado(int id);

        Task<ResultTransaction> UpdateAsyncRepositoryAbogadoImpuestosInternos(Consulta entity);
        Task<ResultTransaction> UpdateAsyncRepositoryAbogadoComercioExterior(Consulta entity);

        #region  registra archivos de consultas
        Task<ResultTransaction> AddFileAsyncRepository(ArchivoConsulta entity, DataFile dataFile);
        Task<ResponseArchivosConsulta> GetByIdArchivoAsyncRepository(int id);
        Task<ResultTransaction> DeleteByIdArchivoAsyncRepository(int id);
        Task<List<ResponseArchivosConsulta>> GetArchivosByIdRegistroAsync_Repository(int id_remision);
        Task<ArchivoConsulta> GetIdArchivoConsultaAsyncRepository(int id);
        Task<ResultTransaction> UpdateDocumentoAsync(ArchivoConsulta entityDocumento, DataFile dataFile);

        #endregion

        Task<ResultTransaction> AsignarConsultaRepository(Consulta entity);
        #region  Historico
        Task<int?> GetHistoricoCountAsyncRepository
        (
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
           int? idUnidadAdmistrativaCentral = null!,
           int? idUnidadAdmistrativa = null!
        );
        Task<List<ResponseConsultaByFilters>> GetHistoricosAsyncRepository
        (
            int pageSize,
            int page,
            string? orderByColumn,
            bool orderDesc,
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
            int? idUnidadAdmistrativaCentral = null!,
            int? idUnidadAdmistrativa = null!
       );
        #endregion
        #region  Bandeja de pendientes
        Task<int?> GetPendientesCountAsyncRepositoryAbogado(
           string? noAsunto,
           DateTime? fechaPresentacionDesde,
           DateTime? fechaPresentacionHasta,
           DateTime? fechaVencimientoDesde,
           DateTime? fechaVencimientoHasta,
           string? rfcPromovente,
           string? promovente,
           List<int>? TipoAsunto,
           List<int>? EstadoTarea,
           List<int>? TipoModalidad,
           List<int>? EstadoProcesal,
           int? idUnidadAdmistrativaCentral = null!,
           int? idUnidadAdmistrativa = null!,
           int? idSubadministracion = null!,
           string? idAbogado = null!);

        Task<List<ResponseConsultaByFiltersAbogado>> GetBandejaPendientesAsyncRepositoryAbogado(
        int pageSize,
               int page,
               string? orderByColumn,
               bool orderDesc,
               string? noAsunto,
               DateTime? fechaPresentacionDesde,
               DateTime? fechaPresentacionHasta,
               DateTime? fechaVencimientoDesde,
               DateTime? fechaVencimientoHasta,
               string? rfcPromovente,
               string? promovente,
               List<int>? TipoAsunto,
               List<int>? EstadoTarea,
               List<int>? TipoModalidad,
               List<int>? EstadoProcesal,
               int? idUnidadAdmistrativaCentral = null!,
               int? idUnidadAdmistrativa = null!,
               int? idSubadministracion = null!,
               string? idAbogado = null!
     );
     #endregion

        Task<ResponseConsultaByFilters> GetByIdRepositoryAbogado(int id);

        Task<ResponseConsultaList> GetByIdDisconnected(int id);

        Task<ResultTransaction> AddAsyncRequerimiento(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<ResultTransaction> SolicitaAsyncRequerimiento(Consulta entity);

        Task<ResponseRequerimientoList> GetByIdAsyncRepositoryRequerimientos(int id);

        Task<List<ResponseRequerimientoList>> GetTablaRequerimientosAsyncRepository(int idConsulta);

        Task<int?> GetTablaRequerimientosCountAsyncRepository(int idConsulta);
        Task<int?> GetTablaAlertaRequerimientosCountAsyncRepository(int idConsulta);

        Task<Requerimientos> GetByIdAllAsyncRepositoryRequerimientos(int id);

        Task<ResultTransaction> UpdateAsyncRequerimientos(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile);

        Task<ResultTransaction> AddAsyncResolucion(Resolucion entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<ResultTransaction> ConcluirAsyncResolucion(Resolucion entity, ArchivoConsulta entityDocumento, DataFile dataFile);//XX

        Task<int> GetValidacionResolucion(int id);

        Task<Resolucion> GetByIdAllAsyncRepositoryResolucionT(int idConsulta);
        Task<List<ResponseResolucion>> GetByIdAllAsyncRepositoryResolucion(int id);
        Task<ResultTransaction> UpdateAsyncResolucion(Resolucion entity);

        Task<ResultTransaction> AddAsyncRequerimientoProdecon(RequerimientosProdecon entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<List<ResponseRequerimientoProdeconList>> GetTablaRequerimientosProdeconAsyncRepository(int idConsulta);
        Task<int?> GetTablaRequerimientosProdeconCountAsyncRepository(int idConsulta);
        Task<RequerimientosProdecon> GetByIdAllAsyncRepositoryRequerimientosProdecon(int id);
        Task<ResultTransaction> UpdateAsyncRequerimientosProdecon(RequerimientosProdecon entity);

        Task<ResultTransaction> AddAsyncRepositorySolicitudInformacion(SolicitudInformacion entity);
        Task<List<ResponseSolicitudInformacionList>> GetTablaSolicitudInformacionAsyncRepository(int idConsulta);
        Task<ResponseSolicitudInformacionList> GetByIdAsyncRepositorySolicitudInformacion(int id);

        Task<int?> GetTablaSolicitudInformacionCountAsyncRepository(int idConsulta);
        Task<int?> GetTablaAlertaSolicitudInformacionCountAsyncRepository(int idConsulta);

        Task<SolicitudInformacion> GetByIdAllAsyncRepositorySolicitudInformacion(int id);

        Task<ResultTransaction> UpdateAsyncSolicitudInformacion(SolicitudInformacion entity);

        Task<List<Consulta>> GetListByIdsAsync(int[] ids);
        Task<ResultTransaction> ReasignarAsync(List<Reasignar> listReasignacion);

        //Avisos y comunicados
        Task<ResultTransaction> AddAsyncAvisosYComunicadosRepository(AvisosComunicados entity);
        Task<AvisosComunicados> GetByIdAsyncRepositoryAvisosComunicados(int id);
        Task<ResultTransaction> UpdateAsyncAvisosYComunicadosRepository(AvisosComunicados entity);
        Task<int?> GetTablaAvisosYComunicadosCountAsyncRepository(int idConsulta);
        Task<int?> GetAlertaAvisosYComunicadosCountAsyncRepository(int idConsulta);
        Task<List<ResponseAvisosComunicadosList>> GetTablaAvisosYComunicadosAsyncRepository(int idConsulta);
        Task<int?> GetArchivosByFiltersCountAsyncRepository
       (
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
          int? idDocumentoSeccion
      );

        Task<List<ResponseArchivosConsulta>> GetArchivoByFiltersAsyncRepository(
                int pageSize,
                int page,
                string? orderByColumn,
                bool orderDesc,
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
                int? idDocumentoSeccion
           );

        Task<ResultTransaction> AddAsyncSolicitudTransparenciaService(SolicitudTransparencia entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<SolicitudTransparencia> GetByIdSolicitudTransparenciaRepository(int id);
        Task<ResultTransaction> UpdateSolicitudTransparenciaRepository(SolicitudTransparencia entity);
        Task<ResponseSolicitudTransparenciaList> GetByIdSolicitudTransparenciaRepositorys(int id);
        Task<int?> GetTablaSolicitudTransparenciaCountRepository(int idConsulta);
        Task<List<ResponseSolicitudTransparenciaList>> GetTablaSolicitudTransparenciaRepository(int idConsulta);
        Task<ResultTransaction> DeleteSolicitudTransparenciaRepository(int id);

        #region Cumplimentacion

        Task<ResultTransaction> AddAsyncResolucionCumplimentacion(ResolucionCumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile);

        Task<ResolucionCumplimentacion> GetByIdAllAsyncRepositoryResolucionCumplimentacionT(int id_cumplimentacion);
        Task<ResultTransaction> UpdateAsyncResolucionCumplimentacion(ResolucionCumplimentacion entity);
        Task<ResultTransaction> ConcluirAsyncResolucionCumplimentacion(ResolucionCumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<List<ResponseResolucion>> GetTablaResolucionAsyncRepository(int idConsulta);
        Task<Cumplimentacion> GetByIdAllAsyncRepositoryCumplimentacion(int id);
        Task<ResultTransaction> UpdateCumplimentacionRepository(Cumplimentacion entity);
        #endregion

    }
}