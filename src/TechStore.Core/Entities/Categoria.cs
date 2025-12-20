namespace TechStore.Core.Entities
{
    public class Categoria : BaseEntity
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }

        // Construtor
        public Categoria(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
            Ativo = true;
        }

        // Métodos de domínio
        public void Atualizar(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;
        
    }
}
