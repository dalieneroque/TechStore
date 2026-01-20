using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Core.Entities;

namespace TechStore.Core.Interfaces
{
    public interface ICupomRepository : IRepository<Cupom>
    {
        Task<Cupom> ObterCupomPorCodigoAsync(string codigo);
        Task<IEnumerable<Cupom>> ObterCuponsAtivosAsync();
        Task<IEnumerable<Cupom>> ObterCuponsExpiradosAsync();
        Task<bool> CupomExisteAsync(string codigo);
        Task<int> ContarUsosCupomAsync(int cupomId);
    }
}