using System;

namespace TechStore.Core.Entities
{
    public class Favorito : BaseEntity
    {
        public string UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }
        public int ProdutoId { get; private set; }
        public Produto Produto { get; private set; }
        public DateTime DataAdicao { get; private set; }

        public Favorito(string usuarioId, int produtoId)
        {
            UsuarioId = usuarioId;
            ProdutoId = produtoId;
            DataAdicao = DateTime.UtcNow;
        }

        // Construtor privado para EF
        private Favorito() { }
    }
}