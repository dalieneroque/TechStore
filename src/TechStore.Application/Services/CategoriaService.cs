using AutoMapper;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;

namespace TechStore.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoriaDTO>> ObterTodasCategoriasAsync()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
        }

        public async Task<IEnumerable<CategoriaDTO>> ObterCategoriasAtivasAsync()
        {
            var categorias = await _categoriaRepository.GetCategoriasAtivasAsync();
            return _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
        }

        public async Task<CategoriaDTO> ObterCategoriaPorIdAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
                throw new KeyNotFoundException($"Categoria com ID {id} não encontrada");

            return _mapper.Map<CategoriaDTO>(categoria);
        }

        public async Task<CategoriaDTO> CriarCategoriaAsync(CriarCategoriaDTO categoriaDTO)
        {
            // Validações de negócio
            var categoriaExistente = await _categoriaRepository.FindAsync(c =>
                c.Nome.ToLower() == categoriaDTO.Nome.ToLower());

            if (categoriaExistente.Any())
                throw new InvalidOperationException($"Já existe uma categoria com o nome '{categoriaDTO.Nome}'");

            var categoria = _mapper.Map<Categoria>(categoriaDTO);
            await _categoriaRepository.AddAsync(categoria);

            var salvou = await _categoriaRepository.SaveChangesAsync();
            if (!salvou)
                throw new Exception("Não foi possível criar a categoria");

            return _mapper.Map<CategoriaDTO>(categoria);
        }

        public async Task AtualizarCategoriaAsync(int id, AtualizarCategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.Id)
                throw new ArgumentException("ID na rota não corresponde ao ID no DTO");

            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
                throw new KeyNotFoundException($"Categoria com ID {id} não encontrada");

            // Verificar se outro categoria já tem este nome
            var categoriaComMesmoNome = await _categoriaRepository.FindAsync(c =>
                c.Nome.ToLower() == categoriaDTO.Nome.ToLower() && c.Id != id);

            if (categoriaComMesmoNome.Any())
                throw new InvalidOperationException($"Já existe outra categoria com o nome '{categoriaDTO.Nome}'");

            _mapper.Map(categoriaDTO, categoria);

            if (categoriaDTO.Ativa)
                categoria.Ativar();
            else
                categoria.Desativar();

            await _categoriaRepository.UpdateAsync(categoria);

            var salvou = await _categoriaRepository.SaveChangesAsync();
            if (!salvou)
                throw new Exception("Não foi possível atualizar a categoria");
        }

        public async Task ExcluirCategoriaAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
                throw new KeyNotFoundException($"Categoria com ID {id} não encontrada");

            // Verificar se existem produtos nesta categoria
            var produtosNaCategoria = await _categoriaRepository.GetCategoriaComProdutosAsync(id);
            if (produtosNaCategoria?.Produtos?.Any() == true)
                throw new InvalidOperationException(
                    $"Não é possível excluir a categoria '{categoria.Nome}' porque existem produtos vinculados a ela");

            await _categoriaRepository.DeleteAsync(categoria);

            var salvou = await _categoriaRepository.SaveChangesAsync();
            if (!salvou)
                throw new Exception("Não foi possível excluir a categoria");
        }

        public async Task<bool> CategoriaExisteAsync(int id)
        {
            return await _categoriaRepository.ExistsAsync(id);
        }
    }
}
