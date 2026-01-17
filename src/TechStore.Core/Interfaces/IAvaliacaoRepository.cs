using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Core.Entities;

namespace TechStore.Core.Interfaces
{
    public interface IAvaliacaoRepository : IRepository<Avaliacao>
    {
        Task<IEnumerable<Avaliacao>> ObterAvaliacoesPorProdutoAsync(int produtoId, bool apenasAprovadas = true);
        Task<IEnumerable<Avaliacao>> ObterAvaliacoesPorUsuarioAsync(string usuarioId);
        Task<IEnumerable<Avaliacao>> ObterAvaliacoesPendentesAsync();
        Task<decimal> ObterMediaAvaliacoesPorProdutoAsync(int produtoId);
        Task<int> ContarAvaliacoesPorProdutoAsync(int produtoId);
    }
}