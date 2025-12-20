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
        }
    }
}