using System;

namespace TechStore.Application.DTOs
{
    public class FavoritoDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public decimal ProdutoPreco { get; set; }
        public string ProdutoImagemUrl { get; set; }
        public string ProdutoCategoria { get; set; }
        public DateTime DataAdicao { get; set; }
    }

    public class AdicionarFavoritoDTO
    {
        public int ProdutoId { get; set; }
    }
}