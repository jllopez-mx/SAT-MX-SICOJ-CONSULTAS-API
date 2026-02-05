using ConsultasAPI.Model.Entities;
using ConsultasAPI.Model.DTO;
using Sicoj.Utils.Models;
using ConsultasAPI.Model.DTO.Response.Administrador;

namespace ConsultasAPI.Model.IDAO.IRepository
{
    public interface IConsultaRepositoryAdministrador
    {
        Task<int?> GetAllByFiltersCountAsyncRepositoryAdministrador(
        List<int>? TipoAsunto,
        int? idUnidadAdmistrativaCentral = null!,
        int? idUnidadAdmistrativa = null!
        );

        Task<List<ResponseConsultaByFilters>> GetBandejaPendientesAsyncRepositoryAdministrador(
         int pageSize,
         int page,
         string? OrderByColumn,
         bool OrderDesc,
        int? idUnidadAdmistrativaCentral = null!,
        int? idUnidadAdmistrativa = null!
     );

        Task<Consulta> GetByIdAsyncRepositoryAdministrador(int id);

        Task<ResultTransaction> UpdateAsyncRepositoryAdministradorComercioExterior(Consulta entity);
        Task<ResultTransaction> UpdateAsyncRepositoryAdministradorImpuestosInternos(Consulta entity);

        Task<ResultTransaction> AddPersonasAutorizadas(PersonasAutorizadas entity);
        Task<PersonasAutorizadas> GetByIdAllAsyncRepositoryPersonasAutorizadas(int id);
        Task<ResultTransaction> UpdateAsyncPersonasAutorizadas(PersonasAutorizadas entity);
        Task<ResultTransaction> DeleteAsyncPersonasAutorizadas(int id);
        Task<List<ResponseTablaPersonasAutorizadas>> GetTablaPersonasAutorizadasAsyncRepository(
        int pageSize,
        int page,
        int idConsulta
        );

        Task<int?> GetTablaPersonasAutorizadasCountAsyncRepository(int idConsulta

        );

        Task<ResponseConsultaByFilters> GetByIdRepositoryAdministrador(int id);

        Task<ResultTransaction> AddRemisionAdministradorRepository(Remision entity, ArchivoConsulta entityDocumento, DataFile dataFile);

        Task<List<ResponseTablaRemision>> GetTablaRemisionAdministradorAsyncRepository(
        int pageSize,
        int page
        );

        Task<int?> GetTablaRemisionAdministradorCountAsyncRepository(

        );

        Task<Consulta> GetByIdAllAsyncRepository(int id);

        #region  registra archivos de consultas
        Task<ResultTransaction> AddFileAsyncRepository(ArchivoConsulta entity, DataFile dataFile);
        Task<ResponseArchivosConsulta> GetByIdArchivoAsyncRepository(int id);
        Task<ResultTransaction> DeleteByIdArchivoAsyncRepository(int id);
        Task<List<ResponseArchivosConsulta>> GetArchivosByIdRegistroAsync_Repository(int id_registro);

        Task<ArchivoConsulta> GetIdArchivoConsultaAsyncRepository(int id);
        Task<ResultTransaction> UpdateDocumentoAsync(ArchivoConsulta entityDocumento, DataFile dataFile);

        #endregion

        Task<ResultTransaction> AsignarConsultaRepository(Consulta entity);

        Task<ResponseConsultaList> GetByIdDisconnected(int id);
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
           List<int>? TipoAsunto,
           int? idAdministracion,
           int? idSubadministracion,
           string? idAbogadoAsigno,
            List<int>? EstadoTarea,
            List<int>? TipoModalidad,
            List<int>? EstadoProcesal,
           bool reasignarAsuntos,
           int? idUnidadAdmistrativaCentral = null!,
           int? idUnidadAdmistrativa = null!
       );

        Task<List<ResponseConsultaByFiltersAdministrador>> GetHistoricoAsyncRepository(
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
               int? idAdministracion,
               int? idSubadministracion,
               string? idAbogadoAsigno,
               List<int>? EstadoTarea,
               List<int>? TipoEntrada,
               List<int>? EstadoProcesal,
               bool reasignarAsuntos,
               int? idUnidadAdmistrativaCentral = null!,
               int? idUnidadAdmistrativa = null!
           );
        #endregion
        Task<ResultTransaction> AddAsyncRequerimiento(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<ResultTransaction> SolicitaAsyncRequerimiento(Consulta entity);
        Task<ResultTransaction> AddAsyncRequerimientoProdecon(RequerimientosProdecon entity, ArchivoConsulta entityDocumento, DataFile dataFile);

        Task<ResponseRequerimientoList> GetByIdAsyncRepositoryRequerimientos(int id);

        Task<List<ResponseRequerimientoList>> GetTablaRequerimientosAsyncRepository(int idConsulta);
        Task<List<ResponseRequerimientoProdeconList>> GetTablaRequerimientosProdeconAsyncRepository(int idConsulta);

        Task<int?> GetTablaSolicitudInformacionCountAsyncRepository(int idConsulta);
        Task<int?> GetTablaAlertaSolicitudInformacionCountAsyncRepository(int idConsulta);

        Task<int?> GetTablaRequerimientosCountAsyncRepository(int idConsulta);
        Task<int?> GetTablaAlertaRequerimientosCountAsyncRepository(int idConsulta);
        Task<int?> GetTablaRequerimientosProdeconCountAsyncRepository(int idConsulta);

        Task<Requerimientos> GetByIdAllAsyncRepositoryRequerimientos(int id);
        Task<RequerimientosProdecon> GetByIdAllAsyncRepositoryRequerimientosProdecon(int id);

        Task<ResultTransaction> UpdateAsyncRequerimientos(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<ResultTransaction> UpdateAsyncRequerimientosProdecon(RequerimientosProdecon entity);

        //Solicitud información
        Task<ResultTransaction> AddAsyncRepositorySolicitudInformacion(SolicitudInformacion entity);
        Task<List<ResponseSolicitudInformacionList>> GetTablaSolicitudInformacionAsyncRepository(int idConsulta);
        Task<ResponseSolicitudInformacionList> GetByIdAsyncRepositorySolicitudInformacion(int id);

        Task<SolicitudInformacion> GetByIdAllAsyncRepositorySolicitudInformacion(int id);

        Task<ResultTransaction> UpdateAsyncSolicitudInformacion(SolicitudInformacion entity);

        Task<ResultTransaction> AddAsyncResolucion(Resolucion entity, ArchivoConsulta entityDocumento, DataFile dataFile);//aqui

        Task<int> GetValidacionResolucion(int id);

        Task<List<ResponseResolucion>> GetByIdAllAsyncRepositoryResolucion(int id);
        Task<Resolucion> GetByIdAllAsyncRepositoryResolucionT(int id);
        Task<ResultTransaction> UpdateAsyncResolucion(Resolucion entity);//XX
        Task<ResultTransaction> ConcluirAsyncResolucion(Resolucion entity, ArchivoConsulta entityDocumento, DataFile dataFile);//XX

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

        Task<Cumplimentacion> GetByIdAllAsyncRepositoryCumplimentacion(int id);

        Task<ResultTransaction> AsignarCumplimentacionRepository(Cumplimentacion entity);
        Task<ResultTransaction> UpdateCumplimentacionRepository(Cumplimentacion entity);
        Task<ResultTransaction> AddAsyncResolucionCumplimentacion(ResolucionCumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile);

        Task<ResolucionCumplimentacion> GetByIdAllAsyncRepositoryResolucionCumplimentacionT(int idCumplimentacion);
        Task<ResultTransaction> UpdateAsyncResolucionCumplimentacion(ResolucionCumplimentacion entity);
        Task<ResultTransaction> ConcluirAsyncResolucionCumplimentacion(ResolucionCumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile);

        Task<List<ResponseResolucion>> GetTablaResolucionAsyncRepository(int idConsulta);

        #region Medios Defensa
        Task<List<ResponseMediosDefensa>> GetTablaMediosDefensaAsyncRepository(List<string> dato);
        #endregion

        #region Exportar
        Task<List<ResponseReporteGlobalConsultas>> ExportarConsultaAsyncRepository(
           string? noAsunto,
           string? rfcPromovente,
           string? promovente,
           int? idAdministracion,
           int? idSubadministracion,
           string? idAbogadoAsigno,
           List<int>? TipoEntrada,
           List<int>? TipoAsunto,
           List<int>? EstadoProcesal,
           int? idTema,
           DateTime? fechaPresentacionDesde,
           DateTime? fechaPresentacionHasta,
           DateTime? fechaVencimientoDesde,
           DateTime? fechaVencimientoHasta,
           DateTime? FechaConclucionDesde,
           DateTime? FechaConclucionHasta,
           DateTime? FechaOficioNotificacionResolucionDesde,
           DateTime? FechaOficioNotificacionResolucionHasta,
           DateTime? FechaIngresoProdeconDesde,
           DateTime? FechaIngresoProdeconHasta,
           DateTime? FechaSolicituTransparenciaDesde,
           DateTime? FechaSolicituTransparenciaHasta

           );
        Task<List<ResponseReporteGlobalCumplimentacion>> ExportarCumplimentacionAsyncRepository(
        string? noAsunto,
        string? rfcPromovente,
        string? promovente,
        int? idAdministracion,
        int? idSubadministracion,
        string? idAbogadoAsigno,
        List<int>? TipoEntrada,
        List<int>? TipoAsunto,
        List<int>? EstadoProcesal,
        int? idTema,
        DateTime? fechaPresentacionDesde,
        DateTime? fechaPresentacionHasta,
        DateTime? fechaVencimientoDesde,
        DateTime? fechaVencimientoHasta,
        DateTime? FechaConclucionDesde,
        DateTime? FechaConclucionHasta,
        DateTime? FechaOficioNotificacionResolucionDesde,
        DateTime? FechaOficioNotificacionResolucionHasta,
        DateTime? FechaIngresoProdeconDesde,
        DateTime? FechaIngresoProdeconHasta,
        DateTime? FechaSolicituTransparenciaDesde,
        DateTime? FechaSolicituTransparenciaHasta

        );

        Task<List<ResponseReporteGeneral>> ExportarReporteGeneralAsyncRepository(
           string? noAsunto,
           string? rfcPromovente,
           string? promovente,
           int? idAdministracion,
           int? idSubadministracion,
           string? idAbogadoAsigno,
           List<int>? TipoEntrada,
           List<int>? TipoAsunto,
           List<int>? EstadoProcesal,
           int? idTema,
           DateTime? fechaPresentacionDesde,
           DateTime? fechaPresentacionHasta,
           DateTime? fechaVencimientoDesde,
           DateTime? fechaVencimientoHasta,
           DateTime? FechaConclucionDesde,
           DateTime? FechaConclucionHasta,
           DateTime? FechaOficioNotificacionResolucionDesde,
           DateTime? FechaOficioNotificacionResolucionHasta,
           DateTime? FechaIngresoProdeconDesde,
           DateTime? FechaIngresoProdeconHasta,
           DateTime? FechaSolicituTransparenciaDesde,
           DateTime? FechaSolicituTransparenciaHasta

           );
        #endregion
        Task<List<Response_Resolucion_PDF>> ExportarPDFAsyncRepository(string no_asunto);

    }
}