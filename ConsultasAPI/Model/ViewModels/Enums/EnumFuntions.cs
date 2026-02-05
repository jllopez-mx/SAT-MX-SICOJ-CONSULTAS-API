using System.Security.Cryptography.X509Certificates;

namespace ConsultasAPI.Model.ViewModels.Enums
{
  public static class EnunFunctions
  {
    public const string CONSULTA_LISTADO_CONSULTAS = "sch_con.fn_lista_consultas";

    public const string CONSULTA_LISTADO_TABLA_REMISIONES = "sch_con.fn_admin_remision_get_table";
    public const string CONSULTA_LISTADO_CONSULTAS_BANDEJA_PENDIENTES_ADMINISTRADOR = "sch_con.fn_admin_pendientes_get_table";
    public const string CONSULTA_UPDATE_ADMINISTRADOR = "sch_con.fn_admin_comercio_exterior_update";

    public const string CONSULTA_UPDATE_ADMINISTRADOR_IMPUESTOS_INTERNOS = "sch_con.fn_admin_impuestos_internos_update";

    public const string CONSULTA_GET_BY_ID = "sch_con.fn_op_consulta_get_by_id";
    public const string CONSULTA_GET_BY_ID_ADMINISTRADOR = "sch_con.fn_admin_consulta_get_by_id";
    public const string CONSULTA_GET_BY_ID_ABOGADO = "sch_con.fn_abogado_consulta_get_by_id";


    public const string CONSULTA_GET_RFC = "sch_con.fn_op_get_rfc_ampliado";

    public const string CONSULTA_INSERT = "sch_con.fn_op_consulta_create";
    public const string CONSULTA_UPDATE = "sch_con.fn_op_consulta_update";
    public const string CONSULTA_DELETE = "sch_con.fn_op_consulta_delete";
    public const string CONSULTA_TURNAR = "sch_con.fn_op_consulta_turnar";

    public const string CONSULTA_ASIGNAR = "sch_con.fn_admin_asignar_consulta";
    public const string CONSULTA_ASIGNAR_ABOGADO = "sch_con.fn_abogado_asignar_update";

    public const string File_Repository_INSERT = "sch_con.fn_op_documento_create";

    public const string FILE_REPOSITORY_UPDATE = "sch_con.fn_admin_documentos_update";
    public const string CONSULTA_DELETE_ARCHIVO = "sch_con.fn_op_documento_delete";
    public const string File_Repository_GET_BY_REGISTRO_ALL = "sch_con.fn_op_documento_get_by_id";
    public const string File_Repository_GET_BY_ID = "sch_con.fn_op_documento_get_ruta";

    // Bandeja Historico
    public const string OP_HISTORICO_COUNT = "sch_con.fn_op_historico_count";
    public const string CONSULTA_GET_FILTERS = "sch_con.fn_op_consulta_get_by_filters";

    // Bandeja de pendientes
    public const string ConsultasByFiltersCount = "sch_con.fn_op_consulta_by_filters_count";
    public const string CONSULTAB_BANDEJA_PENDIENTES_CUMPLIMENTACION_COUNT = "sch_con.fn_op_pendientes_cumplimentacion_count";
    public const string CONSULTA_LISTADO_CONSULTAS_BANDEJA_PENDIENTES = "sch_con.fn_op_pendientes_get_table";
    public const string CONSULTA_LISTADO_CONSULTAS_BANDEJA_PENDIENTES_CUMPLIMENTACION = "sch_con.fn_op_pendientes_cumplimentacion_get_table";

    public const string ConsultasByFiltersCount_Pendientes_Administrador = "sch_con.fn_admin_pendiente_get_by_filters_count";
    public const string ConsultasByFiltersCountRemision = "sch_con.fn_admin_remision_get_count";
    // Rol Abogado


    public const string INSERT_REMISION = "sch_con.fn_admin_remision_create";

    public const string PERSONAS_AUTORIZADAS_INSERT = "sch_con.fn_admin_personas_autorizadas_create";

    public const string PERSONAS_AUTORIZADAS_UPDATE = "sch_con.fn_admin_personas_autorizadas_update";
    public const string PERSONAS_AUTORIZADAS_GET_BY_ID = "sch_con.fn_admin_personas_autorizadas_get_by_id";
    public const string PERSONAS_AUTORIZADAS_DELETE = "sch_con.fn_admin_personas_autorizadas_delete";

    public const string ConsultasByFiltersCountPersonasAutorizadas = "sch_con.fn_admin_personas_autorizadas_get_count";
    public const string CONSULTA_LISTADO_TABLA_PERSONAS_AUTORIZADAS = "sch_con.fn_admin_personas_autorizadas_get_table";

    public const string REMISION_GET_BY_ID = "sch_con.fn_admin_remision_get_by_id";
    public const string File_Repository_Remision_GET_BY_REGISTRO_ALL = "sch_con.fn_admin_documentos_remision_get_by_id";

    public const string CONSULTA_UPDATE_ABOGADO_IMPUESTOS_INTERNOS = "sch_con.fn_abogado_consultas_update_impuestos_internos";

    public const string CONSULTA_UPDATE_ABOGADO_COMERCIO_EXTERNO = "sch_con.fn_abogado_consulta_update_comercio_externo";

    public const string CONSULTA_GET_HISTORICO_ABOGADO = "sch_con.fn_abogado_historico_filters";
    public const string ConsultasHistoricoCountAbogado = "sch_con.fn_abogado_historico_filters_count";

    public const string ConsultasByFiltersCountAbogadoPendientes = "sch_con.fn_abogado_pendientes_filters_count";

    public const string CONSULTA_LISTADO_CONSULTAS_BANDEJA_PENDIENTES_ABOGADO = "sch_con.fn_abogado_pendientes_filters";

    //Administrador
    public const string ConsultasHistoricoCountAdmin = "sch_con.fn_admin_historico_get_by_filters_count";
    public const string CONSULTA_HISTORICO_ADMIN = "sch_con.fn_admin_historico_get_by_filters";

    public const string REQUERIMIENTOS_INSERT = "sch_con.fn_admin_requerimiento_create";
    public const string SOLICITA_REQUERIMIENTO = "sch_con.fn_admin_requerimiento_activa";
    public const string REQUERIMIENTOS_GET_BY_ID = "sch_con.fn_admin_requerimientos_get_by_id";
    public const string REQUERIMIENTOS_PRODECON_GET_BY_ID = "sch_con.fn_abogado_requerimientos_prodecon_get_by_id";

    public const string ConsultasByFiltersCountRequerimientos = "sch_con.fn_admin_requerimientos_get_count";
    public const string ConsultasByFiltersCountAlertaRequerimientos = "sch_con.fn_admin_requerimientos_get_count_alerta";

    public const string CONSULTA_LISTADO_TABLA_REQUERIMIENTOS = "sch_con.fn_admin_requerimientos_get_table";
    public const string CONSULTA_LISTADO_TABLA_REQUERIMIENTOS_PRODECON = "sch_con.fn_abogado_requerimientos_prodecon_get_table";

    public const string REQUERIMIENTOS_UPDATE = "sch_con.fn_admin_requerimientos_update";
    public const string REQUERIMIENTOS_PRODECON_UPDATE = "sch_con.fn_abogado_requerimientos_prodecon_update";

    //Solicitud de información
    public const string SOLICITUDINFORMACION_INSERT = "sch_con.fn_admin_solicitud_informacion_create";
    public const string CONSULTASCOUNT_SOLICITUDINFORMACION = "sch_con.fn_admin_solicitud_informacion_get_count";
    public const string CONSULTAALERTACOUNT_SOLICITUDINFORMACION = "sch_con.fn_admin_solicitud_informacion_get_by_count_alerta";
    public const string SOLICITUDINFORMACION_UPDATE = "sch_con.fn_admin_solicitud_informacion_update";
    public const string SOLICITUDINFORMACION_GET_BY_ID = "sch_con.fn_admin_solicitud_informacion_get_by_id";

    public const string CONSULTA_LISTADO_TABLA_SOLICITUD_INFORMACION = "sch_con.fn_admin_solicitud_Informacion_get_table";

    //resolucion

    public const string RESOLUCION_INSERT = "sch_con.fn_abogado_resolucion_create";

    public const string GET_VALIDACION_RESOLUCION = "sch_con.fn_abogado_resolucion_validation";
    public const string RESOLUCION_GET_BY_ID = "sch_con.fn_abogado_resolucion_get_by_id";
    public const string RESOLUCION_GET_LIST = "sch_con.fn_abogado_resolucion_get_list";
    public const string RESOLUCION_UPDATE = "sch_con.fn_abogado_resolucion_update";
    public const string RESOLUCION_CONCLUIR = "sch_con.fn_abogado_resolucion_concluir_update";

    public const string REQUERIMIENTOS_PRODECON_INSERT = "sch_con.fn_abogado_requerimiento_prodecon_create";


    public const string Reasignar = "sch_con.fn_admin_reasignar_create";
    public const string CONSULTA_GET_BY_IDS_ADMINISTRADOR = "sch_con.fn_admin_consulta_get_by_ids";

    //Avisos Y Comunicados
    public const string AVISOS_Y_COMUNICADOS_INSERT = "sch_con.fn_admin_avisos_comunicados_create";
    public const string AVISOS_Y_COMUNICADOS_GET_BY_ID = "sch_con.fn_admin_avisos_comunicados_get_by_id";
    public const string AVISOS_Y_COMUNICADOS_UPDATE = "sch_con.fn_admin_avisos_comunicados_update";
    public const string AVISOS_Y_COMUNICADOS_COUNT = "sch_con.fn_admin_avisos_comunicados_count";
    public const string AVISOS_Y_COMUNICADOS_COUNT_ALERTA = "sch_con.fn_admin_avisos_comunicados_count_alerta";
    public const string CONSULTA_LISTADO_TABLA_AVISOS_COMUNICADOS = "sch_con.fn_admin_avisos_comunicados_get_table";


    //Otro
    public const string ArchivoByFiltersCountAdmin = "sch_con.fn_admin_documentos_get_by_filters_count";
    public const string Archivo_GET_FILTERS_ADMIN = "sch_con.fn_admin_documento_get_by_filters";

    public const string ModificarEstado = "sch_con.fn_ag_modificar_estado_update";

    // Solicitud de transparencia
    public const string SOLICITUD_TRANSPARENCIA_INSERT = "sch_con.fn_op_solicitud_transparencia_create";
    public const string OficialPartesReadIdSolicitudTransparencia = "sch_con.fn_op_solicitud_transparencia_get_by_id";
    public const string OficialPartesUpdateSolicitudTransparencia = "sch_con.fn_op_solicitud_transparencia_update";
    public const string OficialPartesReadCountSolicitudTransparencia = "sch_con.fn_op_solicitud_transparencia_get_count";
    public const string OficialPartesReadTablaSolicitudTransparencia = "sch_con.fn_op_solicitud_transparencia_get_tabla";
    public const string OficialPartesDeleteSolicitudTransparencia = "sch_con.fn_op_solicitud_transparencia_delete";

    #region Reactivar

    public const string REACTIVAR_UPDATE = "sch_con.fn_ag_reactivar_update";

    #endregion

    public const string UpdateDescartarRequerimientos = "sch_con.fn_ag_requerimientos_descartar";
    public const string UpdateDescartarResolucion = "sch_con.fn_ag_resolucion_descartar";

    #region Historico Asuntos    
    public const string ConsultasByFiltersCountAdminGlobal = "sch_con.fn_ag_historico_get_by_filters_count";
    public const string CONSULTA_GET_FILTERS_ADMIN_GLOBAL = "sch_con.fn_admin_historco_get_by_filters";
    #endregion

    public const string DescartarDatosGenerales = "sch_con.fn_ag_datos_generales_descartar";

    #region Cumplimentacion

    public const string OficialPartesCreateCumplimentacion = "sch_con.fn_op_cumplimentacion_create";
    public const string CUMPLIMENTACION_TURNAR = "sch_con.fn_op_cumplimentacion_turnar";

    public const string CUMPLIMENTACION_GET_BY_ID = "sch_con.fn_op_cumplimentacion_get_by_id";

    public const string CUMPLIMENTACION_UPDATE = "sch_con.fn_op_cumplimentacion_update";
    public const string CONSULTA_GET_BY_NO_ASUNTO = "sch_con.fn_op_consulta_get_by_no_asunto";
    public const string GET_BY_NO_ASUNTO_CONSULTA_CUMPLIMENTACION = "sch_con.fn_op_cumplimentacion_get_by_no_asunto";

    public const string CONSULTA_ASIGNAR_CUMPLIMENTACION = "sch_con.fn_admin_cumplimentacion_asignar";

    #endregion

    #region Medios Defensa
    public const string GetTablaMediosDefensa = "sch_con.fn_admin_medios_defensa_get";
    #endregion
    #region Reportes
    public const string GetReporteConsultas = "sch_con.fn_consultas_reporte_get_table";
    public const string GetReporteCumplimnetacion = "sch_con.fn_cumplimentacion_reporte_get_table";
    public const string GetReporteGeneral = "sch_con.fn_reporte_General_get_table";
    public const string GetReportePDF = "sch_con.fn_pdf_reporte_get_table";
    #endregion
  }
}