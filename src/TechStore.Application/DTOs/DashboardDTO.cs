using System;
using System.Collections.Generic;

namespace TechStore.Application.DTOs
{
    public class DashboardDTO
    {
        public MetricasGeraisDTO MetricasGerais { get; set; }
        public List<PedidoResumoDTO> UltimosPedidos { get; set; }
        public List<ProdutoMaisVendidoDTO> ProdutosMaisVendidos { get; set; }
        public List<VendasPorCategoriaDTO> VendasPorCategoria { get; set; }
    }

    public class MetricasGeraisDTO
    {
        public int TotalPedidos { get; set; }
        public int PedidosHoje { get; set; }
        public decimal FaturamentoTotal { get; set; }
        public decimal FaturamentoMes { get; set; }
        public int TotalClientes { get; set; }
        public int ClientesNovosMes { get; set; }
        public int TotalProdutos { get; set; }
        public int ProdutosSemEstoque { get; set; }
    }

    public class PedidoResumoDTO
    {
        public int Id { get; set; }
        public string ClienteNome { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; }
    }

    public class ProdutoMaisVendidoDTO
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public int QuantidadeVendida { get; set; }
        public decimal Faturamento { get; set; }
    }

    public class VendasPorCategoriaDTO
    {
        public string CategoriaNome { get; set; }
        public int QuantidadeVendida { get; set; }
        public decimal Faturamento { get; set; }
        public decimal Percentual { get; set; }
    }
}