using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Infrastructure.Repositories
{
    public class FavoritoRepository : Repository<Favorito>, IFavoritoRepository
    {
        public FavoritoRepository(TechStoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Favorito>> ObterFavoritosPorUsuarioAsync(string usuarioId)
        {
            return await _context.Favoritos
                .Include(f => f.Produto)
                    .ThenInclude(p => p.Categoria)
                .Where(f => f.UsuarioId == usuarioId)
                .OrderByDescending(f => f.DataAdicao)
                .ToListAsync();
        }

        public async Task<bool> ProdutoEstaFavoritadoAsync(string usuarioId, int produtoId)
        {
            return await _context.Favoritos
                .AnyAsync(f => f.UsuarioId == usuarioId && f.ProdutoId == produtoId);
        }

        public async Task<bool> RemoverFavoritoAsync(string usuarioId, int produtoId)
        {
            var favorito = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.ProdutoId == produtoId);

            if (favorito != null)
            {
                _context.Favoritos.Remove(favorito);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}