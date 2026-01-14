using System.Threading.Tasks;
using TechStore.Core.Entities;

namespace TechStore.Core.Interfaces
{
    public interface ICarrinhoRepository : IRepository<Carrinho>
    {
        Task<Carrinho> ObterCarrinhoPorUsuarioIdAsync(string usuarioId);
        Task<Carrinho> ObterCarrinhoComItensAsync(string usuarioId);
        Task<bool> CarrinhoExisteAsync(string usuarioId);
        Task LimparCarrinhoAsync(string usuarioId);
    }
}