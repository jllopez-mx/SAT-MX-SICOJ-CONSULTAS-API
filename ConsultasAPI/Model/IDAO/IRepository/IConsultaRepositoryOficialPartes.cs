using ConsultasAPI.Model.DAO;
using ConsultasAPI.Model.Entities;
using ConsultasAPI.Model.DTO;
using Sicoj.Utils.Models;

namespace ConsultasAPI.Model.IDAO
{
    public interface IConsultaRepositoryOficialPartes
    {
        #region  Bandejas

        //Contador bandeja de pendientes Cumplimentación
        Task<int?> GetBandejaCumplimentacionCountAsyncRepository(
        List<int>? TipoAsunto, int? idUnidadAdmistrativaCentral = null! );

        Task<List<ResponseConsultaByFilters>> GetBandejaPendientesCumplimentacionAsyncRepository(
        int pageSize,
        int page,
        string? orderByColumn,
        bool orderDesc,
        List<int>? TipoAsunto,
        int? idUnidadAdmistrativaCentral = null!
    );

        //Contador bandeja de pendientes
        Task<int?> GetAllByFiltersCountAsyncRepository(
        List<int>? TipoAsunto, int? idUnidadAdmistrativaCentral = null!
         );
        Task<int?> OPHistoricoCountAsyncRepository(
        string? noAsunto,
            DateTime? desdeRecepcion,
            DateTime? hastaRecepcion,
            string? rfc,
            string? promovente,
            List<int>? TipoAsunto,
            List<int>? EstadoTarea,
            List<int>? TipoModalidad,
            List<int>? EstadoProcesal,
            int? idUnidadAdmistrativaCentral = null!
         );

        //Bandeja de pendientes
        Task<List<ResponseConsultaByFilters>> GetBandejaPendientesAsyncRepository(
        int pageSize,
        int page,
        string? orderByColumn,
        bool orderDesc,
        List<int>? TipoAsunto,
        int? idUnidadAdmistrativaCentral = null!
    );

        //Bandeja de Historico
        Task<List<ResponseConsultaByFilters>> GetAllByFiltersAsyncRepository(
         int pageSize,
         int page,
         string? orderByColumn,
         bool orderDesc,
         string? noAsunto,
         DateTime? desdeRecepcion,
         DateTime? hastaRecepcion,
         string? rfc,
         string? promovente,
         List<int>? TipoAsunto,
         List<int>? EstadoTarea,
         List<int>? TipoModalidad,
         List<int>? EstadoProcesal,
         int? idUnidadAdmistrativaCentral = null!
     );


        #endregion

        Task<ResultTransaction> AddAsyncRepository(Consulta entity);
        Task<ResultTransaction> UpdateAsyncRepository(Consulta entity);
        Task<ResultTransaction> TurnarConsultaRepository(Consulta entity);
        Task<(string NumeroAsunto, string UnidadAdministrativa)> GetNumeroAsuntoYUnidadAdministrativaById(int id);
        Task<ResultTransaction> DeleteAsyncRepository(int id);
        Task<List<ResponseConsultaRfc>> GetRfcampliadosyncRepository(string nombre);


        Task<Consulta> GetByIdAllAsyncRepository(int id);

        Task<RequestUpdateConsulta> GetByIdAsyncUpdateRepository(int id);
        Task<RequestTurnarConsulta> GetByIdConsultaTurnadoRepository(int id);
        Task<ResponseConsultaList> GetbyDeleteRepository(int id);
        Task<ResponseConsultaList> GetByIdAsyncRepository(int id);




        Task<ResponseConsultaList> GetByIdAsync(int id);

        Task<Consulta> GetByIdConsultaAsync(int id);
        Task<Consulta> GetByIdConsultaTurnadoAsync(int id);
        Task<ResultTransaction> TurnarAsync(Consulta entity);
        Task<ResultTransaction> TurnarConsultaAsync(int id, Consulta entity);

        Task<List<ResponseConsultaList>> GetAllAsync();
        Task<List<ResponseConsultaList>> GetAllBandejaPendientesAsync();
        Task<ResultTransaction> DeleteAsync(int id);

        #region  registra archivos de consultas
        Task<ResultTransaction> AddFileAsyncRepository(ArchivoConsulta entity, DataFile dataFile);
        Task<List<ResponseArchivosConsulta>> GetArchivosByIdRegistroAsync_Repository(int id_registro);
        Task<ResponseArchivosConsulta> GetByIdArchivoAsyncRepository(int id);

        Task<ArchivoConsulta> GetIdArchivoConsultaAsyncRepository(int id);
        Task<ResultTransaction> DeleteByIdArchivoAsyncRepository(int id);

        Task<ResponseConsultaList> GetByIdDisconnected(int id);
        Task<ResultTransaction> UpdateDocumentoAsync(ArchivoConsulta entityDocumento, DataFile dataFile);

        #endregion

        #region Solicitud-Transparencia

        Task<ResultTransaction> AddAsyncSolicitudTransparenciaService(SolicitudTransparencia entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<SolicitudTransparencia> GetByIdSolicitudTransparenciaRepository(int id);
        Task<ResultTransaction> UpdateSolicitudTransparenciaRepository(SolicitudTransparencia entity);
        Task<ResponseSolicitudTransparenciaList> GetByIdSolicitudTransparenciaRepositorys(int id);
        Task<int?> GetTablaSolicitudTransparenciaCountRepository(int idConsulta);
        Task<List<ResponseSolicitudTransparenciaList>> GetTablaSolicitudTransparenciaRepository(int idConsulta);
        Task<ResultTransaction> DeleteSolicitudTransparenciaRepository(int id);

        #endregion

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
               bool? permanente
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
                bool? permanente
           );

        #region Cumplimentacion

        Task<ResultTransaction> AddCumplimentacionRepository(Cumplimentacion entity,ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<ResultTransaction> TurnarCumplimentacionRepository(Cumplimentacion entity);

        Task<ResultTransaction> UpdateCumplimentacionRepository(Cumplimentacion entity);

        Task<Cumplimentacion> GetByIdAllAsyncRepositoryCumplimentacion(int id);

        Task<ResponseConsultaList> GetByNoAsuntoConsulta(string noAsunto);
        Task<ResponseCumplimentacionList> GetCumplimentacionById(string no_asunto_consulta);
        
        #endregion
        // #region Envio correo
        //    Task<ResultTransaction> EnvioEmail(Email entity);
        // #endregion
    }
}