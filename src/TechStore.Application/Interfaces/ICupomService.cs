using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Application.DTOs;

namespace TechStore.Application.Interfaces
{
    public interface ICupomService
    {
        Task<CupomDTO> CriarCupomAsync(CriarCupomDTO cupomDTO);
        Task<ValidarCupomResponseDTO> ValidarCupomAsync(ValidarCupomDTO validarDTO);
        Task<IEnumerable<CupomDTO>> ObterCuponsAtivosAsync();
        Task<CupomDTO> ObterCupomPorIdAsync(int id);
        Task<CupomDTO> AtualizarCupomAsync(int id, CupomDTO cupomDTO);
        Task<bool> ExcluirCupomAsync(int id);
        Task<int> ContarUsosCupomAsync(int cupomId);
    }
}