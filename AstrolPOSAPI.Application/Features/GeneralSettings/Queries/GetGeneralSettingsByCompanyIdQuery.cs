using AstrolPOSAPI.Application.Features.GeneralSettings.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.GeneralSettings.Queries
{
    public class GetGeneralSettingsByCompanyIdQuery : IRequest<GeneralSettingsDto>
    {
        public string CompanyId { get; set; } = default!;
    }

    public class GetGeneralSettingsByCompanyIdQueryHandler : IRequestHandler<GetGeneralSettingsByCompanyIdQuery, GeneralSettingsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetGeneralSettingsByCompanyIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GeneralSettingsDto> Handle(GetGeneralSettingsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var allSettings = await _unitOfWork.Repository<Domain.Entities.Core.GeneralSettings>().GetAllAsync();
            var settings = allSettings.FirstOrDefault(gs => gs.CompanyId == request.CompanyId);

            if (settings == null)
                throw new KeyNotFoundException($"GeneralSettings for Company {request.CompanyId} not found");

            return _mapper.Map<GeneralSettingsDto>(settings);
        }
    }
}
