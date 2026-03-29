using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.Queries
{
    public class GetGLEntriesQuery : IRequest<List<GLEntry>>
    {
        public string CompanyId { get; set; } = string.Empty;
        public string? GLAccountNo { get; set; }
    }

    public class GetGLEntriesQueryHandler : IRequestHandler<GetGLEntriesQuery, List<GLEntry>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGLEntriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GLEntry>> Handle(GetGLEntriesQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<GLEntry>().Entities
                .Where(e => e.CompanyId == request.CompanyId && e.DeletedDate == null);

            if (!string.IsNullOrEmpty(request.GLAccountNo))
                query = query.Where(e => e.GLAccountNo == request.GLAccountNo);

            return await query.OrderByDescending(e => e.PostingDate).ThenByDescending(e => e.CreatedDate).ToListAsync(cancellationToken);
        }
    }
}
