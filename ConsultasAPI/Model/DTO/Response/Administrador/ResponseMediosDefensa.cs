namespace ConsultasAPI.Model.DTO.Response.Administrador
{
    public class ResponseMediosDefensa
    {
    public string? asunto { get; set; } = null!;
    public string? medioDefensa { get; set; } = null!;
    public string unidadControlaAsunto { get; set; } = string.Empty;
    public string estadoProcesal { get; set; } = string.Empty;  

    }

    public class WrapperProxyMediosDefensa
    {
        public MediosDefensaViewResponse? Result { get; set; }
    }

    public class MediosDefensaViewResponse
    {
        public List<ResponseMediosDefensa> mediosDefensa { get; set; } = new();
        public bool estadoSeccion => mediosDefensa.Any();
    }
}