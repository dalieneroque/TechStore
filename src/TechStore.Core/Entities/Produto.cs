namespace TechStore.Core.Entities
{
    public class Produto : BaseEntity
    {   
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public int QuantidadeEstoque { get; private set; }
        public string ImagemUrl { get; private set; }
        public int CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public bool Ativo { get; private set; }

        //Contrutor para Entity Framework
        private Produto() { }

        //Contrutor principal
        public Produto(string nome, string descricao, decimal preco, int quantidadeEstoque, string imagemUrl, int categoriaId)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;
            ImagemUrl = imagemUrl;
            CategoriaId = categoriaId;
            DataCriacao = DateTime.UtcNow;
            Ativo = true;

            Validar();
        }

        //Métodos de domínio
        public void Atualizar(string nome, string descricao, decimal preco, string imagemUrl, int categoriaId)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;         
            ImagemUrl = imagemUrl;
            CategoriaId = categoriaId;

            Validar();
        }

        public void AdicionarEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");
                
            QuantidadeEstoque += quantidade;
        }

        public void RemoverEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            if (quantidade > QuantidadeEstoque)
                throw new InvalidOperationException("Estoque insuficiente.");

            QuantidadeEstoque -= quantidade;
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nome))
                throw new ArgumentException("O nome é obrigatório.");

            if (Preco <= 0)
                throw new ArgumentException("O preço deve ser maior que zero.");

            if (QuantidadeEstoque < 0)
                throw new ArgumentException("A quantidade em estoque não pode ser negativa.");
        }
    }
}
