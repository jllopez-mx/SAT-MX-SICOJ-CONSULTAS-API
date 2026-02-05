using ConsultasAPI.Model.IDAO.IRepository;
using ConsultasAPI.Model.IDAO.IServiceDAO;
using ConsultasAPI.Model.ViewModels.Enums;
using ConsultasAPI.Model.Entities;
using Sicoj.Utils.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sicoj.Utils.Models;
using ConsultasAPI.Model.DTO.Response.Administrador;
using Sicoj.Utils.ViewModels;
using Sicoj.Utils.Enums;
using Sicoj.Utils.Middleware;
using ConsultasAPI.Model.Entities.Events.AdministradorGlobal;
using ConsultasAPI.Model.DTO;
using ConsultasAPI.Model.DTO.CatalogsContracts;
using Sicoj.Utils.Redis;
using Microsoft.Extensions.Options;

namespace ConsultasAPI.Model.DAO.ServicesDAO
{
    public class ConsultasAdministradorGlobalService : IConsultasAdministradorGlobalService
    {
        #region variables
        private readonly IConsultaRepositoryAdministradorGlobal _repositoryAdministradorGlobal;
        private readonly IApiService _apiService;
        private readonly IRedisClient _redisClient;
        private readonly ProxyEnpoints _proxyEnpoints;
        private readonly string _routeInfoUsuario = null!;

        public ConsultasAdministradorGlobalService(IConfiguration configuration, IConsultaRepositoryAdministradorGlobal repositoryAdministradorGlobal, IApiService apiService, IRedisClient redisClient, IOptions<ProxyEnpoints> proxyEnpoints)
        {
            _repositoryAdministradorGlobal = repositoryAdministradorGlobal ?? throw new ArgumentNullException(nameof(repositoryAdministradorGlobal));
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            // _routeTipoAsunto = configuration.GetValue<string>("CatalogsEndpoints:RouteTipoAsunto")!;
            // _routeTipoModalidad = configuration.GetValue<string>("CatalogsEndpoints:RouteTipoModalidad")!;
            // _routeEstadoTarea = configuration.GetValue<string>("CatalogsEndpoints:RouteEstadoTarea")!;
            // _routeEstadoProcesal = configuration.GetValue<string>("CatalogsEndpoints:RouteEstadoProcesal")!;
            // _routeUnidadAdministrativa = configuration.GetValue<string>("CatalogsEndpoints:RouteAdministracion")!;
            // _routeSubadministracion = configuration.GetValue<string>("CatalogsEndpoints:RouteAdministracionCentral")!;
            // _routeUsuarioInfoDetalle = configuration.GetValue<string>("CatalogsEndpoints:RouteUsuarioInfoDetalle")!;
            _routeInfoUsuario = configuration.GetValue<string>("CatalogsEndpoints:RouteInfoUsuario")!;
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(redisClient));
            _proxyEnpoints = proxyEnpoints.Value ?? throw new ArgumentNullException(nameof(proxyEnpoints));
        }

        #endregion

        #region Reactivar

        public async Task<Consulta> GetByIdConsultaService(int id) =>
       await _repositoryAdministradorGlobal.GetByIdConsultaRepository(id);

        public async Task<ResultOperation> UpdateReactivarService(int id)
        {
            try
            {
                var consultaDetails = await _repositoryAdministradorGlobal.GetByIdConsultaRepository(id);
                if (consultaDetails is null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                if (consultaDetails.id_estado_procesal == EnumEstadoProcesal.CONCLUIDO_REMITIDO.GetHashCode() || consultaDetails.id_estado_procesal == EnumEstadoProcesal.CONCLUIDO_NOTIFICADO.GetHashCode())
                {
                    var response = await _apiService.GetResultOperationAsync<UserInformationView>($"{_routeInfoUsuario}/{consultaDetails.id_abogado}");
                    if (response is not null && response.Success && response.Result is not null)
                    {
                        var result = await _repositoryAdministradorGlobal.UpdateReactivarRepository(id);
                        if (!result.Success)
                        {
                            return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");
                        }

                        //return ResultOperation.SuccessResponseNoMessage(result.Result, "El asunto " + consultaDetails.no_asunto + " se reactivó correctamente");
                        return ResultOperation.SuccessResponseNoMessage(result.Result);
                    }
                    else
                    {
                        return ResultOperation<int?>.FailureWarningResponse("El abogado no está activo, para poder reactivar el asunto debe reasignarlo a un abogado activo");
                    }
                }
                else
                {
                    return ResultOperation<int?>.FailureWarningResponse("No se puede reactivar este asunto");
                }
            }
            catch (Exception ex)
            {
                return ResultOperation<int?>.FailureErrorResponse($"Ocurrió un error: {ex.Message}");
            }
        }

        #endregion

        #region Requerimiento
        public async Task<Requerimientos> GetByAllServiceRequerimientos(int id) =>
        await _repositoryAdministradorGlobal.GetByIdAllAsyncRepositoryRequerimientos(id);

        // public async Task<ResultOperation> UpdateRequerimientos(Requerimientos entity, ArchivoConsulta entityDocumento, DataFile dataFile)
        // {
        //     try
        //     {
        //         var consultaDetails = await _repositoryAdministradorGlobal.GetByIdDisconnected(entity.id_consulta);
        //         if (consultaDetails == null)
        //         {
        //             return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
        //         }

        //         if (entity.fecha_requerimiento.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
        //         {
        //             return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de requerimiento no puede ser menor a la fecha de presentación en el SAT.");
        //         }

        //         var result = await _repositoryAdministradorGlobal.UpdateAsyncRequerimientos(entity, entityDocumento, dataFile);
        //         if (!result.Success)
        //             return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

        //         return ResultOperation.SuccessResponseNoMessage(result.Result, "El registro se ha actualizado exitosamente");
        //     }
        //     catch (Exception)
        //     {
        //         throw;
        //     }

        // }
        public async Task<ResultOperation> GetRequerimientoById(int id)
        {
            try
            {
                var result = await _repositoryAdministradorGlobal.GetByIdAsyncRepositoryRequerimientos(id);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> GetTablaRequerimientosService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var consultaDetails = await _repositoryAdministradorGlobal.GetByIdDisconnected(idConsulta);
                if (consultaDetails.solicita_requerimiento == false)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                       null!)
                   );
                }


                var countResult = await _repositoryAdministradorGlobal.GetTablaRequerimientosCountAsyncRepository(idConsulta);



                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositoryAdministradorGlobal.GetTablaAlertaRequerimientosCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }
                var result = await _repositoryAdministradorGlobal.GetTablaRequerimientosAsyncRepository(idConsulta);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                    null!)
                );
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                       result)
                   );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> UpdateDescartarRequerimientosService(int idConsulta, int idSeccion, bool descartarUltimoRegistro)
        {
            try
            {
                var consultaDetails = await _repositoryAdministradorGlobal.GetByIdConsultaRepository(idConsulta);
                if (consultaDetails is null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                var result = await _repositoryAdministradorGlobal.UpdateDescartarRequerimientosRepository(idConsulta, idSeccion, descartarUltimoRegistro);
                if (!result.Success)
                {
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");
                }

                if (descartarUltimoRegistro == false)
                {
                    //return ResultOperation.SuccessResponseNoMessage(result.Result, "La sección Requerimiento del asunto" + consultaDetails.no_asunto + " se descartó correctamente");
                    return ResultOperation.SuccessResponseNoMessage(result.Result);
                }
                else
                {

                    //return ResultOperation.SuccessResponseNoMessage(result.Result, "El ultimó registro del asunto" + consultaDetails.no_asunto + " se descartó correctamente");
                    return ResultOperation.SuccessResponseNoMessage(result.Result);
                }

            }
            catch (Exception ex)
            {
                return ResultOperation<int?>.FailureErrorResponse($"Ocurrió un error: {ex.Message}");
            }
        }

        public async Task<ResultOperation> UpdateModificarRequerimientoService(int idConsulta)
        {
            try
            {
                var consultaDetails = await _repositoryAdministradorGlobal.GetByIdConsultaRepository(idConsulta);
                if (consultaDetails is null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                var result = await _repositoryAdministradorGlobal.UpdateModificarRequerimientoRepository(idConsulta);
                if (!result.Success)
                {
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");
                }

                //return ResultOperation.SuccessResponseNoMessage(result.Result, "El asunto " + consultaDetails.no_asunto + " se encuentra listo para modificarse");
                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception ex)
            {
                return ResultOperation<int?>.FailureErrorResponse($"Ocurrió un error: {ex.Message}");
            }
        }

        #endregion

        #region Historico de Asuntos
        public async Task<ResultOperation> GetBandejaHistoricoByFiltersService
       (
           int Fetch,
           int Page,
           string?
           OrderByColumn,
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
       )
        {
            try
            {
                var countResult = await _repositoryAdministradorGlobal.GetAllByFiltersCountAsyncRepository(
                        noAsunto,
                        fechaPresentacionDesde,
                        fechaPresentacionHasta,
                        fechaVencimientoDesde,
                        fechaVencimientoHasta,
                        fechaCSSJDesde,
                        fechaCSSJHasta,
                        rfcPromovente,
                        promovente,
                        TipoAsunto,
                        idAdministracion,
                        idSubadministracion,
                        idAbogadoAsigno,
                        EstadoTarea,
                        TipoModalidad,
                        EstadoProcesal,
                        userInformationView.IdAdministracionCentral,
                        userInformationView.IdAdministracion
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseConsultaByFiltersAdministrador>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }

                var result = await _repositoryAdministradorGlobal.GetAllByFiltersAsyncRepository(
                    Fetch,
                    Page,
                    OrderByColumn,
                    OrderDesc,
                    noAsunto,
                    fechaPresentacionDesde,
                    fechaPresentacionHasta,
                    fechaVencimientoDesde,
                    fechaVencimientoHasta,
                    fechaCSSJDesde,
                    fechaCSSJHasta,
                    rfcPromovente,
                    promovente,
                    TipoAsunto,
                    idAdministracion,
                    idSubadministracion,
                    idAbogadoAsigno,
                    EstadoTarea,
                    TipoModalidad,
                    EstadoProcesal,
                    userInformationView.IdAdministracionCentral,
                    userInformationView.IdAdministracion
                );

                if (result is null)
                {
                    return ResultOperation<List<ResponseConsultaByFiltersAdministrador>>.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableView<ResponseConsultaByFiltersAdministrador>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region Reasignar
        public async Task<ResultOperation<ResponseReasignar>> ReAsignarComercioExteriorAsync(int[] idList, string rfcAbogado, UserInformationView userInformationView)
        {
            var responseAbogado = await _apiService.GetResultOperationAsync<UserInformationView>($"{_routeInfoUsuario}/{rfcAbogado}");
            if (responseAbogado is null || !responseAbogado.Success || responseAbogado.Result is null)
            {
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"No se pudo realizar la validación para verificar que el abogado seleccionado pertenezca a la administracion/subadministración.");
            }

            List<Consulta> entityList = await _repositoryAdministradorGlobal.GetListByIdsAsync(idList);
            if (entityList is null || !entityList.Any())
            {
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"No existen las consultas.");
            }

            var resultOperation = ResultOperation.SuccessResponseNoMessage(new ResponseReasignar());

            List<int> listInvalidos = new List<int>();
            List<Reasignar> listReasignacion = new();
            foreach (var item in idList)
            {
                var entity = entityList.FirstOrDefault(c => c.id == item && c.activo);
                if (entity is null)
                {
                    resultOperation.AddWarningMessage($"La consulta con el identificador {item} no existe o se eliminó.");
                    listInvalidos.Add(item);
                    continue;
                }

                var entityabogado = entityList.FirstOrDefault(c => c.id == item && c.id_abogado == rfcAbogado);
                if (entityabogado is not null)
                {
                    resultOperation.AddWarningMessage($"La consulta con el identificador {item} ya tiene asignado este abogado.");
                    listInvalidos.Add(item);
                    continue;
                }

                try
                {
                    Reasignar entityReasignar = EventsConsultasAdministradorGlobal.UpdateReasignar(ref entity,
                        userInformationView.Rfc,
                        responseAbogado.Result.Rfc,
                        entity.id_abogado,
                        userInformationView.IdAdministracion,
                        userInformationView.IdSubadministracion,
                        responseAbogado.Result.IdAdministracion,
                        responseAbogado.Result.IdSubadministracion);

                    if (!UserSession.ValidateUser(responseAbogado!.Result!, EnumRolesSicoj.ABOGADO, EnumModulosSicoj.CONSULTAS, out string message, false, null!, true, entity.id_administracion_central, true, entity.id_administracion, true, entityReasignar.id_subadministracion_reasignado))
                    {
                        resultOperation.AddWarningMessage($"La consulta de Comercio Exterior con el número de asunto {entity.no_asunto}: {message}");
                    }

                    listReasignacion.Add(entityReasignar);
                }
                catch (Exception _e)
                {
                    resultOperation.AddWarningMessage($"La consulta de Comercio Exterior con el número de asunto {entity.no_asunto}: {_e.Message}");
                    listInvalidos.Add(item);
                    continue;
                }
            }

            resultOperation.Result.ReasignacionesExitosas = listReasignacion.Count;
            resultOperation.Result.ReasignacionesIncorrectas = listInvalidos.Count;
            resultOperation.Result.abogado = responseAbogado.Result.Nombre!;

            if (!listReasignacion.Any())
            {
                return resultOperation;
            }

            var result = await _repositoryAdministradorGlobal.ReasignarAsync(listReasignacion);
            if (!result.Success)
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"{result.MsgError!}:{result.DetailError}");

            return resultOperation;
        }

        public async Task<ResultOperation<ResponseReasignar>> ReAsignarImpuestosInternosAsync(int[] idList, string rfcAbogado, UserInformationView userInformationView)
        {

            var responseAbogado = await _apiService.GetResultOperationAsync<UserInformationView>($"{_routeInfoUsuario}/{rfcAbogado}");
            if (responseAbogado is null || !responseAbogado.Success || responseAbogado.Result is null)
            {
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"No se pudo realizar la validación para verificar que el abogado seleccionado pertenezca a la administracion/subadministración.");
            }

            List<Consulta> entityList = await _repositoryAdministradorGlobal.GetListByIdsAsync(idList);
            if (entityList is null || !entityList.Any())
            {
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"No existen las consultas.");
            }

            var resultOperation = ResultOperation.SuccessResponseNoMessage(new ResponseReasignar());

            List<int> listInvalidos = new List<int>();
            List<Reasignar> listReasignacion = new();
            foreach (var item in idList)
            {
                var entity = entityList.FirstOrDefault(c => c.id == item && c.activo);
                if (entity is null)
                {
                    resultOperation.AddWarningMessage($"La consulta con el identificador {item} no existe o se eliminó.");
                    listInvalidos.Add(item);
                    continue;
                }

                try
                {

                    Reasignar entityReasignar = EventsConsultasAdministradorGlobal.UpdateReasignar(ref entity,
                        userInformationView.Rfc,
                        responseAbogado.Result.Rfc,
                        entity.id_abogado,
                        userInformationView.IdAdministracion,
                        userInformationView.IdSubadministracion,
                        responseAbogado.Result.IdAdministracion,
                        responseAbogado.Result.IdSubadministracion
                        );

                    if (!UserSession.ValidateUser(responseAbogado!.Result!, EnumRolesSicoj.ABOGADO, EnumModulosSicoj.CONSULTAS, out string message, false, null!, true, entity.id_administracion_central, true, entity.id_administracion, true, entityReasignar.id_subadministracion_reasignado))
                    {
                        resultOperation.AddWarningMessage($"La consulta de Impuestos Internos con el número de asunto {entity.no_asunto}: {message}");
                    }

                    listReasignacion.Add(entityReasignar);
                }
                catch (Exception _e)
                {
                    resultOperation.AddWarningMessage($"La consulta de Impuestos Internos con el número de asunto {entity.no_asunto}: {_e.Message}");
                    listInvalidos.Add(item);
                    continue;
                }
            }

            resultOperation.Result.ReasignacionesExitosas = listReasignacion.Count;
            resultOperation.Result.ReasignacionesIncorrectas = listInvalidos.Count;
            resultOperation.Result.abogado = responseAbogado.Result.Nombre!;

            if (!listReasignacion.Any())
            {
                return resultOperation;
            }

            var result = await _repositoryAdministradorGlobal.ReasignarAsync(listReasignacion);
            if (!result.Success)
                return ResultOperation.FailureErrorResponse<ResponseReasignar>($"{result.MsgError!}:{result.DetailError}");

            return resultOperation;
        }
        #endregion

        #region Emisión  Resolución

        public async Task<ResultOperation> GetResolucionById(int id)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                //var consultaDetails = await _repositoryAdministradorGlobal.GetByIdDisconnected(idConsulta);


                var result = await _repositoryAdministradorGlobal.GetByIdAllAsyncRepositoryResolucion(id);
                if (result is not null && result.Any())
                {
                    var resolucion = result.First();
                    if (resolucion.concluido == true)
                    {
                        return ResultOperation.SuccessResponseNoMessage(
                            new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                            result)
                        );
                    }
                    else
                    {
                        idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                        alerta = EnumAlertaSeccion.EN_PROCESO.ToString();

                    }



                    if (result is null)
                    {
                        idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                        alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                        return ResultOperation.SuccessResponseNoMessage(
                            new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                            null!)
                        );
                    }
                }
                else
                {
                    return ResultOperation.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                    result)
                );

            }
            catch (Exception)
            {
                throw;
            }
        }

        // public async Task<ResultOperation> UpdateResolucion(Resolucion entity)
        // {
        //     try
        //     {
        //         var consultaDetails = await _repositoryAdministradorGlobal.GetByIdDisconnected(entity.id_consulta);
        //         if (consultaDetails == null)
        //         {
        //             return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
        //         }

        //         if (entity.fecha_resolucion.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
        //         {
        //             return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha del oficio de resolucion no puede ser menor a la fecha de presentación en el SAT.");
        //         }

        //         if (entity.fecha_notificacion.Date < entity.fecha_resolucion.Date || entity.fecha_notificacion.Date < Convert.ToDateTime(consultaDetails.fecha_presentacion))
        //         {
        //             return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de notificación del oficio del requerimiento no puede ser menor a la fecha del oficio del requerimiento.");
        //         }

        //         var result = await _repositoryAdministradorGlobal.UpdateAsyncResolucion(entity);
        //         if (!result.Success)
        //             return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

        //         return ResultOperation.SuccessResponseNoMessage(result.Result, "El registro se ha actualizado exitosamente");
        //     }
        //     catch (Exception)
        //     {
        //         throw;
        //     }

        // }


        public async Task<ResultOperation> UpdateModificarResoluciuonService(int idConsulta)
        {
            try
            {
                var consultaDetails = await _repositoryAdministradorGlobal.GetByIdConsultaRepository(idConsulta);
                if (consultaDetails is null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                var result = await _repositoryAdministradorGlobal.UpdateModificarResolucionRepository(idConsulta);
                if (!result.Success)
                {
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");
                }

                //return ResultOperation.SuccessResponseNoMessage(result.Result, "El asunto " + consultaDetails.no_asunto + " se encuentra listo para modificarse");
                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception ex)
            {
                return ResultOperation<int?>.FailureErrorResponse($"Ocurrió un error: {ex.Message}");
            }
        }

        public async Task<ResultOperation> UpdateDescartarResolucionService(int idConsulta)
        {
            try
            {
                var consultaDetails = await _repositoryAdministradorGlobal.GetByIdConsultaRepository(idConsulta);
                if (consultaDetails is null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                var result = await _repositoryAdministradorGlobal.UpdateDescartarResolucionRepository(idConsulta);
                if (!result.Success)
                {
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");
                }

                //return ResultOperation.SuccessResponseNoMessage(result.Result, "La sección Resolucion del asunto" + consultaDetails.no_asunto + " se descartó correctamente");
                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception ex)
            {
                return ResultOperation<int?>.FailureErrorResponse($"Ocurrió un error: {ex.Message}");
            }
        }

        public async Task<ResultOperation> GetTablaResolucionService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var consultaDetails = await _repositoryAdministradorGlobal.GetByIdDisconnected(idConsulta);

                var result = await _repositoryAdministradorGlobal.GetTablaResolucionAsyncRepository(idConsulta);
                if (consultaDetails is not null)
                {
                    if (consultaDetails.concluido == true)
                    {
                        return ResultOperation.SuccessResponseNoMessage(
                            new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                            result)
                        );
                    }
                    else
                    {
                        idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                        alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                    }

                    if (result is null)
                    {
                        idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                        alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                        return ResultOperation.SuccessResponseNoMessage(
                            new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                            null!)
                        );
                    }
                }
                else
                {
                    return ResultOperation.SuccessResponseNoMessage("No se encontraron resultados");
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseResolucion>(new(idAlerta, alerta),
                       result)
                   );
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Consulta

        public async Task<ResultOperation> GetByIdDisconnected(int id)
        {
            try
            {
                int idAlerta = EnumAlertas.GRIS.GetHashCode();
                string alerta = EnumAlertas.GRIS.ToString();

                var result = await _repositoryAdministradorGlobal.GetTablaConsultaRepository(id);
                if (result is null)
                {
                    return ResultOperation.FailureWarningResponse<ResponseConsultaList>("No se encontraron resultados.");
                }
                _redisClient.ValidateTakeList(ref result, EnumModulosRedis.CONSULTAS);
                var resultOperation = ResultOperation.SuccessResponseNoMessage(result);

                foreach (var item in result)
                {
                    if (item.fecha_CSSJ != null)
                    {
                        int diasHabiles = 0;

                        for (DateTime fecha = Convert.ToDateTime(item.fecha_CSSJ); fecha <= DateTime.Now; fecha = fecha.AddDays(1))
                        {
                            if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                            {
                                diasHabiles++;
                            }
                        }

                        if (diasHabiles <= 2)
                        {
                            idAlerta = EnumAlertas.VERDE.GetHashCode();
                            alerta = EnumAlertas.VERDE.ToString();
                        }
                        else
                        {
                            idAlerta = EnumAlertas.ROJO.GetHashCode();
                            alerta = EnumAlertas.ROJO.ToString();
                        }
                    }

                    if (item.idTipoAsunto > 0)
                    {
                        var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.TipoAsuntoConsulta, item.idTipoAsunto.ToString());
                        if (!string.IsNullOrEmpty(catalogValue))
                        {
                            item.tipoAsunto = catalogValue;
                        }
                        else
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre del tipo asunto.");

                    }

                    if (item.idTipoModalidad > 0)
                    {

                        var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.TipoEntradaConsulta, item.idTipoModalidad.ToString());
                        if (!string.IsNullOrEmpty(catalogValue))
                        {
                            item.tipoModalidad = catalogValue;
                        }
                        else
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre del tipo de modalidad.");
                    }

                    if (item.idAdministracion > 0)
                    {

                        var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.Administracion, item.idAdministracion.ToString());
                        if (!string.IsNullOrEmpty(catalogValue))
                        {
                            item.administracion = catalogValue;
                        }
                        else
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la unidad administrativa/subadminsitración.");

                    }

                    // if (item.idSubadministracion > 0)
                    // {
                    //     var response = await _apiService.GetResultOperationAsync<CatalogSicoj>($"{_routeSubadministracion}/{item.idSubadministracion}");
                    //     if (response is not null && response.Success && response.Result is not null)
                    //     {
                    //         item.subadministracion = response.Result.nombre!;
                    //     }
                    //     else
                    //     {
                    //         resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la unidad administrativa/subadministración.");
                    //     }
                    // }

                    if (item.idEstadoProcesal > 0)
                    {
                        var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.EstadoProcesalConsulta, item.idEstadoProcesal.ToString());
                        if (!string.IsNullOrEmpty(catalogValue))
                        {
                            item.estadoProcesal = catalogValue;
                        }
                        else
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre del estado procesal.");

                    }

                    if (item.idEstadoTarea > 0)
                    {
                        var catalogValue = await _redisClient.GetCatalogValue(EnumCatalogos.TareaConsulta, item.idEstadoTarea.ToString());
                        if (!string.IsNullOrEmpty(catalogValue))
                        {
                            item.estadoTarea = catalogValue;
                        }
                        else
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre de la tarea.");
                    }

                    if (item is not null && !string.IsNullOrEmpty(item.idAbogado) && !item.remitido)
                    {
                        var response = await _apiService.GetResultOperationAsync<UserInformationView>($"{_routeInfoUsuario}/{item.idAbogado}");
                        if (response is not null && response.Success && response.Result is not null)
                        {
                            item.idAbogado = response.Result.Rfc;
                            item.abogado = response.Result.Nombre;
                            item.abogadoActivo = true;
                        }
                        else
                        {
                            resultOperation.AddWarningMessage("No se pudo recuperar el nombre del abogado.");
                            item.abogadoActivo = false;
                        }
                    }
                }

                return ResultOperation.SuccessResponse(
                   new DataTableViewAlerta<ResponseConsultaList>(new(idAlerta, alerta), result),
                   string.Join("\n", resultOperation.Messages.Select(m => m.detailMessage))
                   );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Consulta> GetByIdServiceAdministrador(int id) =>
       await _repositoryAdministradorGlobal.GetByIdAsyncRepositoryAdministrador(id);

        public async Task<ResultOperation> UpdateConsultaServiceAdministradorImpuestosInternos(Consulta entity)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                if (entity.fecha_presentacion.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de presentación no puede ser mayor a la fecha actual.");
                }

                if (entity.fecha_recepcion.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de presentación no puede ser mayor a la fecha actual.");
                }

                if (entity.fecha_recepcion.Date < entity.fecha_recepcion.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse("La fecha de presentación no puede ser menor a la fecha de recepción.");
                }


                var result = await _repositoryAdministradorGlobal.UpdateAsyncRepositoryAdministradorImpuestosInternos(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> UpdateConsultaServiceAdministradorComercioExterior(Consulta entity)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                if (entity.fecha_presentacion.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de presentación no puede ser mayor a la fecha actual.");
                }

                if (entity.fecha_recepcion.Date > dateTime.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse<int?>("La fecha de presentación no puede ser mayor a la fecha actual.");
                }

                if (entity.fecha_recepcion.Date < entity.fecha_recepcion.Date)
                {
                    return ResultOperation<int?>.FailureWarningResponse("La fecha de presentación no puede ser menor a la fecha de recepción.");
                }


                var result = await _repositoryAdministradorGlobal.UpdateAsyncRepositoryAdministradorComercioExterior(entity);
                if (!result.Success)
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Datos Generales

        public async Task<ResultOperation> UpdateDescartarDatosGeneralesService(int idConsulta)
        {
            try
            {
                var consultaDetails = await _repositoryAdministradorGlobal.GetByIdConsultaRepository(idConsulta);
                if (consultaDetails is null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                var result = await _repositoryAdministradorGlobal.UpdateDescartarDatosGeneralesRepository(idConsulta);
                if (!result.Success)
                {
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");
                }

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception ex)
            {
                return ResultOperation<int?>.FailureErrorResponse($"Ocurrió un error: {ex.Message}");
            }
        }

        public async Task<ResultOperation> UpdateModificarAsuntoService(int idConsulta)
        {
            try
            {
                var consultaDetails = await _repositoryAdministradorGlobal.GetByIdConsultaRepository(idConsulta);
                if (consultaDetails is null)
                {
                    return ResultOperation<int?>.FailureErrorResponse("No se pudo encontrar la consulta con el ID especificado.");
                }

                var result = await _repositoryAdministradorGlobal.UpdateModificarAsuntoRepository(idConsulta);
                if (!result.Success)
                {
                    return ResultOperation<int?>.FailureErrorResponse($"{result.MsgError!}:{result.DetailError}");
                }

                return ResultOperation.SuccessResponseNoMessage(result.Result);
            }
            catch (Exception ex)
            {
                return ResultOperation<int?>.FailureErrorResponse($"Ocurrió un error: {ex.Message}");
            }
        }

        #endregion

        #region Archivos

        public async Task<ResultOperation> GetArchivoByFiltersService
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
      )
        {
            try
            {
                var countResult = await _repositoryAdministradorGlobal.GetArchivosByFiltersCountAsyncRepository(
                        idRol,
                        folio,
                        idSeccion,
                        idConsulta,
                        idRemision,
                        estatus,
                        fechaCreacionDesde,
                        fechaCreacionHasta,
                        remplazable,
                        permanente,
                        idDocumentoSeccion
                    );

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseArchivosConsulta>>.SuccessResponseNoMessage("No hay elementos que mostrar");//.FailureWarningResponse("No se encontraron resultados"); ;
                }

                var result = await _repositoryAdministradorGlobal.GetArchivoByFiltersAsyncRepository(
                    Fetch,
                    Page,
                    OrderByColumn,
                    OrderDesc,
                    idRol,
                    folio,
                    idSeccion,
                    idConsulta,
                    idRemision,
                    estatus,
                    fechaCreacionDesde,
                    fechaCreacionHasta,
                    remplazable,
                    permanente,
                    idDocumentoSeccion
                );

                if (result is null)
                {
                    return ResultOperation<List<ResponseArchivosConsulta>>.SuccessResponseNoMessage("No hay elementos que mostrar");//.FailureWarningResponse("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableView<ResponseArchivosConsulta>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseArchivosConsulta> GetByIdArchivoService(int id) =>
        await _repositoryAdministradorGlobal.GetByIdArchivoAsyncRepository(id);

        #endregion


        public async Task<ResultOperation> GetTablaRequerimientosProdeconService(int idConsulta)
        {

            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var countResult = await _repositoryAdministradorGlobal.GetTablaRequerimientosProdeconCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseRequerimientoProdeconList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var result = await _repositoryAdministradorGlobal.GetTablaRequerimientosProdeconAsyncRepository(idConsulta);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseRequerimientoProdeconList>(new(idAlerta, alerta),
                    null!)
                );
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseRequerimientoProdeconList>(new(idAlerta, alerta),
                       result)
                   );
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ResultOperation> GetTablaSolicitudInformacionService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var countResult = await _repositoryAdministradorGlobal.GetTablaSolicitudInformacionCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseSolicitudInformacionList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositoryAdministradorGlobal.GetTablaAlertaSolicitudInformacionCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }
                var result = await _repositoryAdministradorGlobal.GetTablaSolicitudInformacionAsyncRepository(idConsulta);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseSolicitudInformacionList>(new(idAlerta, alerta),
                    null!)
                );
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseSolicitudInformacionList>(new(idAlerta, alerta),
                       result)
                   );
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region Avisos Y Comunicados
        public async Task<ResultOperation> GetTablaAvisosYComunicados(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();

                var countResult = await _repositoryAdministradorGlobal.GetTablaAvisosYComunicadosCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseAvisosComunicadosList>(new(idAlerta, alerta),
                        null!)
                    );
                }
                var countAlertaResult = await _repositoryAdministradorGlobal.GetAlertaAvisosYComunicadosCountAsyncRepository(idConsulta);

                if (countAlertaResult > 0)
                {
                    idAlerta = EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta = EnumAlertaSeccion.EN_PROCESO.ToString();
                }

                var result = await _repositoryAdministradorGlobal.GetTablaAvisosYComunicadosAsyncRepository(idConsulta);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseAvisosComunicadosList>(new(idAlerta, alerta),
                    null!)
                );
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseAvisosComunicadosList>(new(idAlerta, alerta),
                       result)
                   );
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Solicitud de transparencia
        public async Task<ResultOperation> GetByIdSolicitudTransparenciaServices(int id)
        {
            try
            {
                var result = await _repositoryAdministradorGlobal.GetByIdSolicitudTransparenciaRepositorys(id);
                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultOperation> GetTablaSolicitudTransparenciaService(int idConsulta)
        {
            try
            {
                int idAlerta = EnumAlertas.VERDE.GetHashCode();
                string alerta = EnumAlertas.VERDE.ToString();


                var countResult = await _repositoryAdministradorGlobal.GetTablaSolicitudTransparenciaCountRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    idAlerta = EnumAlertas.GRIS.GetHashCode();
                    alerta = EnumAlertas.GRIS.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseRequerimientoList>(new(idAlerta, alerta),
                        null!)
                    );
                }

                /*
                var countAlertaResult = await _repository.GetTablaAlertaSolicitudTransparenciaCountRepository(idConsulta);
                
                if ( countAlertaResult > 0)
                {
                    idAlerta=EnumAlertaSeccion.EN_PROCESO.GetHashCode();
                    alerta=EnumAlertaSeccion.EN_PROCESO.ToString();
                }
                */
                var result = await _repositoryAdministradorGlobal.GetTablaSolicitudTransparenciaRepository(idConsulta);

                if (result is null)
                {
                    return ResultOperation.SuccessResponseNoMessage(
                    new DataTableViewAlerta<ResponseSolicitudTransparenciaList>(new(idAlerta, alerta),
                    null!)
                );
                }

                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseSolicitudTransparenciaList>(new(idAlerta, alerta),
                       result)
                   );

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Personas Autorizadas
        public async Task<ResultOperation> GetTablaPersonasAutorizadasService(int Fetch, int Page, int idConsulta)
        {
            try
            {
                var countResult = await _repositoryAdministradorGlobal.GetTablaPersonasAutorizadasCountAsyncRepository(idConsulta);

                if (countResult is null || countResult <= 0)
                {
                    return ResultOperation<List<ResponseTablaPersonasAutorizadas>>.SuccessResponseNoMessage("No se encontraron resultados"); ;
                }
                var result = await _repositoryAdministradorGlobal.GetTablaPersonasAutorizadasAsyncRepository(
                    Fetch,
                    Page,
                    idConsulta
                );

                if (result is null)
                {
                    return ResultOperation<List<ResponseTablaPersonasAutorizadas>>.SuccessResponseNoMessage("No se encontraron resultados");
                }
                return ResultOperation.SuccessResponseNoMessage(
                    new DataTableView<ResponseTablaPersonasAutorizadas>(new(Page, Fetch, countResult.GetValueOrDefault()),
                    result)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
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
                await _repositoryAdministradorGlobal.ExportarConsultaAsyncRepository(
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
     await _repositoryAdministradorGlobal.ExportarCumplimentacionAsyncRepository(
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
                     await _repositoryAdministradorGlobal.ExportarReporteGeneralAsyncRepository(
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
        public async Task<ResultOperation> GetTablaMediosDefensaGeneral(List<string> noAsunto)
        {
            try
            {
                int idAlerta = EnumAlertaSeccion.COMPLETA.GetHashCode();
                string alerta = EnumAlertaSeccion.COMPLETA.ToString();



                var request = new RequestMediosDefensa
                {
                    idModulo = EnumModulosSicoj.CONSULTAS.GetHashCode(),
                    dato = noAsunto
                };


                var wrapper = await _apiService.PostAsync<WrapperProxyMediosDefensa>(
                    _proxyEnpoints.RouteMediosDefensa,
                    request
                );

                var result = wrapper?.Result?.mediosDefensa;

                if (result is not null)
                {

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseMediosDefensa>(new(idAlerta, alerta),
                        result)
                    );
                }

                if (result is null)
                {
                    idAlerta = EnumAlertaSeccion.NO_REGISTRADO.GetHashCode();
                    alerta = EnumAlertaSeccion.NO_REGISTRADO.ToString();

                    return ResultOperation.SuccessResponseNoMessage(
                        new DataTableViewAlerta<ResponseMediosDefensa>(new(idAlerta, alerta),
                        null!)
                    );
                }


                return ResultOperation.SuccessResponseNoMessage(
                       new DataTableViewAlerta<ResponseMediosDefensa>(new(idAlerta, alerta),
                       result)
                   );
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
