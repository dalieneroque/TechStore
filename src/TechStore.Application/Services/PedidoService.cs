using AutoMapper;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICarrinhoRepository _carrinhoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        private readonly TechStoreDbContext _context;

        public PedidoService(IPedidoRepository pedidoRepository, ICarrinhoRepository carrinhoRepository, IProdutoRepository produtoRepository, IMapper mapper, TechStoreDbContext context)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoRepository = carrinhoRepository;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<PedidoDTO> CriarPedidoAsync(string usuarioId, CriarPedidoDTO pedidoDTO)
        {
            // Obter carrinho
            var carrinho = await _carrinhoRepository.ObterCarrinhoComItensAsync(usuarioId);
            if (carrinho == null || carrinho.EstaVazio)
                throw new InvalidOperationException("Carrinho vazio. Adicione produtos antes de finalizar.");

            // Criar pedido
            var pedido = new Pedido(usuarioId, pedidoDTO.EnderecoEntrega, pedidoDTO.Observacoes);

            // Transferir itens do carrinho para o pedido
            foreach (var item in carrinho.Itens)
            {
                var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto ID {item.ProdutoId} não encontrado");

                pedido.AdicionarItem(produto, item.Quantidade, item.PrecoUnitario);
            }

            // Salvar pedido
            await _pedidoRepository.AddAsync(pedido);
            await _pedidoRepository.SaveChangesAsync();

            // Limpar carrinho
            await _carrinhoRepository.LimparCarrinhoAsync(usuarioId);

            // Retornar pedido criado
            var pedidoCompleto = await _pedidoRepository.ObterPedidoComItensAsync(pedido.Id);
            return _mapper.Map<PedidoDTO>(pedidoCompleto);
        }

        public async Task<IEnumerable<PedidoDTO>> ObterPedidosPorUsuarioAsync(string usuarioId)
        {
            var pedidos = await _pedidoRepository.ObterPedidosPorUsuarioIdAsync(usuarioId);
            return _mapper.Map<IEnumerable<PedidoDTO>>(pedidos);
        }

        public async Task<PedidoDTO> ObterPedidoPorIdAsync(int pedidoId, string usuarioId = null)
        {
            var pedido = await _pedidoRepository.ObterPedidoComItensAsync(pedidoId);

            if (pedido == null)
                throw new KeyNotFoundException($"Pedido com ID {pedidoId} não encontrado");

            // Verificar se o usuário tem acesso (se fornecido)
            if (usuarioId != null && pedido.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("Você não tem permissão para acessar este pedido");

            return _mapper.Map<PedidoDTO>(pedido);
        }

        public async Task<PedidoDTO> AtualizarStatusAsync(int pedidoId, AtualizarStatusPedidoDTO statusDTO, bool isAdmin = false)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
                throw new KeyNotFoundException($"Pedido com ID {pedidoId} não encontrado");

            // Validações de status
            var statusValidos = new[] { "Pendente", "Pago", "Processando", "Enviado", "Entregue", "Cancelado" };
            if (!statusValidos.Contains(statusDTO.Status))
                throw new ArgumentException($"Status inválido. Use: {string.Join(", ", statusValidos)}");

            // Cliente só pode cancelar pedidos pendentes
            if (!isAdmin && statusDTO.Status == "Cancelado" && pedido.Status != "Pendente")
                throw new InvalidOperationException("Só é possível cancelar pedidos pendentes");

            pedido.AtualizarStatus(statusDTO.Status);
            await _pedidoRepository.UpdateAsync(pedido);
            await _pedidoRepository.SaveChangesAsync();

            var pedidoAtualizado = await _pedidoRepository.ObterPedidoComItensAsync(pedidoId);
            return _mapper.Map<PedidoDTO>(pedidoAtualizado);
        }

        public async Task<IEnumerable<PedidoDTO>> ObterPedidosPorStatusAsync(string status)
        {
            var pedidos = await _pedidoRepository.ObterPedidosPorStatusAsync(status);
            return _mapper.Map<IEnumerable<PedidoDTO>>(pedidos);
        }

        public async Task<int> ContarPedidosPorUsuarioAsync(string usuarioId)
        {
            return await _pedidoRepository.ContarPedidosPorUsuarioAsync(usuarioId);
        }
    }
}