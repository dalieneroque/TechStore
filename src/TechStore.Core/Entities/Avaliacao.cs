using System;

namespace TechStore.Core.Entities
{
    public class Avaliacao : BaseEntity
    {
        public int ProdutoId { get; private set; }
        public Produto Produto { get; private set; }
        public string UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }
        public int Nota { get; private set; } // 1-5
        public string Titulo { get; private set; }
        public string Comentario { get; private set; }
        public DateTime DataAvaliacao { get; private set; }
        public bool Aprovada { get; private set; }

        // Construtor
        public Avaliacao(int produtoId, string usuarioId, int nota, string titulo, string comentario)
        {
            ProdutoId = produtoId;
            UsuarioId = usuarioId;
            Nota = nota;
            Titulo = titulo;
            Comentario = comentario;
            DataAvaliacao = DateTime.UtcNow;
            Aprovada = false; // Precisa ser aprovada por admin
        }

        // Métodos de domínio
        public void AtualizarAvaliacao(int nota, string titulo, string comentario)
        {
            Nota = nota;
            Titulo = titulo;
            Comentario = comentario;
        }

        public void Aprovar() => Aprovada = true;
        public void Reprovar() => Aprovada = false;

        public bool IsValida()
        {
            return Nota >= 1 && Nota <= 5
                && !string.IsNullOrWhiteSpace(Titulo)
                && !string.IsNullOrWhiteSpace(Comentario)
                && Comentario.Length <= 1000;
        }

        // Construtor privado para EF
        private Avaliacao() { }
    }
}