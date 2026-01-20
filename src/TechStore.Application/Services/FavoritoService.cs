using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Application.Services
{
    public class FavoritoService : IFavoritoService
    {
        private readonly IFavoritoRepository _favoritoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        private readonly TechStoreDbContext _context;

        public FavoritoService(
            IFavoritoRepository favoritoRepository,
            IProdutoRepository produtoRepository,
            IMapper mapper,
            TechStoreDbContext context)
        {
            _favoritoRepository = favoritoRepository;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<FavoritoDTO>> ObterFavoritosPorUsuarioAsync(string usuarioId)
        {
            var favoritos = await _favoritoRepository.ObterFavoritosPorUsuarioAsync(usuarioId);
            return _mapper.Map<IEnumerable<FavoritoDTO>>(favoritos);
        }

        public async Task<FavoritoDTO> AdicionarFavoritoAsync(string usuarioId, int produtoId)
        {
            // Verificar se produto existe
            var produto = await _produtoRepository.GetByIdAsync(produtoId);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {produtoId} não encontrado");

            // Verificar se já está favoritado
            var jaFavoritado = await _favoritoRepository.ProdutoEstaFavoritadoAsync(usuarioId, produtoId);
            if (jaFavoritado)
                throw new InvalidOperationException("Produto já está nos favoritos");

            // Adicionar aos favoritos
            var favorito = new Favorito(usuarioId, produtoId);
            await _favoritoRepository.AddAsync(favorito);
            await _favoritoRepository.SaveChangesAsync();

            // Recarregar com includes
            var favoritoCompleto = await _context.Favoritos
                .Include(f => f.Produto)
                    .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(f => f.Id == favorito.Id);

            return _mapper.Map<FavoritoDTO>(favoritoCompleto);
        }

        public async Task<bool> RemoverFavoritoAsync(string usuarioId, int produtoId)
        {
            return await _favoritoRepository.RemoverFavoritoAsync(usuarioId, produtoId);
        }

        public async Task<bool> ProdutoEstaFavoritadoAsync(string usuarioId, int produtoId)
        {
            return await _favoritoRepository.ProdutoEstaFavoritadoAsync(usuarioId, produtoId);
        }

        public async Task<int> ContarFavoritosPorUsuarioAsync(string usuarioId)
        {
            return await _context.Favoritos
                .CountAsync(f => f.UsuarioId == usuarioId);
        }
    }
}