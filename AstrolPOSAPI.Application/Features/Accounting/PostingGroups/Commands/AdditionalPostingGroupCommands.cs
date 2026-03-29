using AstrolPOSAPI.Application.Features.Accounting.PostingGroups.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.PostingGroups.Commands
{
    // Customer Posting Group
    public class CreateCustomerPostingGroupCommand : IRequest<CustomerPostingGroupDto>
    {
        public CreateCustomerPostingGroupDto PostingGroup { get; set; } = default!;
    }

    public class CreateCustomerPostingGroupCommandHandler : IRequestHandler<CreateCustomerPostingGroupCommand, CustomerPostingGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCustomerPostingGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerPostingGroupDto> Handle(CreateCustomerPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<CustomerPostingGroup>(request.PostingGroup);
            entity.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Repository<CustomerPostingGroup>().AddAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<CustomerPostingGroupDto>(entity);
        }
    }

    public class UpdateCustomerPostingGroupCommand : IRequest<CustomerPostingGroupDto>
    {
        public UpdateCustomerPostingGroupDto PostingGroup { get; set; } = default!;
    }

    public class UpdateCustomerPostingGroupCommandHandler : IRequestHandler<UpdateCustomerPostingGroupCommand, CustomerPostingGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCustomerPostingGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerPostingGroupDto> Handle(UpdateCustomerPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<CustomerPostingGroup>().GetByIdAsync(request.PostingGroup.Id);
            if (entity == null) throw new KeyNotFoundException($"Customer Posting Group with ID {request.PostingGroup.Id} not found");

            _mapper.Map(request.PostingGroup, entity);
            await _unitOfWork.Repository<CustomerPostingGroup>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<CustomerPostingGroupDto>(entity);
        }
    }

    // Gen Prod Posting Group
    public class CreateGenProdPostingGroupCommand : IRequest<GenProdPostingGroupDto>
    {
        public CreateGenProdPostingGroupDto PostingGroup { get; set; } = default!;
    }

    public class CreateGenProdPostingGroupCommandHandler : IRequestHandler<CreateGenProdPostingGroupCommand, GenProdPostingGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateGenProdPostingGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenProdPostingGroupDto> Handle(CreateGenProdPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<GenProdPostingGroup>(request.PostingGroup);
            entity.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Repository<GenProdPostingGroup>().AddAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<GenProdPostingGroupDto>(entity);
        }
    }

    public class UpdateGenProdPostingGroupCommand : IRequest<GenProdPostingGroupDto>
    {
        public UpdateGenProdPostingGroupDto PostingGroup { get; set; } = default!;
    }

    public class UpdateGenProdPostingGroupCommandHandler : IRequestHandler<UpdateGenProdPostingGroupCommand, GenProdPostingGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateGenProdPostingGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenProdPostingGroupDto> Handle(UpdateGenProdPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<GenProdPostingGroup>().GetByIdAsync(request.PostingGroup.Id);
            if (entity == null) throw new KeyNotFoundException($"Gen Prod Posting Group with ID {request.PostingGroup.Id} not found");

            _mapper.Map(request.PostingGroup, entity);
            await _unitOfWork.Repository<GenProdPostingGroup>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<GenProdPostingGroupDto>(entity);
        }
    }

    // General Posting Setup
    public class CreateGeneralPostingSetupCommand : IRequest<GeneralPostingSetupDto>
    {
        public CreateGeneralPostingSetupDto PostingSetup { get; set; } = default!;
    }

    public class CreateGeneralPostingSetupCommandHandler : IRequestHandler<CreateGeneralPostingSetupCommand, GeneralPostingSetupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateGeneralPostingSetupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GeneralPostingSetupDto> Handle(CreateGeneralPostingSetupCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<GeneralPostingSetup>(request.PostingSetup);
            entity.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Repository<GeneralPostingSetup>().AddAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<GeneralPostingSetupDto>(entity);
        }
    }

    public class UpdateGeneralPostingSetupCommand : IRequest<GeneralPostingSetupDto>
    {
        public UpdateGeneralPostingSetupDto PostingSetup { get; set; } = default!;
    }

    public class UpdateGeneralPostingSetupCommandHandler : IRequestHandler<UpdateGeneralPostingSetupCommand, GeneralPostingSetupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateGeneralPostingSetupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GeneralPostingSetupDto> Handle(UpdateGeneralPostingSetupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<GeneralPostingSetup>().GetByIdAsync(request.PostingSetup.Id);
            if (entity == null) throw new KeyNotFoundException($"General Posting Setup with ID {request.PostingSetup.Id} not found");

            _mapper.Map(request.PostingSetup, entity);
            await _unitOfWork.Repository<GeneralPostingSetup>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<GeneralPostingSetupDto>(entity);
        }
    }

    // Bank Account
    public class CreateBankAccountCommand : IRequest<BankAccountDto>
    {
        public CreateBankAccountDto BankAccount { get; set; } = default!;
    }

    public class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, BankAccountDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateBankAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BankAccountDto> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<BankAccount>(request.BankAccount);
            entity.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Repository<BankAccount>().AddAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<BankAccountDto>(entity);
        }
    }

    public class UpdateBankAccountCommand : IRequest<BankAccountDto>
    {
        public UpdateBankAccountDto BankAccount { get; set; } = default!;
    }

    public class UpdateBankAccountCommandHandler : IRequestHandler<UpdateBankAccountCommand, BankAccountDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateBankAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BankAccountDto> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<BankAccount>().GetByIdAsync(request.BankAccount.Id);
            if (entity == null) throw new KeyNotFoundException($"Bank Account with ID {request.BankAccount.Id} not found");

            _mapper.Map(request.BankAccount, entity);
            await _unitOfWork.Repository<BankAccount>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<BankAccountDto>(entity);
        }
    }

    // Delete Commands
    public class DeleteCustomerPostingGroupCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteCustomerPostingGroupCommandHandler : IRequestHandler<DeleteCustomerPostingGroupCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCustomerPostingGroupCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<bool> Handle(DeleteCustomerPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<CustomerPostingGroup>().GetByIdAsync(request.Id);
            if (entity == null) return false;
            entity.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<CustomerPostingGroup>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);
            return true;
        }
    }

    public class DeleteGenProdPostingGroupCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteGenProdPostingGroupCommandHandler : IRequestHandler<DeleteGenProdPostingGroupCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteGenProdPostingGroupCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<bool> Handle(DeleteGenProdPostingGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<GenProdPostingGroup>().GetByIdAsync(request.Id);
            if (entity == null) return false;
            entity.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<GenProdPostingGroup>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);
            return true;
        }
    }

    public class DeleteGeneralPostingSetupCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteGeneralPostingSetupCommandHandler : IRequestHandler<DeleteGeneralPostingSetupCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteGeneralPostingSetupCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<bool> Handle(DeleteGeneralPostingSetupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<GeneralPostingSetup>().GetByIdAsync(request.Id);
            if (entity == null) return false;
            entity.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<GeneralPostingSetup>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);
            return true;
        }
    }

    public class DeleteBankAccountCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteBankAccountCommandHandler : IRequestHandler<DeleteBankAccountCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteBankAccountCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<bool> Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<BankAccount>().GetByIdAsync(request.Id);
            if (entity == null) return false;
            entity.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<BankAccount>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);
            return true;
        }
    }
}
