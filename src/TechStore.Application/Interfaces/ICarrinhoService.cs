using System.Threading.Tasks;
using TechStore.Application.DTOs;

namespace TechStore.Application.Interfaces
{
    public interface ICarrinhoService
    {
        Task<CarrinhoDTO> ObterCarrinhoAsync(string usuarioId);
        Task<CarrinhoDTO> AdicionarItemAsync(string usuarioId, AdicionarItemCarrinhoDTO itemDTO);
        Task<CarrinhoDTO> RemoverItemAsync(string usuarioId, int produtoId);
        Task<CarrinhoDTO> AtualizarQuantidadeAsync(string usuarioId, AtualizarItemCarrinhoDTO itemDTO);
        Task<CarrinhoDTO> LimparCarrinhoAsync(string usuarioId);
        Task<int> ObterQuantidadeItensAsync(string usuarioId);
    }
}