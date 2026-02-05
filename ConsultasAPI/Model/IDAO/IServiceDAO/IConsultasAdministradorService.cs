using ConsultasAPI.Model.Entities;
using Sicoj.Utils.Models;
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.DTO.Response.Administrador;

namespace ConsultasAPI.Model.IDAO.IServiceDAO
{

  public interface IConsultasAdministradorService
  {
    Task<ResultOperation> GetBandejaPendientes_AdministradorService(int Fetch, int Page, string? OrderByColumn, bool OrderDesc, UserInformationView userInformationView);
    Task<Consulta> GetByIdServiceAdministrador(int id);
    Task<ResultOperation> UpdateConsultaServiceAdministradorComercioExterior(Consulta entity);
    Task<ResultOperation> UpdateConsultaServiceAdministradorImpuestosInternos(Consulta entity);
    Task<ResultOperation> AddPersonasAutorizadas(PersonasAutorizadas entity);
    Task<PersonasAutorizadas> GetByAllServicePersonaAutorizadas(int id);
    Task<ResultOperation> UpdatePersonasAutorizadas(PersonasAutorizadas entity);
    Task<ResultOperation> DeletePersonasAutorizadas(PersonasAutorizadas entity);
    Task<ResultOperation> GetTablaPersonasAutorizadasService(int Fetch, int Page, int idConsulta);
    Task<ResultOperation> GetByIdAdministradorService(int id);
    Task<ResultOperation> AddRemisionAdministradorService(Remision entity, ArchivoConsulta entityDocumento, DataFile dataFile);
    Task<ResultOperation> GetTablaRemisionAdministradorService(int Fetch, int Page);

    Task<Consulta> GetByAllService(int id);
    Task<ResultOperation<int>> AddArchivosAsyncService(ArchivoConsulta entity, DataFile dataFile);
    Task<ArchivoConsulta> GetIdArchivoConsultaService(int id);
    Task<ResultOperation<int>> UpdateArchivoService(ArchivoConsulta entity, DataFile dataFile);
    Task<ResponseArchivosConsulta> GetByIdArchivoDeleteService(int id);
    Task<ResultOperation> DeleteArchivoConsultaService(ResponseArchivosConsulta entity);
    Task<List<ResponseArchivosConsulta>> GetAllArchivoService(int id_registro);
    Task<ResponseArchivosConsulta> GetByIdArchivoService(int id);
    Task<ResultOperation<ResponseAsignar>> AsignarConsultaService(Consulta entity);
    Task<ResultOperation> GetByIdDisconnected(int id);

    #region Historico
    Task<ResultOperation> GetBandejaHistoricoByFiltersService
    (
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
       List<int>? TipoAsunto,
       int? idAdministracion,
       int? idSubadministracion,
       string? idAbogadoAsigno,
       List<int>? EstadoTarea,
       List<int>? TipoModalidad,
       List<int>? EstadoProcesal,
       UserInformationView userInformationView,
       bool reasignarAsuntos
   );
    #endregion

    Task<ResultOperation> AddRequerimientoService(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile);
    Task<ResultOperation> SolicitaRequerimientoService(Consulta entity);
    Task<ResultOperation> AddRequerimientoProdeconService(RequerimientosProdecon entity, ArchivoConsulta entityDocumento, DataFile dataFile);
    Task<ResultOperation> GetRequerimientoById(int id);
    Task<ResultOperation> GetRequerimientoProdeconById(int id);
    Task<ResultOperation> GetTablaRequerimientosService(int idConsulta);
    Task<ResultOperation> GetTablaRequerimientosProdeconService(int idConsulta);
    Task<Requerimientos> GetByAllServiceRequerimientos(int id);
    Task<RequerimientosProdecon> GetByAllServiceRequerimientosProdecon(int id);
    Task<ResultOperation> UpdateRequerimientos(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile);
    Task<ResultOperation> UpdateRequerimientosProdecon(RequerimientosProdecon entity);

    //solicitud Información
    Task<ResultOperation> AddSolicitudInformacionService(SolicitudInformacion entity);
    Task<ResultOperation> GetTablaSolicitudInformacionService(int idConsulta);
    Task<ResultOperation> GetSolicitudInformacionById(int id);
    Task<SolicitudInformacion> GetByAllServiceSolicitudInformacion(int id);
    Task<ResultOperation> UpdateSolicitudInformacion(SolicitudInformacion entity);

    Task<ResultOperation> AddResolucionService(Resolucion entity, ArchivoConsulta entityDocumento, DataFile dataFile);//Aqui
    Task<Resolucion> GetByAllServiceResolucion(int idConsulta);
    Task<ResultOperation> GetResolucionById(int id);
    Task<ResultOperation> UpdateResolucion(Resolucion entity);//
    Task<ResultOperation> ConcluirResolucion(Resolucion entity, ArchivoConsulta entityDocumento, DataFile dataFile);//

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

    Task<Cumplimentacion> GetByAllServiceCumplimentacion(int id);
    Task<ResultOperation<ResponseAsignarCumplimentacion>> AsignarConsultaCumplimentacionService(Cumplimentacion entity);
    Task<ResultOperation> UpdateCumplimentacionService(Cumplimentacion entity, UserInformationView userInformationView);
    Task<ResultOperation> AddResolucionCumplimentacionService(ResolucionCumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile);
    Task<ResolucionCumplimentacion> GetByAllServiceResolucionCumplimentacion(int idCumplimentacion);
    Task<ResultOperation> UpdateResolucionCumplimentacion(ResolucionCumplimentacion entity);
    Task<ResultOperation> ConcluirResolucionCumplimentacion(ResolucionCumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile);


    Task<ResultOperation> GetTablaResolucionService(int idConsulta);
    #endregion
    #region Medios Defensa
    Task<ResultOperation> GetTablaMediosDefensa(List<string> noAsunto);
    Task<ResultOperation> GetTablaMediosDefensaGeneral(List<string> noAsunto);

    #endregion

    #region Exportar
    Task<List<ResponseReporteGlobalConsultas>> ExportarConsulta
  (
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
    Task<List<ResponseReporteGlobalCumplimentacion>> ExportarCumplimentacion
   (
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
      Task<List<ResponseReporteGeneral>> ExportarReporteGeneral
   (
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
    Task<List<Response_Resolucion_PDF>> Exporta_PDF (string noAsunto );

             
    }
}