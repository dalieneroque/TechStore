using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Application.DTOs;

namespace TechStore.Application.Interfaces
{
    public interface IAvaliacaoService
    {
        Task<AvaliacaoDTO> CriarAvaliacaoAsync(string usuarioId, CriarAvaliacaoDTO avaliacaoDTO);
        Task<ProdutoAvaliacaoDTO> ObterAvaliacoesPorProdutoAsync(int produtoId);
        Task<IEnumerable<AvaliacaoDTO>> ObterAvaliacoesPendentesAsync();
        Task<AvaliacaoDTO> AprovarReprovarAvaliacaoAsync(int avaliacaoId, bool aprovar);
        Task<IEnumerable<AvaliacaoDTO>> ObterAvaliacoesPorUsuarioAsync(string usuarioId);
        Task<bool> ExcluirAvaliacaoAsync(int avaliacaoId, string usuarioId, bool isAdmin = false);
    }
}