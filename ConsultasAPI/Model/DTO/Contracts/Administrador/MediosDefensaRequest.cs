namespace ConsultasAPI.Model.DTO.Contracts.Administrador
{
    public class MediosDefensaRequest
    {
        public List<string> dato { get; set; } = new();
        public int idModulo { get; set; }
    }
}