using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Infrastructure.Repositories
{
    public class CarrinhoRepository : Repository<Carrinho>, ICarrinhoRepository
    {
        public CarrinhoRepository(TechStoreDbContext context) : base(context)
        {
        }

        public async Task<Carrinho> ObterCarrinhoPorUsuarioIdAsync(string usuarioId)
        {
            return await _context.Carrinhos
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task<Carrinho> ObterCarrinhoComItensAsync(string usuarioId)
        {
            return await _context.Carrinhos
                .Include(c => c.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task<bool> CarrinhoExisteAsync(string usuarioId)
        {
            return await _context.Carrinhos
                .AnyAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task LimparCarrinhoAsync(string usuarioId)
        {
            var carrinho = await ObterCarrinhoPorUsuarioIdAsync(usuarioId);
            if (carrinho != null)
            {
                _context.CarrinhoItens.RemoveRange(carrinho.Itens);
                await _context.SaveChangesAsync();
            }
        }
    }
}