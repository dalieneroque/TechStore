namespace TechStore.Core.Entities
{
    public class ItemPedido : BaseEntity
    {
        public int PedidoId { get; private set; }
        public Pedido Pedido { get; private set; }
        public int ProdutoId { get; private set; }
        public Produto Produto { get; private set; }
        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;

        public ItemPedido(int pedidoId, int produtoId, int quantidade, decimal precoUnitario)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }

        // Construtor privado para EF
        private ItemPedido() { }

        public void AtualizarQuantidade(int novaQuantidade)
        {
            if (novaQuantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero");

            Quantidade = novaQuantidade;
        }
    }
}