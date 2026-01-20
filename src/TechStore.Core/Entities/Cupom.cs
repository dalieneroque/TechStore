using System;

namespace TechStore.Core.Entities
{
    public class Cupom : BaseEntity
    {
        public string Codigo { get; private set; }
        public string Descricao { get; private set; }
        public TipoDesconto Tipo { get; private set; }
        public decimal Valor { get; private set; }
        public decimal? ValorMinimoPedido { get; private set; }
        public int? UsosMaximos { get; private set; }
        public int UsosAtuais { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAtualizacao { get; private set; }

        // Tipos de desconto
        public enum TipoDesconto
        {
            Porcentagem,  // Ex: 10% de desconto
            ValorFixo     // Ex: R$ 20,00 de desconto
        }

        // Construtor
        public Cupom(string codigo, string descricao, TipoDesconto tipo, decimal valor, DateTime dataValidade, decimal? valorMinimoPedido = null, int? usosMaximos = null)
        {
            Codigo = codigo;
            Descricao = descricao;
            Tipo = tipo;
            Valor = valor;
            DataValidade = dataValidade;
            ValorMinimoPedido = valorMinimoPedido;
            UsosMaximos = usosMaximos;
            UsosAtuais = 0;
            Ativo = true;
            DataCriacao = DateTime.UtcNow;
        }

        // Métodos de domínio
        public decimal CalcularDesconto(decimal valorTotal)
        {
            if (!PodeSerUtilizado(valorTotal))
                return 0;

            if (Tipo == TipoDesconto.Porcentagem)
            {
                return valorTotal * (Valor / 100);
            }
            else // ValorFixo
            {
                return Valor;
            }
        }

        public bool PodeSerUtilizado(decimal valorTotal)
        {
            if (!Ativo)
                return false;

            if (DateTime.UtcNow > DataValidade)
                return false;

            if (ValorMinimoPedido.HasValue && valorTotal < ValorMinimoPedido.Value)
                return false;

            if (UsosMaximos.HasValue && UsosAtuais >= UsosMaximos.Value)
                return false;

            return true;
        }

        public void Utilizar()
        {
            if (!Ativo)
                throw new InvalidOperationException("Cupom não está ativo");

            UsosAtuais++;
            DataAtualizacao = DateTime.UtcNow;
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;

        public void Atualizar(string descricao, decimal? valorMinimoPedido, int? usosMaximos, DateTime dataValidade)
        {
            Descricao = descricao;
            ValorMinimoPedido = valorMinimoPedido;
            UsosMaximos = usosMaximos;
            DataValidade = dataValidade;
            DataAtualizacao = DateTime.UtcNow;
        }

        // Construtor privado para EF
        private Cupom() { }
    }
}