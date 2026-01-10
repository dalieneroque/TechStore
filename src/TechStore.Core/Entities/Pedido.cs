using System;
using System.Collections.Generic;
using System.Linq;

namespace TechStore.Core.Entities
{
    public class Pedido : BaseEntity
    {
        public string UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }
        public DateTime DataPedido { get; private set; }
        public DateTime? DataPagamento { get; private set; }
        public DateTime? DataEnvio { get; private set; }
        public DateTime? DataEntrega { get; private set; }

        // Status do pedido
        public string Status { get; private set; }
        public decimal ValorTotal { get; private set; }
        public string Observacoes { get; private set; }

        // Endereço de entrega (poderia ser separado)
        public string EnderecoEntrega { get; private set; }

        // Itens do pedido
        public ICollection<ItemPedido> Itens { get; private set; } = new List<ItemPedido>();

        // Construtor
        public Pedido(string usuarioId, string enderecoEntrega, string observacoes = "")
        {
            UsuarioId = usuarioId;
            DataPedido = DateTime.UtcNow;
            Status = "Pendente";
            ValorTotal = 0;
            Observacoes = observacoes;
            EnderecoEntrega = enderecoEntrega;
        }

        // Métodos de domínio
        public void AdicionarItem(Produto produto, int quantidade, decimal precoUnitario)
        {
            if (produto.QuantidadeEstoque < quantidade)
                throw new InvalidOperationException($"Estoque insuficiente para {produto.Nome}");

            var item = new ItemPedido(Id, produto.Id, quantidade, precoUnitario);
            Itens.Add(item);

            // Atualizar valor total
            ValorTotal += item.Subtotal;

            // Atualizar estoque do produto
            produto.RemoverEstoque(quantidade);
        }

        public void RemoverItem(int produtoId)
        {
            var item = Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (item != null)
            {
                ValorTotal -= item.Subtotal;
                Itens.Remove(item);
            }
        }

        public void AtualizarStatus(string novoStatus)
        {
            Status = novoStatus;

            // Atualizar datas baseadas no status
            switch (novoStatus.ToLower())
            {
                case "pago":
                    DataPagamento = DateTime.UtcNow;
                    break;
                case "enviado":
                    DataEnvio = DateTime.UtcNow;
                    break;
                case "entregue":
                    DataEntrega = DateTime.UtcNow;
                    break;
            }
        }

        public void Cancelar()
        {
            if (Status == "Entregue")
                throw new InvalidOperationException("Não é possível cancelar um pedido já entregue");

            Status = "Cancelado";

            // Devolver itens ao estoque
            foreach (var item in Itens)
            {
                // Aqui precisaríamos do repositório de produtos
                // Para simplificar, apenas marcamos que precisa devolver
            }
        }
    }
}