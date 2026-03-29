using AstrolPOSAPI.Application.Features.Store.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Core;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.Store.Commands.CreateStore
{
    public class CreateStoreCommand : IRequest<StoreDto>
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string CompanyId { get; set; } = default!;
        public string? StoreTypeId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class CreateStoreCommandHandler : IRequestHandler<CreateStoreCommand, StoreDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateStoreCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StoreDto> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
        {
            // Validate Company exists
            var company = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Company>().GetByIdAsync(request.CompanyId);
            if (company == null)
                throw new InvalidOperationException($"Company with ID {request.CompanyId} not found");

            var store = new AstrolPOSAPI.Domain.Entities.Core.Store
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                CompanyId = request.CompanyId,
                StoreTypeId = request.StoreTypeId,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber
            };

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Store>().AddAsync(store);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<StoreDto>(store);
        }
    }
}
