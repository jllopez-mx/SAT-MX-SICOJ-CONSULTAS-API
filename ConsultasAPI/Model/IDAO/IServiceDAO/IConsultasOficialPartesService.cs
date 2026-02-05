using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.Entities;
using Sicoj.Utils.Models;


namespace ConsultasAPI.Model.IDAO.IServiceDAO
{
    public interface IConsultasOficialPartesService
    {
        #region  Bandejas
        //Bandeja de Pendientes Cumplimentación
         Task<ResultOperation> GetBandejaPendientesCumplimentacionService(int Fetch, int Page, string? OrderByColumn, bool OrderDesc, UserInformationView userInformationView);
        //Bandeja de Pendientes
        Task<ResultOperation> GetBandejaPendientesService(int Fetch, int Page, string? OrderByColumn, bool OrderDesc, UserInformationView userInformationView);

        //Bandeja de Historico
         Task<ResultOperation> GetBandejaHistoricoByFiltersService(int Fetch, int Page, string? OrderByColumn, bool OrderDesc, 
            string? noAsunto, DateTime? fechaDesde, DateTime? fechaHasta, string? rfc, string? promovente, 
            List<int>? TipoAsunto, List<int>? EstadoTarea, List<int>? TipoModalidad, List<int>? EstadoProcesal, UserInformationView userInformationView);

        #endregion
        
        Task<ResultOperation> AddConsultaService(Consulta entity,UserInformationView userInformationView);
        Task<ResultOperation> UpdateConsultaService(Consulta entity, UserInformationView userInformationView);
        Task<ResultOperation<ResponseConsultaTurnar>> TurnarConsultaService(Consulta entity);
        Task<ResultOperation> DeleteConsultaService(Consulta entity);
        Task<List<ResponseConsultaRfc>> GetRfcampliadosyncService(string nombre);       

        Task<RequestTurnarConsulta> GetByIdConsultaTurnadoService(int id);
        Task<Consulta> GetByAllService(int id);
        Task<ResponseConsultaList> GetByIdDeleteService(int id);
        Task<ResultOperation> GetByIdService(int id);
        //ahi
      


        //Archivos
        Task<ResultOperation<int>> AddArchivosAsyncService(ArchivoConsulta entity,DataFile dataFile);
        Task<ResponseArchivosConsulta> GetByIdArchivoDeleteService(int id);
        Task<ResponseArchivosConsulta> GetByIdArchivoService(int id);
        Task<ResultOperation> DeleteArchivoConsultaService(ResponseArchivosConsulta entity);
        Task<List<ResponseArchivosConsulta>> GetAllArchivoService(int id_consulta);
        Task<ArchivoConsulta> GetIdArchivoConsultaService(int id);
        Task<ResultOperation> GetByIdDisconnected(int id);
        Task<ResultOperation<int>> UpdateArchivoService(ArchivoConsulta entity, DataFile dataFile);

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
           UserInformationView userInformationView
       );   
        
        #region Solicitud Transparencia

        Task<ResultOperation> AddSolicitudTransparenciaService(SolicitudTransparencia entity,ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<SolicitudTransparencia> GetByIdSolicitudTransparenciaService(int id);
        Task<ResultOperation> UpdateSolicitudTransparenciaService(SolicitudTransparencia entity);
        Task<ResultOperation> GetByIdSolicitudTransparenciaServices(int id);
        Task<ResultOperation> GetTablaSolicitudTransparenciaService(int idConsulta);
        Task<ResultOperation> DeleteSolicitudTransparenciaService(SolicitudTransparencia entity);

        #endregion
        
        #region Cumplimentacion

        Task<ResultOperation> AddCumplimentacionService(Cumplimentacion entity,ArchivoConsulta entityDocumento, DataFile dataFile);

        Task<ResultOperation<ResponseConsultaTurnar>> TurnarCumplimentacionService(Cumplimentacion entity);

        Task<ResultOperation> UpdateCumplimentacionService(Cumplimentacion entity, UserInformationView userInformationView);
        Task<Cumplimentacion> GetByAllServiceCumplimentacion(int id);

        Task<ResultOperation> GetByNoAsuntoCumplimentacion(string noAsunto,UserInformationView userInformationView,int contadorvisitas);

        #endregion

        #region Envio correo
           Task<ResultOperation> EnvioEmail(Email entity);
        #endregion


    }
}