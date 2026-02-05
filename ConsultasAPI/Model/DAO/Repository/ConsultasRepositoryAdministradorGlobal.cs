using System.Data;
using ConsultasAPI.Model.IDAO.IRepository;
using ConsultasAPI.Model.ViewModels.Enums;
using ConsultasAPI.Model.Entities;
using Sicoj.Utils.Postgres;
using NpgsqlTypes;
using Sicoj.Utils.Models;
using ConsultasAPI.Model.DTO.Response.Administrador;
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.Entities.Events.AdministradorGlobal;

namespace ConsultasAPI.Model.DAO.Repository
{
    public class ConsultasRepositoryAdministradorGlobal : IConsultaRepositoryAdministradorGlobal
    {
        #region Variables

        private readonly ISqlTools _database;


        public ConsultasRepositoryAdministradorGlobal(ISqlTools database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        #endregion

        #region General
        public async Task<ResponseConsultaList> GetByIdDisconnected(int id)
        {
            ParameterPGsql[] parameters =
             {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID_ADMINISTRADOR,
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
                solicita_requerimiento = response.Data.Tables[0].Rows[0].IsNull(42) ? null! : response.Data.Tables[0].Rows[0].Field<bool>(42),
                concluido = response.Data.Tables[0].Rows[0].IsNull(43) ? false! : response.Data.Tables[0].Rows[0].Field<bool>(43),
                fecha_firmeza = response.Data.Tables[0].Rows[0].IsNull(44) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(44).ToString("yyyy-MM-dd"),
                idOrganoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(45) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(45)!,
                organoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(46) ? null! : response.Data.Tables[0].Rows[0].Field<string>(46)!,
                fecha_asignacion = response.Data.Tables[0].Rows[0].IsNull(47) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(47).ToString("yyyy-MM-dd"),
                numeroJuicio = response.Data.Tables[0].Rows[0].IsNull(48) ? null! : response.Data.Tables[0].Rows[0].Field<string>(48)!,
                noRecurso = response.Data.Tables[0].Rows[0].IsNull(49) ? null! : response.Data.Tables[0].Rows[0].Field<string>(49),
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
                asigno = response.Data.Tables[0].Rows[0].IsNull(63) ? false! : response.Data.Tables[0].Rows[0].Field<bool>(63),
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(64) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(64).ToString("yyyy-MM-dd"),
                turnado = response.Data.Tables[0].Rows[0].IsNull(65) ? false! : response.Data.Tables[0].Rows[0].Field<bool>(65)
            };
        }
        #endregion

        #region Reactivar

        public async Task<Consulta> GetByIdConsultaRepository(int id)
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
                id_abogado = response.Data.Tables[0].Rows[0].IsNull(39) ? null! : response.Data.Tables[0].Rows[0].Field<string>(39),
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(64) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(64),
                turnado = response.Data.Tables[0].Rows[0].IsNull(65) ? false : response.Data.Tables[0].Rows[0].Field<bool>(65)

            };
        }

        public async Task<ResultTransaction> UpdateReactivarRepository(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, id),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.RESUELTO.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.REACTIVAR_UPDATE,
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

        #region Requerimiento

        public async Task<Requerimientos> GetByIdAllAsyncRepositoryRequerimientos(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Administrador.GetHashCode()),
                new ParameterPGsql("p_id_tipo_requerimiento", NpgsqlDbType.Integer, EnumTipoRequerimiento.NORMAL.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.REQUERIMIENTOS_GET_BY_ID,
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
                no_oficio = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                atendio = response.Data.Tables[0].Rows[0].IsNull(5) ? false : response.Data.Tables[0].Rows[0].Field<bool>(5),
                fecha_notificacion = response.Data.Tables[0].Rows[0].IsNull(6) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(6),
                fecha_requerimiento = response.Data.Tables[0].Rows[0].IsNull(7) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(7),
                fecha_vencimiento = response.Data.Tables[0].Rows[0].IsNull(8) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(8),
                fecha_atencion = response.Data.Tables[0].Rows[0].IsNull(9) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(9),
            };
        }
        
        public async Task<ResponseRequerimientoList> GetByIdAsyncRepositoryRequerimientos(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Administrador.GetHashCode()),
                new ParameterPGsql("p_id_tipo_requerimiento", NpgsqlDbType.Integer, EnumTipoRequerimiento.NORMAL.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.REQUERIMIENTOS_GET_BY_ID,
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
                no_oficio = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                atendio = response.Data.Tables[0].Rows[0].IsNull(5) ? null! : response.Data.Tables[0].Rows[0].Field<bool>(5),
                fecha_notificacion = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(6).ToString("yyyy-MM-dd"),
                fecha_oficio = response.Data.Tables[0].Rows[0].IsNull(7) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(7).ToString("yyyy-MM-dd"),
                fecha_vencimiento = response.Data.Tables[0].Rows[0].IsNull(8) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(8).ToString("yyyy-MM-dd"),
                fecha_atencion = response.Data.Tables[0].Rows[0].IsNull(9) || response.Data.Tables[0].Rows[0].Field<DateTime>(9) == DateTime.MinValue
                ? null
                : response.Data.Tables[0].Rows[0].Field<DateTime>(9).ToString("s")
            };
        }
        public async Task<int?> GetTablaRequerimientosCountAsyncRepository(int idConsulta
       )
        {
            ParameterPGsql[] parameters =
            {
              new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
              new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Administrador.GetHashCode()),
              new ParameterPGsql("p_id_tipo_requerimiento", NpgsqlDbType.Integer, EnumTipoRequerimiento.NORMAL.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.ConsultasByFiltersCountRequerimientos,
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
        public async Task<int?> GetTablaAlertaRequerimientosCountAsyncRepository(int idConsulta
           )
        {
            ParameterPGsql[] parameters =
            {
            new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
              new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Administrador.GetHashCode()),
              new ParameterPGsql("p_id_tipo_requerimiento", NpgsqlDbType.Integer, EnumTipoRequerimiento.NORMAL.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.ConsultasByFiltersCountAlertaRequerimientos,
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
        public async Task<List<ResponseRequerimientoList>> GetTablaRequerimientosAsyncRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("order_column", NpgsqlDbType.Varchar, ""),
                new ParameterPGsql("order_desc", NpgsqlDbType.Boolean, false),
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Administrador.GetHashCode()),
                new ParameterPGsql("p_id_tipo_requerimiento", NpgsqlDbType.Integer, EnumTipoRequerimiento.NORMAL.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_LISTADO_TABLA_REQUERIMIENTOS,
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

            List<ResponseRequerimientoList> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        id_rol = item.IsNull(1) ? 0 : item.Field<int>(1),
                        nombre_rol = item.IsNull(2) ? null! : item.Field<string>(2)!,
                        id_consulta = item.IsNull(3) ? 0 : item.Field<int>(3),
                        no_oficio = item.IsNull(4) ? null! : item.Field<string>(4),
                        atendio = item.IsNull(5) ? null! : item.Field<bool>(5),
                        fecha_notificacion = item.IsNull(6) ? null! : item.Field<DateTime>(6).ToString("yyyy-MM-dd"),
                        fecha_oficio = item.IsNull(7) ? null! : item.Field<DateTime>(7).ToString("yyyy-MM-dd"),
                        fecha_vencimiento = item.IsNull(8) ? null! : item.Field<DateTime>(8).ToString("yyyy-MM-dd"),
                        fecha_atencion = item.IsNull(9) ? null! : item.Field<DateTime>(9).ToString("yyyy-MM-dd"),
                    }
                );
            }

            return resultList;

        }

        public async Task<ResultTransaction> UpdateDescartarRequerimientosRepository(int idConsulta, int idSeccion, bool descartarUltimoRegistro)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_id_seccion", NpgsqlDbType.Integer, idSeccion),
                new ParameterPGsql("p_ultimo_registro", NpgsqlDbType.Boolean, descartarUltimoRegistro),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.EN_ESTUDIO.GetHashCode()),
                new ParameterPGsql("p_tipo_operacion", NpgsqlDbType.Integer, EnumTipoOperacion.DESCARTAR_ADMINISTRADOR_GLOCAL.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.UpdateDescartarRequerimientos,
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

        public async Task<ResultTransaction> UpdateModificarRequerimientoRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.EN_REPARACION.GetHashCode()),
                new ParameterPGsql("p_tipo_operacion", NpgsqlDbType.Integer, EnumTipoOperacion.MODIFICAR_ADMINISTRADOR_GLOBAL.GetHashCode()),
                new ParameterPGsql("p_id_seccion_modificar", NpgsqlDbType.Integer, EnumSeccion.REQUERIMIENTO.GetHashCode()),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.ModificarEstado,
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

        #region  Historico de Asuntos

        public async Task<int?> GetAllByFiltersCountAsyncRepository(
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
        )
        {


            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_no_asunto", NpgsqlDbType.Text, noAsunto),
                new ParameterPGsql("p_fecha_presentacion_desde", NpgsqlDbType.Date, fechaPresentacionDesde),
                new ParameterPGsql("p_fecha_presentacion_hasta", NpgsqlDbType.Date, fechaPresentacionHasta),
                new ParameterPGsql("p_fecha_vencimiento_desde", NpgsqlDbType.Date, fechaVencimientoDesde),
                new ParameterPGsql("p_fecha_vencimiento_hasta", NpgsqlDbType.Date, fechaVencimientoHasta),
                new ParameterPGsql("p_fecha_cssj_desde", NpgsqlDbType.Date, fechaCSSJDesde),
                new ParameterPGsql("p_fecha_cssj_hasta", NpgsqlDbType.Date, fechaCSSJHasta),
                new ParameterPGsql("p_rfc_promovente", NpgsqlDbType.Varchar, rfcPromovente),
                new ParameterPGsql("p_promovente", NpgsqlDbType.Text, promovente),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoAsunto is null || !TipoAsunto.Any()) ? DBNull.Value :  TipoAsunto.ToArray()),
                new ParameterPGsql("p_id_administracion", NpgsqlDbType.Integer, idAdministracion),
                new ParameterPGsql("p_id_subadministracion", NpgsqlDbType.Integer, idSubadministracion),
                new ParameterPGsql("p_id_abogado_asigno", NpgsqlDbType.Varchar, idAbogadoAsigno),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Array | NpgsqlDbType.Integer, (EstadoTarea is null || !EstadoTarea.Any()) ? DBNull.Value :  EstadoTarea.ToArray()),
                new ParameterPGsql("p_id_tipo_modalidad", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoModalidad is null || !TipoModalidad.Any()) ? DBNull.Value :  TipoModalidad.ToArray()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Array | NpgsqlDbType.Integer, (EstadoProcesal is null || !EstadoProcesal.Any()) ? DBNull.Value :  EstadoProcesal.ToArray()),
                new ParameterPGsql("p_id_unidad_administrativa_central", NpgsqlDbType.Integer, idUnidadAdmistrativaCentral),
                new ParameterPGsql("p_id_unidad_administrativa", NpgsqlDbType.Integer, idUnidadAdmistrativa),

            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.ConsultasByFiltersCountAdminGlobal,
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

        public async Task<List<ResponseConsultaByFiltersAdministrador>> GetAllByFiltersAsyncRepository(
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
             )
        {



            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_page_size", NpgsqlDbType.Integer, pageSize),
                new ParameterPGsql("p_page", NpgsqlDbType.Integer, page),
                new ParameterPGsql("p_no_asunto", NpgsqlDbType.Text, noAsunto),
                new ParameterPGsql("p_fecha_presentacion_desde", NpgsqlDbType.Date, fechaPresentacionDesde),
                new ParameterPGsql("p_fecha_presentacion_hasta", NpgsqlDbType.Date, fechaPresentacionHasta),
                new ParameterPGsql("p_fecha_vencimiento_desde", NpgsqlDbType.Date, fechaVencimientoDesde),
                new ParameterPGsql("p_fecha_vencimiento_hasta", NpgsqlDbType.Date, fechaVencimientoHasta),
                new ParameterPGsql("p_rfc_promovente", NpgsqlDbType.Varchar, rfcPromovente),
                new ParameterPGsql("p_promovente", NpgsqlDbType.Text, promovente),
               new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoAsunto is null || !TipoAsunto.Any()) ? DBNull.Value :  TipoAsunto.ToArray()),
                new ParameterPGsql("p_id_administracion", NpgsqlDbType.Integer, idAdministracion),
                new ParameterPGsql("p_id_subadministracion", NpgsqlDbType.Integer, idSubadministracion),
                new ParameterPGsql("p_id_abogado_asigno", NpgsqlDbType.Text, idAbogadoAsigno),
                new ParameterPGsql("p_id_tipo_modalidad", NpgsqlDbType.Array | NpgsqlDbType.Integer, (TipoModalidad is null || !TipoModalidad.Any()) ? DBNull.Value :  TipoModalidad.ToArray()),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Array | NpgsqlDbType.Integer, (EstadoTarea is null || !EstadoTarea.Any()) ? DBNull.Value :  EstadoTarea.ToArray()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Array | NpgsqlDbType.Integer, (EstadoProcesal is null || !EstadoProcesal.Any()) ? DBNull.Value :  EstadoProcesal.ToArray()),
                new ParameterPGsql("order_column", NpgsqlDbType.Varchar, orderByColumn),
                new ParameterPGsql("order_desc", NpgsqlDbType.Boolean, orderDesc),
                new ParameterPGsql("p_id_unidad_administrativa_central", NpgsqlDbType.Integer, idUnidadAdmistrativaCentral),
                new ParameterPGsql("p_id_unidad_administrativa", NpgsqlDbType.Integer, idUnidadAdmistrativa),
                new ParameterPGsql("p_fecha_cssj_desde", NpgsqlDbType.Date, fechaCSSJDesde),
                new ParameterPGsql("p_fecha_cssj_hasta", NpgsqlDbType.Date, fechaCSSJHasta),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_FILTERS_ADMIN_GLOBAL,
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

            List<ResponseConsultaByFiltersAdministrador> resultList = new();
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
                        remitido = item.IsNull(29) ? false : item.Field<bool>(29),
                        asignado = item.IsNull(30) ? false : item.Field<bool>(30),
                        idAbogado = item.IsNull(31) ? null! : item.Field<string>(32),
                        abogado = item.IsNull(32) ? null! : item.Field<string>(33),
                        fecha_CSSJ = item.IsNull(34) ? null! : item.Field<DateTime>(34).ToString("yyyy-MM-dd"),
                        idColorFechacssj = item.IsNull(35) ? 0 : item.Field<int>(35),
                        colorFechacssj = item.IsNull(36) ? null! : item.Field<string>(36)!,
                        idAlertaDG = item.IsNull(37) ? 0 : item.Field<int>(37),
                        alertaDG = item.IsNull(38) ? null! : item.Field<string>(38)!,
                        registroVence = item.IsNull(39) ? 0 : item.Field<int>(39),
                        idSemaforo = item.IsNull(40) ? 0 : item.Field<int>(40),
                        semaforo = item.IsNull(41) ? null! : item.Field<string>(41)!,

                        fechaFirmeza = item.IsNull(42) ? null! : item.Field<DateTime>(42).ToString("yyyy-MM-dd"),
                        idOrganoJurisdiccional = item.IsNull(43) ? 0 : item.Field<int>(43),
                        organoJurisdiccional = item.IsNull(44) ? null! : item.Field<string>(44)!,
                        fechaAsignacion = item.IsNull(45) ? null! : item.Field<DateTime>(45).ToString("yyyy-MM-dd"),
                        numeroJuicio = item.IsNull(46) ? null! : item.Field<string>(46)!,
                        noRecurso = item.IsNull(47) ? null! : item.Field<string>(47)!,
                        tiporecurso = item.IsNull(48) ? 0 : item.Field<int>(48),
                        recurso = item.IsNull(49) ? null! : item.Field<string>(49)!,
                        idPlazoCumplimentar = item.IsNull(50) ? 0 : item.Field<int>(50),
                        plazoCumplimentar = item.IsNull(51) ? null! : item.Field<string>(51)!,
                        oficioResolucion = item.IsNull(52) ? null! : item.Field<string>(52)!,
                        fechaOficioResolucion = item.IsNull(53) ? null! : item.Field<DateTime>(53).ToString("yyyy-MM-dd"),
                        horas = item.IsNull(54) ? 0 : item.Field<int>(54),
                        idUnidadAdministrativaSolicitaCump = item.IsNull(55) ? 0 : item.Field<int>(55),
                        UnidadAdministrativaSolicitaCump = item.IsNull(56) ? null! : item.Field<string>(56)!
                    }
                );
            }

            return resultList;
        }
        #endregion

        #region  Reasignar
        public async Task<List<Consulta>> GetListByIdsAsync(int[] ids)
        {
            ParameterPGsql[] parameters = { new ParameterPGsql("p_ids", NpgsqlDbType.Array | NpgsqlDbType.Integer, ids), };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_IDS_ADMINISTRADOR,
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

            List<Consulta> resultList = new();
            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(new()
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
                    fecha_vencimiento = response.Data.Tables[0].Rows[0].IsNull(15) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(15),
                    id_administracion_central = response.Data.Tables[0].Rows[0].IsNull(17) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(17),
                    administracion_central = response.Data.Tables[0].Rows[0].IsNull(18) ? null! : response.Data.Tables[0].Rows[0].Field<string>(18)!,
                    id_administracion = response.Data.Tables[0].Rows[0].IsNull(19) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(19),
                    Administracion = response.Data.Tables[0].Rows[0].IsNull(20) ? null! : response.Data.Tables[0].Rows[0].Field<string>(20)!,
                    id_Subadministracion = response.Data.Tables[0].Rows[0].IsNull(21) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(21),
                    Subadministracion = response.Data.Tables[0].Rows[0].IsNull(22) ? null! : response.Data.Tables[0].Rows[0].Field<string>(22)!,
                    id_estado_tarea = response.Data.Tables[0].Rows[0].IsNull(23) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(23),
                    estado_tarea = response.Data.Tables[0].Rows[0].IsNull(24) ? null! : response.Data.Tables[0].Rows[0].Field<string>(24)!,
                    id_estado_procesal = response.Data.Tables[0].Rows[0].IsNull(25) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(25),
                    estado_procesal = response.Data.Tables[0].Rows[0].IsNull(26) ? null! : response.Data.Tables[0].Rows[0].Field<string>(26)!,
                    numero_empleado = response.Data.Tables[0].Rows[0].IsNull(27) ? null! : response.Data.Tables[0].Rows[0].Field<string>(27),
                    fecha_turnado = response.Data.Tables[0].Rows[0].IsNull(28) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(28),
                    remitido = response.Data.Tables[0].Rows[0].IsNull(29) ? false : response.Data.Tables[0].Rows[0].Field<bool>(29),
                    activo = response.Data.Tables[0].Rows[0].IsNull(39) ? false : response.Data.Tables[0].Rows[0].Field<bool>(39),
                    domicilio_promovente = response.Data.Tables[0].Rows[0].IsNull(34) ? null! : response.Data.Tables[0].Rows[0].Field<string>(34)!,
                    domicilio_notificaciones = response.Data.Tables[0].Rows[0].IsNull(35) ? null! : response.Data.Tables[0].Rows[0].Field<string>(35)!,
                    id_tema = response.Data.Tables[0].Rows[0].IsNull(36) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(36),
                    tema = response.Data.Tables[0].Rows[0].IsNull(37) ? null! : response.Data.Tables[0].Rows[0].Field<string>(37)!,
                    monto = response.Data.Tables[0].Rows[0].IsNull(38) ? 0 : response.Data.Tables[0].Rows[0].Field<decimal>(38),
                    id_abogado = response.Data.Tables[0].Rows[0].IsNull(32) ? null! : response.Data.Tables[0].Rows[0].Field<string>(32)!,
                    abogado = response.Data.Tables[0].Rows[0].IsNull(32) ? null! : response.Data.Tables[0].Rows[0].Field<string>(33)!,
                    monto_determinado = response.Data.Tables[0].Rows[0].IsNull(40) ? false : response.Data.Tables[0].Rows[0].Field<bool>(40),
                });
            }

            return resultList;
        }

        public async Task<ResultTransaction> ReasignarAsync(List<Reasignar> listReasignacion)
        {
            var arrayObject = listReasignacion.Select(c =>
            $"{c.id_consulta},{c.rfc_funcionario_reasignador},{c.rfc_funcionario_reasignado},{c.rfc_funcionario_retirado},{c.id_unidad_administrativa_reasingador},{c.id_subadministracion_reasignador},{c.id_unidad_administrativa_reasignado},{c.id_subadministracion_reasignado},{c.id_estado_procesal_previo}").ToArray();
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_array", NpgsqlDbType.Array |NpgsqlDbType.Text , arrayObject),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Integer, EnumEstadoTarea.REASIGNADO.GetHashCode()),

            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.Reasignar,
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

        #region Emisión Resolución
        public async Task<List<ResponseResolucion>> GetByIdAllAsyncRepositoryResolucion(int id)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, id),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.RESOLUCION_GET_BY_ID,
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

            List<ResponseResolucion> resultList = new();
            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(new()
                {
                    id = response.Data.Tables[0].Rows[0].IsNull(0) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(0),
                    id_rol = response.Data.Tables[0].Rows[0].IsNull(1) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(1),
                    nombre_rol = response.Data.Tables[0].Rows[0].IsNull(2) ? null! : response.Data.Tables[0].Rows[0].Field<string>(2),
                    id_consulta = response.Data.Tables[0].Rows[0].IsNull(3) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(3),
                    no_oficio = response.Data.Tables[0].Rows[0].IsNull(4) ? null! : response.Data.Tables[0].Rows[0].Field<string>(4),
                    fecha_notificacion = response.Data.Tables[0].Rows[0].IsNull(5) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(5).ToString("yyyy-MM-dd"),
                    fecha_resolucion = response.Data.Tables[0].Rows[0].IsNull(6) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(6).ToString("yyyy-MM-dd"),
                    id_sentido = response.Data.Tables[0].Rows[0].IsNull(7) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(7),
                    sentido = response.Data.Tables[0].Rows[0].IsNull(8) ? null! : response.Data.Tables[0].Rows[0].Field<string>(8),
                    fecha_vencimiento = response.Data.Tables[0].Rows[0].IsNull(9) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(9).ToString("yyyy-MM-dd"),
                    concluido = response.Data.Tables[0].Rows[0].IsNull(10) ? false! : response.Data.Tables[0].Rows[0].Field<bool>(10),
                });
            }

            return resultList;


        }       

        public async Task<ResultTransaction> UpdateModificarResolucionRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.EN_REPARACION.GetHashCode()),
                new ParameterPGsql("p_tipo_operacion", NpgsqlDbType.Integer, EnumTipoOperacion.MODIFICAR_ADMINISTRADOR_GLOBAL.GetHashCode()),
                new ParameterPGsql("p_id_seccion_modificar", NpgsqlDbType.Integer, EnumSeccion.EMISIÓN_RESOLUCIÓN.GetHashCode()),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.ModificarEstado,
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

        public async Task<ResultTransaction> UpdateDescartarResolucionRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.EN_ESTUDIO.GetHashCode()),
                new ParameterPGsql("p_tipo_operacion", NpgsqlDbType.Integer, EnumTipoOperacion.DESCARTAR_ADMINISTRADOR_GLOCAL.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.UpdateDescartarResolucion,
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

        public async Task<List<ResponseResolucion>> GetTablaResolucionAsyncRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_idconsulta", NpgsqlDbType.Integer, idConsulta),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.RESOLUCION_GET_LIST,
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

            List<ResponseResolucion> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        id_rol = item.IsNull(1) ? 0 : item.Field<int>(1),
                        nombre_rol = item.IsNull(2) ? null! : item.Field<string>(2),
                        id_consulta = item.IsNull(3) ? 0 : item.Field<int>(3),
                        no_oficio = item.IsNull(4) ? null! : item.Field<string>(4),
                        fecha_notificacion = item.IsNull(5) ? null! : item.Field<DateTime>(5).ToString("yyyy-MM-dd"),
                        fecha_resolucion = item.IsNull(6) ? null! : item.Field<DateTime>(6).ToString("yyyy-MM-dd"),
                        id_sentido = item.IsNull(7) ? 0 : item.Field<int>(7),
                        sentido = item.IsNull(8) ? null! : item.Field<string>(8),
                        fecha_vencimiento = item.IsNull(9) ? null! : item.Field<DateTime>(9).ToString("yyyy-MM-dd"),
                    }
                );
            }

            return resultList;

        }

        #endregion

        #region Consulta

        public async Task<Consulta> GetByIdAsyncRepositoryAdministrador(int id)
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
                id_abogado = response.Data.Tables[0].Rows[0].IsNull(39) ? null! : response.Data.Tables[0].Rows[0].Field<string>(39),
                asignar = response.Data.Tables[0].Rows[0].IsNull(63) ? false : response.Data.Tables[0].Rows[0].Field<bool>(63),
                fecha_registro = response.Data.Tables[0].Rows[0].IsNull(64) ? new() : response.Data.Tables[0].Rows[0].Field<DateTime>(64),
                turnado = response.Data.Tables[0].Rows[0].IsNull(65) ? false : response.Data.Tables[0].Rows[0].Field<bool>(65),
                usuario_asigno = response.Data.Tables[0].Rows[0].IsNull(66) ? null! : response.Data.Tables[0].Rows[0].Field<string>(66)


            };
        }

        public async Task<ResultTransaction> UpdateAsyncRepositoryAdministradorImpuestosInternos(Consulta entity)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_registro", NpgsqlDbType.Integer, entity.id),
                new ParameterPGsql("p_promovente_es_contribuyente",NpgsqlDbType.Boolean,entity.promovente_es_contribuyente!),
                new ParameterPGsql("p_rfc_contribuyente",NpgsqlDbType.Varchar,entity.rfc_contribuyente!),
                new ParameterPGsql("p_contribuyente", NpgsqlDbType.Varchar, entity.contribuyente!),
                new ParameterPGsql("p_domicilio_promovente", NpgsqlDbType.Varchar, entity.domicilio_promovente!),
                new ParameterPGsql("p_domicilio_notificaciones", NpgsqlDbType.Varchar, entity.domicilio_notificaciones!),
                new ParameterPGsql("p_id_tema", NpgsqlDbType.Integer, entity.id_tema!),
                new ParameterPGsql("p_monto",NpgsqlDbType.Numeric,entity.monto),
                new ParameterPGsql("p_fecha_recepcion", NpgsqlDbType.Date, Convert.ToDateTime(entity.fecha_recepcion)!),
                new ParameterPGsql("p_despacho_autorizado", NpgsqlDbType.Varchar, entity.despacho_autorizado!),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Integer,entity.id_tipo_asunto),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Integer,EnumEstadoTarea.PENDIENTE_DE_ASIGNAR.GetHashCode()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.EN_ESTUDIO.GetHashCode()),
                new ParameterPGsql("p_usuario_asigno", NpgsqlDbType.Varchar, entity.usuario_asigno!),
                new ParameterPGsql("p_abogado", NpgsqlDbType.Varchar,entity.id_abogado),
                new ParameterPGsql("p_id_subadministracion", NpgsqlDbType.Integer,entity.id_Subadministracion),
                new ParameterPGsql("p_monto_determinado", NpgsqlDbType.Boolean,entity.monto_determinado),

            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_UPDATE_ADMINISTRADOR_IMPUESTOS_INTERNOS,
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

        public async Task<ResultTransaction> UpdateAsyncRepositoryAdministradorComercioExterior(Consulta entity)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_registro", NpgsqlDbType.Integer, entity.id),
                new ParameterPGsql("p_promovente_es_contribuyente",NpgsqlDbType.Boolean,entity.promovente_es_contribuyente!),
                new ParameterPGsql("p_rfc_contribuyente",NpgsqlDbType.Varchar,entity.rfc_contribuyente!),
                new ParameterPGsql("p_contribuyente", NpgsqlDbType.Varchar, entity.contribuyente!),
                new ParameterPGsql("p_fecha_presentacion",NpgsqlDbType.Date,Convert.ToDateTime(entity.fecha_presentacion)!),
                new ParameterPGsql("p_fecha_recepcion", NpgsqlDbType.Date, Convert.ToDateTime(entity.fecha_recepcion)!),
                new ParameterPGsql("p_id_tipo_asunto", NpgsqlDbType.Integer,entity.id_tipo_asunto),
                new ParameterPGsql("p_id_estado_tarea", NpgsqlDbType.Integer,EnumEstadoTarea.PENDIENTE_DE_ASIGNAR.GetHashCode()),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.EN_ESTUDIO.GetHashCode()),
                new ParameterPGsql("p_abogado", NpgsqlDbType.Varchar,entity.id_abogado),
                new ParameterPGsql("p_usuario_asigno", NpgsqlDbType.Varchar, entity.usuario_asigno!),
                new ParameterPGsql("p_id_subadministracion", NpgsqlDbType.Integer,entity.id_Subadministracion),

            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_UPDATE_ADMINISTRADOR,
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

        public async Task<List<ResponseConsultaList>> GetTablaConsultaRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id", NpgsqlDbType.Integer, idConsulta),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_GET_BY_ID_ADMINISTRADOR,
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

            List<ResponseConsultaList> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
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
                        solicita_requerimiento = response.Data.Tables[0].Rows[0].IsNull(42) ? null! : response.Data.Tables[0].Rows[0].Field<bool>(42),
                        concluido = response.Data.Tables[0].Rows[0].IsNull(43) ? false! : response.Data.Tables[0].Rows[0].Field<bool>(43),
                        fecha_firmeza = response.Data.Tables[0].Rows[0].IsNull(44) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(44).ToString("yyyy-MM-dd"),
                        idOrganoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(45) ? 0 : response.Data.Tables[0].Rows[0].Field<int>(45)!,
                        organoJurisdiccional = response.Data.Tables[0].Rows[0].IsNull(46) ? null! : response.Data.Tables[0].Rows[0].Field<string>(46)!,
                        fecha_asignacion = response.Data.Tables[0].Rows[0].IsNull(47) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(47).ToString("yyyy-MM-dd"),
                        numeroJuicio = response.Data.Tables[0].Rows[0].IsNull(48) ? null! : response.Data.Tables[0].Rows[0].Field<string>(48)!,
                        noRecurso = response.Data.Tables[0].Rows[0].IsNull(49) ? null! : response.Data.Tables[0].Rows[0].Field<string>(49),
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
                        asigno = response.Data.Tables[0].Rows[0].IsNull(63) ? false! : response.Data.Tables[0].Rows[0].Field<bool>(63),
                        fecha_registro = response.Data.Tables[0].Rows[0].IsNull(64) ? null! : response.Data.Tables[0].Rows[0].Field<DateTime>(64).ToString("yyyy-MM-dd"),
                        turnado = response.Data.Tables[0].Rows[0].IsNull(65) ? false! : response.Data.Tables[0].Rows[0].Field<bool>(65)
                    }
                );
            }

            return resultList;

        }

        #endregion

        #region Datos Generales

        public async Task<ResultTransaction> UpdateDescartarDatosGeneralesRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.EN_REPARACION.GetHashCode()),
                new ParameterPGsql("p_tipo_operacion", NpgsqlDbType.Integer, EnumTipoOperacion.DESCARTAR_ADMINISTRADOR_GLOCAL.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.DescartarDatosGenerales,
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

        public async Task<ResultTransaction> UpdateModificarAsuntoRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_id_estado_procesal", NpgsqlDbType.Integer, EnumEstadoProcesal.EN_REPARACION.GetHashCode()),
                new ParameterPGsql("p_tipo_operacion", NpgsqlDbType.Integer, EnumTipoOperacion.MODIFICAR_ADMINISTRADOR_GLOBAL.GetHashCode()),
                new ParameterPGsql("p_id_seccion_modificar", NpgsqlDbType.Integer, EnumSeccion.DATOS_GENERALES.GetHashCode()),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.ModificarEstado,
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

        #region Archivos
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
          bool? permanente,
          int? idDocumentoSeccion
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
                 new ParameterPGsql("p_iddocumentoseccion", NpgsqlDbType.Integer, idDocumentoSeccion),
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
                bool? permanente,
                int? idDocumentoSeccion

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
                 new ParameterPGsql("p_iddocumentoseccion", NpgsqlDbType.Integer, idDocumentoSeccion),
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
                        idDocumentoSeccion = item.IsNull(1) ? 0 : item.Field<int>(19),

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

        #endregion

        public async Task<int?> GetTablaSolicitudInformacionCountAsyncRepository(int idConsulta
          )
        {
            ParameterPGsql[] parameters =
            {
               new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
              new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Administrador.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTASCOUNT_SOLICITUDINFORMACION,
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

        public async Task<int?> GetTablaAlertaSolicitudInformacionCountAsyncRepository(int idConsulta
       )
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
              new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Administrador.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTAALERTACOUNT_SOLICITUDINFORMACION,
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

        public async Task<List<ResponseSolicitudInformacionList>> GetTablaSolicitudInformacionAsyncRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("order_column", NpgsqlDbType.Varchar, ""),
                new ParameterPGsql("order_desc", NpgsqlDbType.Boolean, false),
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Abogado.GetHashCode()),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_LISTADO_TABLA_SOLICITUD_INFORMACION,
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

            List<ResponseSolicitudInformacionList> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        idRol = item.IsNull(1) ? 0 : item.Field<int>(1),
                        nombreRol = item.IsNull(2) ? null! : item.Field<string>(2)!,
                        idConsulta = item.IsNull(3) ? 0 : item.Field<int>(3),
                        unidadEsInterna = item.IsNull(4) ? false : item.Field<bool>(4),
                        idUnidadAdministrativa = item.IsNull(5) ? 0 : item.Field<int>(5),
                        unidadAdministrativaExterna = item.IsNull(6) ? null! : item.Field<string>(6)!,
                        unidadAdministrativa = item.IsNull(7) ? null! : item.Field<string>(7)!,
                        noOficioSolicitud = item.IsNull(8) ? null! : item.Field<string>(8)!,
                        fechaOficioSolicitud = item.IsNull(9) ? null! : item.Field<DateTime>(9).ToString("yyyy-MM-dd"),
                        atendioSolicitud = item.IsNull(10) ? false : item.Field<bool>(10),
                        noOficioRespuesta = item.IsNull(11) ? null! : item.Field<string>(11)!,
                        fechaOficioRespuesta = item.IsNull(12) ? null! : item.Field<DateTime>(12).ToString("yyyy-MM-dd"),
                        fechaRecepcion = item.IsNull(13) ? null! : item.Field<DateTime>(13).ToString("yyyy-MM-dd"),
                    }
                );
            }

            return resultList;

        }


        public async Task<List<ResponseRequerimientoProdeconList>> GetTablaRequerimientosProdeconAsyncRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("order_column", NpgsqlDbType.Varchar, ""),
                new ParameterPGsql("order_desc", NpgsqlDbType.Boolean, false),
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Abogado.GetHashCode()),
                new ParameterPGsql("p_id_tipo_requerimiento", NpgsqlDbType.Integer, EnumTipoRequerimiento.PRODECON.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_LISTADO_TABLA_REQUERIMIENTOS_PRODECON,
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

            List<ResponseRequerimientoProdeconList> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        id_rol = item.IsNull(1) ? 0 : item.Field<int>(1),
                        nombre_rol = item.IsNull(2) ? null! : item.Field<string>(2)!,
                        id_consulta = item.IsNull(3) ? 0 : item.Field<int>(3),
                        no_oficio = item.IsNull(4) ? null! : item.Field<string>(4),
                        no_expediente = item.IsNull(4) ? null! : item.Field<string>(5),
                        accion = item.IsNull(5) ? false : item.Field<bool>(6),
                        fecha_ingreso = item.IsNull(6) ? null! : item.Field<DateTime>(7).ToString("yyyy-MM-dd"),
                        fecha_oficio = item.IsNull(7) ? null! : item.Field<DateTime>(8).ToString("yyyy-MM-dd"),
                        atencion = item.IsNull(4) ? null! : item.Field<string>(9),
                    }
                );
            }

            return resultList;

        }


        public async Task<int?> GetTablaRequerimientosProdeconCountAsyncRepository(int idConsulta
       )
        {
            ParameterPGsql[] parameters =
            {
               new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
              new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Abogado.GetHashCode()),
              new ParameterPGsql("p_id_tipo_requerimiento", NpgsqlDbType.Integer, EnumTipoRequerimiento.PRODECON.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.ConsultasByFiltersCountRequerimientos,
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

        #region Avisos Y Comunicados
        public async Task<int?> GetTablaAvisosYComunicadosCountAsyncRepository(int idConsulta
 )
        {
            ParameterPGsql[] parameters =
            {
              new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
              new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Administrador.GetHashCode())
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.AVISOS_Y_COMUNICADOS_COUNT,
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

        public async Task<List<ResponseAvisosComunicadosList>> GetTablaAvisosYComunicadosAsyncRepository(int idConsulta)
        {
            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("order_column", NpgsqlDbType.Varchar, ""),
                new ParameterPGsql("order_desc", NpgsqlDbType.Boolean, false),
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Administrador.GetHashCode()),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_LISTADO_TABLA_AVISOS_COMUNICADOS,
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

            List<ResponseAvisosComunicadosList> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        idRol = item.IsNull(1) ? 0 : item.Field<int>(1),
                        rol = item.IsNull(2) ? null! : item.Field<string>(2)!,
                        idConsulta = item.IsNull(3) ? 0 : item.Field<int>(3),
                        idTipoAviso = item.IsNull(4) ? 0 : item.Field<int>(4),
                        tipo_aviso = item.IsNull(5) ? null! : item.Field<string>(5)!,
                        folio = item.IsNull(6) ? null! : item.Field<string>(6)!,
                        tieneFolio = item.IsNull(7) ? false : item.Field<bool>(7),
                        fechaIngreso = item.IsNull(8) ? null! : item.Field<DateTime>(8).ToString("yyyy-MM-dd"),
                        observaciones = item.IsNull(9) ? null! : item.Field<string>(9)!,
                        atencionAdicional = item.IsNull(10) ? false : item.Field<bool>(10),
                        descripcionAtencion = item.IsNull(11) ? null! : item.Field<string>(11)!,
                        fechaRegistro = item.IsNull(12) ? null! : item.Field<DateTime>(12).ToString("yyyy-MM-dd"),
                    }
                );
            }

            return resultList;

        }

        public async Task<int?> GetAlertaAvisosYComunicadosCountAsyncRepository(int idConsulta
       )
        {
            ParameterPGsql[] parameters =
            {
            new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
              new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, EnumRol.Administrador.GetHashCode()),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.AVISOS_Y_COMUNICADOS_COUNT_ALERTA,
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

        #endregion

        #region Solicitud de transparencia
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
        #endregion

        #region  Personas Autorizadas
        public async Task<List<ResponseTablaPersonasAutorizadas>> GetTablaPersonasAutorizadasAsyncRepository(
         int pageSize,
         int page,
         int idConsulta
     )
        {

            PersonasAutorizadas entity = EventsConsultasAdministradorGlobal.GetPersonasAutorizadasAdministrador();

            ParameterPGsql[] parameters =
            {
                new ParameterPGsql("p_page_size", NpgsqlDbType.Integer, pageSize),
                new ParameterPGsql("p_page", NpgsqlDbType.Integer, page),
                new ParameterPGsql("order_column", NpgsqlDbType.Varchar, ""),
                new ParameterPGsql("order_desc", NpgsqlDbType.Boolean, false),
                new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
                new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, entity.id_rol),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.CONSULTA_LISTADO_TABLA_PERSONAS_AUTORIZADAS,
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

            List<ResponseTablaPersonasAutorizadas> resultList = new();

            foreach (DataRow item in response.Data.Tables[0].Rows)
            {
                resultList.Add(
                    new()
                    {
                        id = item.IsNull(0) ? 0 : item.Field<int>(0),
                        idRol = item.IsNull(1) ? 0 : item.Field<int>(1),
                        rol = item.IsNull(2) ? null! : item.Field<string>(2)!,
                        usuario = item.IsNull(3) ? null! : item.Field<string>(3),
                        nombre = item.IsNull(4) ? null! : item.Field<string>(4),
                        rfc = item.IsNull(5) ? null! : item.Field<string>(5),
                        telefono = item.IsNull(6) ? null! : item.Field<string>(6),
                        email = item.IsNull(7) ? null! : item.Field<string>(7),
                        fechaRegistro = item.IsNull(9) ? null! : item.Field<DateTime>(9).ToString("yyyy-MM-dd"),
                        fechaModificacion = item.IsNull(10) ? null! : item.Field<DateTime>(10).ToString("yyyy-MM-dd"),


                    }
                );
            }

            return resultList;

        }
        public async Task<int?> GetTablaPersonasAutorizadasCountAsyncRepository(int idConsulta

       )
        {
            PersonasAutorizadas entity = EventsConsultasAdministradorGlobal.GetPersonasAutorizadasAdministrador();

            ParameterPGsql[] parameters =
            {
              new ParameterPGsql("p_id_consulta", NpgsqlDbType.Integer, idConsulta),
              new ParameterPGsql("p_id_rol", NpgsqlDbType.Integer, entity.id_rol),
            };

            var response = await _database.ExecuteFunctionAsync(
                EnunFunctions.ConsultasByFiltersCountPersonasAutorizadas,
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
