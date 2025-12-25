using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TechStore.Application.DTOs;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;

namespace TechStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        // GET: api/categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(categoriasDTO);
        }

        // GET: api/categorias/ativas
        [HttpGet("ativas")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasAtivas()
        {
            var categorias = await _categoriaRepository.GetCategoriasAtivasAsync();
            var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(categoriasDTO);
        }

        // GET: api/categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoria(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);

            if (categoria == null)
            {
                return NotFound(new { message = $"Categoria com ID {id} não encontrada" });
            }

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(categoriaDTO);
        }

        // POST: api/categorias
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> PostCategoria(CriarCategoriaDTO criarCategoriaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoria = _mapper.Map<Categoria>(criarCategoriaDTO);
            await _categoriaRepository.AddAsync(categoria);

            var salvou = await _categoriaRepository.SaveChangesAsync();
            if (!salvou)
            {
                return BadRequest(new { message = "Não foi possível criar a categoria" });
            }

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoriaDTO);
        }

        // PUT: api/categorias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, AtualizarCategoriaDTO atualizarCategoriaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != atualizarCategoriaDTO.Id && atualizarCategoriaDTO.Id != 0)
            {
                return BadRequest(new { message = "ID na rota não corresponde ao ID no corpo da requisição" });
            }

            var categoriaExistente = await _categoriaRepository.GetByIdAsync(id);
            if (categoriaExistente == null)
            {
                return NotFound(new { message = $"Categoria com ID {id} não encontrada" });
            }

            _mapper.Map(atualizarCategoriaDTO, categoriaExistente);

            // Atualizar status da categoria
            if (atualizarCategoriaDTO.Ativa)
            {
                categoriaExistente.Ativar();
            }
            else
            {
                categoriaExistente.Desativar();
            }

            await _categoriaRepository.UpdateAsync(categoriaExistente);
            var salvou = await _categoriaRepository.SaveChangesAsync();

            if (!salvou)
            {
                return BadRequest(new { message = "Não foi possível atualizar a categoria" });
            }

            return NoContent();
        }

        // DELETE: api/categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
            {
                return NotFound(new { message = $"Categoria com ID {id} não encontrada" });
            }

            await _categoriaRepository.DeleteAsync(categoria);
            var salvou = await _categoriaRepository.SaveChangesAsync();

            if (!salvou)
            {
                return BadRequest(new { message = "Não foi possível excluir a categoria" });
            }

            return NoContent();
        }
    }
}