using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Application.Services
{
    public class CupomService : ICupomService
    {
        private readonly ICupomRepository _cupomRepository;
        private readonly IMapper _mapper;
        private readonly TechStoreDbContext _context;

        public CupomService(
            ICupomRepository cupomRepository,
            IMapper mapper,
            TechStoreDbContext context)
        {
            _cupomRepository = cupomRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<CupomDTO> CriarCupomAsync(CriarCupomDTO cupomDTO)
        {
            // Verificar se código já existe
            var cupomExistente = await _cupomRepository.ObterCupomPorCodigoAsync(cupomDTO.Codigo);
            if (cupomExistente != null)
                throw new InvalidOperationException($"Já existe um cupom com o código '{cupomDTO.Codigo}'");

            // Converter string para enum
            if (!Enum.TryParse<Cupom.TipoDesconto>(cupomDTO.Tipo, out var tipoDesconto))
                throw new ArgumentException($"Tipo de desconto inválido: {cupomDTO.Tipo}");

            // Criar cupom
            var cupom = new Cupom(
                cupomDTO.Codigo,
                cupomDTO.Descricao,
                tipoDesconto,
                cupomDTO.Valor,
                cupomDTO.DataValidade,
                cupomDTO.ValorMinimoPedido,
                cupomDTO.UsosMaximos);

            await _cupomRepository.AddAsync(cupom);
            await _cupomRepository.SaveChangesAsync();

            return _mapper.Map<CupomDTO>(cupom);
        }

        public async Task<ValidarCupomResponseDTO> ValidarCupomAsync(ValidarCupomDTO validarDTO)
        {
            var cupom = await _cupomRepository.ObterCupomPorCodigoAsync(validarDTO.CodigoCupom);

            if (cupom == null)
            {
                return new ValidarCupomResponseDTO
                {
                    Valido = false,
                    Mensagem = "Cupom não encontrado"
                };
            }

            if (!cupom.PodeSerUtilizado(validarDTO.ValorTotal))
            {
                return new ValidarCupomResponseDTO
                {
                    Valido = false,
                    Mensagem = "Cupom não pode ser utilizado"
                };
            }

            var desconto = cupom.CalcularDesconto(validarDTO.ValorTotal);
            var valorFinal = validarDTO.ValorTotal - desconto;

            return new ValidarCupomResponseDTO
            {
                Valido = true,
                Mensagem = "Cupom válido",
                ValorDesconto = desconto,
                ValorFinal = valorFinal,
                Cupom = _mapper.Map<CupomDTO>(cupom)
            };
        }

        public async Task<IEnumerable<CupomDTO>> ObterCuponsAtivosAsync()
        {
            var cupons = await _cupomRepository.ObterCuponsAtivosAsync();
            return _mapper.Map<IEnumerable<CupomDTO>>(cupons);
        }

        public async Task<CupomDTO> ObterCupomPorIdAsync(int id)
        {
            var cupom = await _cupomRepository.GetByIdAsync(id);
            if (cupom == null)
                throw new KeyNotFoundException($"Cupom com ID {id} não encontrado");

            return _mapper.Map<CupomDTO>(cupom);
        }

        public async Task<CupomDTO> AtualizarCupomAsync(int id, CupomDTO cupomDTO)
        {
            var cupom = await _cupomRepository.GetByIdAsync(id);
            if (cupom == null)
                throw new KeyNotFoundException($"Cupom com ID {id} não encontrado");

            // Verificar se novo código já existe (se foi alterado)
            if (cupom.Codigo != cupomDTO.Codigo)
            {
                var cupomComMesmoCodigo = await _cupomRepository.ObterCupomPorCodigoAsync(cupomDTO.Codigo);
                if (cupomComMesmoCodigo != null && cupomComMesmoCodigo.Id != id)
                    throw new InvalidOperationException($"Já existe outro cupom com o código '{cupomDTO.Codigo}'");
            }

            // Atualizar propriedades
            cupom.Atualizar(
                cupomDTO.Descricao,
                cupomDTO.ValorMinimoPedido,
                cupomDTO.UsosMaximos,
                cupomDTO.DataValidade);

            if (cupomDTO.Ativo)
                cupom.Ativar();
            else
                cupom.Desativar();

            await _cupomRepository.UpdateAsync(cupom);
            await _cupomRepository.SaveChangesAsync();

            return _mapper.Map<CupomDTO>(cupom);
        }

        public async Task<bool> ExcluirCupomAsync(int id)
        {
            var cupom = await _cupomRepository.GetByIdAsync(id);
            if (cupom == null)
                throw new KeyNotFoundException($"Cupom com ID {id} não encontrado");

            // Não excluir fisicamente, apenas desativar
            cupom.Desativar();

            await _cupomRepository.UpdateAsync(cupom);
            await _cupomRepository.SaveChangesAsync();

            return true;
        }

        public async Task<int> ContarUsosCupomAsync(int cupomId)
        {
            return await _cupomRepository.ContarUsosCupomAsync(cupomId);
        }
    }
}