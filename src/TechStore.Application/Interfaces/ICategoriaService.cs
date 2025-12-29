using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Application.DTOs;

namespace TechStore.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDTO>> ObterTodasCategoriasAsync();
        Task<IEnumerable<CategoriaDTO>> ObterCategoriasAtivasAsync();
        Task<CategoriaDTO> ObterCategoriaPorIdAsync(int id);
        Task<CategoriaDTO> CriarCategoriaAsync(CriarCategoriaDTO categoriaDTO);
        Task AtualizarCategoriaAsync(int id, AtualizarCategoriaDTO categoriaDTO);
        Task ExcluirCategoriaAsync(int id);
        Task<bool> CategoriaExisteAsync(int id);
    }
}
