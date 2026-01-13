using System;
using System.Collections.Generic;
using System.Linq;

namespace TechStore.Core.Entities
{
    public class Carrinho : BaseEntity
    {
        public string UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAtualizacao { get; private set; }
        public decimal ValorTotal => Itens.Sum(i => i.Subtotal);

        // Itens do carrinho
        public ICollection<CarrinhoItem> Itens { get; private set; } = new List<CarrinhoItem>();

        // Construtor
        public Carrinho(string usuarioId)
        {
            UsuarioId = usuarioId;
            DataCriacao = DateTime.UtcNow;
        }

        // Métodos de domínio
        public void AdicionarItem(Produto produto, int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero");

            if (produto.QuantidadeEstoque < quantidade)
                throw new InvalidOperationException($"Estoque insuficiente para {produto.Nome}. Disponível: {produto.QuantidadeEstoque}");

            var itemExistente = Itens.FirstOrDefault(i => i.ProdutoId == produto.Id);

            if (itemExistente != null)
            {
                itemExistente.AdicionarQuantidade(quantidade);
            }
            else
            {
                var novoItem = new CarrinhoItem(produto.Id, quantidade, produto.Preco);
                Itens.Add(novoItem);
            }

            DataAtualizacao = DateTime.UtcNow;
        }

        public void RemoverItem(int produtoId)
        {
            var item = Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (item != null)
            {
                Itens.Remove(item);
                DataAtualizacao = DateTime.UtcNow;
            }
        }

        public void AtualizarQuantidade(int produtoId, int novaQuantidade)
        {
            if (novaQuantidade <= 0)
            {
                RemoverItem(produtoId);
                return;
            }

            var item = Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (item != null)
            {
                // Verificar estoque (precisaríamos do produto aqui)
                item.AtualizarQuantidade(novaQuantidade);
                DataAtualizacao = DateTime.UtcNow;
            }
        }

        public void LimparCarrinho()
        {
            Itens.Clear();
            DataAtualizacao = DateTime.UtcNow;
        }

        public bool EstaVazio => !Itens.Any();

        // Construtor privado para EF
        private Carrinho() { }
    }
}