using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Company.Commands.DeleteCompany
{
    public class DeleteCompanyCommand : IRequest<bool>
    {
        public string Id { get; set; } = default!;
    }

    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Company>()
                .Entities
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.DeletedDate == null, cancellationToken);

            if (company == null)
            {
                throw new KeyNotFoundException($"Company with ID {request.Id} not found");
            }

            // Soft delete - the AppDbContext SaveChangesAsync will set DeletedDate
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Company>().DeleteAsync(company);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}
