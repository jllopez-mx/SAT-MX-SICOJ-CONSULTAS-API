
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.Entities;
using Sicoj.Utils.Models;

namespace ConsultasAPI.Model.IDAO.IServiceDAO
{
  public interface IConsultasSupervisorService
  {
    #region Historico de Asuntos
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
     DateTime? fechaCSSJDesde,
     DateTime? fechaCSSJHasta,
     string? rfcPromovente,
     string? promovente,
     List<int>? TipoAsunto,
     int? idAdministracion,
     int? idSubadministracion,
     string? idAbogadoAsigno,
     List<int>? EstadoTarea,
     List<int>? TipoModalidad,
     List<int>? EstadoProcesal,
     UserInformationView userInformationView
 );
    #endregion

    #region Consulta
    Task<ResultOperation> GetByIdDisconnected(int id);
    #endregion

    #region Solicitud de transparencia
    Task<ResultOperation> GetTablaSolicitudTransparenciaService(int idConsulta);
    #endregion
    #region Archivos
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

    Task<ResponseArchivosConsulta> GetByIdArchivoService(int id);

    #endregion
    #region  Requerimientos

    Task<ResultOperation> GetTablaRequerimientosService(int idConsulta);

    #endregion
    #region  Solicitud de opnion de la información
    Task<ResultOperation> GetTablaSolicitudInformacionService(int idConsulta);
    #endregion
    #region Emisión Resolución

    Task<ResultOperation> GetTablaResolucionService(int idConsulta);
    Task<ResultOperation> GetResolucionById(int id);
    #endregion
    #region Avisos Y comunicados
    Task<ResultOperation> GetTablaAvisosYComunicados(int idConsulta);
    #endregion
    #region  Requerimiento Prodecon
    Task<ResultOperation> GetTablaRequerimientosProdeconService(int idConsulta);
    #endregion
    #region Medios Defensa
    Task<ResultOperation> GetTablaMediosDefensa(string noAsunto);
    Task<ResultOperation> GetTablaMediosDefensaGeneral(string noAsunto);

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

    Task<ResultOperation> GetTablaMediosDefensaGeneral(List<string> noAsunto);
  }
}