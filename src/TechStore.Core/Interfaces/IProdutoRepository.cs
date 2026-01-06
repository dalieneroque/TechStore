using System.Collections.Generic;
using System.Linq.Expressions;
using TechStore.Core.Entities;

namespace TechStore.Core.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        // Métodos específicos para Produto
        Task<IEnumerable<Produto>> GetProdutosAtivosAsync();
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<Produto>> GetProdutosComEstoqueAsync();
        Task<Produto> GetProdutoComCategoriaAsync(int id);
        Task<IEnumerable<Produto>> SearchProdutosAsync(string searchTerm);

        Task<IQueryable<Produto>> GetQueryableAsync();
        Task<int> CountAsync(Expression<Func<Produto, bool>> predicate = null);

    }
}
