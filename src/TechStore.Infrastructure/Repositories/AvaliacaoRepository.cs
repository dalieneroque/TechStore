using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Infrastructure.Repositories
{
    public class AvaliacaoRepository : Repository<Avaliacao>, IAvaliacaoRepository
    {
        public AvaliacaoRepository(TechStoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Avaliacao>> ObterAvaliacoesPorProdutoAsync(int produtoId, bool apenasAprovadas = true)
        {
            var query = _context.Avaliacoes
                .Include(a => a.Usuario)
                .Where(a => a.ProdutoId == produtoId);

            if (apenasAprovadas)
                query = query.Where(a => a.Aprovada);

            return await query
                .OrderByDescending(a => a.DataAvaliacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Avaliacao>> ObterAvaliacoesPorUsuarioAsync(string usuarioId)
        {
            return await _context.Avaliacoes
                .Include(a => a.Produto)
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.DataAvaliacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Avaliacao>> ObterAvaliacoesPendentesAsync()
        {
            return await _context.Avaliacoes
                .Include(a => a.Produto)
                .Include(a => a.Usuario)
                .Where(a => !a.Aprovada)
                .OrderBy(a => a.DataAvaliacao)
                .ToListAsync();
        }

        public async Task<decimal> ObterMediaAvaliacoesPorProdutoAsync(int produtoId)
        {
            var media = await _context.Avaliacoes
                .Where(a => a.ProdutoId == produtoId && a.Aprovada)
                .AverageAsync(a => (decimal?)a.Nota);

            return media ?? 0;
        }

        public async Task<int> ContarAvaliacoesPorProdutoAsync(int produtoId)
        {
            return await _context.Avaliacoes
                .CountAsync(a => a.ProdutoId == produtoId && a.Aprovada);
        }
    }
}