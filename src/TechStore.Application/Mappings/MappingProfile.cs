using AutoMapper;
using TechStore.Application.DTOs;
using TechStore.Core.Entities;

namespace TechStore.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<CriarCategoriaDTO, Categoria>();
            CreateMap<AtualizarCategoriaDTO, Categoria>();


            CreateMap<Produto, ProdutoDTO>()
                .ForMember(dest => dest.CategoriaNome, 
                           opt => opt.MapFrom(src => src.Categoria.Nome))
                .ReverseMap();

            CreateMap<CriarProdutoDTO, Produto>();
            CreateMap<AtualizarProdutoDTO, Produto>();

            // Mapeamentos de Carrinho
            CreateMap<Carrinho, CarrinhoDTO>()
                .ForMember(dest => dest.TotalItens, opt => opt.MapFrom(src => src.Itens.Sum(i => i.Quantidade)))
                .ReverseMap();

            CreateMap<CarrinhoItem, CarrinhoItemDTO>()
                .ForMember(dest => dest.ProdutoNome, opt => opt.MapFrom(src => src.Produto.Nome))
                .ForMember(dest => dest.ProdutoImagemUrl, opt => opt.MapFrom(src => src.Produto.ImagemUrl))
                .ForMember(dest => dest.EstoqueDisponivel, opt => opt.MapFrom(src => src.Produto.QuantidadeEstoque));

            // Mapeamentos de Pedido
            CreateMap<Pedido, PedidoDTO>()
                .ForMember(dest => dest.UsuarioNome, opt => opt.MapFrom(src => src.Usuario.NomeCompleto));

            CreateMap<ItemPedido, ItemPedidoDTO>()
                .ForMember(dest => dest.ProdutoNome, opt => opt.MapFrom(src => src.Produto.Nome))
                .ForMember(dest => dest.ProdutoImagemUrl, opt => opt.MapFrom(src => src.Produto.ImagemUrl));



        }     
    }
}