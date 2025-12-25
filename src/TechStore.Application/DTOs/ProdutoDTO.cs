using System.ComponentModel.DataAnnotations;

namespace TechStore.Application.DTOs
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "o nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, 1000000, ErrorMessage = "O preço deve estar entre 0,01 e 1.000.000")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória")]
        [Range(0, 10000, ErrorMessage = "A quantidade deve estar entre 0 e 10.000")]
        public int QuantidadeEstoque { get; set; }

        [Url(ErrorMessage = "A URL da imagem deve ser válida")]
        public string ImagemUrl { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória")]
        public int CategoriaId { get; set; }

        //Para exibir informação da categoria
        public string CategoriaNome { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
    }

    public class CriarProdutoDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, 1000000, ErrorMessage = "O preço deve estar entre 0,01 e 1.000.000")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória")]
        [Range(0, 10000, ErrorMessage = "A quantidade deve estar entre 0 e 10.000")]
        public int QuantidadeEstoque { get; set; }

        [Url(ErrorMessage = "A URL da imagem deve ser válida")]
        public string ImagemUrl { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória")]
        public int CategoriaId { get; set; }
    }

    public class AtualizarProdutoDTO
    {
        [Required(ErrorMessage = "O ID é obrigatório")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, 1000000, ErrorMessage = "O preço deve estar entre 0,01 e 1.000.000")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória")]
        [Range(0, 10000, ErrorMessage = "A quantidade deve estar entre 0 e 10.000")]
        public int QuantidadeEstoque { get; set; }

        [Url(ErrorMessage = "A URL da imagem deve ser válida")]
        public string ImagemUrl { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória")]
        public int CategoriaId { get; set; }

        public bool Ativo { get; set; }
    }

    public class  AtualizarEstoqueDTO
    {
        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Range(-10000, 10000, ErrorMessage = "A quantidade deve estar entre -10.000 e 10.000")]
        public int Quantidade { get; set; }
    }
}
