using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.Entities;
using Sicoj.Utils.Models;

namespace ConsultasAPI.Model.IDAO.IRepository
{
    public interface IConsultaRepositoryAdministradorGlobal
    {
        #region General
        Task<ResponseConsultaList> GetByIdDisconnected(int id);

        #endregion

        #region Reactivar

        Task<Consulta> GetByIdConsultaRepository(int id);

        Task<ResultTransaction> UpdateReactivarRepository(int id);

        #endregion

        #region Requerimientos
        Task<Requerimientos> GetByIdAllAsyncRepositoryRequerimientos(int id);
        //Task<ResultTransaction> UpdateAsyncRequerimientos(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile);
        Task<ResponseRequerimientoList> GetByIdAsyncRepositoryRequerimientos(int id);
        Task<int?> GetTablaRequerimientosCountAsyncRepository(int idConsulta);
        Task<int?> GetTablaAlertaRequerimientosCountAsyncRepository(int idConsulta);
        Task<List<ResponseRequerimientoList>> GetTablaRequerimientosAsyncRepository(int idConsulta);
        Task<ResultTransaction> UpdateDescartarRequerimientosRepository(int idConsulta, int idSeccion, bool descartarUltimoRegistro);
        Task<ResultTransaction> UpdateModificarAsuntoRepository(int idConsulta);
        Task<ResultTransaction> UpdateModificarRequerimientoRepository(int idConsulta);
        #endregion

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

        #region Reasignar
        Task<List<Consulta>> GetListByIdsAsync(int[] ids);
        Task<ResultTransaction> ReasignarAsync(List<Reasignar> listReasignacion);
        #endregion

        #region Emisión Resolución

        Task<List<ResponseResolucion>> GetByIdAllAsyncRepositoryResolucion(int id);
        //Task<ResultTransaction> UpdateAsyncResolucion(Resolucion entity);
        Task<ResultTransaction> UpdateModificarResolucionRepository(int idConsulta);
        Task<ResultTransaction> UpdateDescartarResolucionRepository(int idConsulta);
        Task<List<ResponseResolucion>> GetTablaResolucionAsyncRepository(int idConsulta);
        #endregion

        #region Consultas

        Task<Consulta> GetByIdAsyncRepositoryAdministrador(int id);
        Task<ResultTransaction> UpdateAsyncRepositoryAdministradorImpuestosInternos(Consulta entity);
        Task<ResultTransaction> UpdateAsyncRepositoryAdministradorComercioExterior(Consulta entity);
        Task<List<ResponseConsultaList>> GetTablaConsultaRepository(int idConsulta);

        #endregion

        #region Datos Generales

        Task<ResultTransaction> UpdateDescartarDatosGeneralesRepository(int idConsulta);

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

        Task<int?> GetTablaSolicitudInformacionCountAsyncRepository(int idConsulta);
        Task<int?> GetTablaAlertaSolicitudInformacionCountAsyncRepository(int idConsulta);
        Task<List<ResponseSolicitudInformacionList>> GetTablaSolicitudInformacionAsyncRepository(int idConsulta);

        Task<List<ResponseRequerimientoProdeconList>> GetTablaRequerimientosProdeconAsyncRepository(int idConsulta);
        Task<int?> GetTablaRequerimientosProdeconCountAsyncRepository(int idConsulta);

        #region  Avisos y comunicados
        Task<int?> GetTablaAvisosYComunicadosCountAsyncRepository(int idConsulta);
        Task<int?> GetAlertaAvisosYComunicadosCountAsyncRepository(int idConsulta);
        Task<List<ResponseAvisosComunicadosList>> GetTablaAvisosYComunicadosAsyncRepository(int idConsulta);
        #endregion

        #region Solicitud de transparencia
        Task<ResponseSolicitudTransparenciaList> GetByIdSolicitudTransparenciaRepositorys(int id);
        Task<int?> GetTablaSolicitudTransparenciaCountRepository(int idConsulta);
        Task<List<ResponseSolicitudTransparenciaList>> GetTablaSolicitudTransparenciaRepository(int idConsulta);
        #endregion

        #region Personas Autorizadas
        Task<List<ResponseTablaPersonasAutorizadas>> GetTablaPersonasAutorizadasAsyncRepository(
    int pageSize,
    int page,
    int idConsulta
    );

        Task<int?> GetTablaPersonasAutorizadasCountAsyncRepository(int idConsulta

        );
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
