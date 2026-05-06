using AutoMapper;
using VirtualShop.ProductApi.Models;

namespace VirtualShop.ProductApi.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>().ReverseMap();
    }
}
