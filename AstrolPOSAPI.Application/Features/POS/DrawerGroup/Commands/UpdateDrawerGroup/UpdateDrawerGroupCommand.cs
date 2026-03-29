using AstrolPOSAPI.Application.Features.POS.DrawerGroup.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.DrawerGroup.Commands.UpdateDrawerGroup
{
    public class UpdateDrawerGroupCommand : IRequest<DrawerGroupDto>
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateDrawerGroupCommandValidator : AbstractValidator<UpdateDrawerGroupCommand>
    {
        public UpdateDrawerGroupCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.Code).NotEmpty().MaximumLength(32);
            RuleFor(p => p.Name).NotEmpty().MaximumLength(64);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(128);
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
        }
    }

    public class UpdateDrawerGroupCommandHandler : IRequestHandler<UpdateDrawerGroupCommand, DrawerGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDrawerGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DrawerGroupDto> Handle(UpdateDrawerGroupCommand request, CancellationToken cancellationToken)
        {
            var drawerGroup = await _unitOfWork.Repository<Domain.Entities.POS.DrawerGroup>().GetByIdAsync(request.Id);

            if (drawerGroup == null)
                throw new KeyNotFoundException($"DrawerGroup with ID {request.Id} not found");

            if (drawerGroup.DeletedDate != null)
                throw new KeyNotFoundException($"DrawerGroup with ID {request.Id} was deleted");

            _mapper.Map(request, drawerGroup);
            await _unitOfWork.Repository<Domain.Entities.POS.DrawerGroup>().UpdateAsync(drawerGroup);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<DrawerGroupDto>(drawerGroup);
        }
    }
}
