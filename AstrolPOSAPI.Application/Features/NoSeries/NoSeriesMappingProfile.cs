using AstrolPOSAPI.Application.Features.NoSeries.DTOs;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.NoSeries
{
    public class NoSeriesMappingProfile : Profile
    {
        public NoSeriesMappingProfile()
        {
            CreateMap<Domain.Entities.Core.NoSeries, NoSeriesDto>();
            CreateMap<CreateNoSeriesDto, Domain.Entities.Core.NoSeries>();
            CreateMap<UpdateNoSeriesDto, Domain.Entities.Core.NoSeries>();
        }
    }
}
