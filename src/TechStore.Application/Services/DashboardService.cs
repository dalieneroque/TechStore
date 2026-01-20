using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly TechStoreDbContext _context;
        private readonly IMapper _mapper;

        public DashboardService(
            IPedidoRepository pedidoRepository,
            IProdutoRepository produtoRepository,
            ICategoriaRepository categoriaRepository,
            TechStoreDbContext context,
            IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<DashboardDTO> ObterDashboardAdminAsync()
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);

            var dashboard = new DashboardDTO
            {
                MetricasGerais = await ObterMetricasGeraisAsync(hoje, inicioMes),
                UltimosPedidos = await ObterUltimosPedidosAsync(),
                ProdutosMaisVendidos = await ObterProdutosMaisVendidosAsync(),
                VendasPorCategoria = await ObterVendasPorCategoriaAsync()
            };

            return dashboard;
        }

        private async Task<MetricasGeraisDTO> ObterMetricasGeraisAsync(DateTime hoje, DateTime inicioMes)
        {
            return new MetricasGeraisDTO
            {
                TotalPedidos = await _context.Pedidos.CountAsync(),
                PedidosHoje = await _context.Pedidos.CountAsync(p => p.DataPedido.Date == hoje),
                FaturamentoTotal = await _context.Pedidos.SumAsync(p => p.ValorTotal),
                FaturamentoMes = await _context.Pedidos
                    .Where(p => p.DataPedido >= inicioMes)
                    .SumAsync(p => p.ValorTotal),
                TotalClientes = await _context.Users.CountAsync(),
                ClientesNovosMes = await _context.Users
                    .CountAsync(u => u.DataCadastro >= inicioMes),
                TotalProdutos = await _context.Produtos.CountAsync(p => p.Ativo),
                ProdutosSemEstoque = await _context.Produtos.CountAsync(p => p.Ativo && p.QuantidadeEstoque == 0)
            };
        }

        private async Task<List<PedidoResumoDTO>> ObterUltimosPedidosAsync(int quantidade = 10)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .OrderByDescending(p => p.DataPedido)
                .Take(quantidade)
                .Select(p => new PedidoResumoDTO
                {
                    Id = p.Id,
                    ClienteNome = p.Usuario.NomeCompleto,
                    DataPedido = p.DataPedido,
                    ValorTotal = p.ValorTotal,
                    Status = p.Status
                })
                .ToListAsync();

            return pedidos;
        }

        private async Task<List<ProdutoMaisVendidoDTO>> ObterProdutosMaisVendidosAsync(int quantidade = 10)
        {
            var produtosMaisVendidos = await _context.ItensPedido
                .Include(i => i.Produto)
                .GroupBy(i => new { i.ProdutoId, i.Produto.Nome })
                .Select(g => new ProdutoMaisVendidoDTO
                {
                    ProdutoId = g.Key.ProdutoId,
                    ProdutoNome = g.Key.Nome,
                    QuantidadeVendida = g.Sum(i => i.Quantidade),
                    Faturamento = g.Sum(i => i.Subtotal)
                })
                .OrderByDescending(p => p.QuantidadeVendida)
                .Take(quantidade)
                .ToListAsync();

            return produtosMaisVendidos;
        }

        private async Task<List<VendasPorCategoriaDTO>> ObterVendasPorCategoriaAsync()
        {
            var vendasPorCategoria = await _context.ItensPedido
                .Include(i => i.Produto)
                    .ThenInclude(p => p.Categoria)
                .Where(i => i.Pedido.DataPedido >= DateTime.UtcNow.AddMonths(-1)) // Último mês
                .GroupBy(i => new { i.Produto.CategoriaId, i.Produto.Categoria.Nome })
                .Select(g => new VendasPorCategoriaDTO
                {
                    CategoriaNome = g.Key.Nome,
                    QuantidadeVendida = g.Sum(i => i.Quantidade),
                    Faturamento = g.Sum(i => i.Subtotal)
                })
                .OrderByDescending(v => v.Faturamento)
                .ToListAsync();

            var totalFaturamento = vendasPorCategoria.Sum(v => v.Faturamento);

            foreach (var categoria in vendasPorCategoria)
            {
                categoria.Percentual = totalFaturamento > 0
                    ? Math.Round((categoria.Faturamento / totalFaturamento) * 100, 1)
                    : 0;
            }

            return vendasPorCategoria;
        }
    }
}