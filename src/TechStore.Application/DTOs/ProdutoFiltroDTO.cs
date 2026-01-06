using System.ComponentModel.DataAnnotations;

namespace TechStore.Application.DTOs
{
    public class ProdutoFiltroDTO
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal? PrecoMinimo { get; set; }
        public decimal? PrecoMaximo { get; set; }
        public int? CategoriaId { get; set; }
        public bool? Ativo { get; set; } = true;
        public bool? ComEstoque { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "O preço mínimo deve estar entre 0,01 e 1.000.000")]
        public decimal? PrecoMin { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "O preço máximo deve estar entre 0,01 e 1.000.000")]
        public decimal? PrecoMax { get; set; }

        public string? OrdenarPor { get; set; } = "nome";
        public bool OrdemDescendente { get; set; } = false;
    }
}