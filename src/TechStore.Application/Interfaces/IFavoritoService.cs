using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Application.DTOs;

namespace TechStore.Application.Interfaces
{
    public interface IFavoritoService
    {
        Task<IEnumerable<FavoritoDTO>> ObterFavoritosPorUsuarioAsync(string usuarioId);
        Task<FavoritoDTO> AdicionarFavoritoAsync(string usuarioId, int produtoId);
        Task<bool> RemoverFavoritoAsync(string usuarioId, int produtoId);
        Task<bool> ProdutoEstaFavoritadoAsync(string usuarioId, int produtoId);
        Task<int> ContarFavoritosPorUsuarioAsync(string usuarioId);
    }
}