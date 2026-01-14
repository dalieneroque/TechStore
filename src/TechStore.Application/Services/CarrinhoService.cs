using AutoMapper;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;


namespace TechStore.Application.Services
{
    public class CarrinhoService : ICarrinhoService
    {
        private readonly ICarrinhoRepository _carrinhoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        private readonly TechStoreDbContext _context;

        public CarrinhoService(ICarrinhoRepository carrinhoRepository, IProdutoRepository produtoRepository, IMapper mapper, TechStoreDbContext context)
        {
            _carrinhoRepository = carrinhoRepository;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<CarrinhoDTO> ObterCarrinhoAsync(string usuarioId)
        {
            var carrinho = await _carrinhoRepository.ObterCarrinhoComItensAsync(usuarioId);

            // Se não existir, criar um novo
            if (carrinho == null)
            {
                carrinho = new Carrinho(usuarioId);
                await _carrinhoRepository.AddAsync(carrinho);
                await _carrinhoRepository.SaveChangesAsync();

                // Recarregar com includes
                carrinho = await _carrinhoRepository.ObterCarrinhoComItensAsync(usuarioId);
            }

            return _mapper.Map<CarrinhoDTO>(carrinho);
        }

        public async Task<CarrinhoDTO> AdicionarItemAsync(string usuarioId, AdicionarItemCarrinhoDTO itemDTO)
        {
            var carrinho = await ObterOuCriarCarrinhoAsync(usuarioId);
            var produto = await _produtoRepository.GetByIdAsync(itemDTO.ProdutoId);

            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {itemDTO.ProdutoId} não encontrado");

            if (!produto.Ativo)
                throw new InvalidOperationException($"Produto {produto.Nome} não está disponível");

            if (produto.QuantidadeEstoque < itemDTO.Quantidade)
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {produto.QuantidadeEstoque}");

            carrinho.AdicionarItem(produto, itemDTO.Quantidade);
            await _carrinhoRepository.UpdateAsync(carrinho);
            await _carrinhoRepository.SaveChangesAsync();

            return await ObterCarrinhoAsync(usuarioId);
        }

        public async Task<CarrinhoDTO> RemoverItemAsync(string usuarioId, int produtoId)
        {
            var carrinho = await ObterOuCriarCarrinhoAsync(usuarioId);
            carrinho.RemoverItem(produtoId);

            await _carrinhoRepository.UpdateAsync(carrinho);
            await _carrinhoRepository.SaveChangesAsync();

            return await ObterCarrinhoAsync(usuarioId);
        }

        public async Task<CarrinhoDTO> AtualizarQuantidadeAsync(string usuarioId, AtualizarItemCarrinhoDTO itemDTO)
        {
            var carrinho = await ObterOuCriarCarrinhoAsync(usuarioId);

            if (itemDTO.Quantidade == 0)
            {
                carrinho.RemoverItem(itemDTO.ProdutoId);
            }
            else
            {
                // Verificar estoque
                var produto = await _produtoRepository.GetByIdAsync(itemDTO.ProdutoId);
                if (produto != null && produto.QuantidadeEstoque < itemDTO.Quantidade)
                    throw new InvalidOperationException($"Estoque insuficiente. Disponível: {produto.QuantidadeEstoque}");

                carrinho.AtualizarQuantidade(itemDTO.ProdutoId, itemDTO.Quantidade);
            }

            await _carrinhoRepository.UpdateAsync(carrinho);
            await _carrinhoRepository.SaveChangesAsync();

            return await ObterCarrinhoAsync(usuarioId);
        }

        public async Task<CarrinhoDTO> LimparCarrinhoAsync(string usuarioId)
        {
            var carrinho = await ObterOuCriarCarrinhoAsync(usuarioId);
            carrinho.LimparCarrinho();

            await _carrinhoRepository.UpdateAsync(carrinho);
            await _carrinhoRepository.SaveChangesAsync();

            return await ObterCarrinhoAsync(usuarioId);
        }

        public async Task<int> ObterQuantidadeItensAsync(string usuarioId)
        {
            var carrinho = await _carrinhoRepository.ObterCarrinhoComItensAsync(usuarioId);
            return carrinho?.Itens.Sum(i => i.Quantidade) ?? 0;
        }

        private async Task<Carrinho> ObterOuCriarCarrinhoAsync(string usuarioId)
        {
            var carrinho = await _carrinhoRepository.ObterCarrinhoComItensAsync(usuarioId);

            if (carrinho == null)
            {
                carrinho = new Carrinho(usuarioId);
                await _carrinhoRepository.AddAsync(carrinho);
                await _carrinhoRepository.SaveChangesAsync();
            }

            return carrinho;
        }
    }
}