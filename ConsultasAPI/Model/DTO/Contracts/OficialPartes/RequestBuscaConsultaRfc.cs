using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FluentValidation;

namespace ConsultasAPI.Model.DTO
{
    public class RequestBuscaConsultaRfc
    {
        [Required]
        public string? nombre { get; set; } = null!;

    }

}