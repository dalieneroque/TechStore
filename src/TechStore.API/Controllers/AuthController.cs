using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;

namespace TechStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: api/auth/perfil
        [HttpGet("perfil")]
        [Authorize]
        public ActionResult GetPerfil()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new { claims });
        }

        // GET: api/auth/admin
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult AcessoAdmin()
        {
            return Ok(new { mensagem = "Acesso permitido para administradores" });
        }

        // POST: api/auth/registrar
        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDTO>> Registrar(RegistrarUsuarioDTO registrarDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _authService.RegistrarAsync(registrarDTO);

            if (!resultado.Sucesso)
                return BadRequest(new { resultado.Mensagem });

            return Ok(resultado);
        }

        // POST: api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _authService.LoginAsync(loginDTO);

            if (!resultado.Sucesso)
                return Unauthorized(new { resultado.Mensagem });

            return Ok(resultado);
        }

        // POST: api/auth/alterar-senha
        [HttpPost("alterar-senha")]
        [Authorize]
        public async Task<ActionResult> AlterarSenha(AlterarSenhaDTO alterarSenhaDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized();

            var sucesso = await _authService.AlterarSenhaAsync(usuarioId, alterarSenhaDTO);

            if (!sucesso)
                return BadRequest(new { mensagem = "Não foi possível alterar a senha" });

            return Ok(new { mensagem = "Senha alterada com sucesso" });
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized();

            await _authService.LogoutAsync(usuarioId);
            return Ok(new { mensagem = "Logout realizado com sucesso" });
        }


        // GET: api/auth/teste-auth
        [HttpGet("teste-auth")]
        [Authorize]
        public IActionResult TesteAuth()
        {
            return Ok(new
            {
                Usuario = User.Identity.Name,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }


    }
}