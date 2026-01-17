using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechStore.Core.Entities;

namespace TechStore.Infrastructure.Data
{
    public class TechStoreDbContext : IdentityDbContext<Usuario>
    {
        public TechStoreDbContext(DbContextOptions<TechStoreDbContext> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Carrinho> Carrinhos { get; set; }
        public DbSet<CarrinhoItem> CarrinhoItens { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // Ensina ao EF Core como as entidades se relacionam no banco de dados
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechStoreDbContext).Assembly);

            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Itens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Carrinho>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Carrinho>()
                .HasMany(c => c.Itens)
                .WithOne(i => i.Carrinho)
                .HasForeignKey(i => i.CarrinhoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CarrinhoItem>()
                .HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Avaliacao>() //Configura a entidade Avaliacao
                .HasOne(a => a.Produto)     // Uma Avaliacao tem um Produto
                .WithMany(p => p.Avaliacoes) // Um Produto tem muitas Avaliacoes
                .HasForeignKey(a => a.ProdutoId) // Chave estrangeira esta em Avaliacao
                .OnDelete(DeleteBehavior.Cascade); // Se o Produto for deletado, todas as Avaliacoes relacionadas também serão deletadas

            modelBuilder.Entity<Avaliacao>()
                .HasOne(a => a.Usuario) 
                .WithMany() 
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); // Não permite deletar um usuário se ele tiver avaliações

            modelBuilder.Entity<Favorito>()
                .HasOne(f => f.Usuario)
                .WithMany()
                .HasForeignKey(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Favorito>()
                .HasOne(f => f.Produto)
                .WithMany()
                .HasForeignKey(f => f.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Garantir que um usuário não possa favoritar o mesmo produto duas vezes
            modelBuilder.Entity<Favorito>()
                .HasIndex(f => new { f.UsuarioId, f.ProdutoId }) 
                .IsUnique(); 

        }
    }
}
