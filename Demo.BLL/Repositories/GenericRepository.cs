using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private protected readonly MVCAppDbContext _dbContext;
        public GenericRepository(MVCAppDbContext dbContext)  //Ask CLR for creating object from DbContext
        {
            _dbContext = dbContext;
        }
        public async Task Add(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
            //return _dbContext.SaveChanges();
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
            //return _dbContext.SaveChanges();
        }

        public async Task<T> Get(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            if(typeof(T) == typeof(Employee))
               return  (IEnumerable<T>) await _dbContext.Employees.Include(e=>e.Department).ToListAsync();
            else
                return await _dbContext.Set<T>().ToListAsync();
        }
          

        public void Update(T item)
           => _dbContext.Set<T>().Update(item);
            //return _dbContext.SaveChanges();
    }
}
