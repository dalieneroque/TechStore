using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechStore.Application.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string UsuarioNome { get; set; }
        public DateTime DataPedido { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataEntrega { get; set; }
        public string Status { get; set; }
        public decimal ValorTotal { get; set; }
        public string Observacoes { get; set; }
        public string EnderecoEntrega { get; set; }
        public List<ItemPedidoDTO> Itens { get; set; } = new List<ItemPedidoDTO>();
    }

    public class ItemPedidoDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public string ProdutoImagemUrl { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class CriarPedidoDTO
    {
        [Required(ErrorMessage = "Endereço de entrega é obrigatório")]
        [StringLength(500, ErrorMessage = "Endereço não pode exceder 500 caracteres")]
        public string EnderecoEntrega { get; set; }

        [StringLength(1000, ErrorMessage = "Observações não podem exceder 1000 caracteres")]
        public string Observacoes { get; set; }
    }

    public class AtualizarStatusPedidoDTO
    {
        [Required(ErrorMessage = "Status é obrigatório")]
        public string Status { get; set; }
    }
}