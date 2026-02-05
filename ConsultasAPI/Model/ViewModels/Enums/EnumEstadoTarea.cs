namespace ConsultasAPI.Model.ViewModels.Enums
{
    public enum EnumEstadoTarea
    {
            PENDIENTE_DE_REGISTRAR = 1,
            PENDIENTE_DE_TURNAR = 2,
            PENDIENTE_DE_ASIGNAR=3,
            ASIGNADO=4,
            CONCLUIDO_REMITIDO=5,
            Concluido_Notificado = 6,
            ATENDIDO =7,
            REASIGNADO = 8,
    }

    public class EnumEstadoTareaCons
    {
        public const int Pendiente_de_registrar = 1;
        public const int Pendiente_de_turnar = 2;
        public const int Pendiente_de_asignar = 3;
        public const int Asingado = 4;
        public const int Concluido_Remitido = 5;
        public const int Concluido_Notificado = 6;
        public const int Atendido = 7;
        public const int Reasingado = 8;
    }
}