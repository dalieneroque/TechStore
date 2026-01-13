namespace TechStore.Core.Entities
{
    public class CarrinhoItem : BaseEntity
    {
        public int CarrinhoId { get; private set; }
        public Carrinho Carrinho { get; private set; }
        public int ProdutoId { get; private set; }
        public Produto Produto { get; private set; }
        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;

        public CarrinhoItem(int produtoId, int quantidade, decimal precoUnitario)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }

        // Métodos
        public void AdicionarQuantidade(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero");

            Quantidade += quantidade;
        }

        public void AtualizarQuantidade(int novaQuantidade)
        {
            if (novaQuantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero");

            Quantidade = novaQuantidade;
        }

        public void AtualizarPreco(decimal novoPreco)
        {
            if (novoPreco <= 0)
                throw new ArgumentException("Preço deve ser maior que zero");

            PrecoUnitario = novoPreco;
        }

        // Construtor privado para EF
        private CarrinhoItem() { }
    }
}