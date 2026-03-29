using AstrolPOSAPI.Application.Features.Accounting.PostingGroups.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.PostingGroups.Commands
{
    public class CreateGenBusPostingGroupCommand : IRequest<GenBusPostingGroupDto>
    {
        public CreateGenBusPostingGroupDto PostingGroup { get; set; } = default!;
    }

    public class CreateGenBusPostingGroupCommandHandler : IRequestHandler<CreateGenBusPostingGroupCommand, GenBusPostingGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateGenBusPostingGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenBusPostingGroupDto> Handle(CreateGenBusPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<GenBusPostingGroup>(request.PostingGroup);
            entity.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Repository<GenBusPostingGroup>().AddAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<GenBusPostingGroupDto>(entity);
        }
    }

    public class GetAllGenBusPostingGroupsQuery : IRequest<List<GenBusPostingGroupDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetAllGenBusPostingGroupsQueryHandler : IRequestHandler<GetAllGenBusPostingGroupsQuery, List<GenBusPostingGroupDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllGenBusPostingGroupsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GenBusPostingGroupDto>> Handle(GetAllGenBusPostingGroupsQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository<GenBusPostingGroup>().Entities
                .Where(x => x.CompanyId == request.CompanyId && x.DeletedDate == null)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<GenBusPostingGroupDto>>(items);
        }
    }

    public class UpdateGenBusPostingGroupCommand : IRequest<GenBusPostingGroupDto>
    {
        public UpdateGenBusPostingGroupDto PostingGroup { get; set; } = default!;
    }

    public class UpdateGenBusPostingGroupCommandHandler : IRequestHandler<UpdateGenBusPostingGroupCommand, GenBusPostingGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateGenBusPostingGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenBusPostingGroupDto> Handle(UpdateGenBusPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<GenBusPostingGroup>().GetByIdAsync(request.PostingGroup.Id);
            if (entity == null) throw new Exception("Posting Group not found");

            _mapper.Map(request.PostingGroup, entity);
            await _unitOfWork.Repository<GenBusPostingGroup>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<GenBusPostingGroupDto>(entity);
        }
    }

    public class DeleteGenBusPostingGroupCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteGenBusPostingGroupCommandHandler : IRequestHandler<DeleteGenBusPostingGroupCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGenBusPostingGroupCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteGenBusPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<GenBusPostingGroup>().GetByIdAsync(request.Id);
            if (entity == null) return false;

            entity.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<GenBusPostingGroup>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);
            return true;
        }
    }
}
