using Microsoft.EntityFrameworkCore;
using TechStore.Core.Entities;

namespace TechStore.Infrastructure.Data
{
    public class TechStoreDbContext : DbContext
    {
        public TechStoreDbContext(DbContextOptions<TechStoreDbContext> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
