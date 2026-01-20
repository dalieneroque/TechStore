using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
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
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        private readonly TechStoreDbContext _context;

        public AvaliacaoService(
            IAvaliacaoRepository avaliacaoRepository,
            IProdutoRepository produtoRepository,
            IMapper mapper,
            TechStoreDbContext context)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<AvaliacaoDTO> CriarAvaliacaoAsync(string usuarioId, CriarAvaliacaoDTO avaliacaoDTO)
        {
            // Verificar se produto existe
            var produto = await _produtoRepository.GetByIdAsync(avaliacaoDTO.ProdutoId);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {avaliacaoDTO.ProdutoId} não encontrado");

            // Verificar se usuário já avaliou este produto
            var avaliacaoExistente = await _context.Avaliacoes
                .FirstOrDefaultAsync(a => a.UsuarioId == usuarioId && a.ProdutoId == avaliacaoDTO.ProdutoId);

            if (avaliacaoExistente != null)
                throw new InvalidOperationException("Você já avaliou este produto");

            // Criar avaliação
            var avaliacao = new Avaliacao(
                avaliacaoDTO.ProdutoId,
                usuarioId,
                avaliacaoDTO.Nota,
                avaliacaoDTO.Titulo,
                avaliacaoDTO.Comentario);

            if (!avaliacao.IsValida())
                throw new ArgumentException("Avaliação inválida");

            await _avaliacaoRepository.AddAsync(avaliacao);
            await _avaliacaoRepository.SaveChangesAsync();

            var avaliacaoCompleta = await _context.Avaliacoes
                .Include(a => a.Produto)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Id == avaliacao.Id);

            return _mapper.Map<AvaliacaoDTO>(avaliacaoCompleta);
        }

        public async Task<ProdutoAvaliacaoDTO> ObterAvaliacoesPorProdutoAsync(int produtoId)
        {
            var produto = await _produtoRepository.GetByIdAsync(produtoId);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {produtoId} não encontrado");

            var avaliacoes = await _avaliacaoRepository.ObterAvaliacoesPorProdutoAsync(produtoId);
            var avaliacoesDTO = _mapper.Map<List<AvaliacaoDTO>>(avaliacoes);

            return new ProdutoAvaliacaoDTO
            {
                MediaNotas = produto.MediaAvaliacoes,
                TotalAvaliacoes = produto.TotalAvaliacoes,
                Avaliacoes = avaliacoesDTO
            };
        }

        public async Task<IEnumerable<AvaliacaoDTO>> ObterAvaliacoesPendentesAsync()
        {
            var avaliacoes = await _avaliacaoRepository.ObterAvaliacoesPendentesAsync();
            return _mapper.Map<IEnumerable<AvaliacaoDTO>>(avaliacoes);
        }

        public async Task<AvaliacaoDTO> AprovarReprovarAvaliacaoAsync(int avaliacaoId, bool aprovar)
        {
            var avaliacao = await _avaliacaoRepository.GetByIdAsync(avaliacaoId);
            if (avaliacao == null)
                throw new KeyNotFoundException($"Avaliação com ID {avaliacaoId} não encontrada");

            if (aprovar)
                avaliacao.Aprovar();
            else
                avaliacao.Reprovar();

            await _avaliacaoRepository.UpdateAsync(avaliacao);
            await _avaliacaoRepository.SaveChangesAsync();

            var avaliacaoAtualizada = await _context.Avaliacoes
                .Include(a => a.Produto)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Id == avaliacaoId);

            return _mapper.Map<AvaliacaoDTO>(avaliacaoAtualizada);
        }

        public async Task<IEnumerable<AvaliacaoDTO>> ObterAvaliacoesPorUsuarioAsync(string usuarioId)
        {
            var avaliacoes = await _avaliacaoRepository.ObterAvaliacoesPorUsuarioAsync(usuarioId);
            return _mapper.Map<IEnumerable<AvaliacaoDTO>>(avaliacoes);
        }

        public async Task<bool> ExcluirAvaliacaoAsync(int avaliacaoId, string usuarioId, bool isAdmin = false)
        {
            var avaliacao = await _avaliacaoRepository.GetByIdAsync(avaliacaoId);
            if (avaliacao == null)
                throw new KeyNotFoundException($"Avaliação com ID {avaliacaoId} não encontrada");

            // Verificar permissão
            if (!isAdmin && avaliacao.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("Você não tem permissão para excluir esta avaliação");

            await _avaliacaoRepository.DeleteAsync(avaliacao);
            await _avaliacaoRepository.SaveChangesAsync();

            return true;
        }
    }
}