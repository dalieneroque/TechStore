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
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        private string ObterUsuarioId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        // GET: api/pedidos
        [HttpGet]
        public async Task<ActionResult> GetPedidos()
        {
            var usuarioId = ObterUsuarioId();
            var pedidos = await _pedidoService.ObterPedidosPorUsuarioAsync(usuarioId);
            return Ok(pedidos);
        }

        // GET: api/pedidos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDTO>> GetPedido(int id)
        {
            var usuarioId = ObterUsuarioId();
            var pedido = await _pedidoService.ObterPedidoPorIdAsync(id, usuarioId);
            return Ok(pedido);
        }

        // GET: api/pedidos/status/{status} (Admin only)
        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetPedidosPorStatus(string status)
        {
            var pedidos = await _pedidoService.ObterPedidosPorStatusAsync(status);
            return Ok(pedidos);
        }

        // GET: api/pedidos/contagem
        [HttpGet("contagem")]
        public async Task<ActionResult<int>> GetContagemPedidos()
        {
            var usuarioId = ObterUsuarioId();
            var contagem = await _pedidoService.ContarPedidosPorUsuarioAsync(usuarioId);
            return Ok(contagem);
        }

        // POST: api/pedidos
        [HttpPost]
        public async Task<ActionResult<PedidoDTO>> CriarPedido(CriarPedidoDTO pedidoDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioId = ObterUsuarioId();
            var pedido = await _pedidoService.CriarPedidoAsync(usuarioId, pedidoDTO);
            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
        }

        // PUT: api/pedidos/{id}/status
        [HttpPut("{id}/status")]
        public async Task<ActionResult<PedidoDTO>> AtualizarStatus(int id, AtualizarStatusPedidoDTO statusDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pedido = await _pedidoService.AtualizarStatusAsync(id, statusDTO, IsAdmin());
            return Ok(pedido);
        }
       
    }
}