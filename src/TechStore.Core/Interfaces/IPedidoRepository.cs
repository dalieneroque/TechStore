using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Core.Entities;

namespace TechStore.Core.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<IEnumerable<Pedido>> ObterPedidosPorUsuarioIdAsync(string usuarioId);
        Task<Pedido> ObterPedidoComItensAsync(int pedidoId);
        Task<IEnumerable<Pedido>> ObterPedidosPorStatusAsync(string status);
        Task<int> ContarPedidosPorUsuarioAsync(string usuarioId);
    }
}