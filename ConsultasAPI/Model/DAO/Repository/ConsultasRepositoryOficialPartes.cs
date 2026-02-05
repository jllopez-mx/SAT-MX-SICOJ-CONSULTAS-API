using System.Data;
using ConsultasAPI.Model.ViewModels.Enums;
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.IDAO;
using ConsultasAPI.Model.Entities;
using NpgsqlTypes;
using Npgsql;
using System;
using System.Numerics;
using Sicoj.Utils.Models;
using Sicoj.Utils.Postgres;
using ConsultasAPI.Model.ViewModels;
using FluentValidation.Validators;
using System.Reflection.Metadata;
using ConsultasAPI.Model.Entities.Events.OficialPartes;
using System.Diagnostics.CodeAnalysis;

namespace ConsultasAPI.Model.DAO.ServicesDAO
{
    public class ConsultasRepositoryOficialPartes : IConsultaRepositoryOficialPartes
    {
        #region Variables

        private readonly ISqlTools _database;
        #endregion

        #region Constructor
        public ConsultasRepositoryOficialPartes(ISqlTools database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }
        #endregion

        #region Bandejas

        //Bandeja de pendientes Cumplimentación

        public async Task<int?> GetBandejaCumplimentacionCountAsyncRepository(List<int>? TipoAsunto, int? idUnidadAdmistrativaCentral = null!)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoAsunto is null || !TipoAsunto.Any()) ? DBNull.Value :  TipoAsunto.ToArray()),
                new ParameterPGsql("p_activo", NpgsqlDbType.Boolean, true),
                new ParameterPGsql("p_id_unidad_administrativa_central", NpgsqlDbType.Integer, idUnidadAdmistrativaCentral),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTAB_BANDEJA_PENDIENTES_CUMPLIMENTACION_COUNT,
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

            return response.Data.Tables[0].Rows[0].IsNull(0)
                ? null!
                : response.Data.Tables[0].Rows[0].Field<int>(0);
        }

         public async Task<List<ResponseConsultaByFilters>> GetBandejaPendientesCumplimentacionAsyncRepository(
           int pageSize, int page, string? orderByColumn, bool orderDesc,
           List<int>? TipoAsunto, int? idUnidadAdmistrativaCentral = null!
       )
        {


            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_page_size", NpgsqlDbType.Integer, pageSize),
                new ParameterPGsql("p_page", NpgsqlDbType.Integer, page),
                new ParameterPGsql("p_order_column", NpgsqlDbType.Varchar, orderByColumn),
                new ParameterPGsql("p_order_desc", NpgsqlDbType.Boolean, orderDesc),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoAsunto is null || !TipoAsunto.Any()) ? DBNull.Value :  TipoAsunto.ToArray()),
                new ParameterPGsql("p_activo", NpgsqlDbType.Boolean, true),
                new ParameterPGsql("p_id_unidad_administrativa_central", NpgsqlDbType.Integer, idUnidadAdmistrativaCentral),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_LISTADO_CONSULTAS_BANDEJA_PENDIENTES_CUMPLIMENTACION,
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

            List<ResponseConsultaByFilters> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        noAsunto = item.IsNull(1) ? null! : item.Field<string>(1),
                        rfc = item.IsNull(2) ? null! : item.Field<string>(2),
                        promovente = item.IsNull(3) ? null! : item.Field<string>(3),
                        rfcContribuyente = item.IsNull(4) ? null! : item.Field<string>(4),
                        promoventeEsContribuyente = item.IsNull(5) ? false : item.Field<bool>(5),
                        contribuyente = item.IsNull(6) ? null! : item.Field<string>(6),
                        idTipoAsunto = item.IsNull(7) ? 0 : item.Field<int>(7),
                        tipoAsunto = item.IsNull(8) ? null! : item.Field<string>(8)!,
                        idTipoModalidad = item.IsNull(9) ? 0 : item.Field<int>(9),
                        tipoModalidad = item.IsNull(10) ? null! : item.Field<string>(10)!,
                        despachoAutorizado = item.IsNull(11) ? null! : item.Field<string>(11),
                        fechaPresentacion = item.IsNull(12) ? null! : item.Field<DateTime>(12).ToString("yyyy-MM-dd"),
                        fechaRecepcion = item.IsNull(13) ? null! : item.Field<DateTime>(13).ToString("yyyy-MM-dd"),
                        fechaVencimiento = item.IsNull(15) ? null! : item.Field<DateTime>(15).ToString("yyyy-MM-dd"),
                        idAdministracionCentral = item.IsNull(17) ? 0 : item.Field<int>(17),
                        administracionCentral = item.IsNull(18) ? null! : item.Field<string>(18)!,
                        idAdministracion = item.IsNull(19) ? 0 : item.Field<int>(19),
                        administracion = item.IsNull(20) ? null! : item.Field<string>(20)!,
                        idsubadministracion = item.IsNull(21) ? 0 : item.Field<int>(21),
                        subadministracion = item.IsNull(22) ? null! : item.Field<string>(22)!,
                        idEstadoTarea = item.IsNull(23) ? 0 : item.Field<int>(23),
                        estadoTarea = item.IsNull(24) ? null! : item.Field<string>(24)!,
                        idEstadoProcesal = item.IsNull(25) ? 0 : item.Field<int>(25),
                        estadoProcesal = item.IsNull(26) ? null! : item.Field<string>(26)!,
                        idEmpleado = item.IsNull(27) ? null! : item.Field<string>(27),
                        fechaTurnado = item.IsNull(28) ? null! : item.Field<DateTime>(28).ToString("yyyy-MM-dd"),
                        idColorFechacssj = item.IsNull(30) ? 0 : item.Field<int>(30),
                        colorFechacssj = item.IsNull(31) ? null! : item.Field<string>(31)!,
                        idAlertaDG = item.IsNull(32) ? 0 : item.Field<int>(32),
                        alertaDG = item.IsNull(33) ? null! : item.Field<string>(33)!,
                        registroVence = item.IsNull(34) ? 0 : item.Field<int>(34)
                    }
                );
            }

            return resultList;

        }

        //Contador bandeja de Pendientes
        public async Task<int?> GetAllByFiltersCountAsyncRepository(List<int>? TipoAsunto, int? idUnidadAdmistrativaCentral = null!)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoAsunto is null || !TipoAsunto.Any()) ? DBNull.Value :  TipoAsunto.ToArray()),
                new ParameterPGsql("p_activo", NpgsqlDbType.Boolean, true),
                new ParameterPGsql("p_id_unidad_administrativa_central", NpgsqlDbType.Integer, idUnidadAdmistrativaCentral),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.ConsultasByFiltersCount,
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

            return response.Data.Tables[0].Rows[0].IsNull(0)
                ? null!
                : response.Data.Tables[0].Rows[0].Field<int>(0);
        }

        //Bandeja de pendientes
        public async Task<List<ResponseConsultaByFilters>> GetBandejaPendientesAsyncRepository(
           int pageSize, int page, string? orderByColumn, bool orderDesc,
           List<int>? TipoAsunto, int? idUnidadAdmistrativaCentral = null!
       )
        {


            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_page_size", NpgsqlDbType.Integer, pageSize),
                new ParameterPGsql("p_page", NpgsqlDbType.Integer, page),
                new ParameterPGsql("p_order_column", NpgsqlDbType.Varchar, orderByColumn),
                new ParameterPGsql("p_order_desc", NpgsqlDbType.Boolean, orderDesc),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoAsunto is null || !TipoAsunto.Any()) ? DBNull.Value :  TipoAsunto.ToArray()),
                new ParameterPGsql("p_activo", NpgsqlDbType.Boolean, true),
                new ParameterPGsql("p_id_unidad_administrativa_central", NpgsqlDbType.Integer, idUnidadAdmistrativaCentral),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_LISTADO_CONSULTAS_BANDEJA_PENDIENTES,
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

            List<ResponseConsultaByFilters> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        noAsunto = item.IsNull(1) ? null! : item.Field<string>(1),
                        rfc = item.IsNull(2) ? null! : item.Field<string>(2),
                        promovente = item.IsNull(3) ? null! : item.Field<string>(3),
                        rfcContribuyente = item.IsNull(4) ? null! : item.Field<string>(4),
                        promoventeEsContribuyente = item.IsNull(5) ? false : item.Field<bool>(5),
                        contribuyente = item.IsNull(6) ? null! : item.Field<string>(6),
                        idTipoAsunto = item.IsNull(7) ? 0 : item.Field<int>(7),
                        tipoAsunto = item.IsNull(8) ? null! : item.Field<string>(8)!,
                        idTipoModalidad = item.IsNull(9) ? 0 : item.Field<int>(9),
                        tipoModalidad = item.IsNull(10) ? null! : item.Field<string>(10)!,
                        despachoAutorizado = item.IsNull(11) ? null! : item.Field<string>(11),
                        fechaPresentacion = item.IsNull(12) ? null! : item.Field<DateTime>(12).ToString("yyyy-MM-dd"),
                        fechaRecepcion = item.IsNull(13) ? null! : item.Field<DateTime>(13).ToString("yyyy-MM-dd"),
                        fechaVencimiento = item.IsNull(15) ? null! : item.Field<DateTime>(15).ToString("yyyy-MM-dd"),
                        idAdministracionCentral = item.IsNull(17) ? 0 : item.Field<int>(17),
                        administracionCentral = item.IsNull(18) ? null! : item.Field<string>(18)!,
                        idAdministracion = item.IsNull(19) ? 0 : item.Field<int>(19),
                        administracion = item.IsNull(20) ? null! : item.Field<string>(20)!,
                        idsubadministracion = item.IsNull(21) ? 0 : item.Field<int>(21),
                        subadministracion = item.IsNull(22) ? null! : item.Field<string>(22)!,
                        idEstadoTarea = item.IsNull(23) ? 0 : item.Field<int>(23),
                        estadoTarea = item.IsNull(24) ? null! : item.Field<string>(24)!,
                        idEstadoProcesal = item.IsNull(25) ? 0 : item.Field<int>(25),
                        estadoProcesal = item.IsNull(26) ? null! : item.Field<string>(26)!,
                        idEmpleado = item.IsNull(27) ? null! : item.Field<string>(27),
                        fechaTurnado = item.IsNull(28) ? null! : item.Field<DateTime>(28).ToString("yyyy-MM-dd"),
                        idColorFechacssj = item.IsNull(30) ? 0 : item.Field<int>(30),
                        colorFechacssj = item.IsNull(31) ? null! : item.Field<string>(31)!,
                        idAlertaDG = item.IsNull(32) ? 0 : item.Field<int>(32),
                        alertaDG = item.IsNull(33) ? null! : item.Field<string>(33)!,
                        registroVence = item.IsNull(34) ? 0 : item.Field<int>(34),
                        idSemaforo = item.IsNull(35) ? 0 : item.Field<int>(35),
                        semaforo = item.IsNull(36) ? null! : item.Field<string>(36)!,

                        fechaFirmeza = item.IsNull(37) ? null! : item.Field<DateTime>(37).ToString("yyyy-MM-dd"),
                        idOrganoJurisdiccional = item.IsNull(38) ? 0 : item.Field<int>(38),
                        organoJurisdiccional = item.IsNull(39) ? null! : item.Field<string>(39)!,
                        fechaAsignacion = item.IsNull(40) ? null! : item.Field<DateTime>(40).ToString("yyyy-MM-dd"),
                        numeroJuicio = item.IsNull(41) ? null! : item.Field<string>(41)!,
                        noRecurso = item.IsNull(42) ? null! : item.Field<string>(42)!,
                        tiporecurso = item.IsNull(43) ? 0 : item.Field<int>(43),
                        recurso = item.IsNull(44) ? null! : item.Field<string>(44)!,
                        idPlazoCumplimentar = item.IsNull(45) ? 0 : item.Field<int>(45),
                        plazoCumplimentar = item.IsNull(46) ? null! : item.Field<string>(46)!,
                        oficioResolucion = item.IsNull(47) ? null! : item.Field<string>(47)!,
                        fechaOficioResolucion = item.IsNull(48) ? null! : item.Field<DateTime>(48).ToString("yyyy-MM-dd"),
                        horas = item.IsNull(49) ? 0 : item.Field<int>(49),
                        idUnidadAdministrativaSolicitaCump = item.IsNull(50) ? 0 : item.Field<int>(50),
                        UnidadAdministrativaSolicitaCump = item.IsNull(51) ? null! : item.Field<string>(51)!
                    }
                );
            }

            return resultList;

        }
        //Contador Historico
        public async Task<int?> OPHistoricoCountAsyncRepository(
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
        )
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_no_asunto", NpgsqlDbType.Text, noAsunto),
                new ParameterPGsql("p_fecha_recepcion_desde", NpgsqlDbType.Date, desdeRecepcion),
                new ParameterPGsql("p_fecha_recepcion_hasta", NpgsqlDbType.Date, hastaRecepcion),
                new ParameterPGsql("p_rfc", NpgsqlDbType.Varchar, rfc),
                new ParameterPGsql("p_promovente", NpgsqlDbType.Text, promovente),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoAsunto is null || !TipoAsunto.Any()) ? DBNull.Value :  TipoAsunto.ToArray()),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Array | NpgsqlDbType.Integer, (EstadoTarea is null || !EstadoTarea.Any()) ? DBNull.Value :  EstadoTarea.ToArray()),
                new ParameterPGsql("p_id_tipo_modalidad", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoModalidad is null || !TipoModalidad.Any()) ? DBNull.Value :  TipoModalidad.ToArray()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Array | NpgsqlDbType.Integer, (EstadoProcesal is null || !EstadoProcesal.Any()) ? DBNull.Value :  EstadoProcesal.ToArray()),
                new ParameterPGsql("p_activo", NpgsqlDbType.Boolean, true),
                new ParameterPGsql("p_id_unidad_administrativa_central", NpgsqlDbType.Integer, idUnidadAdmistrativaCentral),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.OP_HISTORICO_COUNT,
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

            return response.Data.Tables[0].Rows[0].IsNull(0)
                ? null!
                : response.Data.Tables[0].Rows[0].Field<int>(0);
        }

        //Bandeja de Historico
        public async Task<List<ResponseConsultaByFilters>> GetAllByFiltersAsyncRepository(
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
      )
        {

            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_page_size", NpgsqlDbType.Integer, pageSize),
                new ParameterPGsql("p_page", NpgsqlDbType.Integer, page),
                new ParameterPGsql("p_no_asunto", NpgsqlDbType.Text, noAsunto),
                new ParameterPGsql("p_fecha_recepcion_desde", NpgsqlDbType.Date, desdeRecepcion),
                new ParameterPGsql("p_fecha_recepcion_hasta", NpgsqlDbType.Date, hastaRecepcion),
                new ParameterPGsql("p_rfc", NpgsqlDbType.Varchar, rfc),
                new ParameterPGsql("p_promovente", NpgsqlDbType.Text, promovente),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoAsunto is null || !TipoAsunto.Any()) ? DBNull.Value :  TipoAsunto.ToArray()),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Array | NpgsqlDbType.Integer, (EstadoTarea is null || !EstadoTarea.Any()) ? DBNull.Value :  EstadoTarea.ToArray()),
                new ParameterPGsql("p_id_tipo_modalidad", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoModalidad is null || !TipoModalidad.Any()) ? DBNull.Value :  TipoModalidad.ToArray()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Array | NpgsqlDbType.Integer, (EstadoProcesal is null || !EstadoProcesal.Any()) ? DBNull.Value :  EstadoProcesal.ToArray()),
                new ParameterPGsql("p_order_column", NpgsqlDbType.Varchar, orderByColumn),
                new ParameterPGsql("p_order_desc", NpgsqlDbType.Boolean, orderDesc),
                new ParameterPGsql("p_activo", NpgsqlDbType.Boolean, true),
                new ParameterPGsql("p_id_unidad_administrativa_central", NpgsqlDbType.Integer, idUnidadAdmistrativaCentral),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_FILTERS,
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

            List<ResponseConsultaByFilters> resultList = new();
            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        noAsunto = item.IsNull(1) ? null! : item.Field<string>(1),
                        rfc = item.IsNull(2) ? null! : item.Field<string>(2),
                        promovente = item.IsNull(3) ? null! : item.Field<string>(3),
                        rfcContribuyente = item.IsNull(4) ? null! : item.Field<string>(4),
                        promoventeEsContribuyente = item.IsNull(5) ? false : item.Field<bool>(5),
                        contribuyente = item.IsNull(6) ? null! : item.Field<string>(6),
                        idTipoAsunto = item.IsNull(7) ? 0 : item.Field<int>(7),
                        tipoAsunto = item.IsNull(8) ? null! : item.Field<string>(8)!,
                        idTipoModalidad = item.IsNull(9) ? 0 : item.Field<int>(9),
                        tipoModalidad = item.IsNull(10) ? null! : item.Field<string>(10)!,
                        despachoAutorizado = item.IsNull(11) ? null! : item.Field<string>(11),
                        fechaPresentacion = item.IsNull(12) ? null! : item.Field<DateTime>(12).ToString("yyyy-MM-dd"),
                        fechaRecepcion = item.IsNull(13) ? null! : item.Field<DateTime>(13).ToString("yyyy-MM-dd"),
                        fechaVencimiento = item.IsNull(15) ? null! : item.Field<DateTime>(15).ToString("yyyy-MM-dd"),
                        idAdministracionCentral = item.IsNull(17) ? 0 : item.Field<int>(17),
                        administracionCentral = item.IsNull(18) ? null! : item.Field<string>(18)!,
                        idAdministracion = item.IsNull(19) ? 0 : item.Field<int>(19),
                        administracion = item.IsNull(20) ? null! : item.Field<string>(20)!,
                        idsubadministracion = item.IsNull(21) ? 0 : item.Field<int>(21),
                        subadministracion = item.IsNull(22) ? null! : item.Field<string>(22)!,
                        idEstadoTarea = item.IsNull(23) ? 0 : item.Field<int>(23),
                        estadoTarea = item.IsNull(24) ? null! : item.Field<string>(24)!,
                        idEstadoProcesal = item.IsNull(25) ? 0 : item.Field<int>(25),
                        estadoProcesal = item.IsNull(26) ? null! : item.Field<string>(26)!,
                        idEmpleado = item.IsNull(27) ? null! : item.Field<string>(27),
                        fechaTurnado = item.IsNull(28) ? null! : item.Field<DateTime>(28).ToString("yyyy-MM-dd"),
                        idColorFechacssj = item.IsNull(31) ? 0 : item.Field<int>(31),
                        colorFechacssj = item.IsNull(32) ? null! : item.Field<string>(32)!,
                        idAlertaDG = item.IsNull(33) ? 0 : item.Field<int>(33),
                        alertaDG = item.IsNull(34) ? null! : item.Field<string>(34)!,
                        registroVence = item.IsNull(35) ? 0 : item.Field<int>(35),
                        idSemaforo = item.IsNull(36) ? 0 : item.Field<int>(36),
                        semaforo = item.IsNull(37) ? null! : item.Field<string>(37)!,

                        fechaFirmeza = item.IsNull(38) ? null! : item.Field<DateTime>(38).ToString("yyyy-MM-dd"),
                        idOrganoJurisdiccional = item.IsNull(39) ? 0 : item.Field<int>(39),
                        organoJurisdiccional = item.IsNull(40) ? null! : item.Field<string>(40)!,
                        fechaAsignacion = item.IsNull(41) ? null! : item.Field<DateTime>(41).ToString("yyyy-MM-dd"),
                        numeroJuicio = item.IsNull(42) ? null! : item.Field<string>(42)!,
                        noRecurso = item.IsNull(43) ? null! : item.Field<string>(43)!,
                        tiporecurso = item.IsNull(44) ? 0 : item.Field<int>(44),
                        recurso = item.IsNull(45) ? null! : item.Field<string>(45)!,
                        idPlazoCumplimentar = item.IsNull(46) ? 0 : item.Field<int>(46),
                        plazoCumplimentar = item.IsNull(47) ? null! : item.Field<string>(47)!,
                        oficioResolucion = item.IsNull(48) ? null! : item.Field<string>(48)!,
                        fechaOficioResolucion = item.IsNull(49) ? null! : item.Field<DateTime>(49).ToString("yyyy-MM-dd"),
                        horas = item.IsNull(50) ? 0 : item.Field<int>(50),
                        idUnidadAdministrativaSolicitaCump = item.IsNull(51) ? 0 : item.Field<int>(51),
                        UnidadAdministrativaSolicitaCump = item.IsNull(52) ? null! : item.Field<string>(52)!
                    }
                );
            }

            return resultList;
        }

        #endregion

        public async Task<ResultTransaction> AddAsyncRepository(Consulta entity)
        {


            ParameterPGsql[] parameters =
            {

                new ParameterPGsql("p_rfc", NpgsqlDbType.Varchar, entity.rfc!),
                new ParameterPGsql("p_promovente", NpgsqlDbType.Varchar, entity.promovente!),
                new ParameterPGsql("p_promovente_es_contribuyente",NpgsqlDbType.Boolean,entity.promovente_es_contribuyente!),
                new ParameterPGsql("p_rfc_contribuyente",NpgsqlDbType.Varchar,entity.rfc_contribuyente!),
                new ParameterPGsql("p_contribuyente", NpgsqlDbType.Varchar, entity.contribuyente!),
                new ParameterPGsql("p_despacho_autorizado", NpgsqlDbType.Varchar, entity.despacho_autorizado!),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Integer, entity.id_tipo_asunto),
                new ParameterPGsql("p_id_tipo_modalidad", NpgsqlDbType.Integer, entity.id_tipo_modalidad),
                new ParameterPGsql("p_fecha_presentacion",NpgsqlDbType.Date,entity.fecha_presentacion!),
                new ParameterPGsql("p_fecha_recepcion", NpgsqlDbType.Date, entity.fecha_recepcion!),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Integer,EnumEstadoTarea.PENDIENTE_DE_TURNAR.GetHashCode()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.ACTIVO.GetHashCode()),
                new ParameterPGsql("p_id_administracion_central", NpgsqlDbType.Integer, entity.id_administracion_central),
                new ParameterPGsql("p_id_administracion", NpgsqlDbType.Integer, entity.id_administracion),
                new ParameterPGsql("p_usuario_creacion", NpgsqlDbType.Varchar, entity.usuario_creacion!),
                new ParameterPGsql("p_id_subadministracion", NpgsqlDbType.Integer, entity.id_Subadministracion),
                new ParameterPGsql("p_fecha_vencimiento", NpgsqlDbType.Date, entity.fecha_vencimiento)
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_INSERT,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }
        public async Task<ResultTransaction> UpdateAsyncRepository(Consulta entity)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_registro", NpgsqlDbType.Integer, entity.id),
                new ParameterPGsql("p_rfc", NpgsqlDbType.Varchar, entity.rfc!),
                new ParameterPGsql("p_promovente", NpgsqlDbType.Varchar, entity.promovente!),
                new ParameterPGsql("p_promovente_es_contribuyente",NpgsqlDbType.Boolean,entity.promovente_es_contribuyente!),
                new ParameterPGsql("p_rfc_contribuyente",NpgsqlDbType.Varchar,entity.rfc_contribuyente!),
                new ParameterPGsql("p_contribuyente", NpgsqlDbType.Varchar, entity.contribuyente!),
                new ParameterPGsql("p_despacho_autorizado", NpgsqlDbType.Varchar, entity.despacho_autorizado!),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Integer, entity.id_tipo_asunto),
                new ParameterPGsql("p_id_tipo_modalidad", NpgsqlDbType.Integer, entity.id_tipo_modalidad),
                new ParameterPGsql("p_fecha_presentacion",NpgsqlDbType.Date,Convert.ToDateTime(entity.fecha_presentacion)!),
                new ParameterPGsql("p_fecha_recepcion", NpgsqlDbType.Date, Convert.ToDateTime(entity.fecha_recepcion)!),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Integer,EnumEstadoTarea.PENDIENTE_DE_TURNAR.GetHashCode()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.ACTIVO.GetHashCode()),
                new ParameterPGsql("p_usuario_modificacion",NpgsqlDbType.Text,entity.usuario_modificacion),
                new ParameterPGsql("p_id_administracion",NpgsqlDbType.Integer,entity.id_administracion),
                new ParameterPGsql("p_id_subadministracion", NpgsqlDbType.Integer, entity.id_Subadministracion),
                new ParameterPGsql("p_fecha_vencimiento", NpgsqlDbType.Date, entity.fecha_vencimiento)

            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_UPDATE,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }

        public async Task<ResultTransaction> TurnarConsultaRepository(Consulta entity)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_registro", NpgsqlDbType.Integer, entity.id),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Integer,EnumEstadoTarea.PENDIENTE_DE_ASIGNAR.GetHashCode()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer,EnumEstadoProcesal.ACTIVO.GetHashCode()),
                new ParameterPGsql("p_numero_empleado", NpgsqlDbType.Varchar, entity.numero_empleado!)
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_TURNAR,
                parameters
            );

            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            var result = response.Data.Tables[0].Rows[0];

            return new()
            {
                Result = result.Field<int?>(0),
                Success = result.Field<bool>(1),
                MsgError = result.Field<string?>(2)!,
                DetailError = result.Field<string?>(3)!,
                NoError = result.Field<string?>(4)!
            };
        }
        public async Task<(string NumeroAsunto, string UnidadAdministrativa)> GetNumeroAsuntoYUnidadAdministrativaById(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID,
                parameters
            );

            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return (string.Empty, string.Empty);
            }

            var row = response.Data.Tables[0].Rows[0];
            var numeroAsunto = row.Field<string>("no_asunto") ?? string.Empty;
            var unidadAdministrativa = row.Field<string>("administracion") ?? string.Empty;

            return (numeroAsunto, unidadAdministrativa);
        }

        public async Task<ResultTransaction> DeleteAsyncRepository(int id)
        {
            ParameterPGsql[] parameters = { new ParameterPGsql("p_id", NpgsqlDbType.Integer, id), };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_DELETE,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }

        public async Task<RequestUpdateConsulta> GetByIdAsyncUpdateRepository(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                rfc = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2)!,
                promovente = response.Data.Tables[0].Rows[0].IsNull(3) ? null! : response.Data.Tables[0].Rows[0].Field<string>(3)!,
                rfcContribuyente = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                promoventeEsContribuyente = response.Data.Tables[0].Rows[0].IsNull(5) ? false : response.Data.Tables[0].Rows[0].Field<bool>(5),
                contribuyente = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<string>(6),
                despachoAutorizado = response.Data.Tables[0].Rows[0].IsNull(11) ? null! : response.Data.Tables[0].Rows[0].Field<string>(11),


            };
        }
        public async Task<RequestTurnarConsulta> GetByIdConsultaTurnadoRepository(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {


                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),


            };
        }

        public async Task<ResponseConsultaList> GetbyDeleteRepository(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                no_asunto = response.Data.Tables[0].Rows[0].IsNull(1) ? null! : response.Data.Tables[0].Rows[0].Field<string>(1),
                rfc = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                promovente = response.Data.Tables[0].Rows[0].IsNull(3) ? null! : response.Data.Tables[0].Rows[0].Field<string>(3),
                rfc_contribuyente = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                promovente_es_contribuyente = response.Data.Tables[0].Rows[0].IsNull(5) ? false : response.Data.Tables[0].Rows[0].Field<bool>(5),
                contribuyente = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<string>(6),
                idTipoAsunto = response.Data.Tables[0].Rows[0].IsNull(7) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(7),
                tipoAsunto = response.Data.Tables[0].Rows[0].IsNull(8) ? null! : response.Data.Tables[0].Rows[0].Field<string>(8)!,
                idTipoModalidad = response.Data.Tables[0].Rows[0].IsNull(9) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(9),
                tipoModalidad = response.Data.Tables[0].Rows[0].IsNull(10) ? null! : response.Data.Tables[0].Rows[0].Field<string>(10)!,
                despacho_autorizado = response.Data.Tables[0].Rows[0].IsNull(11) ? null! : response.Data.Tables[0].Rows[0].Field<string>(11),
                fecha_presentacion = response.Data.Tables[0].Rows[0].IsNull(12) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(12).ToString("yyyy-MM-dd"),
                fecha_recepcion = response.Data.Tables[0].Rows[0].IsNull(13) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(13).ToString("yyyy-MM-dd"),
                fecha_vencimiento = response.Data.Tables[0].Rows[0].IsNull(14) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(14).ToString("yyyy-MM-dd"),
                idAdministracionCentral = response.Data.Tables[0].Rows[0].IsNull(15) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(15),
                administracionCentral = response.Data.Tables[0].Rows[0].IsNull(16) ? null! : response.Data.Tables[0].Rows[0].Field<string>(16)!,
                idAdministracion = response.Data.Tables[0].Rows[0].IsNull(17) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(17),
                administracion = response.Data.Tables[0].Rows[0].IsNull(18) ? null! : response.Data.Tables[0].Rows[0].Field<string>(18)!,
                idSubadministracion = response.Data.Tables[0].Rows[0].IsNull(19) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                subadministracion = response.Data.Tables[0].Rows[0].IsNull(20) ? null! : response.Data.Tables[0].Rows[0].Field<string>(20)!,
                idEstadoTarea = response.Data.Tables[0].Rows[0].IsNull(21) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(21),
                estadoTarea = response.Data.Tables[0].Rows[0].IsNull(22) ? null! : response.Data.Tables[0].Rows[0].Field<string>(22)!,
                idEstadoProcesal = response.Data.Tables[0].Rows[0].IsNull(23) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(23),
                estadoProcesal = response.Data.Tables[0].Rows[0].IsNull(24) ? null! : response.Data.Tables[0].Rows[0].Field<string>(24)!,
                id_empleado = response.Data.Tables[0].Rows[0].IsNull(25) ? null! : response.Data.Tables[0].Rows[0].Field<string>(25),
                fecha_turnado = response.Data.Tables[0].Rows[0].IsNull(26) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(26).ToString("yyyy-MM-dd"),                
                remitido = response.Data.Tables[0].Rows[0].IsNull(27) ? false : response.Data.Tables[0].Rows[0].Field<bool>(27),
                activo = response.Data.Tables[0].Rows[0].IsNull(28) ? false : response.Data.Tables[0].Rows[0].Field<bool>(28),                                
                idColorFechacssj = response.Data.Tables[0].Rows[0].IsNull(29) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(29),
                colorFechacssj = response.Data.Tables[0].Rows[0].IsNull(30) ? null! : response.Data.Tables[0].Rows[0].Field<string>(30)!,
                idAlertaDG = response.Data.Tables[0].Rows[0].IsNull(31) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(31),
                alertaDG = response.Data.Tables[0].Rows[0].IsNull(32) ? null! : response.Data.Tables[0].Rows[0].Field<string>(32)!,                                
                registroVence = response.Data.Tables[0].Rows[0].IsNull(33) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(33),                                              
                domicilioPromovente = response.Data.Tables[0].Rows[0].IsNull(34) ? null! : response.Data.Tables[0].Rows[0].Field<string>(34)!,
                domicilioNotificaciones = response.Data.Tables[0].Rows[0].IsNull(35) ? null! : response.Data.Tables[0].Rows[0].Field<string>(35)!,
                idTema = response.Data.Tables[0].Rows[0].IsNull(36) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(36),
                tema = response.Data.Tables[0].Rows[0].IsNull(37) ? null! : response.Data.Tables[0].Rows[0].Field<string>(37)!,
                monto = response.Data.Tables[0].Rows[0].IsNull(38) ? 0 : response.Data.Tables[0].Rows[0].Field<decimal>(38),                
                idAbogado = response.Data.Tables[0].Rows[0].IsNull(39) ? null! : response.Data.Tables[0].Rows[0].Field<string>(39)!,
                abogado = response.Data.Tables[0].Rows[0].IsNull(40) ? null! : response.Data.Tables[0].Rows[0].Field<string>(40)!,
                montoDeterminado = response.Data.Tables[0].Rows[0].IsNull(41) ? false : response.Data.Tables[0].Rows[0].Field<bool>(41),
                solicita_requerimiento=response.Data.Tables[0].Rows[0].IsNull(42) ? null!  : response.Data.Tables[0].Rows[0].Field<bool>(42),
                concluido=response.Data.Tables[0].Rows[0].IsNull(43) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(43),  
                fecha_firmeza = response.Data.Tables[0].Rows[0].IsNull(44) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(44).ToString("yyyy-MM-dd"),
                idOrganoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(45) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(45)!,
                organoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(46) ? null! : response.Data.Tables[0].Rows[0].Field<string>(46)!,
                fecha_asignacion = response.Data.Tables[0].Rows[0].IsNull(47) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(47).ToString("yyyy-MM-dd"),   
                numeroJuicio = response.Data.Tables[0].Rows[0].IsNull(48) ? null! : response.Data.Tables[0].Rows[0].Field<string>(48)!, 
                noRecurso= response.Data.Tables[0].Rows[0].IsNull(49) ? null! : response.Data.Tables[0].Rows[0].Field<string>(49),
                tipo_recurso = response.Data.Tables[0].Rows[0].IsNull(50) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(50),     
                recurso = response.Data.Tables[0].Rows[0].IsNull(51) ? null! : response.Data.Tables[0].Rows[0].Field<string>(51)!,   
                id_plazo_cumplimentar = response.Data.Tables[0].Rows[0].IsNull(52) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(52),
                plazo_cumplimentar = response.Data.Tables[0].Rows[0].IsNull(53) ? null! : response.Data.Tables[0].Rows[0].Field<string>(53)!,
                oficioResolucion = response.Data.Tables[0].Rows[0].IsNull(54) ? null! : response.Data.Tables[0].Rows[0].Field<string>(54)!,
                fechaOficioResolucion = response.Data.Tables[0].Rows[0].IsNull(55) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(55).ToString("yyyy-MM-dd"),                 
                horas = response.Data.Tables[0].Rows[0].IsNull(56) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(56),
                idSeccionDescartar = response.Data.Tables[0].Rows[0].IsNull(57) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(57),
                idSeccionModificar = response.Data.Tables[0].Rows[0].IsNull(58) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(58),                
                idSemaforo = response.Data.Tables[0].Rows[0].IsNull(59) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(59),
                semaforo = response.Data.Tables[0].Rows[0].IsNull(60) ? null! : response.Data.Tables[0].Rows[0].Field<string>(60)!,               
                idUnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(61) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(61),
                UnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(62) ? null! : response.Data.Tables[0].Rows[0].Field<string>(62)!,
                asigno=response.Data.Tables[0].Rows[0].IsNull(63) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(63),  
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(64) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(64).ToString("yyyy-MM-dd"),
                turnado=response.Data.Tables[0].Rows[0].IsNull(65) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(65)

            };
        }

        public async Task<ResponseConsultaList> GetByIdAsyncRepository(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                 id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                no_asunto = response.Data.Tables[0].Rows[0].IsNull(1) ? null! : response.Data.Tables[0].Rows[0].Field<string>(1),
                rfc = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                promovente = response.Data.Tables[0].Rows[0].IsNull(3) ? null! : response.Data.Tables[0].Rows[0].Field<string>(3),
                rfc_contribuyente = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                promovente_es_contribuyente = response.Data.Tables[0].Rows[0].IsNull(5) ? false : response.Data.Tables[0].Rows[0].Field<bool>(5),
                contribuyente = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<string>(6),
                idTipoAsunto = response.Data.Tables[0].Rows[0].IsNull(7) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(7),
                tipoAsunto = response.Data.Tables[0].Rows[0].IsNull(8) ? null! : response.Data.Tables[0].Rows[0].Field<string>(8)!,
                idTipoModalidad = response.Data.Tables[0].Rows[0].IsNull(9) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(9),
                tipoModalidad = response.Data.Tables[0].Rows[0].IsNull(10) ? null! : response.Data.Tables[0].Rows[0].Field<string>(10)!,
                despacho_autorizado = response.Data.Tables[0].Rows[0].IsNull(11) ? null! : response.Data.Tables[0].Rows[0].Field<string>(11),
                fecha_presentacion = response.Data.Tables[0].Rows[0].IsNull(12) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(12).ToString("yyyy-MM-dd"),
                fecha_recepcion = response.Data.Tables[0].Rows[0].IsNull(13) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(13).ToString("yyyy-MM-dd"),
                fecha_vencimiento = response.Data.Tables[0].Rows[0].IsNull(14) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(14).ToString("yyyy-MM-dd"),
                idAdministracionCentral = response.Data.Tables[0].Rows[0].IsNull(15) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(15),
                administracionCentral = response.Data.Tables[0].Rows[0].IsNull(16) ? null! : response.Data.Tables[0].Rows[0].Field<string>(16)!,
                idAdministracion = response.Data.Tables[0].Rows[0].IsNull(17) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(17),
                administracion = response.Data.Tables[0].Rows[0].IsNull(18) ? null! : response.Data.Tables[0].Rows[0].Field<string>(18)!,
                idSubadministracion = response.Data.Tables[0].Rows[0].IsNull(19) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                subadministracion = response.Data.Tables[0].Rows[0].IsNull(20) ? null! : response.Data.Tables[0].Rows[0].Field<string>(20)!,
                idEstadoTarea = response.Data.Tables[0].Rows[0].IsNull(21) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(21),
                estadoTarea = response.Data.Tables[0].Rows[0].IsNull(22) ? null! : response.Data.Tables[0].Rows[0].Field<string>(22)!,
                idEstadoProcesal = response.Data.Tables[0].Rows[0].IsNull(23) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(23),
                estadoProcesal = response.Data.Tables[0].Rows[0].IsNull(24) ? null! : response.Data.Tables[0].Rows[0].Field<string>(24)!,
                id_empleado = response.Data.Tables[0].Rows[0].IsNull(25) ? null! : response.Data.Tables[0].Rows[0].Field<string>(25),
                fecha_turnado = response.Data.Tables[0].Rows[0].IsNull(26) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(26).ToString("yyyy-MM-dd"),                
                remitido = response.Data.Tables[0].Rows[0].IsNull(27) ? false : response.Data.Tables[0].Rows[0].Field<bool>(27),
                activo = response.Data.Tables[0].Rows[0].IsNull(28) ? false : response.Data.Tables[0].Rows[0].Field<bool>(28),                                
                idColorFechacssj = response.Data.Tables[0].Rows[0].IsNull(29) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(29),
                colorFechacssj = response.Data.Tables[0].Rows[0].IsNull(30) ? null! : response.Data.Tables[0].Rows[0].Field<string>(30)!,
                idAlertaDG = response.Data.Tables[0].Rows[0].IsNull(31) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(31),
                alertaDG = response.Data.Tables[0].Rows[0].IsNull(32) ? null! : response.Data.Tables[0].Rows[0].Field<string>(32)!,                                
                registroVence = response.Data.Tables[0].Rows[0].IsNull(33) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(33),                                              
                domicilioPromovente = response.Data.Tables[0].Rows[0].IsNull(34) ? null! : response.Data.Tables[0].Rows[0].Field<string>(34)!,
                domicilioNotificaciones = response.Data.Tables[0].Rows[0].IsNull(35) ? null! : response.Data.Tables[0].Rows[0].Field<string>(35)!,
                idTema = response.Data.Tables[0].Rows[0].IsNull(36) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(36),
                tema = response.Data.Tables[0].Rows[0].IsNull(37) ? null! : response.Data.Tables[0].Rows[0].Field<string>(37)!,
                monto = response.Data.Tables[0].Rows[0].IsNull(38) ? 0 : response.Data.Tables[0].Rows[0].Field<decimal>(38),                
                idAbogado = response.Data.Tables[0].Rows[0].IsNull(39) ? null! : response.Data.Tables[0].Rows[0].Field<string>(39)!,
                abogado = response.Data.Tables[0].Rows[0].IsNull(40) ? null! : response.Data.Tables[0].Rows[0].Field<string>(40)!,
                montoDeterminado = response.Data.Tables[0].Rows[0].IsNull(41) ? false : response.Data.Tables[0].Rows[0].Field<bool>(41),
                solicita_requerimiento=response.Data.Tables[0].Rows[0].IsNull(42) ? null!  : response.Data.Tables[0].Rows[0].Field<bool>(42),
                concluido=response.Data.Tables[0].Rows[0].IsNull(43) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(43),  
                fecha_firmeza = response.Data.Tables[0].Rows[0].IsNull(44) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(44).ToString("yyyy-MM-dd"),
                idOrganoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(45) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(45)!,
                organoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(46) ? null! : response.Data.Tables[0].Rows[0].Field<string>(46)!,
                fecha_asignacion = response.Data.Tables[0].Rows[0].IsNull(47) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(47).ToString("yyyy-MM-dd"),   
                numeroJuicio = response.Data.Tables[0].Rows[0].IsNull(48) ? null! : response.Data.Tables[0].Rows[0].Field<string>(48)!, 
                noRecurso= response.Data.Tables[0].Rows[0].IsNull(49) ? null! : response.Data.Tables[0].Rows[0].Field<string>(49),
                tipo_recurso = response.Data.Tables[0].Rows[0].IsNull(50) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(50),     
                recurso = response.Data.Tables[0].Rows[0].IsNull(51) ? null! : response.Data.Tables[0].Rows[0].Field<string>(51)!,   
                id_plazo_cumplimentar = response.Data.Tables[0].Rows[0].IsNull(52) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(52),
                plazo_cumplimentar = response.Data.Tables[0].Rows[0].IsNull(53) ? null! : response.Data.Tables[0].Rows[0].Field<string>(53)!,
                oficioResolucion = response.Data.Tables[0].Rows[0].IsNull(54) ? null! : response.Data.Tables[0].Rows[0].Field<string>(54)!,
                fechaOficioResolucion = response.Data.Tables[0].Rows[0].IsNull(55) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(55).ToString("yyyy-MM-dd"),                 
                horas = response.Data.Tables[0].Rows[0].IsNull(56) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(56),
                idSeccionDescartar = response.Data.Tables[0].Rows[0].IsNull(57) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(57),
                idSeccionModificar = response.Data.Tables[0].Rows[0].IsNull(58) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(58),                
                idSemaforo = response.Data.Tables[0].Rows[0].IsNull(59) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(59),
                semaforo = response.Data.Tables[0].Rows[0].IsNull(60) ? null! : response.Data.Tables[0].Rows[0].Field<string>(60)!,               
                idUnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(61) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(61),
                UnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(62) ? null! : response.Data.Tables[0].Rows[0].Field<string>(62)!,
                asigno=response.Data.Tables[0].Rows[0].IsNull(63) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(63),  
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(64) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(64).ToString("yyyy-MM-dd"),
                turnado=response.Data.Tables[0].Rows[0].IsNull(65) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(65)

            };
        }



        public async Task<Consulta> GetByIdAllAsyncRepository(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                no_asunto = response.Data.Tables[0].Rows[0].IsNull(1) ? null! : response.Data.Tables[0].Rows[0].Field<string>(1),
                rfc = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                promovente = response.Data.Tables[0].Rows[0].IsNull(3) ? null! : response.Data.Tables[0].Rows[0].Field<string>(3),
                rfc_contribuyente = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                promovente_es_contribuyente = response.Data.Tables[0].Rows[0].IsNull(5) ? false : response.Data.Tables[0].Rows[0].Field<bool>(5),
                contribuyente = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<string>(6),
                id_tipo_asunto = response.Data.Tables[0].Rows[0].IsNull(7) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(7),
                tipo_asunto = response.Data.Tables[0].Rows[0].IsNull(8) ? null! : response.Data.Tables[0].Rows[0].Field<string>(8)!,
                id_tipo_modalidad = response.Data.Tables[0].Rows[0].IsNull(9) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(9),
                tipo_modalidad = response.Data.Tables[0].Rows[0].IsNull(10) ? null! : response.Data.Tables[0].Rows[0].Field<string>(10)!,
                despacho_autorizado = response.Data.Tables[0].Rows[0].IsNull(11) ? null! : response.Data.Tables[0].Rows[0].Field<string>(11),
                fecha_presentacion = response.Data.Tables[0].Rows[0].IsNull(12) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(12),
                fecha_recepcion = response.Data.Tables[0].Rows[0].IsNull(13) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(13),                
                fecha_vencimiento = response.Data.Tables[0].Rows[0].IsNull(14) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(14),                
                id_administracion_central = response.Data.Tables[0].Rows[0].IsNull(15) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(15),
                administracion_central = response.Data.Tables[0].Rows[0].IsNull(16) ? null! : response.Data.Tables[0].Rows[0].Field<string>(16)!,
                id_administracion = response.Data.Tables[0].Rows[0].IsNull(17) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(17),
                Administracion = response.Data.Tables[0].Rows[0].IsNull(18) ? null! : response.Data.Tables[0].Rows[0].Field<string>(18)!,
                id_Subadministracion = response.Data.Tables[0].Rows[0].IsNull(19) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                Subadministracion = response.Data.Tables[0].Rows[0].IsNull(20) ? null! : response.Data.Tables[0].Rows[0].Field<string>(20)!,
                id_estado_tarea = response.Data.Tables[0].Rows[0].IsNull(21) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(21),
                estado_tarea = response.Data.Tables[0].Rows[0].IsNull(22) ? null! : response.Data.Tables[0].Rows[0].Field<string>(22)!,
                id_estado_procesal = response.Data.Tables[0].Rows[0].IsNull(23) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(23),
                estado_procesal = response.Data.Tables[0].Rows[0].IsNull(24) ? null! : response.Data.Tables[0].Rows[0].Field<string>(24)!,
                numero_empleado = response.Data.Tables[0].Rows[0].IsNull(25) ? null! : response.Data.Tables[0].Rows[0].Field<string>(25),
                fecha_turnado = response.Data.Tables[0].Rows[0].IsNull(26) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(26),
                remitido = response.Data.Tables[0].Rows[0].IsNull(27) ? false : response.Data.Tables[0].Rows[0].Field<bool>(27),
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(64) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(64),
                turnado = response.Data.Tables[0].Rows[0].IsNull(65) ? false : response.Data.Tables[0].Rows[0].Field<bool>(65)
            };
        }


        public async Task<List<ResponseConsultaRfc>> GetRfcampliadosyncRepository(string nombre)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_nombre", NpgsqlDbType.Varchar,nombre),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_RFC,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }


            List<ResponseConsultaRfc> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        rfc = item.IsNull(0) ? null! : item.Field<string>(0),
                        nombre = item.IsNull(1) ? null! : item.Field<string>(1),


                    }
                );
            }

            return resultList;
        }

        public async Task<ResponseConsultaList> GetByIdAsync(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                no_asunto = response.Data.Tables[0].Rows[0].IsNull(1) ? null! : response.Data.Tables[0].Rows[0].Field<string>(1),
                rfc = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                promovente = response.Data.Tables[0].Rows[0].IsNull(3) ? null! : response.Data.Tables[0].Rows[0].Field<string>(3),
                rfc_contribuyente = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                promovente_es_contribuyente = response.Data.Tables[0].Rows[0].IsNull(5) ? false : response.Data.Tables[0].Rows[0].Field<bool>(5),
                contribuyente = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<string>(6),
                idTipoAsunto = response.Data.Tables[0].Rows[0].IsNull(7) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(7),
                tipoAsunto = response.Data.Tables[0].Rows[0].IsNull(8) ? null! : response.Data.Tables[0].Rows[0].Field<string>(8)!,
                idTipoModalidad = response.Data.Tables[0].Rows[0].IsNull(9) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(9),
                tipoModalidad = response.Data.Tables[0].Rows[0].IsNull(10) ? null! : response.Data.Tables[0].Rows[0].Field<string>(10)!,
                despacho_autorizado = response.Data.Tables[0].Rows[0].IsNull(11) ? null! : response.Data.Tables[0].Rows[0].Field<string>(11),
                fecha_presentacion = response.Data.Tables[0].Rows[0].IsNull(12) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(12).ToString("yyyy-MM-dd"),
                fecha_recepcion = response.Data.Tables[0].Rows[0].IsNull(13) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(13).ToString("yyyy-MM-dd"),
                fecha_vencimiento = response.Data.Tables[0].Rows[0].IsNull(14) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(14).ToString("yyyy-MM-dd"),
                idAdministracionCentral = response.Data.Tables[0].Rows[0].IsNull(15) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(15),
                administracionCentral = response.Data.Tables[0].Rows[0].IsNull(16) ? null! : response.Data.Tables[0].Rows[0].Field<string>(16)!,
                idAdministracion = response.Data.Tables[0].Rows[0].IsNull(17) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(17),
                administracion = response.Data.Tables[0].Rows[0].IsNull(18) ? null! : response.Data.Tables[0].Rows[0].Field<string>(18)!,
                idSubadministracion = response.Data.Tables[0].Rows[0].IsNull(19) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                subadministracion = response.Data.Tables[0].Rows[0].IsNull(20) ? null! : response.Data.Tables[0].Rows[0].Field<string>(20)!,
                idEstadoTarea = response.Data.Tables[0].Rows[0].IsNull(21) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(21),
                estadoTarea = response.Data.Tables[0].Rows[0].IsNull(22) ? null! : response.Data.Tables[0].Rows[0].Field<string>(22)!,
                idEstadoProcesal = response.Data.Tables[0].Rows[0].IsNull(23) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(23),
                estadoProcesal = response.Data.Tables[0].Rows[0].IsNull(24) ? null! : response.Data.Tables[0].Rows[0].Field<string>(24)!,
                id_empleado = response.Data.Tables[0].Rows[0].IsNull(25) ? null! : response.Data.Tables[0].Rows[0].Field<string>(25),
                fecha_turnado = response.Data.Tables[0].Rows[0].IsNull(26) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(26).ToString("yyyy-MM-dd"),                
                remitido = response.Data.Tables[0].Rows[0].IsNull(27) ? false : response.Data.Tables[0].Rows[0].Field<bool>(27),
                activo = response.Data.Tables[0].Rows[0].IsNull(28) ? false : response.Data.Tables[0].Rows[0].Field<bool>(28),                                
                idColorFechacssj = response.Data.Tables[0].Rows[0].IsNull(29) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(29),
                colorFechacssj = response.Data.Tables[0].Rows[0].IsNull(30) ? null! : response.Data.Tables[0].Rows[0].Field<string>(30)!,
                idAlertaDG = response.Data.Tables[0].Rows[0].IsNull(31) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(31),
                alertaDG = response.Data.Tables[0].Rows[0].IsNull(32) ? null! : response.Data.Tables[0].Rows[0].Field<string>(32)!,                                
                registroVence = response.Data.Tables[0].Rows[0].IsNull(33) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(33),                                              
                domicilioPromovente = response.Data.Tables[0].Rows[0].IsNull(34) ? null! : response.Data.Tables[0].Rows[0].Field<string>(34)!,
                domicilioNotificaciones = response.Data.Tables[0].Rows[0].IsNull(35) ? null! : response.Data.Tables[0].Rows[0].Field<string>(35)!,
                idTema = response.Data.Tables[0].Rows[0].IsNull(36) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(36),
                tema = response.Data.Tables[0].Rows[0].IsNull(37) ? null! : response.Data.Tables[0].Rows[0].Field<string>(37)!,
                monto = response.Data.Tables[0].Rows[0].IsNull(38) ? 0 : response.Data.Tables[0].Rows[0].Field<decimal>(38),                
                idAbogado = response.Data.Tables[0].Rows[0].IsNull(39) ? null! : response.Data.Tables[0].Rows[0].Field<string>(39)!,
                abogado = response.Data.Tables[0].Rows[0].IsNull(40) ? null! : response.Data.Tables[0].Rows[0].Field<string>(40)!,
                montoDeterminado = response.Data.Tables[0].Rows[0].IsNull(41) ? false : response.Data.Tables[0].Rows[0].Field<bool>(41),
                solicita_requerimiento=response.Data.Tables[0].Rows[0].IsNull(42) ? null!  : response.Data.Tables[0].Rows[0].Field<bool>(42),
                concluido=response.Data.Tables[0].Rows[0].IsNull(43) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(43),  
                fecha_firmeza = response.Data.Tables[0].Rows[0].IsNull(44) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(44).ToString("yyyy-MM-dd"),
                idOrganoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(45) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(45)!,
                organoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(46) ? null! : response.Data.Tables[0].Rows[0].Field<string>(46)!,
                fecha_asignacion = response.Data.Tables[0].Rows[0].IsNull(47) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(47).ToString("yyyy-MM-dd"),   
                numeroJuicio = response.Data.Tables[0].Rows[0].IsNull(48) ? null! : response.Data.Tables[0].Rows[0].Field<string>(48)!, 
                noRecurso= response.Data.Tables[0].Rows[0].IsNull(49) ? null! : response.Data.Tables[0].Rows[0].Field<string>(49),
                tipo_recurso = response.Data.Tables[0].Rows[0].IsNull(50) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(50),     
                recurso = response.Data.Tables[0].Rows[0].IsNull(51) ? null! : response.Data.Tables[0].Rows[0].Field<string>(51)!,   
                id_plazo_cumplimentar = response.Data.Tables[0].Rows[0].IsNull(52) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(52),
                plazo_cumplimentar = response.Data.Tables[0].Rows[0].IsNull(53) ? null! : response.Data.Tables[0].Rows[0].Field<string>(53)!,
                oficioResolucion = response.Data.Tables[0].Rows[0].IsNull(54) ? null! : response.Data.Tables[0].Rows[0].Field<string>(54)!,
                fechaOficioResolucion = response.Data.Tables[0].Rows[0].IsNull(55) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(55).ToString("yyyy-MM-dd"),                 
                horas = response.Data.Tables[0].Rows[0].IsNull(56) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(56),
                idSeccionDescartar = response.Data.Tables[0].Rows[0].IsNull(57) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(57),
                idSeccionModificar = response.Data.Tables[0].Rows[0].IsNull(58) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(58),                
                idSemaforo = response.Data.Tables[0].Rows[0].IsNull(59) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(59),
                semaforo = response.Data.Tables[0].Rows[0].IsNull(60) ? null! : response.Data.Tables[0].Rows[0].Field<string>(60)!,               
                idUnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(61) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(61),
                UnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(62) ? null! : response.Data.Tables[0].Rows[0].Field<string>(62)!,
                asigno=response.Data.Tables[0].Rows[0].IsNull(63) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(63),  
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(64) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(64).ToString("yyyy-MM-dd"),
                turnado=response.Data.Tables[0].Rows[0].IsNull(65) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(65)

            };
        }

        public async Task<Consulta> GetByIdConsultaAsync(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                no_asunto = response.Data.Tables[0].Rows[0].IsNull(1) ? null! : response.Data.Tables[0].Rows[0].Field<string>(1),
                rfc = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                promovente = response.Data.Tables[0].Rows[0].IsNull(3) ? null! : response.Data.Tables[0].Rows[0].Field<string>(3),
                rfc_contribuyente = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                promovente_es_contribuyente = response.Data.Tables[0].Rows[0].IsNull(5) ? false : response.Data.Tables[0].Rows[0].Field<bool>(5),
                contribuyente = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<string>(6),
                id_tipo_asunto = response.Data.Tables[0].Rows[0].IsNull(7) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(7),
                id_tipo_modalidad = response.Data.Tables[0].Rows[0].IsNull(9) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(9),
                despacho_autorizado = response.Data.Tables[0].Rows[0].IsNull(11) ? null! : response.Data.Tables[0].Rows[0].Field<string>(11),
                fecha_presentacion = response.Data.Tables[0].Rows[0].IsNull(12) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(12),
                fecha_recepcion = response.Data.Tables[0].Rows[0].IsNull(13) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(13),               
                fecha_vencimiento = response.Data.Tables[0].Rows[0].IsNull(14) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(15),                
                id_administracion = response.Data.Tables[0].Rows[0].IsNull(17) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(17),
                id_estado_tarea = response.Data.Tables[0].Rows[0].IsNull(21) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                id_estado_procesal = response.Data.Tables[0].Rows[0].IsNull(23) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(21),
                numero_empleado = response.Data.Tables[0].Rows[0].IsNull(25) ? null! : response.Data.Tables[0].Rows[0].Field<string>(23),
                fecha_turnado = response.Data.Tables[0].Rows[0].IsNull(26) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(26),
                 remitido = response.Data.Tables[0].Rows[0].IsNull(27) ? false : response.Data.Tables[0].Rows[0].Field<bool>(27),
                 fecha_registro = response.Data.Tables[0].Rows[0].IsNull(64) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(64)
                
               
              

            };
        }
        public async Task<Consulta> GetByIdConsultaTurnadoAsync(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                numero_empleado = response.Data.Tables[0].Rows[0].IsNull(23) ? null! : response.Data.Tables[0].Rows[0].Field<string>(23)!

            };
        }
        public async Task<ResultTransaction> TurnarAsync(Consulta entity)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_registro", NpgsqlDbType.Integer, entity.id),
                new ParameterPGsql("p_id_administracion",NpgsqlDbType.Integer,entity.id_administracion!),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Integer,EnumEstadoTarea.PENDIENTE_DE_ASIGNAR.GetHashCode()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer,EnumEstadoProcesal.ACTIVO.GetHashCode()),
                new ParameterPGsql("p_numero_empleado", NpgsqlDbType.Varchar, entity.numero_empleado!)
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_TURNAR,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };

            }


            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }
        public async Task<ResultTransaction> TurnarConsultaAsync(int id, Consulta entity)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_registro", NpgsqlDbType.Integer, id),
                new ParameterPGsql("p_id_administracion",NpgsqlDbType.Integer,entity.id_administracion!),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Integer,EnumEstadoTarea.PENDIENTE_DE_ASIGNAR.GetHashCode()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer,EnumEstadoProcesal.ACTIVO.GetHashCode()),
                new ParameterPGsql("p_numero_empleado", NpgsqlDbType.Varchar, entity.numero_empleado!)
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_TURNAR,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };

            }


            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }
        public async Task<ResultTransaction> DeleteAsync(int id)
        {
            ParameterPGsql[] parameters = { new ParameterPGsql("p_id", NpgsqlDbType.Integer, id), };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_DELETE,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }



        public async Task<List<ResponseConsultaList>> GetAllAsync()
        {


            var response = await _database.ExecuteFunctionAsync(
              EnunFunctions.CONSULTA_LISTADO_CONSULTAS,
              null!
          );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            List<ResponseConsultaList> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        no_asunto = item.IsNull(1) ? null! : item.Field<string>(1),
                        rfc = item.IsNull(2) ? null! : item.Field<string>(2),
                        promovente = item.IsNull(3) ? null! : item.Field<string>(3),
                        rfc_contribuyente = item.IsNull(4) ? null! : item.Field<string>(4),
                        promovente_es_contribuyente = item.IsNull(5) ? false : item.Field<bool>(5),
                        contribuyente = item.IsNull(6) ? null! : item.Field<string>(6),
                        idTipoAsunto = item.IsNull(7) ? 0 : item.Field<int>(7),
                        tipoAsunto = item.IsNull(8) ? null! : item.Field<string>(8)!,
                        idTipoModalidad = item.IsNull(9) ? 0 : item.Field<int>(9),
                        tipoModalidad = item.IsNull(10) ? null! : item.Field<string>(10)!,
                        despacho_autorizado = item.IsNull(11) ? null! : item.Field<string>(11),
                        fecha_presentacion = item.IsNull(12) ? null! : item.Field<DateTime>(12).ToString("yyyy-MM-dd"),
                        fecha_recepcion = item.IsNull(13) ? null! : item.Field<DateTime>(13).ToString("yyyy-MM-dd"),
                        fecha_registro = item.IsNull(14) ? null! : item.Field<DateTime>(14).ToString("yyyy-MM-dd"),
                        fecha_vencimiento = item.IsNull(15) ? null! : item.Field<DateTime>(15).ToString("yyyy-MM-dd"),
                        turnado = item.IsNull(16) ? false : item.Field<bool>(16),
                        idAdministracionCentral = item.IsNull(17) ? 0 : item.Field<int>(17),
                        administracionCentral = item.IsNull(18) ? null! : item.Field<string>(18)!,
                        idAdministracion = item.IsNull(19) ? 0 : item.Field<int>(19),
                        administracion = item.IsNull(20) ? null! : item.Field<string>(20)!,
                        idSubadministracion = item.IsNull(21) ? 0 : item.Field<int>(21),
                        subadministracion = item.IsNull(22) ? null! : item.Field<string>(22)!,
                        idEstadoTarea = item.IsNull(23) ? 0 : item.Field<int>(23),
                        estadoTarea = item.IsNull(24) ? null! : item.Field<string>(24)!,
                        idEstadoProcesal = item.IsNull(25) ? 0 : item.Field<int>(25),
                        estadoProcesal = item.IsNull(26) ? null! : item.Field<string>(26)!,
                        id_empleado = item.IsNull(27) ? null! : item.Field<string>(27),
                        fecha_turnado = item.IsNull(28) ? null! : item.Field<DateTime>(28).ToString("yyyy-MM-dd"),
                        remitido = item.IsNull(29) ? false : item.Field<bool>(29)


                    }
                );
            }

            return resultList;
        }

        public async Task<List<ResponseConsultaList>> GetAllBandejaPendientesAsync()
        {


            var response = await _database.ExecuteFunctionAsync(
              EnunFunctions.CONSULTA_LISTADO_CONSULTAS_BANDEJA_PENDIENTES,
              null!
          );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            List<ResponseConsultaList> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        no_asunto = item.IsNull(1) ? null! : item.Field<string>(1),
                        rfc = item.IsNull(2) ? null! : item.Field<string>(2),
                        promovente = item.IsNull(3) ? null! : item.Field<string>(3),
                        rfc_contribuyente = item.IsNull(4) ? null! : item.Field<string>(4),
                        promovente_es_contribuyente = item.IsNull(5) ? false : item.Field<bool>(5),
                        contribuyente = item.IsNull(6) ? null! : item.Field<string>(6),
                        idTipoAsunto = item.IsNull(7) ? 0 : item.Field<int>(7),
                        tipoAsunto = item.IsNull(8) ? null! : item.Field<string>(8)!,
                        idTipoModalidad = item.IsNull(9) ? 0 : item.Field<int>(9),
                        tipoModalidad = item.IsNull(10) ? null! : item.Field<string>(10)!,
                        despacho_autorizado = item.IsNull(11) ? null! : item.Field<string>(11),
                        fecha_presentacion = item.IsNull(12) ? null : item.Field<DateTime>(12).ToString("yyyy-MM-dd"),
                        fecha_recepcion = item.IsNull(13) ? null : item.Field<DateTime>(13).ToString("yyyy-MM-dd"),
                        fecha_registro = item.IsNull(14) ? null : item.Field<DateTime>(14).ToString("yyyy-MM-dd"),
                        fecha_vencimiento = item.IsNull(15) ? null : item.Field<DateTime>(15).ToString("yyyy-MM-dd"),
                        turnado = item.IsNull(16) ? false : item.Field<bool>(16),
                        idAdministracionCentral = item.IsNull(17) ? 0 : item.Field<int>(17),
                        administracionCentral = item.IsNull(18) ? null! : item.Field<string>(18)!,
                        idAdministracion = item.IsNull(19) ? 0 : item.Field<int>(19),
                        administracion = item.IsNull(20) ? null! : item.Field<string>(20)!,
                        idSubadministracion = item.IsNull(21) ? 0 : item.Field<int>(21),
                        subadministracion = item.IsNull(22) ? null! : item.Field<string>(22)!,
                        idEstadoTarea = item.IsNull(23) ? 0 : item.Field<int>(23),
                        estadoTarea = item.IsNull(24) ? null! : item.Field<string>(24)!,
                        idEstadoProcesal = item.IsNull(25) ? 0 : item.Field<int>(25),
                        estadoProcesal = item.IsNull(26) ? null! : item.Field<string>(26)!,
                        id_empleado = item.IsNull(27) ? null! : item.Field<string>(27),
                        fecha_turnado = item.IsNull(28) ? null! : item.Field<DateTime>(28).ToString("yyyy-MM-dd"),
                        remitido = item.IsNull(29) ? false : item.Field<bool>(29)


                    }
                );
            }

            return resultList;
        }


        #region  Archivos
        public async Task<ResultTransaction> AddFileAsyncRepository(ArchivoConsulta entity, DataFile dataFile)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, entity.id_rol!),
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, entity.id_consulta!),
                new ParameterPGsql("p_id_remision", NpgsqlDbType.Integer, entity.id_remision!),
                new ParameterPGsql("p_no_folio", NpgsqlDbType.Varchar, entity.no_folio),
                new ParameterPGsql("p_id_seccion", NpgsqlDbType.Integer, entity.id_seccion!),
                new ParameterPGsql("p_id_tipo_documento", NpgsqlDbType.Integer, entity.id_tipo_documento!),
                new ParameterPGsql("p_file_name", NpgsqlDbType.Text, entity.file_name!),
                new ParameterPGsql("p_path_file", NpgsqlDbType.Text, entity.path_file!),
                new ParameterPGsql("p_content_type", NpgsqlDbType.Text, entity.content_type!),
                new ParameterPGsql("p_owner_name", NpgsqlDbType.Text, entity.owner_name!),
                new ParameterPGsql("p_estatus", NpgsqlDbType.Boolean, entity.estatus!),
                new ParameterPGsql("p_size", NpgsqlDbType.Varchar, entity.size!),
                new ParameterPGsql("p_id_documento_seccion", NpgsqlDbType.Integer, entity.id_documento_seccion!),
            };

            var response = await _database.ExecuteFunctionFileAsync(EnunFunctions.File_Repository_INSERT, dataFile, parameters);
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }

        public async Task<List<ResponseArchivosConsulta>> GetArchivosByIdRegistroAsync_Repository(int id_registro)
        {

            ArchivoConsulta entity = EventsArchivosConsultasOficialPartesEvents.GetRolOficialPartes();

            ParameterPGsql[] parameters =
          {
                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, entity.id_rol),
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, id_registro!),
                 new ParameterPGsql("p_id_seccion", NpgsqlDbType.Integer, 1!),
            };
            var response = await _database.ExecuteFunctionAsync(
              EnunFunctions.File_Repository_GET_BY_REGISTRO_ALL, parameters

          );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            List<ResponseArchivosConsulta> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        idRol = item.IsNull(1) ? 0 : item.Field<int>(1),
                        rol = item.IsNull(2) ? null! : item.Field<string>(2)!,
                        id_consulta = item.IsNull(3) ? 0 : item.Field<int>(3),
                        id_remision = item.IsNull(4) ? 0! : item.Field<int>(4),
                        folio = item.IsNull(5) ? null! : item.Field<string>(5),
                        idSeccion = item.IsNull(6) ? 0 : item.Field<int>(6),
                        seccion = item.IsNull(7) ? null! : item.Field<string>(7)!,
                        nombre = item.IsNull(8) ? null! : item.Field<string>(8),
                        path_file = item.IsNull(9) ? null! : item.Field<string>(9),
                        idTipoDocumento = item.IsNull(10) ? 0 : item.Field<int>(10),
                        tipoDocumento = item.IsNull(11) ? null! : item.Field<string>(11)!,
                        owner_name = item.IsNull(12) ? null! : item.Field<string>(12),
                        estatus = item.IsNull(13) ? false : item.Field<bool>(13),
                        tamanoDocumento = item.IsNull(14) ? null! : item.Field<string>(14),
                        fecha_creacion = item.IsNull(15) ? null! : item.Field<DateTime>(15).ToString("yyyy-MM-dd"),
                        id_administracion = item.IsNull(17) ? 0 : item.Field<int>(17),
                        administracion = item.IsNull(18) ? null! : item.Field<string>(18)!,


                    }
                );
            }

            return resultList;
        }
        public async Task<ResponseArchivosConsulta> GetByIdArchivoAsyncRepository(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.File_Repository_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                nombre = response.Data.Tables[0].Rows[0].IsNull(1) ? null! : response.Data.Tables[0].Rows[0].Field<string>(1),
                path_file = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),



            };
        }

        public async Task<ArchivoConsulta> GetIdArchivoConsultaAsyncRepository(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.File_Repository_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                file_name = response.Data.Tables[0].Rows[0].IsNull(1) ? null! : response.Data.Tables[0].Rows[0].Field<string>(1),
                path_file = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                id_consulta = response.Data.Tables[0].Rows[0].IsNull(3) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(3),
                id_remision = response.Data.Tables[0].Rows[0].IsNull(4) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(4),
                no_folio = response.Data.Tables[0].Rows[0].IsNull(5) ? null! : response.Data.Tables[0].Rows[0].Field<string>(5),
                id_seccion = response.Data.Tables[0].Rows[0].IsNull(6) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(6),
                id_tipo_documento = response.Data.Tables[0].Rows[0].IsNull(8) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(8),
                content_type = response.Data.Tables[0].Rows[0].IsNull(9) ? null! : response.Data.Tables[0].Rows[0].Field<string>(9),
                owner_name = response.Data.Tables[0].Rows[0].IsNull(10) ? null! : response.Data.Tables[0].Rows[0].Field<string>(10),
                estatus = response.Data.Tables[0].Rows[0].IsNull(11) ? false : response.Data.Tables[0].Rows[0].Field<bool>(11),
                size = response.Data.Tables[0].Rows[0].IsNull(12) ? null! : response.Data.Tables[0].Rows[0].Field<string>(12),
                fecha_creacion = response.Data.Tables[0].Rows[0].IsNull(13) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(13),
                fecha_modificacion = response.Data.Tables[0].Rows[0].IsNull(14) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(14),
                permanente = response.Data.Tables[0].Rows[0].IsNull(15) ? false : response.Data.Tables[0].Rows[0].Field<bool>(15),
                remplazable = response.Data.Tables[0].Rows[0].IsNull(16) ? false : response.Data.Tables[0].Rows[0].Field<bool>(16),
                id_rol = response.Data.Tables[0].Rows[0].IsNull(17) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(17),
                rol = response.Data.Tables[0].Rows[0].IsNull(18) ? null! : response.Data.Tables[0].Rows[0].Field<string>(18)
            };
        }

        public async Task<ResultTransaction> UpdateDocumentoAsync(ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql(
                    "p_id",
                    NpgsqlDbType.Integer,
                    entityDocumento.id!
                ),
                new ParameterPGsql(
                    "p_id_consulta",
                    NpgsqlDbType.Integer,
                    entityDocumento.id_consulta!
                ),
                new ParameterPGsql(
                    "p_usuario_modificacion",
                    NpgsqlDbType.Text,
                    entityDocumento.usuario_modificacion!
                ),
                new ParameterPGsql(
                    "p_archivo",
                    NpgsqlDbType.Boolean,
                    dataFile is not null
                ),
                new ParameterPGsql("p_id_tipo_documento", NpgsqlDbType.Integer, entityDocumento.id_tipo_documento!),
                new ParameterPGsql("p_id_seccion", NpgsqlDbType.Integer,entityDocumento.id_seccion!),
                new ParameterPGsql("p_file_name", NpgsqlDbType.Text, entityDocumento.file_name!),
                new ParameterPGsql("p_file_path", NpgsqlDbType.Text, entityDocumento.path_file !),
                new ParameterPGsql("p_content_type", NpgsqlDbType.Text,entityDocumento.content_type!),
                new ParameterPGsql("p_file_size", NpgsqlDbType.Text, entityDocumento.size!),
                new ParameterPGsql("p_owner_name", NpgsqlDbType.Text, entityDocumento.owner_name!),
                new ParameterPGsql("p_numero_folio", NpgsqlDbType.Text, entityDocumento.no_folio!),
                new ParameterPGsql("p_permanente", NpgsqlDbType.Boolean, entityDocumento.permanente!),
                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, entityDocumento.id_rol!),
            };

            var response = await _database.ExecuteFunctionFileAsync(
                EnunFunctions.FILE_REPOSITORY_UPDATE,
                dataFile!,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }

        public async Task<ResultTransaction> DeleteByIdArchivoAsyncRepository(int id)
        {
            ParameterPGsql[] parameters = { new ParameterPGsql("p_id", NpgsqlDbType.Integer, id), };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_DELETE_ARCHIVO,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }
        #endregion

        public async Task<ResponseConsultaList> GetByIdDisconnected(int id)
        {
            ParameterPGsql[] parameters =
             {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                no_asunto = response.Data.Tables[0].Rows[0].IsNull(1) ? null! : response.Data.Tables[0].Rows[0].Field<string>(1),
                rfc = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                promovente = response.Data.Tables[0].Rows[0].IsNull(3) ? null! : response.Data.Tables[0].Rows[0].Field<string>(3),
                rfc_contribuyente = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                promovente_es_contribuyente = response.Data.Tables[0].Rows[0].IsNull(5) ? false : response.Data.Tables[0].Rows[0].Field<bool>(5),
                contribuyente = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<string>(6),
                idTipoAsunto = response.Data.Tables[0].Rows[0].IsNull(7) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(7),
                tipoAsunto = response.Data.Tables[0].Rows[0].IsNull(8) ? null! : response.Data.Tables[0].Rows[0].Field<string>(8)!,
                idTipoModalidad = response.Data.Tables[0].Rows[0].IsNull(9) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(9),
                tipoModalidad = response.Data.Tables[0].Rows[0].IsNull(10) ? null! : response.Data.Tables[0].Rows[0].Field<string>(10)!,
                despacho_autorizado = response.Data.Tables[0].Rows[0].IsNull(11) ? null! : response.Data.Tables[0].Rows[0].Field<string>(11),
                fecha_presentacion = response.Data.Tables[0].Rows[0].IsNull(12) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(12).ToString("yyyy-MM-dd"),
                fecha_recepcion = response.Data.Tables[0].Rows[0].IsNull(13) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(13).ToString("yyyy-MM-dd"),
                fecha_vencimiento = response.Data.Tables[0].Rows[0].IsNull(14) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(14).ToString("yyyy-MM-dd"),
                idAdministracionCentral = response.Data.Tables[0].Rows[0].IsNull(15) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(15),
                administracionCentral = response.Data.Tables[0].Rows[0].IsNull(16) ? null! : response.Data.Tables[0].Rows[0].Field<string>(16)!,
                idAdministracion = response.Data.Tables[0].Rows[0].IsNull(17) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(17),
                administracion = response.Data.Tables[0].Rows[0].IsNull(18) ? null! : response.Data.Tables[0].Rows[0].Field<string>(18)!,
                idSubadministracion = response.Data.Tables[0].Rows[0].IsNull(19) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                subadministracion = response.Data.Tables[0].Rows[0].IsNull(20) ? null! : response.Data.Tables[0].Rows[0].Field<string>(20)!,
                idEstadoTarea = response.Data.Tables[0].Rows[0].IsNull(21) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(21),
                estadoTarea = response.Data.Tables[0].Rows[0].IsNull(22) ? null! : response.Data.Tables[0].Rows[0].Field<string>(22)!,
                idEstadoProcesal = response.Data.Tables[0].Rows[0].IsNull(23) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(23),
                estadoProcesal = response.Data.Tables[0].Rows[0].IsNull(24) ? null! : response.Data.Tables[0].Rows[0].Field<string>(24)!,
                id_empleado = response.Data.Tables[0].Rows[0].IsNull(25) ? null! : response.Data.Tables[0].Rows[0].Field<string>(25),
                fecha_turnado = response.Data.Tables[0].Rows[0].IsNull(26) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(26).ToString("yyyy-MM-dd"),                
                remitido = response.Data.Tables[0].Rows[0].IsNull(27) ? false : response.Data.Tables[0].Rows[0].Field<bool>(27),
                activo = response.Data.Tables[0].Rows[0].IsNull(28) ? false : response.Data.Tables[0].Rows[0].Field<bool>(28),                                
                idColorFechacssj = response.Data.Tables[0].Rows[0].IsNull(29) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(29),
                colorFechacssj = response.Data.Tables[0].Rows[0].IsNull(30) ? null! : response.Data.Tables[0].Rows[0].Field<string>(30)!,
                idAlertaDG = response.Data.Tables[0].Rows[0].IsNull(31) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(31),
                alertaDG = response.Data.Tables[0].Rows[0].IsNull(32) ? null! : response.Data.Tables[0].Rows[0].Field<string>(32)!,                                
                registroVence = response.Data.Tables[0].Rows[0].IsNull(33) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(33),                                              
                domicilioPromovente = response.Data.Tables[0].Rows[0].IsNull(34) ? null! : response.Data.Tables[0].Rows[0].Field<string>(34)!,
                domicilioNotificaciones = response.Data.Tables[0].Rows[0].IsNull(35) ? null! : response.Data.Tables[0].Rows[0].Field<string>(35)!,
                idTema = response.Data.Tables[0].Rows[0].IsNull(36) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(36),
                tema = response.Data.Tables[0].Rows[0].IsNull(37) ? null! : response.Data.Tables[0].Rows[0].Field<string>(37)!,
                monto = response.Data.Tables[0].Rows[0].IsNull(38) ? 0 : response.Data.Tables[0].Rows[0].Field<decimal>(38),                
                idAbogado = response.Data.Tables[0].Rows[0].IsNull(39) ? null! : response.Data.Tables[0].Rows[0].Field<string>(39)!,
                abogado = response.Data.Tables[0].Rows[0].IsNull(40) ? null! : response.Data.Tables[0].Rows[0].Field<string>(40)!,
                montoDeterminado = response.Data.Tables[0].Rows[0].IsNull(41) ? false : response.Data.Tables[0].Rows[0].Field<bool>(41),
                solicita_requerimiento=response.Data.Tables[0].Rows[0].IsNull(42) ? null!  : response.Data.Tables[0].Rows[0].Field<bool>(42),
                concluido=response.Data.Tables[0].Rows[0].IsNull(43) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(43),  
                fecha_firmeza = response.Data.Tables[0].Rows[0].IsNull(44) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(44).ToString("yyyy-MM-dd"),
                idOrganoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(45) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(45)!,
                organoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(46) ? null! : response.Data.Tables[0].Rows[0].Field<string>(46)!,
                fecha_asignacion = response.Data.Tables[0].Rows[0].IsNull(47) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(47).ToString("yyyy-MM-dd"),   
                numeroJuicio = response.Data.Tables[0].Rows[0].IsNull(48) ? null! : response.Data.Tables[0].Rows[0].Field<string>(48)!, 
                noRecurso= response.Data.Tables[0].Rows[0].IsNull(49) ? null! : response.Data.Tables[0].Rows[0].Field<string>(49),
                tipo_recurso = response.Data.Tables[0].Rows[0].IsNull(50) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(50),     
                recurso = response.Data.Tables[0].Rows[0].IsNull(51) ? null! : response.Data.Tables[0].Rows[0].Field<string>(51)!,   
                id_plazo_cumplimentar = response.Data.Tables[0].Rows[0].IsNull(52) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(52),
                plazo_cumplimentar = response.Data.Tables[0].Rows[0].IsNull(53) ? null! : response.Data.Tables[0].Rows[0].Field<string>(53)!,
                oficioResolucion = response.Data.Tables[0].Rows[0].IsNull(54) ? null! : response.Data.Tables[0].Rows[0].Field<string>(54)!,
                fechaOficioResolucion = response.Data.Tables[0].Rows[0].IsNull(55) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(55).ToString("yyyy-MM-dd"),                 
                horas = response.Data.Tables[0].Rows[0].IsNull(56) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(56),
                idSeccionDescartar = response.Data.Tables[0].Rows[0].IsNull(57) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(57),
                idSeccionModificar = response.Data.Tables[0].Rows[0].IsNull(58) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(58),                
                idSemaforo = response.Data.Tables[0].Rows[0].IsNull(59) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(59),
                semaforo = response.Data.Tables[0].Rows[0].IsNull(60) ? null! : response.Data.Tables[0].Rows[0].Field<string>(60)!,               
                idUnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(61) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(61),
                UnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(62) ? null! : response.Data.Tables[0].Rows[0].Field<string>(62)!,
                asigno=response.Data.Tables[0].Rows[0].IsNull(63) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(63),  
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(64) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(64).ToString("yyyy-MM-dd"),
                turnado=response.Data.Tables[0].Rows[0].IsNull(65) ? false!  : response.Data.Tables[0].Rows[0].Field<bool>(65)
              
            };
        }

        #region Solicitud Transparencia

        public async Task<ResultTransaction> AddAsyncSolicitudTransparenciaService(SolicitudTransparencia entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {


            ParameterPGsql[] parameters =
            {

                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, entity.id_rol!),
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, entity.id_consulta!),
                new ParameterPGsql("p_no_solicitud",NpgsqlDbType.Varchar,entity.noSolicitud!),
                new ParameterPGsql("p_fecha_solicitud", NpgsqlDbType.Date, entity.fechaSolicitud!),
                new ParameterPGsql(
                    "p_archivo",
                    NpgsqlDbType.Boolean,
                    dataFile is not null
                ),
                new ParameterPGsql("p_id_tipo_documento", NpgsqlDbType.Integer, entityDocumento is null ? DBNull.Value : entityDocumento.id_tipo_documento!),
                new ParameterPGsql("p_id_seccion", NpgsqlDbType.Integer, entityDocumento is null ? DBNull.Value : entityDocumento.id_seccion!),
                new ParameterPGsql("p_file_name", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.file_name!),
                new ParameterPGsql("p_file_path", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.path_file !),
                new ParameterPGsql("p_content_type", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.content_type!),
                new ParameterPGsql("p_file_size", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.size!),
                new ParameterPGsql("p_owner_name", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.owner_name!),
                new ParameterPGsql("p_no_folio", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.no_folio!),

            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.SOLICITUD_TRANSPARENCIA_INSERT,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }

        public async Task<SolicitudTransparencia> GetByIdSolicitudTransparenciaRepository(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.OficialPartesReadIdSolicitudTransparencia,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                id_rol = response.Data.Tables[0].Rows[0].IsNull(1) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(1),
                nombre_rol = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                id_consulta = response.Data.Tables[0].Rows[0].IsNull(3) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(3),
                noSolicitud = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                fechaSolicitud = response.Data.Tables[0].Rows[0].IsNull(5) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(5),
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(6) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(6),
                fecha_modificacion = response.Data.Tables[0].Rows[0].IsNull(7) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(7),
            };
        }

        public async Task<ResultTransaction> UpdateSolicitudTransparenciaRepository(SolicitudTransparencia entity)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, entity.id!),
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, entity.id_consulta!),
                new ParameterPGsql("p_no_solicitud",NpgsqlDbType.Varchar,entity.noSolicitud!),
                new ParameterPGsql("p_fecha_solicitud", NpgsqlDbType.Date, entity.fechaSolicitud!),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.OficialPartesUpdateSolicitudTransparencia,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }

        public async Task<ResponseSolicitudTransparenciaList> GetByIdSolicitudTransparenciaRepositorys(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.OficialPartesReadIdSolicitudTransparencia,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                id_rol = response.Data.Tables[0].Rows[0].IsNull(1) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(1),
                nombre_rol = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                id_consulta = response.Data.Tables[0].Rows[0].IsNull(3) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(3),
                no_solicitud = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                fecha_solicitud = response.Data.Tables[0].Rows[0].IsNull(5) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(5).ToString("yyyy-MM-dd"),
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(6).ToString("yyyy-MM-dd"),
                fecha_modificacion = response.Data.Tables[0].Rows[0].IsNull(7) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(7).ToString("yyyy-MM-dd"),
            };
        }

        public async Task<int?> GetTablaSolicitudTransparenciaCountRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
            new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.OficialPartesReadCountSolicitudTransparencia,
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

            return response.Data.Tables[0].Rows[0].IsNull(0)
                ? null!
                : response.Data.Tables[0].Rows[0].Field<int>(0);
        }

        public async Task<List<ResponseSolicitudTransparenciaList>> GetTablaSolicitudTransparenciaRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("order_column", NpgsqlDbType.Varchar, ""),
                new ParameterPGsql("order_desc", NpgsqlDbType.Boolean, false),
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.OficialPartesReadTablaSolicitudTransparencia,
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

            List<ResponseSolicitudTransparenciaList> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        id_rol = item.IsNull(1) ? 0 : item.Field<int>(1),
                        nombre_rol = item.IsNull(2) ? null! : item.Field<string>(2)!,
                        id_consulta = item.IsNull(3) ? 0 : item.Field<int>(3),
                        no_solicitud = item.IsNull(4) ? null! : item.Field<string>(4),
                        fecha_solicitud = item.IsNull(5) ? null! : item.Field<DateTime>(5).ToString("yyyy-MM-dd"),
                        fecha_registro = item.IsNull(6) ? null! : item.Field<DateTime>(6).ToString("yyyy-MM-dd"),
                        fecha_modificacion = item.IsNull(7) ? null! : item.Field<DateTime>(7).ToString("yyyy-MM-dd"),
                    }
                );
            }

            return resultList;

        }

        public async Task<ResultTransaction> DeleteSolicitudTransparenciaRepository(int id)
        {
            ParameterPGsql[] parameters = { new ParameterPGsql("p_id", NpgsqlDbType.Integer, id), };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.OficialPartesDeleteSolicitudTransparencia,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }

        #endregion

        public async Task<int?> GetArchivosByFiltersCountAsyncRepository(
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
       )
        {


            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_idrol", NpgsqlDbType.Integer, idRol),
                new ParameterPGsql("p_folio", NpgsqlDbType.Text, folio),
                new ParameterPGsql("p_idseccion", NpgsqlDbType.Integer, idSeccion),
                new ParameterPGsql("p_idconsulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_idremision", NpgsqlDbType.Integer, idRemision),
                new ParameterPGsql("p_estatus", NpgsqlDbType.Boolean, estatus),
                new ParameterPGsql("p_fechacreaciondesde", NpgsqlDbType.Date, fechaCreacionDesde),
                new ParameterPGsql("p_fechacreacionhasta", NpgsqlDbType.Date, fechaCreacionHasta),
                new ParameterPGsql("p_remplazable", NpgsqlDbType.Boolean, remplazable),
                new ParameterPGsql("p_permanente", NpgsqlDbType.Boolean, permanente),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.ArchivoByFiltersCountAdmin,
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

            return response.Data.Tables[0].Rows[0].IsNull(0)
                ? null!
                : response.Data.Tables[0].Rows[0].Field<int>(0);
        }

        public async Task<List<ResponseArchivosConsulta>> GetArchivoByFiltersAsyncRepository(
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
             )
        {


            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_page_size", NpgsqlDbType.Integer, pageSize),
                new ParameterPGsql("p_page", NpgsqlDbType.Integer, page),
                new ParameterPGsql("p_idrol", NpgsqlDbType.Integer, idRol),
                new ParameterPGsql("p_folio", NpgsqlDbType.Varchar, folio),
                new ParameterPGsql("p_idseccion", NpgsqlDbType.Integer, idSeccion),
                new ParameterPGsql("p_idconsulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_idremision", NpgsqlDbType.Integer, idRemision),
                new ParameterPGsql("p_estatus", NpgsqlDbType.Boolean, estatus),
                new ParameterPGsql("p_fechacreaciondesde", NpgsqlDbType.Date, fechaCreacionDesde),
                new ParameterPGsql("p_fechacreacionhasta", NpgsqlDbType.Date, fechaCreacionHasta),
                new ParameterPGsql("p_remplazable", NpgsqlDbType.Boolean, remplazable),
                new ParameterPGsql("p_permanente", NpgsqlDbType.Boolean, permanente),
                new ParameterPGsql("order_column", NpgsqlDbType.Varchar, orderByColumn),
                new ParameterPGsql("order_desc", NpgsqlDbType.Boolean, orderDesc),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.Archivo_GET_FILTERS_ADMIN,
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

            List<ResponseArchivosConsulta> resultList = new();
            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        idRol = item.IsNull(1) ? 0 : item.Field<int>(1),
                        rol = item.IsNull(2) ? null! : item.Field<string>(2)!,
                        id_consulta = item.IsNull(3) ? 0 : item.Field<int>(3),
                        id_remision = item.IsNull(4) ? 0 : item.Field<int>(4),
                        folio = item.IsNull(5) ? null! : item.Field<string>(5),
                        idSeccion = item.IsNull(6) ? 0 : item.Field<int>(6),
                        seccion = item.IsNull(7) ? null! : item.Field<string>(7)!,
                        nombre = item.IsNull(8) ? null! : item.Field<string>(8),
                        path_file = item.IsNull(9) ? null! : item.Field<string>(9),
                        idTipoDocumento = item.IsNull(10) ? 0 : item.Field<int>(10),
                        tipoDocumento = item.IsNull(11) ? null! : item.Field<string>(11)!,
                        estatus = item.IsNull(13) ? false : item.Field<bool>(13)!,
                        tamanoDocumento = item.IsNull(14) ? null! : item.Field<string>(14)!,
                        fecha_creacion = item.IsNull(15) ? null! : item.Field<DateTime>(15).ToString("yyyy-MM-dd"),
                        id_administracion = item.IsNull(16) ? 0 : item.Field<int>(16),
                        permanente = item.IsNull(17) ? false : item.Field<bool>(17)!,
                        remplazable = item.IsNull(18) ? false : item.Field<bool>(18)!,

                    }
                );
            }

            return resultList;
        }

        #region Cumplimentacion

        public async Task<ResultTransaction> AddCumplimentacionRepository(Cumplimentacion entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_no_asunto_consulta", NpgsqlDbType.Varchar, entity.no_asunto_consulta!),
                new ParameterPGsql("p_rfc", NpgsqlDbType.Varchar, entity.rfc!),
                new ParameterPGsql("p_promovente", NpgsqlDbType.Varchar, entity.promovente!),
                new ParameterPGsql("p_promovente_es_contribuyente",NpgsqlDbType.Boolean,entity.promovente_es_contribuyente!),
                new ParameterPGsql("p_rfc_contribuyente",NpgsqlDbType.Varchar,entity.rfc_contribuyente!),
                new ParameterPGsql("p_contribuyente", NpgsqlDbType.Varchar, entity.contribuyente!),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Integer, entity.id_tipo_asunto),
                new ParameterPGsql("p_id_tipo_modalidad", NpgsqlDbType.Integer, entity.id_tipo_modalidad),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Integer,EnumEstadoTarea.PENDIENTE_DE_TURNAR.GetHashCode()),
                 new ParameterPGsql("p_id_administracion_central", NpgsqlDbType.Integer, entity.idAdministracionCentral),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.ACTIVO.GetHashCode()),
                new ParameterPGsql("p_id_administracion", NpgsqlDbType.Integer, entity.id_administracion),
                new ParameterPGsql("p_id_administracion_solicita", NpgsqlDbType.Integer, entity.idUnidadAdministrativaSolicitaCump),
                new ParameterPGsql("p_id_plazo_cumplimentar", NpgsqlDbType.Integer, entity.plazoCumplimentar),                
                new ParameterPGsql("p_usuario_creacion", NpgsqlDbType.Varchar, entity.usuario_creacion!),
                new ParameterPGsql("p_id_subadministracion", NpgsqlDbType.Integer, entity.id_Subadministracion),
                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Oficial_De_Partes.GetHashCode()),
                new ParameterPGsql("p_tipo_recurso", NpgsqlDbType.Integer, EnumTipoRecurso.CUMPLIMENTACION.GetHashCode()),
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, entity.id_consulta!),
                new ParameterPGsql("p_numero_juicio", NpgsqlDbType.Varchar, entity.numero_juicio!),
                new ParameterPGsql("p_fecha_recepcion",NpgsqlDbType.Date,entity.fecha_recepcion!),
                new ParameterPGsql("p_fecha_firmeza", NpgsqlDbType.Date, entity.fecha_firmeza!),
                new ParameterPGsql("p_fecha_vencimiento", NpgsqlDbType.Date, entity.fecha_vencimiento),
                new ParameterPGsql("p_id_organo_jurisdiccional", NpgsqlDbType.Integer, entity.id_organo_jurisdiccional!),
                new ParameterPGsql(
                    "p_archivo",
                    NpgsqlDbType.Boolean,
                    dataFile is not null
                ),
                new ParameterPGsql("p_id_tipo_documento", NpgsqlDbType.Integer, entityDocumento is null ? DBNull.Value : entityDocumento.id_tipo_documento!),
                new ParameterPGsql("p_id_seccion", NpgsqlDbType.Integer, entityDocumento is null ? DBNull.Value : entityDocumento.id_seccion!),
                new ParameterPGsql("p_file_name", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.file_name!),
                new ParameterPGsql("p_file_path", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.path_file !),
                new ParameterPGsql("p_content_type", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.content_type!),
                new ParameterPGsql("p_file_size", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.size!),
                new ParameterPGsql("p_owner_name", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.owner_name!),
                new ParameterPGsql("p_no_folio", NpgsqlDbType.Text, entityDocumento is null ? DBNull.Value : entityDocumento.no_folio!),
            };

 

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.OficialPartesCreateCumplimentacion,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }

        public async Task<ResultTransaction> TurnarCumplimentacionRepository(Cumplimentacion entity)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_registro", NpgsqlDbType.Integer, entity.id),
                new ParameterPGsql("p_id_administracion", NpgsqlDbType.Integer, entity.id_administracion!),
                new ParameterPGsql("p_tipo_recurso", NpgsqlDbType.Integer, EnumTipoRecurso.CUMPLIMENTACION.GetHashCode()),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Integer,EnumEstadoTarea.PENDIENTE_DE_ASIGNAR.GetHashCode()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer,EnumEstadoProcesal.ACTIVO.GetHashCode()),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CUMPLIMENTACION_TURNAR,
                parameters
            );

            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            var result = response.Data.Tables[0].Rows[0];

            return new()
            {
                Result = result.Field<int?>(0),
                Success = result.Field<bool>(1),
                MsgError = result.Field<string?>(2)!,
                DetailError = result.Field<string?>(3)!,
                NoError = result.Field<string?>(4)!
            };
        }
        public async Task<ResultTransaction> UpdateCumplimentacionRepository(Cumplimentacion entity)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_registro", NpgsqlDbType.Integer, entity.id),
                new ParameterPGsql("p_numero_juicio", NpgsqlDbType.Varchar, entity.numero_juicio!),
                new ParameterPGsql("p_fecha_recepcion",NpgsqlDbType.Date,entity.fecha_recepcion!),
                new ParameterPGsql("p_fecha_firmeza", NpgsqlDbType.Date, entity.fecha_firmeza!),
                new ParameterPGsql("p_fecha_vencimiento", NpgsqlDbType.Date, entity.fecha_vencimiento),
                new ParameterPGsql("p_id_organo_jurisdiccional", NpgsqlDbType.Integer, entity.id_organo_jurisdiccional!),
                new ParameterPGsql("p_id_administracion", NpgsqlDbType.Integer, entity.id_administracion),
                new ParameterPGsql("p_id_administracion_solicita", NpgsqlDbType.Integer, entity.idUnidadAdministrativaSolicitaCump),
                new ParameterPGsql("p_id_subadministracion", NpgsqlDbType.Integer, entity.id_Subadministracion),
                new ParameterPGsql("p_id_plazo_cumplimentar", NpgsqlDbType.Integer, entity.plazoCumplimentar), 

            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CUMPLIMENTACION_UPDATE,
                parameters
            );
            if (response.ExisteError)
            {
                return new()
                {
                    Success = false,
                    MsgError = response.Mensaje,
                    NoError = response.CodeSqlError,
                };
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return new()
                {
                    Success = false,
                    MsgError = "No se pudo obtener la respuesta de la operación en base de datos.",
                };
            }

            return new()
            {
                Result = response.Data.Tables[0].Rows[0].Field<int?>(0),
                Success = response.Data.Tables[0].Rows[0].Field<bool>(1),
                MsgError = response.Data.Tables[0].Rows[0].Field<string?>(2)!,
                DetailError = response.Data.Tables[0].Rows[0].Field<string?>(3)!,
                NoError = response.Data.Tables[0].Rows[0].Field<string?>(4)!,
            };
        }

        public async Task<Cumplimentacion> GetByIdAllAsyncRepositoryCumplimentacion(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CUMPLIMENTACION_GET_BY_ID,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                no_asunto = response.Data.Tables[0].Rows[0].IsNull(1) ? null! : response.Data.Tables[0].Rows[0].Field<string>(1),
                rfc = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                promovente = response.Data.Tables[0].Rows[0].IsNull(3) ? null! : response.Data.Tables[0].Rows[0].Field<string>(3),
                rfc_contribuyente = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                promovente_es_contribuyente = response.Data.Tables[0].Rows[0].IsNull(5) ? false : response.Data.Tables[0].Rows[0].Field<bool>(5),
                contribuyente = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<string>(6),
                id_tipo_asunto = response.Data.Tables[0].Rows[0].IsNull(7) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(7),
                tipo_asunto = response.Data.Tables[0].Rows[0].IsNull(8) ? null! : response.Data.Tables[0].Rows[0].Field<string>(8)!,
                id_tipo_modalidad = response.Data.Tables[0].Rows[0].IsNull(9) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(9),
                tipo_modalidad = response.Data.Tables[0].Rows[0].IsNull(10) ? null! : response.Data.Tables[0].Rows[0].Field<string>(10)!,
                despacho_autorizado = response.Data.Tables[0].Rows[0].IsNull(11) ? null! : response.Data.Tables[0].Rows[0].Field<string>(11),
                fecha_presentacion = response.Data.Tables[0].Rows[0].IsNull(12) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(12),
                fecha_recepcion = response.Data.Tables[0].Rows[0].IsNull(13) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(13),
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(14) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(14),
                fechaVencimientoCump = response.Data.Tables[0].Rows[0].IsNull(15) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(15),
                turnado = response.Data.Tables[0].Rows[0].IsNull(16) ? false : response.Data.Tables[0].Rows[0].Field<bool>(16),
                idAdministracionCentral = response.Data.Tables[0].Rows[0].IsNull(17) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(17),
                administracionCentral = response.Data.Tables[0].Rows[0].IsNull(18) ? null! : response.Data.Tables[0].Rows[0].Field<string>(18)!,
                id_administracion = response.Data.Tables[0].Rows[0].IsNull(19) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                Administracion = response.Data.Tables[0].Rows[0].IsNull(20) ? null! : response.Data.Tables[0].Rows[0].Field<string>(20)!,
                id_Subadministracion = response.Data.Tables[0].Rows[0].IsNull(21) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(21),
                Subadministracion = response.Data.Tables[0].Rows[0].IsNull(22) ? null! : response.Data.Tables[0].Rows[0].Field<string>(22)!,
                idEstadoTarea = response.Data.Tables[0].Rows[0].IsNull(23) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(23),
                estadoTarea = response.Data.Tables[0].Rows[0].IsNull(24) ? null! : response.Data.Tables[0].Rows[0].Field<string>(24)!,
                idEstadoProcesal = response.Data.Tables[0].Rows[0].IsNull(25) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(25),
                estadoProcesal = response.Data.Tables[0].Rows[0].IsNull(26) ? null! : response.Data.Tables[0].Rows[0].Field<string>(26)!,
                id_empleado = response.Data.Tables[0].Rows[0].IsNull(27) ? null! : response.Data.Tables[0].Rows[0].Field<string>(27),
                fecha_turnado = response.Data.Tables[0].Rows[0].IsNull(28) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(28),
                remitido = response.Data.Tables[0].Rows[0].IsNull(29) ? false : response.Data.Tables[0].Rows[0].Field<bool>(29),
                activo = response.Data.Tables[0].Rows[0].IsNull(30) ? false : response.Data.Tables[0].Rows[0].Field<bool>(30),
                id_abogado = response.Data.Tables[0].Rows[0].IsNull(31) ? null! : response.Data.Tables[0].Rows[0].Field<string>(31)!,
                //USUARIO ASIGNO
                asignado = response.Data.Tables[0].Rows[0].IsNull(33) ? false : response.Data.Tables[0].Rows[0].Field<bool>(33),
                idColorFechacssj = response.Data.Tables[0].Rows[0].IsNull(34) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(34),
                colorFechacssj = response.Data.Tables[0].Rows[0].IsNull(35) ? null! : response.Data.Tables[0].Rows[0].Field<string>(35)!,
                idAlertaDG = response.Data.Tables[0].Rows[0].IsNull(36) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(36),
                alertaDG = response.Data.Tables[0].Rows[0].IsNull(37) ? null! : response.Data.Tables[0].Rows[0].Field<string>(37)!,
                registroVence = response.Data.Tables[0].Rows[0].IsNull(38) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(38),
                fecha_firmeza = response.Data.Tables[0].Rows[0].IsNull(39) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(39),
                id_organo_jurisdiccional = response.Data.Tables[0].Rows[0].IsNull(40) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(40),
                organoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(41) ? null! : response.Data.Tables[0].Rows[0].Field<string>(41)!,
                fecha_asignacion = response.Data.Tables[0].Rows[0].IsNull(42) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(42),   
                numero_juicio = response.Data.Tables[0].Rows[0].IsNull(43) ? null! : response.Data.Tables[0].Rows[0].Field<string>(43)!,
                no_asunto_consulta = response.Data.Tables[0].Rows[0].IsNull(44) ? null! : response.Data.Tables[0].Rows[0].Field<string>(44)!,
                tipo_recurso = response.Data.Tables[0].Rows[0].IsNull(45) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(45),
                idUnidadAdministativaCump = response.Data.Tables[0].Rows[0].IsNull(19) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                unidadAdministativaCump = response.Data.Tables[0].Rows[0].IsNull(20) ? null! : response.Data.Tables[0].Rows[0].Field<string>(20)!,
                idUnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(46) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(46),
                UnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(47) ? null! : response.Data.Tables[0].Rows[0].Field<string>(47)!,
            };
        }

        public async Task<ResponseConsultaList> GetByNoAsuntoConsulta(string noAsunto)
        {
            ParameterPGsql[] parameters =
             {
                new ParameterPGsql("p_no_asunto", NpgsqlDbType.Varchar, noAsunto),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_NO_ASUNTO,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                no_asunto = response.Data.Tables[0].Rows[0].IsNull(1) ? null! : response.Data.Tables[0].Rows[0].Field<string>(1),
                rfc = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                promovente = response.Data.Tables[0].Rows[0].IsNull(3) ? null! : response.Data.Tables[0].Rows[0].Field<string>(3),
                rfc_contribuyente = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                promovente_es_contribuyente = response.Data.Tables[0].Rows[0].IsNull(5) ? false : response.Data.Tables[0].Rows[0].Field<bool>(5),
                contribuyente = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<string>(6),
                idTipoAsunto = response.Data.Tables[0].Rows[0].IsNull(7) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(7),
                tipoAsunto = response.Data.Tables[0].Rows[0].IsNull(8) ? null! : response.Data.Tables[0].Rows[0].Field<string>(8)!,
                idTipoModalidad = response.Data.Tables[0].Rows[0].IsNull(9) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(9),
                tipoModalidad = response.Data.Tables[0].Rows[0].IsNull(10) ? null! : response.Data.Tables[0].Rows[0].Field<string>(10)!,
                 fecha_recepcion = response.Data.Tables[0].Rows[0].IsNull(13) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(13).ToString("yyyy-MM-dd"),
                 idAdministracionCentral = response.Data.Tables[0].Rows[0].IsNull(17) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(17),
                 administracionCentral = response.Data.Tables[0].Rows[0].IsNull(18) ? null! : response.Data.Tables[0].Rows[0].Field<string>(18)!,
                 idAdministracion = response.Data.Tables[0].Rows[0].IsNull(19) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                 administracion = response.Data.Tables[0].Rows[0].IsNull(20) ? null! : response.Data.Tables[0].Rows[0].Field<string>(20)!,
                 idSubadministracion = response.Data.Tables[0].Rows[0].IsNull(21) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(21),
                 subadministracion = response.Data.Tables[0].Rows[0].IsNull(22) ? null! : response.Data.Tables[0].Rows[0].Field<string>(22)!,
                 idEstadoTarea = response.Data.Tables[0].Rows[0].IsNull(23) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(23),
                 estadoTarea = response.Data.Tables[0].Rows[0].IsNull(24) ? null! : response.Data.Tables[0].Rows[0].Field<string>(24)!,
                 idEstadoProcesal = response.Data.Tables[0].Rows[0].IsNull(25) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(25),
                 estadoProcesal = response.Data.Tables[0].Rows[0].IsNull(26) ? null! : response.Data.Tables[0].Rows[0].Field<string>(26)!,
                 intentos = response.Data.Tables[0].Rows[0].IsNull(39) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(39),
                cumplimentar = response.Data.Tables[0].Rows[0].IsNull(40) ? false : response.Data.Tables[0].Rows[0].Field<bool>(40),
                
            
            
            };
        }

        public async Task<ResponseCumplimentacionList> GetCumplimentacionById(string no_asunto_consulta)
        {
            ParameterPGsql[] parameters =
             {
                new ParameterPGsql("p_no_asunto_consulta", NpgsqlDbType.Varchar, no_asunto_consulta),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.GET_BY_NO_ASUNTO_CONSULTA_CUMPLIMENTACION,
                parameters
            );
            if (response.ExisteError)
            {
                throw new Exception(response.Mensaje);
            }

            if (response.Data.Tables.Count <= 0 || response.Data.Tables[0].Rows.Count <= 0)
            {
                return null!;
            }

            return new()
            {
                id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                no_asunto = response.Data.Tables[0].Rows[0].IsNull(1) ? null! : response.Data.Tables[0].Rows[0].Field<string>(1),
                rfc = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                promovente = response.Data.Tables[0].Rows[0].IsNull(3) ? null! : response.Data.Tables[0].Rows[0].Field<string>(3),
                rfc_contribuyente = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                promovente_es_contribuyente = response.Data.Tables[0].Rows[0].IsNull(5) ? false : response.Data.Tables[0].Rows[0].Field<bool>(5),
                contribuyente = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<string>(6),
                idTipoAsunto = response.Data.Tables[0].Rows[0].IsNull(7) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(7),
                tipoAsunto = response.Data.Tables[0].Rows[0].IsNull(8) ? null! : response.Data.Tables[0].Rows[0].Field<string>(8)!,
                idTipoModalidad = response.Data.Tables[0].Rows[0].IsNull(9) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(9),
                tipoModalidad = response.Data.Tables[0].Rows[0].IsNull(10) ? null! : response.Data.Tables[0].Rows[0].Field<string>(10)!,
                despacho_autorizado = response.Data.Tables[0].Rows[0].IsNull(11) ? null! : response.Data.Tables[0].Rows[0].Field<string>(11),
                fecha_presentacion = response.Data.Tables[0].Rows[0].IsNull(12) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(12).ToString("yyyy-MM-dd"),
                fecha_recepcion = response.Data.Tables[0].Rows[0].IsNull(13) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(13).ToString("yyyy-MM-dd"),
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(14) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(14).ToString("yyyy-MM-dd"),
                fechaVencimientoCump = response.Data.Tables[0].Rows[0].IsNull(15) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(15).ToString("yyyy-MM-dd"),
                turnado = response.Data.Tables[0].Rows[0].IsNull(16) ? false : response.Data.Tables[0].Rows[0].Field<bool>(16),
                idAdministracionCentral = response.Data.Tables[0].Rows[0].IsNull(17) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(17),
                administracionCentral = response.Data.Tables[0].Rows[0].IsNull(18) ? null! : response.Data.Tables[0].Rows[0].Field<string>(18)!,
                idAdministracion = response.Data.Tables[0].Rows[0].IsNull(19) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                administracion = response.Data.Tables[0].Rows[0].IsNull(20) ? null! : response.Data.Tables[0].Rows[0].Field<string>(20)!,
                idSubadministracion = response.Data.Tables[0].Rows[0].IsNull(21) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(21),
                subadministracion = response.Data.Tables[0].Rows[0].IsNull(22) ? null! : response.Data.Tables[0].Rows[0].Field<string>(22)!,
                idEstadoTarea = response.Data.Tables[0].Rows[0].IsNull(23) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(23),
                estadoTarea = response.Data.Tables[0].Rows[0].IsNull(24) ? null! : response.Data.Tables[0].Rows[0].Field<string>(24)!,
                idEstadoProcesal = response.Data.Tables[0].Rows[0].IsNull(25) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(25),
                estadoProcesal = response.Data.Tables[0].Rows[0].IsNull(26) ? null! : response.Data.Tables[0].Rows[0].Field<string>(26)!,
                id_empleado = response.Data.Tables[0].Rows[0].IsNull(27) ? null! : response.Data.Tables[0].Rows[0].Field<string>(27),
                fecha_turnado = response.Data.Tables[0].Rows[0].IsNull(28) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(28).ToString("yyyy-MM-dd"),
                remitido = response.Data.Tables[0].Rows[0].IsNull(29) ? false : response.Data.Tables[0].Rows[0].Field<bool>(29),
                activo = response.Data.Tables[0].Rows[0].IsNull(30) ? false : response.Data.Tables[0].Rows[0].Field<bool>(30),
                idAbogado = response.Data.Tables[0].Rows[0].IsNull(31) ? null! : response.Data.Tables[0].Rows[0].Field<string>(31)!,
                //USUARIO ASIGNO
                asignado = response.Data.Tables[0].Rows[0].IsNull(33) ? false : response.Data.Tables[0].Rows[0].Field<bool>(33),
                idColorFechacssj = response.Data.Tables[0].Rows[0].IsNull(34) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(34),
                colorFechacssj = response.Data.Tables[0].Rows[0].IsNull(35) ? null! : response.Data.Tables[0].Rows[0].Field<string>(35)!,
                idAlertaDG = response.Data.Tables[0].Rows[0].IsNull(36) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(36),
                alertaDG = response.Data.Tables[0].Rows[0].IsNull(37) ? null! : response.Data.Tables[0].Rows[0].Field<string>(37)!,
                registroVence = response.Data.Tables[0].Rows[0].IsNull(38) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(38),
                fecha_firmeza = response.Data.Tables[0].Rows[0].IsNull(39) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(39).ToString("yyyy-MM-dd"),
                id_organo_jurisdiccional = response.Data.Tables[0].Rows[0].IsNull(40) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(40),
                organoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(41) ? null! : response.Data.Tables[0].Rows[0].Field<string>(41)!,
                fecha_asignacion = response.Data.Tables[0].Rows[0].IsNull(42) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(42).ToString("yyyy-MM-dd"),   
                numeroJuicio = response.Data.Tables[0].Rows[0].IsNull(43) ? null! : response.Data.Tables[0].Rows[0].Field<string>(43)!,
                no_asunto_consulta = response.Data.Tables[0].Rows[0].IsNull(44) ? null! : response.Data.Tables[0].Rows[0].Field<string>(44)!,
                tipo_recurso = response.Data.Tables[0].Rows[0].IsNull(45) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(45),
                idUnidadAdministativaCump = response.Data.Tables[0].Rows[0].IsNull(19) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                unidadAdministativaCump = response.Data.Tables[0].Rows[0].IsNull(20) ? null! : response.Data.Tables[0].Rows[0].Field<string>(20)!,
                idUnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(46) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(46),
                UnidadAdministrativaSolicitaCump = response.Data.Tables[0].Rows[0].IsNull(47) ? null! : response.Data.Tables[0].Rows[0].Field<string>(47)!,
            };

        }

        #endregion
    }
}