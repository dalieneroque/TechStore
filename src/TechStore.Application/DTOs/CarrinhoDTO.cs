using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechStore.Application.DTOs
{
    public class CarrinhoDTO
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public decimal ValorTotal { get; set; }
        public int TotalItens { get; set; }
        public List<CarrinhoItemDTO> Itens { get; set; } = new List<CarrinhoItemDTO>();
        public bool EstaVazio { get; set; }
    }

    public class CarrinhoItemDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public string ProdutoImagemUrl { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public int EstoqueDisponivel { get; set; }
    }

    public class AdicionarItemCarrinhoDTO
    {
        [Required(ErrorMessage = "ID do produto é obrigatório")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Range(1, 100, ErrorMessage = "Quantidade deve estar entre 1 e 100")]
        public int Quantidade { get; set; }
    }

    public class AtualizarItemCarrinhoDTO
    {
        [Required(ErrorMessage = "ID do produto é obrigatório")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Range(0, 100, ErrorMessage = "Quantidade deve estar entre 0 e 100")]
        public int Quantidade { get; set; } // 0 = remover
    }
}