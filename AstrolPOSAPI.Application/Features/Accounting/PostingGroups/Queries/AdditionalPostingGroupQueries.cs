using AstrolPOSAPI.Application.Features.Accounting.PostingGroups.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.PostingGroups.Queries
{
    // Queries for Customer Posting Group
    public class GetAllCustomerPostingGroupsQuery : IRequest<List<CustomerPostingGroupDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetAllCustomerPostingGroupsQueryHandler : IRequestHandler<GetAllCustomerPostingGroupsQuery, List<CustomerPostingGroupDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCustomerPostingGroupsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CustomerPostingGroupDto>> Handle(GetAllCustomerPostingGroupsQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository<CustomerPostingGroup>().Entities
                .Where(x => x.CompanyId == request.CompanyId && x.DeletedDate == null)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<CustomerPostingGroupDto>>(items);
        }
    }

    // Queries for Gen Prod Posting Group
    public class GetAllGenProdPostingGroupsQuery : IRequest<List<GenProdPostingGroupDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetAllGenProdPostingGroupsQueryHandler : IRequestHandler<GetAllGenProdPostingGroupsQuery, List<GenProdPostingGroupDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllGenProdPostingGroupsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GenProdPostingGroupDto>> Handle(GetAllGenProdPostingGroupsQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository<GenProdPostingGroup>().Entities
                .Where(x => x.CompanyId == request.CompanyId && x.DeletedDate == null)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<GenProdPostingGroupDto>>(items);
        }
    }

    // Queries for General Posting Setup
    public class GetAllGeneralPostingSetupsQuery : IRequest<List<GeneralPostingSetupDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetAllGeneralPostingSetupsQueryHandler : IRequestHandler<GetAllGeneralPostingSetupsQuery, List<GeneralPostingSetupDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllGeneralPostingSetupsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GeneralPostingSetupDto>> Handle(GetAllGeneralPostingSetupsQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository<GeneralPostingSetup>().Entities
                .Where(x => x.CompanyId == request.CompanyId && x.DeletedDate == null)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<GeneralPostingSetupDto>>(items);
        }
    }

    // Queries for Bank Account
    public class GetAllBankAccountsQuery : IRequest<List<BankAccountDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetAllBankAccountsQueryHandler : IRequestHandler<GetAllBankAccountsQuery, List<BankAccountDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllBankAccountsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<BankAccountDto>> Handle(GetAllBankAccountsQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository<BankAccount>().Entities
                .Where(x => x.CompanyId == request.CompanyId && x.DeletedDate == null)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<BankAccountDto>>(items);
        }
    }
}
