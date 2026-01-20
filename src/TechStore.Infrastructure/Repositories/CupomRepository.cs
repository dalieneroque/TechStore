using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Infrastructure.Repositories
{
    public class CupomRepository : Repository<Cupom>, ICupomRepository
    {
        public CupomRepository(TechStoreDbContext context) : base(context)
        {
        }

        public async Task<Cupom> ObterCupomPorCodigoAsync(string codigo)
        {
            return await _context.Cupons
                .FirstOrDefaultAsync(c => c.Codigo == codigo);
        }

        public async Task<IEnumerable<Cupom>> ObterCuponsAtivosAsync()
        {
            return await _context.Cupons
                .Where(c => c.Ativo && c.DataValidade > DateTime.UtcNow)
                .OrderBy(c => c.DataValidade)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cupom>> ObterCuponsExpiradosAsync()
        {
            return await _context.Cupons
                .Where(c => c.DataValidade <= DateTime.UtcNow)
                .OrderByDescending(c => c.DataValidade)
                .ToListAsync();
        }

        public async Task<bool> CupomExisteAsync(string codigo)
        {
            return await _context.Cupons
                .AnyAsync(c => c.Codigo == codigo);
        }

        public async Task<int> ContarUsosCupomAsync(int cupomId)
        {
            return await _context.Cupons
                .Where(c => c.Id == cupomId)
                .Select(c => c.UsosAtuais)
                .FirstOrDefaultAsync();
        }
    }
}