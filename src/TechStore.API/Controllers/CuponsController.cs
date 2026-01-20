using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;

namespace TechStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponsController : ControllerBase
    {
        private readonly ICupomService _cupomService;

        public CuponsController(ICupomService cupomService)
        {
            _cupomService = cupomService;
        }

        // GET: api/cupons/ativos
        [HttpGet("ativos")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCuponsAtivos()
        {
            var cupons = await _cupomService.ObterCuponsAtivosAsync();
            return Ok(cupons);
        }

        // GET: api/cupons/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CupomDTO>> GetCupom(int id)
        {
            var cupom = await _cupomService.ObterCupomPorIdAsync(id);
            return Ok(cupom);
        }

        // GET: api/cupons/{id}/usos
        [HttpGet("{id}/usos")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> GetUsosCupom(int id)
        {
            var usos = await _cupomService.ContarUsosCupomAsync(id);
            return Ok(usos);
        }

        // POST: api/cupons/validar
        [HttpPost("validar")]
        [AllowAnonymous]
        public async Task<ActionResult<ValidarCupomResponseDTO>> ValidarCupom(ValidarCupomDTO validarDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _cupomService.ValidarCupomAsync(validarDTO);
            return Ok(resultado);
        }

        // POST: api/cupons
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CupomDTO>> CriarCupom(CriarCupomDTO cupomDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cupom = await _cupomService.CriarCupomAsync(cupomDTO);
            return CreatedAtAction(nameof(GetCupom), new { id = cupom.Id }, cupom);
        }

        // PUT: api/cupons/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CupomDTO>> AtualizarCupom(int id, CupomDTO cupomDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != cupomDTO.Id)
                return BadRequest("ID na rota não corresponde ao ID no corpo");

            var cupom = await _cupomService.AtualizarCupomAsync(id, cupomDTO);
            return Ok(cupom);
        }

        // DELETE: api/cupons/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ExcluirCupom(int id)
        {
            await _cupomService.ExcluirCupomAsync(id);
            return NoContent();
        }
       
    }
}