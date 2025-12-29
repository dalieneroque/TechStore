using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;

namespace TechStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        // GET: api/produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos()
        {
            try
            {
                var produtos = await _produtoService.ObterProdutosAtivosAsync();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // GET: api/produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDTO>> GetProduto(int id)
        {
            try
            {
                var produto = await _produtoService.ObterProdutoPorIdAsync(id);
                return Ok(produto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // GET: api/produtos/categoria/5
        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPorCategoria(int categoriaId)
        {
            try
            {
                var produtos = await _produtoService.ObterProdutosPorCategoriaAsync(categoriaId);
                return Ok(produtos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // GET: api/produtos/estoque
        [HttpGet("estoque")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosComEstoque()
        {
            try
            {
                var produtos = await _produtoService.ObterProdutosComEstoqueAsync();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // GET: api/produtos/recentes/5
        [HttpGet("recentes/{quantidade}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosRecentes(int quantidade)
        {
            try
            {
                if (quantidade <= 0 || quantidade > 50)
                    return BadRequest(new { message = "A quantidade deve estar entre 1 e 50" });

                var produtos = await _produtoService.ObterProdutosRecentesAsync(quantidade);
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // GET: api/produtos/promocao
        [HttpGet("promocao")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosEmPromocao()
        {
            try
            {
                var produtos = await _produtoService.ObterProdutosEmPromocaoAsync();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // GET: api/produtos/search/termo
        [HttpGet("search/{term}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> SearchProdutos(string term)
        {
            try
            {
                var produtos = await _produtoService.BuscarProdutosAsync(term);
                return Ok(produtos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // POST: api/produtos
        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> PostProduto(CriarProdutoDTO criarProdutoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var produto = await _produtoService.CriarProdutoAsync(criarProdutoDTO);
                return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // PUT: api/produtos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, AtualizarProdutoDTO atualizarProdutoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _produtoService.AtualizarProdutoAsync(id, atualizarProdutoDTO);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // PATCH: api/produtos/5/estoque
        [HttpPatch("{id}/estoque")]
        public async Task<IActionResult> PatchEstoque(int id, AtualizarEstoqueDTO atualizarEstoqueDTO)
        {
            try
            {
                await _produtoService.AtualizarEstoqueAsync(id, atualizarEstoqueDTO);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // DELETE: api/produtos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            try
            {
                await _produtoService.ExcluirProdutoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }
    }
}