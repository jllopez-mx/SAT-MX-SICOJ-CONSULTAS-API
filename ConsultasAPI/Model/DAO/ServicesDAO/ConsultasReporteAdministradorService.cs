using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.IDAO.IRepository;
using ConsultasAPI.Model.IDAO.IServiceDAO;

namespace ConsultasAPI.Model.DAO.ServicesDAO
{
    public class ConsultasReporteAdministradorService : IConsultasReporteAdministradorService
    {
        private readonly IConsultaRepositoryReporteAdministrador _repositoryReporteAdministrador;

        public ConsultasReporteAdministradorService(IConsultaRepositoryReporteAdministrador repositoryReporteAdministrador)
        {
            _repositoryReporteAdministrador = repositoryReporteAdministrador ?? throw new ArgumentNullException(nameof(repositoryReporteAdministrador));
        }

        #region  Exportar
        public async Task<List<ResponseReporteGlobalConsultas>> ExportarConsulta(
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

        ) =>
                await _repositoryReporteAdministrador.ExportarConsultaAsyncRepository(
                     noAsunto,
                     rfcPromovente,
                     promovente,
                     idAdministracion,
                     idSubadministracion,
                     idAbogadoAsigno,
                     TipoEntrada,
                     TipoAsunto,
                     EstadoProcesal,
                     idTema,
                     fechaPresentacionDesde,
                     fechaPresentacionHasta,
                     fechaVencimientoDesde,
                     fechaVencimientoHasta,
                     FechaConclucionDesde,
                     FechaConclucionHasta,
                     FechaOficioNotificacionResolucionDesde,
                     FechaOficioNotificacionResolucionHasta,
                     FechaIngresoProdeconDesde,
                     FechaIngresoProdeconHasta,
                     FechaSolicituTransparenciaDesde,
                     FechaSolicituTransparenciaHasta

                );

        public async Task<List<ResponseReporteGlobalCumplimentacion>> ExportarCumplimentacion(
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

) =>
     await _repositoryReporteAdministrador.ExportarCumplimentacionAsyncRepository(
          noAsunto,
          rfcPromovente,
          promovente,
          idAdministracion,
          idSubadministracion,
          idAbogadoAsigno,
          TipoEntrada,
          TipoAsunto,
          EstadoProcesal,
          idTema,
          fechaPresentacionDesde,
          fechaPresentacionHasta,
          fechaVencimientoDesde,
          fechaVencimientoHasta,
          FechaConclucionDesde,
          FechaConclucionHasta,
          FechaOficioNotificacionResolucionDesde,
          FechaOficioNotificacionResolucionHasta,
          FechaIngresoProdeconDesde,
          FechaIngresoProdeconHasta,
          FechaSolicituTransparenciaDesde,
          FechaSolicituTransparenciaHasta

     );
        public async Task<List<ResponseReporteGeneral>> ExportarReporteGeneral(
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

             ) =>
                     await _repositoryReporteAdministrador.ExportarReporteGeneralAsyncRepository(
               noAsunto,
               rfcPromovente,
               promovente,
               idAdministracion,
               idSubadministracion,
               idAbogadoAsigno,
               TipoEntrada,
               TipoAsunto,
               EstadoProcesal,
               idTema,
               fechaPresentacionDesde,
               fechaPresentacionHasta,
               fechaVencimientoDesde,
               fechaVencimientoHasta,
               FechaConclucionDesde,
               FechaConclucionHasta,
               FechaOficioNotificacionResolucionDesde,
               FechaOficioNotificacionResolucionHasta,
               FechaIngresoProdeconDesde,
               FechaIngresoProdeconHasta,
               FechaSolicituTransparenciaDesde,
               FechaSolicituTransparenciaHasta

          );
        #endregion


    }
}