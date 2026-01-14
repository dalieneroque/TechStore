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
    public class CarrinhoController : ControllerBase
    {
        private readonly ICarrinhoService _carrinhoService;

        public CarrinhoController(ICarrinhoService carrinhoService)
        {
            _carrinhoService = carrinhoService;
        }

        private string ObterUsuarioId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        // GET: api/carrinho
        [HttpGet]
        public async Task<ActionResult<CarrinhoDTO>> GetCarrinho()
        {
            var usuarioId = ObterUsuarioId();
            var carrinho = await _carrinhoService.ObterCarrinhoAsync(usuarioId);
            return Ok(carrinho);
        }

        // GET: api/carrinho/quantidade
        [HttpGet("quantidade")]
        public async Task<ActionResult<int>> GetQuantidadeItens()
        {
            var usuarioId = ObterUsuarioId();
            var quantidade = await _carrinhoService.ObterQuantidadeItensAsync(usuarioId);
            return Ok(quantidade);
        }

        // POST: api/carrinho/itens
        [HttpPost("itens")]
        public async Task<ActionResult<CarrinhoDTO>> AdicionarItem(AdicionarItemCarrinhoDTO itemDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioId = ObterUsuarioId();
            var carrinho = await _carrinhoService.AdicionarItemAsync(usuarioId, itemDTO);
            return Ok(carrinho);
        }

        // PUT: api/carrinho/itens
        [HttpPut("itens")]
        public async Task<ActionResult<CarrinhoDTO>> AtualizarQuantidade(AtualizarItemCarrinhoDTO itemDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioId = ObterUsuarioId();
            var carrinho = await _carrinhoService.AtualizarQuantidadeAsync(usuarioId, itemDTO);
            return Ok(carrinho);
        }

        // DELETE: api/carrinho/itens/{produtoId}
        [HttpDelete("itens/{produtoId}")]
        public async Task<ActionResult<CarrinhoDTO>> RemoverItem(int produtoId)
        {
            var usuarioId = ObterUsuarioId();
            var carrinho = await _carrinhoService.RemoverItemAsync(usuarioId, produtoId);
            return Ok(carrinho);
        }

        // DELETE: api/carrinho
        [HttpDelete]
        public async Task<ActionResult<CarrinhoDTO>> LimparCarrinho()
        {
            var usuarioId = ObterUsuarioId();
            var carrinho = await _carrinhoService.LimparCarrinhoAsync(usuarioId);
            return Ok(carrinho);
        }
    }
}