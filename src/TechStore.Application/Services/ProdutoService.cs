using AutoMapper;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Application.Models;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;

namespace TechStore.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public ProdutoService(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProdutoDTO>> ObterTodosProdutosAsync()
        {
            var produtos = await _produtoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        }

        public async Task<IEnumerable<ProdutoDTO>> ObterProdutosAtivosAsync()
        {
            var produtos = await _produtoRepository.GetProdutosAtivosAsync();
            return _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        }

        public async Task<IEnumerable<ProdutoDTO>> ObterProdutosPorCategoriaAsync(int categoriaId)
        {
            if (!await _categoriaRepository.ExistsAsync(categoriaId))
                throw new KeyNotFoundException($"Categoria com ID {categoriaId} não encontrada");

            var produtos = await _produtoRepository.GetProdutosPorCategoriaAsync(categoriaId);
            return _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        }

        public async Task<IEnumerable<ProdutoDTO>> ObterProdutosComEstoqueAsync()
        {
            var produtos = await _produtoRepository.GetProdutosComEstoqueAsync();
            return _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        }

        public async Task<ProdutoDTO> ObterProdutoPorIdAsync(int id)
        {
            var produto = await _produtoRepository.GetProdutoComCategoriaAsync(id);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado");

            return _mapper.Map<ProdutoDTO>(produto);
        }

        public async Task<ProdutoDTO> CriarProdutoAsync(CriarProdutoDTO produtoDTO)
        {
            // Validações de negócio
            if (!await _categoriaRepository.ExistsAsync(produtoDTO.CategoriaId))
                throw new KeyNotFoundException($"Categoria com ID {produtoDTO.CategoriaId} não encontrada");

            var produtoExistente = await _produtoRepository.FindAsync(p =>
                p.Nome.ToLower() == produtoDTO.Nome.ToLower() &&
                p.CategoriaId == produtoDTO.CategoriaId);

            if (produtoExistente.Any())
                throw new InvalidOperationException(
                    $"Já existe um produto com o nome '{produtoDTO.Nome}' nesta categoria");

            var produto = _mapper.Map<Produto>(produtoDTO);
            await _produtoRepository.AddAsync(produto);

            var salvou = await _produtoRepository.SaveChangesAsync();
            if (!salvou)
                throw new Exception("Não foi possível criar o produto");

            // Retornar produto com informações da categoria
            var produtoCompleto = await _produtoRepository.GetProdutoComCategoriaAsync(produto.Id);
            return _mapper.Map<ProdutoDTO>(produtoCompleto);
        }

        public async Task AtualizarProdutoAsync(int id, AtualizarProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.Id)
                throw new ArgumentException("ID na rota não corresponde ao ID no DTO");

            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado");

            if (!await _categoriaRepository.ExistsAsync(produtoDTO.CategoriaId))
                throw new KeyNotFoundException($"Categoria com ID {produtoDTO.CategoriaId} não encontrada");

            // Verificar se outro produto já tem este nome na mesma categoria
            var produtoComMesmoNome = await _produtoRepository.FindAsync(p =>
                p.Nome.ToLower() == produtoDTO.Nome.ToLower() &&
                p.CategoriaId == produtoDTO.CategoriaId &&
                p.Id != id);

            if (produtoComMesmoNome.Any())
                throw new InvalidOperationException(
                    $"Já existe outro produto com o nome '{produtoDTO.Nome}' nesta categoria");

            _mapper.Map(produtoDTO, produto);

            if (produtoDTO.Ativo)
                produto.Ativar();
            else
                produto.Desativar();

            await _produtoRepository.UpdateAsync(produto);

            var salvou = await _produtoRepository.SaveChangesAsync();
            if (!salvou)
                throw new Exception("Não foi possível atualizar o produto");
        }

        public async Task AtualizarEstoqueAsync(int id, AtualizarEstoqueDTO estoqueDTO)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado");

            try
            {
                if (estoqueDTO.Quantidade >= 0)
                {
                    produto.AdicionarEstoque(estoqueDTO.Quantidade);
                }
                else
                {
                    produto.RemoverEstoque(-estoqueDTO.Quantidade);
                }

                await _produtoRepository.UpdateAsync(produto);

                var salvou = await _produtoRepository.SaveChangesAsync();
                if (!salvou)
                    throw new Exception("Não foi possível atualizar o estoque");
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task ExcluirProdutoAsync(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado");

            // Soft delete - apenas desativa
            produto.Desativar();

            await _produtoRepository.UpdateAsync(produto);

            var salvou = await _produtoRepository.SaveChangesAsync();
            if (!salvou)
                throw new Exception("Não foi possível excluir o produto");
        }

        public async Task<IEnumerable<ProdutoDTO>> BuscarProdutosAsync(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo) || termo.Length < 3)
                throw new ArgumentException("O termo de busca deve ter pelo menos 3 caracteres");

            var produtos = await _produtoRepository.SearchProdutosAsync(termo);
            return _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        }

        public async Task<IEnumerable<ProdutoDTO>> ObterProdutosRecentesAsync(int quantidade)
        {
            var produtos = await _produtoRepository.GetProdutosAtivosAsync();
            return _mapper.Map<IEnumerable<ProdutoDTO>>(
                produtos.OrderByDescending(p => p.DataCriacao).Take(quantidade));
        }

        public async Task<IEnumerable<ProdutoDTO>> ObterProdutosEmPromocaoAsync()
        {
            // Exemplo: produtos com mais de 10% de desconto
            var produtos = await _produtoRepository.GetProdutosAtivosAsync();
            return _mapper.Map<IEnumerable<ProdutoDTO>>(
                produtos.Where(p => p.Preco < 1000) // Exemplo simples
                       .OrderByDescending(p => p.DataCriacao)
                       .Take(10));
        }


        // NOVO: Método para obter query base (reutilizável)
        private async Task<IQueryable<Produto>> GetBaseQueryAsync(bool incluirCategoria = true)
        {
            var query = await _produtoRepository.GetQueryableAsync();

            if (!incluirCategoria)
            {
                // Se não precisar incluir categoria, refaz a query
                var produtos = await _produtoRepository.GetAllAsync();
                return produtos.AsQueryable();
            }

            return query;
        }

        // Método corrigido para paginação
        public async Task<PagedResult<ProdutoDTO>> ObterProdutosPaginadosAsync(PagedRequest request)
        {
            var query = await GetBaseQueryAsync();

            // Aplicar filtro padrão (apenas ativos)
            query = query.Where(p => p.Ativo);

            // Ordenação
            query = request.SortBy?.ToLower() switch
            {
                "preco" => request.SortDescending ? query.OrderByDescending(p => p.Preco)
                                                  : query.OrderBy(p => p.Preco),
                "datacriacao" => request.SortDescending ? query.OrderByDescending(p => p.DataCriacao)
                                                       : query.OrderBy(p => p.DataCriacao),
                "nome" => request.SortDescending ? query.OrderByDescending(p => p.Nome)
                                                : query.OrderBy(p => p.Nome),
                _ => query.OrderBy(p => p.Nome)
            };

            // Substitua esta linha:
            // var totalCount = await query.CountAsync();

            // Por esta linha:
            var totalCount = query.Count();

            // Paginação
            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList(); // ToList() já é síncrono porque é IQueryable

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(items);

            return new PagedResult<ProdutoDTO>
            {
                Items = produtosDTO,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        // Método corrigido para filtros
        public async Task<PagedResult<ProdutoDTO>> FiltrarProdutosAsync(ProdutoFiltroDTO filtro, PagedRequest paginacao)
        {
            var query = await GetBaseQueryAsync();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                query = query.Where(p => p.Nome.Contains(filtro.Nome));
            }

            if (!string.IsNullOrWhiteSpace(filtro.Descricao))
            {
                query = query.Where(p => p.Descricao.Contains(filtro.Descricao));
            }

            if (filtro.CategoriaId.HasValue)
            {
                query = query.Where(p => p.CategoriaId == filtro.CategoriaId.Value);
            }

            if (filtro.PrecoMinimo.HasValue)
            {
                query = query.Where(p => p.Preco >= filtro.PrecoMinimo.Value);
            }

            if (filtro.PrecoMaximo.HasValue)
            {
                query = query.Where(p => p.Preco <= filtro.PrecoMaximo.Value);
            }

            if (filtro.Ativo.HasValue)
            {
                query = query.Where(p => p.Ativo == filtro.Ativo.Value);
            }

            if (filtro.ComEstoque.HasValue && filtro.ComEstoque.Value)
            {
                query = query.Where(p => p.QuantidadeEstoque > 0);
            }

            // Ordenação
            query = filtro.OrdenarPor?.ToLower() switch
            {
                "preco" => filtro.OrdemDescendente ? query.OrderByDescending(p => p.Preco)
                                                  : query.OrderBy(p => p.Preco),
                "datacriacao" => filtro.OrdemDescendente ? query.OrderByDescending(p => p.DataCriacao)
                                                       : query.OrderBy(p => p.DataCriacao),
                "nome" => filtro.OrdemDescendente ? query.OrderByDescending(p => p.Nome)
                                                : query.OrderBy(p => p.Nome),
                _ => query.OrderBy(p => p.Nome)
            };

            // Contagem total (antes da paginação)
            var totalCount = query.Count();

            // Paginação
            var items = query
                .Skip((paginacao.PageNumber - 1) * paginacao.PageSize)
                .Take(paginacao.PageSize)
                .ToList();

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(items);

            return new PagedResult<ProdutoDTO>
            {
                Items = produtosDTO,
                TotalCount = totalCount,
                PageNumber = paginacao.PageNumber,
                PageSize = paginacao.PageSize
            };
        }

    }
}