using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;


namespace TechStore.Infrastructure.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(TechStoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Produto>> GetProdutosAtivosAsync()
        {
            return await _context.Produtos
                .Where(p => p.Ativo)
                .Include(p => p.Categoria)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int categoriaId)
        {
            return await _context.Produtos
                .Where(p => p.CategoriaId == categoriaId && p.Ativo)
                .Include(p => p.Categoria)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetProdutosComEstoqueAsync()
        {
            return await _context.Produtos
                .Where(p => p.QuantidadeEstoque > 0 && p.Ativo)
                .Include(p => p.Categoria)
                .ToListAsync();
        }

        public async Task<Produto> GetProdutoComCategoriaAsync(int id)
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> SearchProdutosAsync(string searchTerm)
        {
            return await _context.Produtos
                .Where(p => p.Ativo &&
                           (p.Nome.Contains(searchTerm) ||
                            p.Descricao.Contains(searchTerm)))
                .Include(p => p.Categoria)
                .Take(20) // Limitar resultados
                .ToListAsync();
        }

    }
}
