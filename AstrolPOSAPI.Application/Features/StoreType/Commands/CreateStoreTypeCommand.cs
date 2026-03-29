using AstrolPOSAPI.Application.Features.StoreType.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.StoreType.Commands.CreateStoreType
{
    public class CreateStoreTypeCommand : IRequest<StoreTypeDto>
    {
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool HasOtp { get; set; }
        public string? CompanyId { get; set; }
    }

    public class CreateStoreTypeCommandHandler : IRequestHandler<CreateStoreTypeCommand, StoreTypeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateStoreTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StoreTypeDto> Handle(CreateStoreTypeCommand request, CancellationToken cancellationToken)
        {
            var storeType = new AstrolPOSAPI.Domain.Entities.Core.StoreType
            {
                Code = request.Code,
                Description = request.Description,
                HasOtp = request.HasOtp,
                CompanyId = request.CompanyId
            };

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.StoreType>().AddAsync(storeType);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<StoreTypeDto>(storeType);
        }
    }
}
