using AstrolPOSAPI.Application.Features.POS.Item.Commands.CreateItem;
using AstrolPOSAPI.Application.Features.POS.Item.Commands.UpdateItem;
using AstrolPOSAPI.Application.Features.POS.Item.DTOs;
using AutoMapper;
using ItemEntity = AstrolPOSAPI.Domain.Entities.POS.Item;

namespace AstrolPOSAPI.Application.Features.POS.Item
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            CreateMap<ItemEntity, ItemDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.GenProdPostingGroupCode, opt => opt.MapFrom(src => src.GenProdPostingGroup != null ? src.GenProdPostingGroup.Code : null));
            CreateMap<ItemDto, ItemEntity>();
            CreateMap<CreateItemDto, ItemEntity>();
            CreateMap<UpdateItemDto, ItemEntity>();
            CreateMap<CreateItemCommand, ItemEntity>();
            CreateMap<UpdateItemCommand, ItemEntity>();
        }
    }
}
