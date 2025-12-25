using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechStore.Application.DTOs;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;

namespace TechStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository produtoRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        // GET: api/produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos()
        {
            var produtos = await _produtoRepository.GetProdutosAtivosAsync();
            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDTO);
        }

        // GET: api/produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDTO>> GetProduto(int id)
        {
            var produto = await _produtoRepository.GetProdutoComCategoriaAsync(id);

            if (produto == null)
            {
                return NotFound(new { message = $"Produto com ID {id} não encontrado" });
            }

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoDTO);
        }

        // GET: api/produtos/categoria/5
        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPorCategoria(int categoriaId)
        {
            var produtos = await _produtoRepository.GetProdutosPorCategoriaAsync(categoriaId);
            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDTO);
        }

        // GET: api/produtos/estoque
        [HttpGet("estoque")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosComEstoque()
        {
            var produtos = await _produtoRepository.GetProdutosComEstoqueAsync();
            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDTO);
        }

        // GET: api/produtos/search/termo
        [HttpGet("search/{term}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> SearchProdutos(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 3)
            {
                return BadRequest(new { message = "O termo de busca deve ter pelo menos 3 caracteres" });
            }

            var produtos = await _produtoRepository.SearchProdutosAsync(term);
            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDTO);
        }

        // POST: api/produtos
        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> PostProduto(CriarProdutoDTO criarProdutoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produto = _mapper.Map<Produto>(criarProdutoDTO);
            await _produtoRepository.AddAsync(produto);

            var salvou = await _produtoRepository.SaveChangesAsync();
            if (!salvou)
            {
                return BadRequest(new { message = "Não foi possível criar o produto" });
            }

            // Recuperar com categoria para o DTO
            var produtoCompleto = await _produtoRepository.GetProdutoComCategoriaAsync(produto.Id);
            var produtoDTO = _mapper.Map<ProdutoDTO>(produtoCompleto);

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produtoDTO);
        }

        // PUT: api/produtos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, AtualizarProdutoDTO atualizarProdutoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != atualizarProdutoDTO.Id)
            {
                return BadRequest(new { message = "ID na rota não corresponde ao ID no corpo da requisição" });
            }

            var produtoExistente = await _produtoRepository.GetByIdAsync(id);
            if (produtoExistente == null)
            {
                return NotFound(new { message = $"Produto com ID {id} não encontrado" });
            }

            _mapper.Map(atualizarProdutoDTO, produtoExistente);

            // Atualizar status do produto
            if (atualizarProdutoDTO.Ativo)
            {
                produtoExistente.Ativar();
            }
            else
            {
                produtoExistente.Desativar();
            }

            await _produtoRepository.UpdateAsync(produtoExistente);
            var salvou = await _produtoRepository.SaveChangesAsync();

            if (!salvou)
            {
                return BadRequest(new { message = "Não foi possível atualizar o produto" });
            }

            return NoContent();
        }

        // PATCH: api/produtos/5/estoque
        [HttpPatch("{id}/estoque")]
        public async Task<IActionResult> PatchEstoque(int id, AtualizarEstoqueDTO atualizarEstoqueDTO)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
            {
                return NotFound(new { message = $"Produto com ID {id} não encontrado" });
            }

            try
            {
                if (atualizarEstoqueDTO.Quantidade >= 0)
                {
                    produto.AdicionarEstoque(atualizarEstoqueDTO.Quantidade);
                }
                else
                {
                    produto.RemoverEstoque(-atualizarEstoqueDTO.Quantidade);
                }

                await _produtoRepository.UpdateAsync(produto);
                var salvou = await _produtoRepository.SaveChangesAsync();

                if (!salvou)
                {
                    return BadRequest(new { message = "Não foi possível atualizar o estoque" });
                }

                return Ok(new
                {
                    message = "Estoque atualizado com sucesso",
                    novoEstoque = produto.QuantidadeEstoque
                });
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/produtos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
            {
                return NotFound(new { message = $"Produto com ID {id} não encontrado" });
            }

            // Soft delete - apenas desativa
            produto.Desativar();

            await _produtoRepository.UpdateAsync(produto);
            var salvou = await _produtoRepository.SaveChangesAsync();

            if (!salvou)
            {
                return BadRequest(new { message = "Não foi possível excluir o produto" });
            }

            return NoContent();
        }
    }
}