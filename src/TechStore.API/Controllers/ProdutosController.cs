using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Application.Models;

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

        // GET: api/produtos/paginados
        [HttpGet("paginados")]
        public async Task<ActionResult<PagedResult<ProdutoDTO>>> GetProdutosPaginados(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool sortDescending = false)
        {
            try
            {
                var request = new PagedRequest
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortDescending = sortDescending
                };

                var result = await _produtoService.ObterProdutosPaginadosAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // GET: api/produtos/{id}/imagem
        [HttpGet("{id}/imagem")]
        public async Task<ActionResult> GetImagemProduto(int id)
        {
            try
            {
                var produto = await _produtoService.ObterProdutoPorIdAsync(id);

                if (produto == null || string.IsNullOrEmpty(produto.ImagemUrl))
                    return NotFound(new { message = "Imagem não encontrada" });

                // Aqui retornaria o arquivo físico
                // Por enquanto, retornamos a URL
                return Ok(new
                {
                    imagemUrl = produto.ImagemUrl,
                    mensagem = "Implementação completa requer servidor de arquivos estáticos configurado"
                });
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

        // POST: api/produtos/filtrar
        [HttpPost("filtrar")]
        public async Task<ActionResult<PagedResult<ProdutoDTO>>> FiltrarProdutos(
            [FromBody] ProdutoFiltroDTO filtro,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var paginacao = new PagedRequest
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                var result = await _produtoService.FiltrarProdutosAsync(filtro, paginacao);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        // POST: api/produtos/{id}/upload-imagem
        [HttpPost("{id}/upload-imagem")]
        public async Task<ActionResult> UploadImagem(int id, [FromForm] UploadImagemDTO uploadDTO)
        {
            try
            {
                // Verificar se produto existe
                var produto = await _produtoService.ObterProdutoPorIdAsync(id);

                if (produto == null)
                    return NotFound(new { message = $"Produto com ID {id} não encontrado" });

                // Validar arquivo
                if (uploadDTO.Arquivo == null || uploadDTO.Arquivo.Length == 0)
                    return BadRequest(new { message = "Nenhum arquivo enviado" });

                // Aqui você precisaria injetar IFileUploadService no controller
                // Por enquanto, retornaremos uma mensagem de sucesso simulada
                return Ok(new
                {
                    message = "Upload realizado com sucesso (implementação completa requer IFileUploadService)",
                    produtoId = id,
                    fileName = uploadDTO.Arquivo.FileName,
                    size = uploadDTO.Arquivo.Length
                });
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