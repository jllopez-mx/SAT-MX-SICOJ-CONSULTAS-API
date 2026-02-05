namespace ConsultasAPI.Model.DTO.Response.Administrador
{
     public class RequestMediosDefensa
    {
        public List<string> dato { get; set; } = new();
        public int idModulo { get; set; }
    }
}