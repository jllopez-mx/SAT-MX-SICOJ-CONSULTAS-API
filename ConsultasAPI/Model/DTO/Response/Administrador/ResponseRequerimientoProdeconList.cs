using Sicoj.Utils.ViewModels;

namespace ConsultasAPI.Model.DTO
{
  public class ResponseRequerimientoProdeconList
  {

    public int id { get; set; }
    public int id_rol { get; set; }
    public string? nombre_rol { get; set; } = null!;
    public int id_consulta { get; set; }
    public string? no_oficio { get; set; } = null!;
    public string? no_expediente { get; set; } = null!;
    public Boolean accion { get; set; }
    public string? fecha_ingreso { get; set; }
    public string? fecha_oficio { get; set; }
    public string? atencion { get; set; } = null!;
    
  }

}