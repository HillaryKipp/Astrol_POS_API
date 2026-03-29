using AstrolPOSAPI.Application.Features.POS.ItemCategory.Commands.CreateItemCategory;
using AstrolPOSAPI.Application.Features.POS.ItemCategory.Commands.UpdateItemCategory;
using AstrolPOSAPI.Application.Features.POS.ItemCategory.DTOs;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.POS.ItemCategory
{
    public class ItemCategoryMappingProfile : Profile
    {
        public ItemCategoryMappingProfile()
        {
            CreateMap<Domain.Entities.POS.ItemCategory, ItemCategoryDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null))
                .ForMember(dest => dest.StoreOfOperationName, opt => opt.MapFrom(src => src.StoreOfOperation != null ? src.StoreOfOperation.Name : null));

            CreateMap<CreateItemCategoryDto, Domain.Entities.POS.ItemCategory>();
            CreateMap<UpdateItemCategoryDto, Domain.Entities.POS.ItemCategory>();
            CreateMap<CreateItemCategoryCommand, Domain.Entities.POS.ItemCategory>();
            CreateMap<UpdateItemCategoryCommand, Domain.Entities.POS.ItemCategory>();
        }
    }
}
