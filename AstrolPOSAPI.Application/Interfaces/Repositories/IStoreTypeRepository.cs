using AstrolPOSAPI.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AstrolPOSAPI.Application.Interfaces.Repositories
{
    public interface IStoreTypeRepository
    {
        Task<List<StoreType>> GetStoreTypeByCompanyIdAsync(string companyId);
    }
}
