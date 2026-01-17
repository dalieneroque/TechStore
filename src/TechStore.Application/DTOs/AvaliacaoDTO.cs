using System;
using System.ComponentModel.DataAnnotations;

namespace TechStore.Application.DTOs
{
    public class AvaliacaoDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public string UsuarioId { get; set; }
        public string UsuarioNome { get; set; }
        public int Nota { get; set; }
        public string Titulo { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public bool Aprovada { get; set; }
    }

    public class CriarAvaliacaoDTO
    {
        [Required(ErrorMessage = "ID do produto é obrigatório")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Nota é obrigatória")]
        [Range(1, 5, ErrorMessage = "Nota deve ser entre 1 e 5")]
        public int Nota { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(100, ErrorMessage = "Título não pode exceder 100 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Comentário é obrigatório")]
        [StringLength(1000, ErrorMessage = "Comentário não pode exceder 1000 caracteres")]
        public string Comentario { get; set; }
    }

    public class AtualizarAvaliacaoDTO
    {
        [Required(ErrorMessage = "Nota é obrigatória")]
        [Range(1, 5, ErrorMessage = "Nota deve ser entre 1 e 5")]
        public int Nota { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(100, ErrorMessage = "Título não pode exceder 100 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Comentário é obrigatório")]
        [StringLength(1000, ErrorMessage = "Comentário não pode exceder 1000 caracteres")]
        public string Comentario { get; set; }
    }

    public class AprovarAvaliacaoDTO
    {
        public bool Aprovada { get; set; }
    }

    public class ProdutoAvaliacaoDTO
    {
        public decimal MediaNotas { get; set; }
        public int TotalAvaliacoes { get; set; }
        public List<AvaliacaoDTO> Avaliacoes { get; set; } = new List<AvaliacaoDTO>();
    }
}