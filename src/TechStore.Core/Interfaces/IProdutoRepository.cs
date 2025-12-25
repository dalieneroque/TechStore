using System.Collections.Generic;
using TechStore.Core.Entities;

namespace TechStore.Core.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        // Métodos específicos para Produto
        Task<IEnumerable<Produto>> GetProdutosAtivosAsync();
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<Produto>> GetProdutoComEstoqueAsync();
        Task<Produto> GetProdutoComCategoriaAsync(int id);
        Task<IEnumerable<Produto>> SearchProdutosAsync(string searchTerm);

    }
}
