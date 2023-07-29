using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MVCAppDbContext _dbContext;

        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        public UnitOfWork(MVCAppDbContext dbContext)
        {
            EmployeeRepository=new EmployeeRepository(dbContext);
            DepartmentRepository=new DepartmentRepository(dbContext);
            _dbContext = dbContext;
        }

         public async Task<int> complete()
           =>await _dbContext.SaveChangesAsync();

        public void Dispose()
           => _dbContext.Dispose();
    }
}
