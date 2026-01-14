using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Application.DTOs;

namespace TechStore.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoDTO> CriarPedidoAsync(string usuarioId, CriarPedidoDTO pedidoDTO);
        Task<IEnumerable<PedidoDTO>> ObterPedidosPorUsuarioAsync(string usuarioId);
        Task<PedidoDTO> ObterPedidoPorIdAsync(int pedidoId, string usuarioId = null);
        Task<PedidoDTO> AtualizarStatusAsync(int pedidoId, AtualizarStatusPedidoDTO statusDTO, bool isAdmin = false);
        Task<IEnumerable<PedidoDTO>> ObterPedidosPorStatusAsync(string status);
        Task<int> ContarPedidosPorUsuarioAsync(string usuarioId);
    }
}