using Microsoft.EntityFrameworkCore;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Infrastructure.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(TechStoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasAtivasAsync()
        {
            return await _context.Categorias
                .Where(c => c.Ativa)
                .ToListAsync();
        }

        public async Task<Categoria> GetCategoriaComProdutosAsync(int id)
        {
            return await _context.Categorias
                .Include(c => c.Produtos)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}