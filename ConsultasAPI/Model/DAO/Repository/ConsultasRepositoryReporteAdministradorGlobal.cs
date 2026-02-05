using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.IDAO.IRepository;
using Sicoj.Utils.Postgres;
using System.Data;
using ConsultasAPI.Model.ViewModels.Enums;
using NpgsqlTypes;

namespace ConsultasAPI.Model.DAO.Repository
{
    public class ConsultasRepositoryReporteAdministradorGlobal : IConsultaRepositoryReporteAdministradorGlobal
    {
        #region Variables

        private readonly ISqlTools _database;


        public ConsultasRepositoryReporteAdministradorGlobal(ISqlTools database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        #endregion
        #region Exportar
        public async Task<List<ResponseReporteGlobalConsultas>> ExportarConsultaAsyncRepository(
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

                  )
        {


            ParameterPGsql[] parameters =
            {

                new ParameterPGsql("p_no_asunto", NpgsqlDbType.Text, noAsunto),
                new ParameterPGsql("p_rfc_promovente", NpgsqlDbType.Varchar, rfcPromovente),
                new ParameterPGsql("p_promovente", NpgsqlDbType.Text, promovente),
                new ParameterPGsql("p_id_administracion", NpgsqlDbType.Integer, idAdministracion),
                new ParameterPGsql("p_id_subadministracion", NpgsqlDbType.Integer, idSubadministracion),
                new ParameterPGsql("p_id_abogado_asigno", NpgsqlDbType.Text, idAbogadoAsigno),
                new ParameterPGsql("p_id_tipo_entrada", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoEntrada is null || !TipoEntrada.Any()) ? DBNull.Value :  TipoEntrada.ToArray()),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoAsunto is null || !TipoAsunto.Any()) ? DBNull.Value :  TipoAsunto.ToArray()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Array | NpgsqlDbType.Integer, (EstadoProcesal is null || !EstadoProcesal.Any()) ? DBNull.Value :  EstadoProcesal.ToArray()),
                new ParameterPGsql("p_id_tema", NpgsqlDbType.Integer, idTema),
                new ParameterPGsql("p_fecha_presentacion_desde", NpgsqlDbType.Date, fechaPresentacionDesde),
                new ParameterPGsql("p_fecha_presentacion_hasta", NpgsqlDbType.Date, fechaPresentacionHasta),
                new ParameterPGsql("p_fecha_vencimiento_desde", NpgsqlDbType.Date, fechaVencimientoDesde),
                new ParameterPGsql("p_fecha_vencimiento_hasta", NpgsqlDbType.Date, fechaVencimientoHasta),
                new ParameterPGsql("p_fecha_conclucion_desde", NpgsqlDbType.Date, FechaConclucionDesde),
                new ParameterPGsql("p_fecha_conclucion_hasta", NpgsqlDbType.Date, FechaConclucionHasta),
                new ParameterPGsql("p_fecha_oficio_notificacion_resolucion_desde", NpgsqlDbType.Date, FechaOficioNotificacionResolucionDesde),
                new ParameterPGsql("p_fecha_oficio_notificacion_resolucion_hasta", NpgsqlDbType.Date, FechaOficioNotificacionResolucionHasta),
                new ParameterPGsql("p_fecha_ingreso_prodecon_desde", NpgsqlDbType.Date, FechaIngresoProdeconDesde),
                new ParameterPGsql("p_fecha_ingreso_prodecon_hasta", NpgsqlDbType.Date, FechaIngresoProdeconHasta),
                new ParameterPGsql("p_fecha_solicitudtransparencia_desde", NpgsqlDbType.Date, FechaSolicituTransparenciaDesde),
                new ParameterPGsql("p_fecha_solicitudtransparencia_hasta", NpgsqlDbType.Date, FechaSolicituTransparenciaHasta),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.GetReporteConsultas,
                     parameters!
             );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            List<ResponseReporteGlobalConsultas> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {

                        //Datos Generales
                        administracion = item.IsNull(0) ? null! : item.Field<string>(0)!,
                        subadministracion = item.IsNull(1) ? null! : item.Field<string>(1)!,
                        no_asunto = item.IsNull(2) ? null! : item.Field<string>(2),
                        rfc = item.IsNull(3) ? null! : item.Field<string>(3)!,
                        promovente = item.IsNull(4) ? null! : item.Field<string>(4),
                        rfc_contribuyente = item.IsNull(5) ? null! : item.Field<string>(5)!,
                        contribuyente = item.IsNull(6) ? null! : item.Field<string>(6)!,
                        despacho_autorizado = item.IsNull(7) ? null! : item.Field<string>(7)!,
                        fecha_presentacion = item.IsNull(8) ? null! : item.Field<DateTime>(8).ToString("yyyy-MM-dd")!,
                        tipoAsunto = item.IsNull(9) ? null! : item.Field<string>(9)!,
                        tema = item.IsNull(10) ? null! : item.Field<string>(10)!,
                        monto = item.IsNull(11) ? 0 : item.Field<decimal>(11)!,
                        fecha_recepcion = item.IsNull(12) ? null! : item.Field<DateTime>(12).ToString("yyyy-MM-dd")!,
                        fecha_vencimiento = item.IsNull(13) ? null! : item.Field<DateTime>(13).ToString("yyyy-MM-dd")!,
                        estadoProcesal = item.IsNull(14) ? null! : item.Field<string>(14)!,
                        //Asignación       
                        abogado = item.IsNull(15) ? null! : item.Field<string>(15)!,
                        //Remisión
                        administracion_remite = item.IsNull(16) ? null! : item.Field<string>(16)!,
                        no_oficio_remision = item.IsNull(17) ? null! : item.Field<string>(17)!,
                        fecha_oficio_remision = item.IsNull(18) ? null! : item.Field<DateTime>(18).ToString("yyyy-MM-dd")!,
                        //Requerimiento
                        no_oficio_Requerimiento = item.IsNull(19) ? null! : item.Field<string>(19)!,
                        fecha_oficio_requerimiento = item.IsNull(20) ? null! : item.Field<DateTime>(20).ToString("yyyy-MM-dd")!,
                        fecha_notificacion_requerimiento = item.IsNull(21) ? null! : item.Field<DateTime>(21).ToString("yyyy-MM-dd")!,
                        fecha_vencimiento_Requerimiento = item.IsNull(22) ? null! : item.Field<DateTime>(22).ToString("yyyy-MM-dd")!,
                        atendio_requerimiento = item.IsNull(23) ? false : item.Field<bool>(23)!,
                        fecha_atencion_requerimiento = item.IsNull(24) ? null! : item.Field<DateTime>(24).ToString("yyyy-MM-dd")!,
                        //Solicitud de opinion de la información
                        unidadAdministrativaSolicitudInformacion = item.IsNull(25) ? null! : item.Field<string>(25)!,
                        noOficioSolicitudInformacion = item.IsNull(26) ? null! : item.Field<string>(26)!,
                        fechaOficioSolicitudInformacion = item.IsNull(27) ? null! : item.Field<DateTime>(27).ToString("yyyy-MM-dd")!,
                        atendioSolicitudInformacion = item.IsNull(28) ? false : item.Field<bool>(28)!,
                        noOficioRespuestaSolicitudInformacion = item.IsNull(29) ? null! : item.Field<string>(29)!,
                        fechaOficioRespuestaSolicitudInformacion = item.IsNull(30) ? null! : item.Field<DateTime>(30).ToString("yyyy-MM-dd")!,
                        fechaRecepcionSolicitudInformacion = item.IsNull(31) ? null! : item.Field<DateTime>(31).ToString("yyyy-MM-dd")!,
                        //Emisión de la resolución
                        noOficioResolucion = item.IsNull(32) ? null! : item.Field<string>(32)!,
                        fechaOficionResolucion = item.IsNull(33) ? null! : item.Field<DateTime>(33).ToString("yyyy-MM-dd")!,
                        sentido = item.IsNull(34) ? null! : item.Field<string>(34)!,
                        fechaOficioNotificacion = item.IsNull(35) ? null! : item.Field<DateTime>(35).ToString("yyyy-MM-dd")!,
                        //Requerimientos prodecon
                        noOficioProdecon = item.IsNull(36) ? null! : item.Field<string>(36)!,
                        noExpedienteProdecon = item.IsNull(37) ? null! : item.Field<string>(37)!,
                        fechaOficioProdecon = item.IsNull(38) ? null! : item.Field<DateTime>(38).ToString("yyyy-MM-dd")!,
                        fechaIngresoProdecon = item.IsNull(39) ? null! : item.Field<DateTime>(39).ToString("yyyy-MM-dd")!,
                        atencionProdecon = item.IsNull(40) ? null! : item.Field<string>(40)!,
                        //Solicitud Transparencia
                        noSolicitudTrasparencia = item.IsNull(41) ? null! : item.Field<string>(41)!,
                        fechaSolicitudTransparencia = item.IsNull(42) ? null! : item.Field<DateTime>(42).ToString("yyyy-MM-dd")!,

                    }
                );
            }

            return resultList;

        }
        public async Task<List<ResponseReporteGlobalCumplimentacion>> ExportarCumplimentacionAsyncRepository(
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

             )
        {


            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_no_asunto", NpgsqlDbType.Text, noAsunto),
                new ParameterPGsql("p_rfc_promovente", NpgsqlDbType.Varchar, rfcPromovente),
                new ParameterPGsql("p_promovente", NpgsqlDbType.Text, promovente),
                new ParameterPGsql("p_id_administracion", NpgsqlDbType.Integer, idAdministracion),
                new ParameterPGsql("p_id_subadministracion", NpgsqlDbType.Integer, idSubadministracion),
                new ParameterPGsql("p_id_abogado_asigno", NpgsqlDbType.Text, idAbogadoAsigno),
                new ParameterPGsql("p_id_tipo_entrada", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoEntrada is null || !TipoEntrada.Any()) ? DBNull.Value :  TipoEntrada.ToArray()),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoAsunto is null || !TipoAsunto.Any()) ? DBNull.Value :  TipoAsunto.ToArray()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Array | NpgsqlDbType.Integer, (EstadoProcesal is null || !EstadoProcesal.Any()) ? DBNull.Value :  EstadoProcesal.ToArray()),
                new ParameterPGsql("p_id_tema", NpgsqlDbType.Integer, idTema),
                new ParameterPGsql("p_fecha_presentacion_desde", NpgsqlDbType.Date, fechaPresentacionDesde),
                new ParameterPGsql("p_fecha_presentacion_hasta", NpgsqlDbType.Date, fechaPresentacionHasta),
                new ParameterPGsql("p_fecha_vencimiento_desde", NpgsqlDbType.Date, fechaVencimientoDesde),
                new ParameterPGsql("p_fecha_vencimiento_hasta", NpgsqlDbType.Date, fechaVencimientoHasta),
                new ParameterPGsql("p_fecha_conclucion_desde", NpgsqlDbType.Date, FechaConclucionDesde),
                new ParameterPGsql("p_fecha_conclucion_hasta", NpgsqlDbType.Date, FechaConclucionHasta),
                new ParameterPGsql("p_fecha_oficio_notificacion_resolucion_desde", NpgsqlDbType.Date, FechaOficioNotificacionResolucionDesde),
                new ParameterPGsql("p_fecha_oficio_notificacion_resolucion_hasta", NpgsqlDbType.Date, FechaOficioNotificacionResolucionHasta),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.GetReporteCumplimnetacion,
                     parameters!
             );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            List<ResponseReporteGlobalCumplimentacion> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        //Datos Generales
                        administracion = item.IsNull(0) ? null! : item.Field<string>(0)!,
                        subadministracion = item.IsNull(1) ? null! : item.Field<string>(1)!,
                        no_asunto = item.IsNull(2) ? null! : item.Field<string>(2),
                        rfc = item.IsNull(3) ? null! : item.Field<string>(3)!,
                        promovente = item.IsNull(4) ? null! : item.Field<string>(4),
                        rfc_contribuyente = item.IsNull(5) ? null! : item.Field<string>(5)!,
                        contribuyente = item.IsNull(6) ? null! : item.Field<string>(6)!,
                        fecha_recepcion = item.IsNull(7) ? null! : item.Field<DateTime>(7).ToString("yyyy-MM-dd")!,
                        tipoAsunto = item.IsNull(8) ? null! : item.Field<string>(8)!,
                        no_asunto_cumplimentar = item.IsNull(9) ? null! : item.Field<string>(9)!,
                        fecha_vencimiento = item.IsNull(10) ? null! : item.Field<DateTime>(10).ToString("yyyy-MM-dd")!,
                        estadoProcesal = item.IsNull(11) ? null! : item.Field<string>(11)!,
                        //Asignación       
                        abogado = item.IsNull(12) ? null! : item.Field<string>(12)!,
                        //Emisión de la resolución
                        noOficioResolucion = item.IsNull(13) ? null! : item.Field<string>(13)!,
                        fechaOficionResolucion = item.IsNull(14) ? null! : item.Field<DateTime>(14).ToString("yyyy-MM-dd")!,
                        sentido = item.IsNull(15) ? null! : item.Field<string>(15)!,
                        fechaOficioNotificacion = item.IsNull(16) ? null! : item.Field<DateTime>(16).ToString("yyyy-MM-dd")!,
                        //Informacion de la cumplimentacion
                        numeroJuicioCumplimentacion = item.IsNull(17) ? null! : item.Field<string>(17)!,
                        fecha_firmeza_cumplimnetacion = item.IsNull(18) ? null! : item.Field<DateTime>(18).ToString("yyyy-MM-dd")!,
                        organoJurisdiccionalCumplimnetacion = item.IsNull(19) ? null! : item.Field<int>(19).ToString()!,
                        UnidadAdministrativaSolicitaCump = item.IsNull(20) ? null! : item.Field<string>(20)!,
                        oficioResolucionCumplimnetacion = item.IsNull(21) ? null! : item.Field<string>(21)!,
                        fechaOficioResolucionCumplimentacion = item.IsNull(22) ? null! : item.Field<DateTime>(22).ToString("yyyy-MM-dd")!,
                    }
                );
            }

            return resultList;

        }
        public async Task<List<ResponseReporteGeneral>> ExportarReporteGeneralAsyncRepository(
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

           )
        {


            ParameterPGsql[] parameters =
            {

                new ParameterPGsql("p_no_asunto", NpgsqlDbType.Text, noAsunto),
                new ParameterPGsql("p_rfc_promovente", NpgsqlDbType.Varchar, rfcPromovente),
                new ParameterPGsql("p_promovente", NpgsqlDbType.Text, promovente),
                new ParameterPGsql("p_id_administracion", NpgsqlDbType.Integer, idAdministracion),
                new ParameterPGsql("p_id_subadministracion", NpgsqlDbType.Integer, idSubadministracion),
                new ParameterPGsql("p_id_abogado_asigno", NpgsqlDbType.Text, idAbogadoAsigno),
                new ParameterPGsql("p_id_tipo_entrada", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoEntrada is null || !TipoEntrada.Any()) ? DBNull.Value :  TipoEntrada.ToArray()),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoAsunto is null || !TipoAsunto.Any()) ? DBNull.Value :  TipoAsunto.ToArray()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Array | NpgsqlDbType.Integer, (EstadoProcesal is null || !EstadoProcesal.Any()) ? DBNull.Value :  EstadoProcesal.ToArray()),
                new ParameterPGsql("p_id_tema", NpgsqlDbType.Integer, idTema),
                new ParameterPGsql("p_fecha_presentacion_desde", NpgsqlDbType.Date, fechaPresentacionDesde),
                new ParameterPGsql("p_fecha_presentacion_hasta", NpgsqlDbType.Date, fechaPresentacionHasta),
                new ParameterPGsql("p_fecha_vencimiento_desde", NpgsqlDbType.Date, fechaVencimientoDesde),
                new ParameterPGsql("p_fecha_vencimiento_hasta", NpgsqlDbType.Date, fechaVencimientoHasta),
                new ParameterPGsql("p_fecha_conclucion_desde", NpgsqlDbType.Date, FechaConclucionDesde),
                new ParameterPGsql("p_fecha_conclucion_hasta", NpgsqlDbType.Date, FechaConclucionHasta),
                new ParameterPGsql("p_fecha_oficio_notificacion_resolucion_desde", NpgsqlDbType.Date, FechaOficioNotificacionResolucionDesde),
                new ParameterPGsql("p_fecha_oficio_notificacion_resolucion_hasta", NpgsqlDbType.Date, FechaOficioNotificacionResolucionHasta),
                new ParameterPGsql("p_fecha_ingreso_prodecon_desde", NpgsqlDbType.Date, FechaIngresoProdeconDesde),
                new ParameterPGsql("p_fecha_ingreso_prodecon_hasta", NpgsqlDbType.Date, FechaIngresoProdeconHasta),
                new ParameterPGsql("p_fecha_solicitudtransparencia_desde", NpgsqlDbType.Date, FechaSolicituTransparenciaDesde),
                new ParameterPGsql("p_fecha_solicitudtransparencia_hasta", NpgsqlDbType.Date, FechaSolicituTransparenciaHasta),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.GetReporteGeneral,
                     parameters!
             );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            List<ResponseReporteGeneral> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        //Datos Generales
                        administracion = item.IsNull(0) ? null! : item.Field<string>(0)!,
                        subadministracion = item.IsNull(1) ? null! : item.Field<string>(1)!,
                        no_asunto = item.IsNull(2) ? null! : item.Field<string>(2),
                        rfc = item.IsNull(3) ? null! : item.Field<string>(3)!,
                        promovente = item.IsNull(4) ? null! : item.Field<string>(4),
                        rfc_contribuyente = item.IsNull(5) ? null! : item.Field<string>(5)!,
                        contribuyente = item.IsNull(6) ? null! : item.Field<string>(6)!,
                        despacho_autorizado = item.IsNull(7) ? null! : item.Field<string>(7)!,
                        fecha_presentacion = item.IsNull(8) ? null! : item.Field<DateTime>(8).ToString("yyyy-MM-dd")!,
                        tipoAsunto = item.IsNull(9) ? null! : item.Field<string>(9)!,
                        tema = item.IsNull(10) ? null! : item.Field<string>(10)!,
                        no_asunto_cumplimentar = item.IsNull(11) ? null! : item.Field<string>(11)!,
                        monto = item.IsNull(12) ? 0 : item.Field<decimal>(12)!,
                        fecha_recepcion = item.IsNull(13) ? null! : item.Field<DateTime>(13).ToString("yyyy-MM-dd")!,
                        fecha_vencimiento = item.IsNull(14) ? null! : item.Field<DateTime>(14).ToString("yyyy-MM-dd")!,
                        estadoProcesal = item.IsNull(15) ? null! : item.Field<string>(15)!,
                        //Asignación       
                        abogado = item.IsNull(16) ? null! : item.Field<string>(16)!,
                        //Remisión
                        administracion_remite = item.IsNull(17) ? null! : item.Field<string>(17)!,
                        no_oficio_remision = item.IsNull(18) ? null! : item.Field<string>(18)!,
                        fecha_oficio_remision = item.IsNull(19) ? null! : item.Field<DateTime>(19).ToString("yyyy-MM-dd")!,
                        //Requerimiento
                        no_oficio_Requerimiento = item.IsNull(20) ? null! : item.Field<string>(20)!,
                        fecha_oficio_requerimiento = item.IsNull(21) ? null! : item.Field<DateTime>(21).ToString("yyyy-MM-dd")!,
                        fecha_notificacion_requerimiento = item.IsNull(22) ? null! : item.Field<DateTime>(22).ToString("yyyy-MM-dd")!,
                        fecha_vencimiento_Requerimiento = item.IsNull(23) ? null! : item.Field<DateTime>(23).ToString("yyyy-MM-dd")!,
                        atendio_requerimiento = item.IsNull(24) ? false : item.Field<bool>(24)!,
                        fecha_atencion_requerimiento = item.IsNull(25) ? null! : item.Field<DateTime>(25).ToString("yyyy-MM-dd")!,
                        //Solicitud de opinion de la información
                        unidadAdministrativaSolicitudInformacion = item.IsNull(26) ? null! : item.Field<string>(26)!,
                        noOficioSolicitudInformacion = item.IsNull(27) ? null! : item.Field<string>(27)!,
                        fechaOficioSolicitudInformacion = item.IsNull(28) ? null! : item.Field<DateTime>(28).ToString("yyyy-MM-dd")!,
                        atendioSolicitudInformacion = item.IsNull(29) ? false : item.Field<bool>(29)!,
                        noOficioRespuestaSolicitudInformacion = item.IsNull(30) ? null! : item.Field<string>(30)!,
                        fechaOficioRespuestaSolicitudInformacion = item.IsNull(31) ? null! : item.Field<DateTime>(31).ToString("yyyy-MM-dd")!,
                        fechaRecepcionSolicitudInformacion = item.IsNull(32) ? null! : item.Field<DateTime>(32).ToString("yyyy-MM-dd")!,
                        //Emisión de la resolución
                        noOficioResolucion = item.IsNull(33) ? null! : item.Field<string>(33)!,
                        fechaOficionResolucion = item.IsNull(34) ? null! : item.Field<DateTime>(34).ToString("yyyy-MM-dd")!,
                        sentido = item.IsNull(35) ? null! : item.Field<string>(35)!,
                        fechaOficioNotificacion = item.IsNull(36) ? null! : item.Field<DateTime>(36).ToString("yyyy-MM-dd")!,
                        //Requerimientos prodecon
                        noOficioProdecon = item.IsNull(37) ? null! : item.Field<string>(37)!,
                        noExpedienteProdecon = item.IsNull(38) ? null! : item.Field<string>(38)!,
                        fechaOficioProdecon = item.IsNull(39) ? null! : item.Field<DateTime>(39).ToString("yyyy-MM-dd")!,
                        fechaIngresoProdecon = item.IsNull(40) ? null! : item.Field<DateTime>(40).ToString("yyyy-MM-dd")!,
                        atencionProdecon = item.IsNull(41) ? null! : item.Field<string>(41)!,
                        //Solicitud Transparencia
                        noSolicitudTrasparencia = item.IsNull(42) ? null! : item.Field<string>(42)!,
                        fechaSolicitudTransparencia = item.IsNull(43) ? null! : item.Field<DateTime>(43).ToString("yyyy-MM-dd")!,

                        //Informacion de la cumplimentacion
                        numeroJuicioCumplimentacion = item.IsNull(44) ? null! : item.Field<string>(44)!,
                        fecha_firmeza_cumplimnetacion = item.IsNull(45) ? null! : item.Field<DateTime>(45).ToString("yyyy-MM-dd")!,
                        organoJurisdiccionalCumplimnetacion = item.IsNull(46) ? null! : item.Field<int>(46).ToString()!,
                        UnidadAdministrativaSolicitaCump = item.IsNull(47) ? null! : item.Field<string>(47)!,
                        oficioResolucionCumplimnetacion = item.IsNull(48) ? null! : item.Field<string>(48)!,
                        fechaOficioResolucionCumplimentacion = item.IsNull(49) ? null! : item.Field<DateTime>(49).ToString("yyyy-MM-dd")!,
                    }
                );
            }

            return resultList;

        }
        #endregion


    }
}