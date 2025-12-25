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
        }     
    }
}