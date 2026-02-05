using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.Entities;
using Sicoj.Utils.Models;
namespace ConsultasAPI.Model.IDAO.IRepository
{
    public interface IConsultaRepositorySupervisor
    {
        #region  Historico de Asuntos
        Task<int?> GetAllByFiltersCountAsyncRepository
        (
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
           int? idUnidadAdmistrativaCentral = null!,
           int? idUnidadAdmistrativa = null!
       );

        Task<List<ResponseConsultaByFiltersAdministrador>> GetAllByFiltersAsyncRepository(
              int pageSize,
              int page,
              string? orderByColumn,
              bool orderDesc,
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
              int? idUnidadAdmistrativaCentral = null!,
              int? idUnidadAdmistrativa = null!
          );

        #endregion

        #region Consultas
        Task<ResponseConsultaList> GetByIdDisconnected(int id);
        Task<List<ResponseConsultaList>> GetTablaConsultaRepository(int idConsulta);
        #endregion

        #region Solicitud de transparencia
        Task<int?> GetTablaSolicitudTransparenciaCountRepository(int idConsulta);
        Task<List<ResponseSolicitudTransparenciaList>> GetTablaSolicitudTransparenciaRepository(int idConsulta);
        #endregion
        #region Archivos

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

        Task<ResponseArchivosConsulta> GetByIdArchivoAsyncRepository(int id);
        #endregion
        #region  Requerimientos
        Task<int?> GetTablaRequerimientosCountAsyncRepository(int idConsulta);
        Task<int?> GetTablaAlertaRequerimientosCountAsyncRepository(int idConsulta);
        Task<List<ResponseRequerimientoList>> GetTablaRequerimientosAsyncRepository(int idConsulta);
        #endregion
        #region Solicitur de opinion de la información
        Task<int?> GetTablaSolicitudInformacionCountAsyncRepository(int idConsulta);
        Task<int?> GetTablaAlertaSolicitudInformacionCountAsyncRepository(int idConsulta);
        Task<List<ResponseSolicitudInformacionList>> GetTablaSolicitudInformacionAsyncRepository(int idConsulta);
        #endregion
        #region emisión de la resolución
        Task<List<ResponseResolucion>> GetTablaResolucionAsyncRepository(int idConsulta);
        Task<List<ResponseResolucion>> GetByIdAllAsyncRepositoryResolucion(int id);
        #endregion
        #region  Avisos y comunicados
        Task<int?> GetTablaAvisosYComunicadosCountAsyncRepository(int idConsulta);
        Task<int?> GetAlertaAvisosYComunicadosCountAsyncRepository(int idConsulta);
        Task<List<ResponseAvisosComunicadosList>> GetTablaAvisosYComunicadosAsyncRepository(int idConsulta);
        #endregion
        #region  Requerimientos prodecon
        Task<List<ResponseRequerimientoProdeconList>> GetTablaRequerimientosProdeconAsyncRepository(int idConsulta);
        Task<int?> GetTablaRequerimientosProdeconCountAsyncRepository(int idConsulta);
        #endregion
        #region Medios Defensa
        Task<List<ResponseMediosDefensa>> GetTablaMediosDefensaAsyncRepository(string noAsunto);
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
    }
}