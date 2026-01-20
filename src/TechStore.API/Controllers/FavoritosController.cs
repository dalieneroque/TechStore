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
    public class FavoritosController : ControllerBase
    {
        private readonly IFavoritoService _favoritoService;

        public FavoritosController(IFavoritoService favoritoService)
        {
            _favoritoService = favoritoService;
        }

        private string ObterUsuarioId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // GET: api/favoritos
        [HttpGet]
        public async Task<ActionResult> GetFavoritos()
        {
            var usuarioId = ObterUsuarioId();
            var favoritos = await _favoritoService.ObterFavoritosPorUsuarioAsync(usuarioId);
            return Ok(favoritos);
        }

        // GET: api/favoritos/contagem
        [HttpGet("contagem")]
        public async Task<ActionResult<int>> GetContagemFavoritos()
        {
            var usuarioId = ObterUsuarioId();
            var contagem = await _favoritoService.ContarFavoritosPorUsuarioAsync(usuarioId);
            return Ok(contagem);
        }

        // GET: api/favoritos/{produtoId}/verificar
        [HttpGet("{produtoId}/verificar")]
        public async Task<ActionResult<bool>> VerificarFavoritado(int produtoId)
        {
            var usuarioId = ObterUsuarioId();
            var favoritado = await _favoritoService.ProdutoEstaFavoritadoAsync(usuarioId, produtoId);
            return Ok(favoritado);
        }

        // POST: api/favoritos
        [HttpPost]
        public async Task<ActionResult<FavoritoDTO>> AdicionarFavorito(AdicionarFavoritoDTO favoritoDTO)
        {
            var usuarioId = ObterUsuarioId();
            var favorito = await _favoritoService.AdicionarFavoritoAsync(usuarioId, favoritoDTO.ProdutoId);
            return CreatedAtAction(nameof(GetFavoritos), favorito);
        }

        // DELETE: api/favoritos/{produtoId}
        [HttpDelete("{produtoId}")]
        public async Task<ActionResult> RemoverFavorito(int produtoId)
        {
            var usuarioId = ObterUsuarioId();
            await _favoritoService.RemoverFavoritoAsync(usuarioId, produtoId);
            return NoContent();
        }
    }
}