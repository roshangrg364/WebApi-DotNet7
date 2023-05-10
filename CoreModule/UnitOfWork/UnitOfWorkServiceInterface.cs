using CoreModule.Src;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.UnitOfWork
{
    public interface UnitOfWorkServiceInterface
    {
        VillaRepositoryInterface Villas { get; }
        VillaNumberRepositoryInterface VillaNumbers { get; }
        Task<IDbContextTransaction> BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        Task CompleteAsync();
    }
}
