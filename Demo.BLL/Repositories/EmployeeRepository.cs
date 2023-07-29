using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        //private protected readonly MVCAppDbContext _dbContext;

        ///private readonly MVCAppDbContext _dbContext;
        ///public EmployeeRepository(MVCAppDbContext dbContext)  //Ask CLR for creating object from DbContext
        ///{
        ///    //dbContext = new MVCAppDbContext();
        ///    _dbContext = dbContext;
        ///}
        ///public int Add(Employee employee)
        ///{
        ///    _dbContext.Employees.Add(employee);
        ///    return _dbContext.SaveChanges();
        ///}
        ///public int Delete(Employee employee)
        ///{
        ///    _dbContext.Employees.Remove(employee);
        ///    return _dbContext.SaveChanges();
        ///}
        ///public Employee Get(int id)
        ///{
        ///    //var Employee= _dbContext.Employees.Local.Where(d=>d.Id == id).FirstOrDefault();
        ///    //if(Employee is null)
        ///    //    Employee = _dbContext.Employees.Where(d => d.Id == id).FirstOrDefault();
        ///    //return Employee;
        ///    return _dbContext.Employees.Find(id);
        ///}
        ///public IEnumerable<Employee> GetAll()
        ///    => _dbContext.Employees.ToList();
        ///public int Update(Employee employee)
        ///{
        ///    _dbContext.Employees.Update(employee);
        ///    return _dbContext.SaveChanges();
        ///}

        public EmployeeRepository(MVCAppDbContext dbContext):base(dbContext)  //Ask CLR for creating object from DbContext
        {
            //_dbContext = dbContext;
        }
        public IQueryable<Employee> GetEmployeeByAddress(string address)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Employee> SearchEmployeeByName(string name)
            => _dbContext.Employees.Where(e => e.Name.ToLower().Contains(name.ToLower()));
    }
}
