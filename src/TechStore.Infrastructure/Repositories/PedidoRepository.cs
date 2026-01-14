using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Infrastructure.Repositories
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(TechStoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosPorUsuarioIdAsync(string usuarioId)
        {
            return await _context.Pedidos
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<Pedido> ObterPedidoComItensAsync(int pedidoId)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosPorStatusAsync(string status)
        {
            return await _context.Pedidos
                .Where(p => p.Status == status)
                .OrderBy(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<int> ContarPedidosPorUsuarioAsync(string usuarioId)
        {
            return await _context.Pedidos
                .CountAsync(p => p.UsuarioId == usuarioId);
        }
    }
}