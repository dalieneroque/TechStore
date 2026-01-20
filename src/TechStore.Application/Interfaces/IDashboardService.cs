using System.Threading.Tasks;
using TechStore.Application.DTOs;

namespace TechStore.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDTO> ObterDashboardAdminAsync();
    }
}