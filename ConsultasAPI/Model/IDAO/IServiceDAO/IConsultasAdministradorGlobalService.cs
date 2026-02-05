using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.Entities;
using Sicoj.Utils.Models;

namespace ConsultasAPI.Model.IDAO.IServiceDAO
{
  public interface IConsultasAdministradorGlobalService
  {

    #region Reactivar

    Task<Consulta> GetByIdConsultaService(int id);

    Task<ResultOperation> UpdateReactivarService(int id);

    #endregion

    #region Requerimientos
    Task<Requerimientos> GetByAllServiceRequerimientos(int id);

    Task<ResultOperation> GetRequerimientoById(int id);
    Task<ResultOperation> GetTablaRequerimientosService(int idConsulta);
    Task<ResultOperation> UpdateDescartarRequerimientosService(int idConsulta, int idSeccion, bool descartarUltimoRegistro);
    Task<ResultOperation> UpdateModificarAsuntoService(int idConsulta);
    Task<ResultOperation> UpdateModificarRequerimientoService(int idConsulta);
    #endregion

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

    #region Reasignar
    Task<ResultOperation<ResponseReasignar>> ReAsignarComercioExteriorAsync(int[] idList, string rfcAbogado, UserInformationView userInformationView);
    Task<ResultOperation<ResponseReasignar>> ReAsignarImpuestosInternosAsync(int[] idList, string rfcAbogado, UserInformationView userInformationView);
    #endregion

    #region Emisión Resolución

    Task<ResultOperation> GetResolucionById(int id);

    Task<ResultOperation> UpdateModificarResoluciuonService(int idConsulta);
    Task<ResultOperation> UpdateDescartarResolucionService(int idConsulta);
    Task<ResultOperation> GetTablaResolucionService(int idConsulta);

    #endregion

    #region Consulta
    Task<ResultOperation> GetByIdDisconnected(int id);
    Task<Consulta> GetByIdServiceAdministrador(int id);
    Task<ResultOperation> UpdateConsultaServiceAdministradorImpuestosInternos(Consulta entity);
    Task<ResultOperation> UpdateConsultaServiceAdministradorComercioExterior(Consulta entity);

    #endregion

    #region Datos Generales

    Task<ResultOperation> UpdateDescartarDatosGeneralesService(int idConsulta);

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


    Task<ResultOperation> GetTablaSolicitudInformacionService(int idConsulta);
    Task<ResultOperation> GetTablaRequerimientosProdeconService(int idConsulta);

    #region Avisos Y comunicados
    Task<ResultOperation> GetTablaAvisosYComunicados(int idConsulta);
    #endregion

    #region Solicitud de transparencia
    Task<ResultOperation> GetByIdSolicitudTransparenciaServices(int id);
    Task<ResultOperation> GetTablaSolicitudTransparenciaService(int idConsulta);
    #endregion

    #region Personas Autorizadas
    Task<ResultOperation> GetTablaPersonasAutorizadasService(int Fetch, int Page, int idConsulta);
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
