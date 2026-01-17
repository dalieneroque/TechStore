using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Core.Entities;

namespace TechStore.Core.Interfaces
{
    public interface IFavoritoRepository : IRepository<Favorito>
    {
        Task<IEnumerable<Favorito>> ObterFavoritosPorUsuarioAsync(string usuarioId);
        Task<bool> ProdutoEstaFavoritadoAsync(string usuarioId, int produtoId);
        Task<bool> RemoverFavoritoAsync(string usuarioId, int produtoId);
    }
}