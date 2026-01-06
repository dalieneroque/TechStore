using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TechStore.Application.DTOs
{
    public class UploadImagemDTO
    {
        [Required(ErrorMessage = "A imagem é obrigatória")]
        public IFormFile Arquivo { get; set; }

        [StringLength(200, ErrorMessage = "A descrição não pode exceder 200 caracteres")]
        public string Descricao { get; set; }

        public bool IsPrincipal { get; set; } = false;
    }
}