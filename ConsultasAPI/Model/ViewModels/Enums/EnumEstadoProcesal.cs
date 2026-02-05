using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasAPI.Model.ViewModels.Enums
{
  public enum EnumEstadoProcesal
  {
    ACTIVO = 1,
    REMITIDO = 2,
    EN_ESTUDIO = 3,
    REQUERIDO = 4,
    EN_FIRMA = 5,
    CONCLUIDO_REMITIDO = 6,
    RESUELTO = 7,
    CONCLUIDO_NOTIFICADO = 8,
    AVISO_PENDIENTE=9,
    AVISO_CONCLUIDO=10,
    EN_REPARACION = 11,
  }

   public class EnumEstadoProcesalCons
    {
        public const int Activo = 1;
        public const int Remitido = 2;
        public const int En_estudio = 3;
        public const int Requerido = 4;
        public const int En_firma = 5;
        public const int Concluido_Remitido = 6;
        public const int Resuelto = 7;
        public const int Concluido_Notificado = 8;
        public const int Aviso_Pendiente = 9;
        public const int Aviso_Concluido = 10;
        
    }
}