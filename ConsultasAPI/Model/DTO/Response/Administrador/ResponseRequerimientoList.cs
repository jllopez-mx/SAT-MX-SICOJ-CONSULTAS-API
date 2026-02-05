using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
  public class ResponseRequerimientoList
  {

    public int id { get; set; }
    public int id_rol { get; set; }
    public string? nombre_rol { get; set; } = null!;
    public int id_consulta { get; set; }
    public string? no_oficio { get; set; } = null!;
    public Boolean? atendio { get; set; }
    public string? fecha_notificacion { get; set; }
    public string? fecha_oficio { get; set; }
    public string? fecha_vencimiento { get; set; }
    public string? fecha_atencion { get; set; }
    
  }

}