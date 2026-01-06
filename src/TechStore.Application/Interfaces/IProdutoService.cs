using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Application.Models;

namespace TechStore.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<ProdutoDTO>> ObterTodosProdutosAsync();
        Task<IEnumerable<ProdutoDTO>> ObterProdutosAtivosAsync();
        Task<IEnumerable<ProdutoDTO>> ObterProdutosPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<ProdutoDTO>> ObterProdutosComEstoqueAsync();
        Task<ProdutoDTO> ObterProdutoPorIdAsync(int id);
        Task<ProdutoDTO> CriarProdutoAsync(CriarProdutoDTO produtoDTO);
        Task AtualizarProdutoAsync(int id, AtualizarProdutoDTO produtoDTO);
        Task AtualizarEstoqueAsync(int id, AtualizarEstoqueDTO estoqueDTO);
        Task ExcluirProdutoAsync(int id);
        Task<IEnumerable<ProdutoDTO>> BuscarProdutosAsync(string termo);

        // Novos métodos específicos
        Task<IEnumerable<ProdutoDTO>> ObterProdutosRecentesAsync(int quantidade);
        Task<IEnumerable<ProdutoDTO>> ObterProdutosEmPromocaoAsync();

        Task<PagedResult<ProdutoDTO>> ObterProdutosPaginadosAsync(PagedRequest request);
        Task<PagedResult<ProdutoDTO>> FiltrarProdutosAsync(ProdutoFiltroDTO filtro, PagedRequest paginacao);
    }
}