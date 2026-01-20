using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;

namespace TechStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AvaliacoesController : ControllerBase
    {
        private readonly IAvaliacaoService _avaliacaoService;

        public AvaliacoesController(IAvaliacaoService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }

        private string ObterUsuarioId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        private bool IsAdmin() => User.IsInRole("Admin");

        // GET: api/avaliacoes/produto/{produtoId}
        [HttpGet("produto/{produtoId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProdutoAvaliacaoDTO>> GetAvaliacoesPorProduto(int produtoId)
        {
            var avaliacoes = await _avaliacaoService.ObterAvaliacoesPorProdutoAsync(produtoId);
            return Ok(avaliacoes);
        }

        // GET: api/avaliacoes/minhas
        [HttpGet("minhas")]
        public async Task<ActionResult> GetMinhasAvaliacoes()
        {
            var usuarioId = ObterUsuarioId();
            var avaliacoes = await _avaliacaoService.ObterAvaliacoesPorUsuarioAsync(usuarioId);
            return Ok(avaliacoes);
        }

        // GET: api/avaliacoes/pendentes (Admin only)
        [HttpGet("pendentes")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAvaliacoesPendentes()
        {
            var avaliacoes = await _avaliacaoService.ObterAvaliacoesPendentesAsync();
            return Ok(avaliacoes);
        }

        // POST: api/avaliacoes
        [HttpPost]
        public async Task<ActionResult<AvaliacaoDTO>> CriarAvaliacao(CriarAvaliacaoDTO avaliacaoDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioId = ObterUsuarioId();
            var avaliacao = await _avaliacaoService.CriarAvaliacaoAsync(usuarioId, avaliacaoDTO);
            return CreatedAtAction(nameof(GetMinhasAvaliacoes), avaliacao);
        }

        // PUT: api/avaliacoes/{id}/aprovar (Admin only)
        [HttpPut("{id}/aprovar")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AvaliacaoDTO>> AprovarReprovarAvaliacao(int id, [FromBody] bool aprovar)
        {
            var avaliacao = await _avaliacaoService.AprovarReprovarAvaliacaoAsync(id, aprovar);
            return Ok(avaliacao);
        }

        // DELETE: api/avaliacoes/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> ExcluirAvaliacao(int id)
        {
            var usuarioId = ObterUsuarioId();
            await _avaliacaoService.ExcluirAvaliacaoAsync(id, usuarioId, IsAdmin());
            return NoContent();
        }
    }
}