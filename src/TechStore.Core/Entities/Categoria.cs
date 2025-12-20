namespace TechStore.Core.Entities
{
    public class Categoria : BaseEntity
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativa { get; private set; }
        public ICollection<Produto> Produtos { get; private set; } = new List<Produto>();

        // Construtor
        public Categoria(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
            Ativa = true;
        }

        // Métodos de domínio
        public void Atualizar(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
        }

        public void Ativar() => Ativa = true;
        public void Desativar() => Ativa = false;
        
    }
}
