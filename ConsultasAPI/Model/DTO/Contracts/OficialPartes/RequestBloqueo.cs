using System.Text.RegularExpressions;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ConsultasAPI.Model.ViewModels;

namespace ConsultasAPI.Model.DTO
{
    public class RequestBloqueo
    {
        

         [Required]
        public int id { get; set; } 


    }
}