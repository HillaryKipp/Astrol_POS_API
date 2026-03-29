using AstrolPOSAPI.Application.Features.Accounting.GLAccount.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.GLAccount.Commands
{
    public class CreateGLAccountCommand : IRequest<GLAccountDto>
    {
        public CreateGLAccountDto Account { get; set; } = default!;
    }

    public class CreateGLAccountCommandHandler : IRequestHandler<CreateGLAccountCommand, GLAccountDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateGLAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GLAccountDto> Handle(CreateGLAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Accounting.GLAccount>(request.Account);
            entity.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Repository<Domain.Entities.Accounting.GLAccount>().AddAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<GLAccountDto>(entity);
        }
    }

    public class UpdateGLAccountCommand : IRequest<GLAccountDto>
    {
        public UpdateGLAccountDto Account { get; set; } = default!;
    }

    public class UpdateGLAccountCommandHandler : IRequestHandler<UpdateGLAccountCommand, GLAccountDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateGLAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GLAccountDto> Handle(UpdateGLAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Domain.Entities.Accounting.GLAccount>().GetByIdAsync(request.Account.Id);
            if (entity == null) throw new KeyNotFoundException($"GL Account with ID {request.Account.Id} not found");

            _mapper.Map(request.Account, entity);
            await _unitOfWork.Repository<Domain.Entities.Accounting.GLAccount>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<GLAccountDto>(entity);
        }
    }

    public class DeleteGLAccountCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteGLAccountCommandHandler : IRequestHandler<DeleteGLAccountCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGLAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteGLAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Domain.Entities.Accounting.GLAccount>().GetByIdAsync(request.Id);
            if (entity == null) return false;

            entity.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Domain.Entities.Accounting.GLAccount>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);
            return true;
        }
    }

    public class GetAllGLAccountsQuery : IRequest<List<GLAccountDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetAllGLAccountsQueryHandler : IRequestHandler<GetAllGLAccountsQuery, List<GLAccountDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllGLAccountsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GLAccountDto>> Handle(GetAllGLAccountsQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _unitOfWork.Repository<Domain.Entities.Accounting.GLAccount>().Entities
                .Where(x => x.CompanyId == request.CompanyId && x.DeletedDate == null)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<GLAccountDto>>(accounts);
        }
    }

    public class GetGLAccountByIdQuery : IRequest<GLAccountDto>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetGLAccountByIdQueryHandler : IRequestHandler<GetGLAccountByIdQuery, GLAccountDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetGLAccountByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GLAccountDto> Handle(GetGLAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Domain.Entities.Accounting.GLAccount>().GetByIdAsync(request.Id);
            if (entity == null || entity.DeletedDate != null)
                throw new KeyNotFoundException($"GL Account with ID {request.Id} not found");

            return _mapper.Map<GLAccountDto>(entity);
        }
    }
}
