using CoreModule.DbContextConfig;
using CoreModule.Src;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.UnitOfWork
{
    public class UnitOfWorkService : UnitOfWorkServiceInterface
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWorkService(ApplicationDbContext context)
        {
            _context = context;
            Villas = new VillaRepository(_context);
            VillaNumbers = new VillaNumberRepository(_context);
        }
        public VillaRepositoryInterface Villas { get; }
        public VillaNumberRepositoryInterface VillaNumbers { get; }


        public async Task<IDbContextTransaction> BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            return await _context.Database.BeginTransactionAsync(isolationLevel);
        }

        public async Task CompleteAsync()
        {
             await _context.SaveChangesAsync();
        }
    }
}
