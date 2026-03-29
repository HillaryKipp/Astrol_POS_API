using AstrolPOSAPI.Application.Features.POS.Item.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using ItemEntity = AstrolPOSAPI.Domain.Entities.POS.Item;

namespace AstrolPOSAPI.Application.Features.POS.Item.Commands.UpdateItem
{
    public class UpdateItemCommand : IRequest<ItemDto>
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? CategoryId { get; set; }
        public string UnitOfMeasure { get; set; } = "EA";
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string? ImageUrl { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal ReorderLevel { get; set; }
        public decimal TaxRate { get; set; }
        public bool IsActive { get; set; }
        public string? Barcode { get; set; }
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
    {
        public UpdateItemCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.Code).NotEmpty().MaximumLength(32);
            RuleFor(p => p.Name).NotEmpty().MaximumLength(128);
            RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(0);
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
        }
    }

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ItemDto> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.Repository<ItemEntity>().GetByIdAsync(request.Id);

            if (item == null || item.DeletedDate != null)
                throw new KeyNotFoundException($"Item with ID {request.Id} not found");

            _mapper.Map(request, item);
            await _unitOfWork.Repository<ItemEntity>().UpdateAsync(item);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<ItemDto>(item);
        }
    }
}
