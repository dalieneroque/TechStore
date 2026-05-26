using TechStore.Core.Entities;

namespace TechStore.Core.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasAtivasAsync();
        Task<Categoria> GetCategoriaComProdutosAsync(int id);
    }
}
