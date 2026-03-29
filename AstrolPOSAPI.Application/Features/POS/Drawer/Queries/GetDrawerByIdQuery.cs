using AstrolPOSAPI.Application.Features.POS.Drawer.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Drawer.Queries
{
    public class GetDrawerByIdQuery : IRequest<DrawerDto>
    {
        public string Id { get; set; } = default!;
    }

    public class GetDrawerByIdQueryHandler : IRequestHandler<GetDrawerByIdQuery, DrawerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDrawerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DrawerDto> Handle(GetDrawerByIdQuery request, CancellationToken cancellationToken)
        {
            var drawer = await _unitOfWork.Repository<Domain.Entities.POS.Drawer>().GetByIdAsync(request.Id);

            if (drawer == null || drawer.DeletedDate != null)
                throw new KeyNotFoundException($"Drawer with ID {request.Id} not found");

            return _mapper.Map<DrawerDto>(drawer);
        }
    }
}
