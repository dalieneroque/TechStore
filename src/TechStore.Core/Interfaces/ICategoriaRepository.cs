using TechStore.Core.Entities;

namespace TechStore.Core.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        // Métodos específicos para Categoria
        Task<IEnumerable<Categoria>> GetCategoriasAtivasAsync();
        Task<Categoria> GetCategoriaComProdutosAsync(int id);
    }
}
