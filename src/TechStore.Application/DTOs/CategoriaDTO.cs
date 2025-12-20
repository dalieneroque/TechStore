using System.ComponentModel.DataAnnotations;

namespace TechStore.Application.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }

        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres")]
        public string Descricao { get; set; }
        public bool Ativa { get; set; }
    }

    public class CriarCategoriaDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }

        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres")]
        public string Descricao { get; set; }
    }

    public class AtualizarCategoriaDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }

        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres")]
        public string Descricao { get; set; }
        public bool Ativa { get; set; }
    }
}