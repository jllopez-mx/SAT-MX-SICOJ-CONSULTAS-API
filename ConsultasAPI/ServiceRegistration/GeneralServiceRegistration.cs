using ConsultasAPI.Model.DAO.Repository;
using ConsultasAPI.Model.DAO.ServicesDAO;
using ConsultasAPI.Model.IDAO;
using ConsultasAPI.Model.IDAO.IRepository;
using ConsultasAPI.Model.IDAO.IServiceDAO;
using Sicoj.Utils.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.ServiceRegistration
{
    public static class GeneralServiceRegistration
    {
        public static IServiceCollection AddGeneralServices(this IServiceCollection services)
        {
            #region Repositorios
            services.AddScoped<IConsultaRepositoryOficialPartes, ConsultasRepositoryOficialPartes>();
            services.AddScoped<IConsultaRepositoryAdministrador, ConsultasRepositoryAdministrador>();
            services.AddScoped<IConsultaRepositoryAbogado, ConsultasRepositoryAbogado>();
            services.AddScoped<IConsultaRepositoryAdministradorGlobal, ConsultasRepositoryAdministradorGlobal>();
            services.AddScoped<IConsultaRepositoryAdministradorUnidadAdministrativa, ConsultasRepositoryAdministradorUnidadAdministrativa>();
            services.AddScoped<IConsultaRepositorySupervisor, ConsultasRepositorySupervisor>();
            services.AddScoped<IConsultaRepositoryReporteAdministrador, ConsultasRepositoryReporteAdministrador>();
            services.AddScoped<IConsultaRepositoryReporteAdministradorGlobal, ConsultasRepositoryReporteAdministradorGlobal>();
            services.AddScoped<IConsultaRepositoryReporteAdministradorUA, ConsultasRepositoryReporteAdministradorUA>();


            #endregion

            #region Servicios
            services.AddScoped<IConsultasOficialPartesService, ConsultasOficialPartesService>();
            services.AddScoped<IConsultasAdministradorService, ConsultasAdministradorService>();
            services.AddScoped<IConsultasAbogadoService, ConsultasAbogadoService>();
            services.AddScoped<IConsultasAdministradorGlobalService, ConsultasAdministradorGlobalService>();
            services.AddScoped<IConsultasAdministradorUnidadAdministrativaService, ConsultasAdministradorUnidadAdministrativaService>();
            services.AddScoped<IConsultasSupervisorService, ConsultasSupervisorService>();
            services.AddScoped<IConsultasReporteAdministradorService, ConsultasReporteAdministradorService>();
            services.AddScoped<IConsultasReporteAdministradorGlobalService, ConsultasReporteAdministradorGlobalService>();
            services.AddScoped<IConsultasReporteAdministradorUAService, ConsultasReporteAdministradorUAService>();
            services.AddScoped<ApiService>();
            #endregion

            return services;
        }
    }
}