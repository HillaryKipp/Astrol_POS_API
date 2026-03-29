using AstrolPOSAPI.Application.Features.POS.DefaultScreen.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.DefaultScreen.Commands.UpdateDefaultScreen
{
    public class UpdateDefaultScreenCommand : IRequest<DefaultScreenDto>
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateDefaultScreenCommandValidator : AbstractValidator<UpdateDefaultScreenCommand>
    {
        public UpdateDefaultScreenCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.Code).NotEmpty().MaximumLength(32);
            RuleFor(p => p.Name).NotEmpty().MaximumLength(128);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(128);
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
        }
    }

    public class UpdateDefaultScreenCommandHandler : IRequestHandler<UpdateDefaultScreenCommand, DefaultScreenDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDefaultScreenCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultScreenDto> Handle(UpdateDefaultScreenCommand request, CancellationToken cancellationToken)
        {
            var defaultScreen = await _unitOfWork.Repository<Domain.Entities.POS.DefaultScreen>().GetByIdAsync(request.Id);

            if (defaultScreen == null || defaultScreen.DeletedDate != null)
                throw new KeyNotFoundException($"DefaultScreen with ID {request.Id} not found");

            _mapper.Map(request, defaultScreen);
            await _unitOfWork.Repository<Domain.Entities.POS.DefaultScreen>().UpdateAsync(defaultScreen);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<DefaultScreenDto>(defaultScreen);
        }
    }
}
