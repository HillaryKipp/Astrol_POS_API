using AstrolPOSAPI.Application.Features.Accounting.PostingGroups.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.PostingGroups.Commands
{
    public class CreateVendorPostingGroupCommand : IRequest<VendorPostingGroupDto>
    {
        public CreateVendorPostingGroupDto PostingGroup { get; set; } = default!;
    }

    public class CreateVendorPostingGroupCommandHandler : IRequestHandler<CreateVendorPostingGroupCommand, VendorPostingGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateVendorPostingGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VendorPostingGroupDto> Handle(CreateVendorPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<VendorPostingGroup>(request.PostingGroup);
            entity.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Repository<VendorPostingGroup>().AddAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<VendorPostingGroupDto>(entity);
        }
    }

    public class GetAllVendorPostingGroupsQuery : IRequest<List<VendorPostingGroupDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetAllVendorPostingGroupsQueryHandler : IRequestHandler<GetAllVendorPostingGroupsQuery, List<VendorPostingGroupDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllVendorPostingGroupsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<VendorPostingGroupDto>> Handle(GetAllVendorPostingGroupsQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository<VendorPostingGroup>().Entities
                .Where(x => x.CompanyId == request.CompanyId && x.DeletedDate == null)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<VendorPostingGroupDto>>(items);
        }
    }

    public class UpdateVendorPostingGroupCommand : IRequest<VendorPostingGroupDto>
    {
        public UpdateVendorPostingGroupDto PostingGroup { get; set; } = default!;
    }

    public class UpdateVendorPostingGroupCommandHandler : IRequestHandler<UpdateVendorPostingGroupCommand, VendorPostingGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateVendorPostingGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VendorPostingGroupDto> Handle(UpdateVendorPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<VendorPostingGroup>().GetByIdAsync(request.PostingGroup.Id);
            if (entity == null) throw new Exception("Posting Group not found");

            _mapper.Map(request.PostingGroup, entity);
            await _unitOfWork.Repository<VendorPostingGroup>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<VendorPostingGroupDto>(entity);
        }
    }

    public class DeleteVendorPostingGroupCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteVendorPostingGroupCommandHandler : IRequestHandler<DeleteVendorPostingGroupCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteVendorPostingGroupCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteVendorPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<VendorPostingGroup>().GetByIdAsync(request.Id);
            if (entity == null) return false;

            entity.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<VendorPostingGroup>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);
            return true;
        }
    }
}
