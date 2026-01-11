using System.Threading.Tasks;
using TechStore.Application.DTOs;

namespace TechStore.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegistrarAsync(RegistrarUsuarioDTO registrarDTO);
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task<AuthResponseDTO> RefreshTokenAsync(string token);
        Task<bool> AlterarSenhaAsync(string usuarioId, AlterarSenhaDTO alterarSenhaDTO);
        Task<bool> LogoutAsync(string usuarioId);
    }
}