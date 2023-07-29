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
    public class DepartmentRepository :GenericRepository<Department> , IDepartmentRepository
    {
        ///private readonly MVCAppDbContext _dbContext;
        ///public DepartmentRepository(MVCAppDbContext dbContext)  //Ask CLR for creating object from DbContext
        ///{
        ///    //dbContext = new MVCAppDbContext();
        ///    _dbContext= dbContext;
        ///}
        ///public int Add(Department department)
        ///{
        ///    _dbContext.Departments.Add(department);
        ///    return _dbContext.SaveChanges();
        ///}
        ///public int Delete(Department department)
        ///{
        ///    _dbContext.Departments.Remove(department);
        ///    return _dbContext.SaveChanges();
        ///}
        ///public Department Get(int id)
        ///{
        ///    //var department= _dbContext.Departments.Local.Where(d=>d.Id == id).FirstOrDefault();
        ///    //if(department is null)
        ///    //    department = _dbContext.Departments.Where(d => d.Id == id).FirstOrDefault();
        ///    //return department;
        ///    return _dbContext.Departments.Find(id);
        ///}
        ///public IEnumerable<Department> GetAll()
        ///    => _dbContext.Departments.ToList();
        ///public int Update(Department department)
        ///{
        ///    _dbContext.Departments.Update(department);
        ///    return _dbContext.SaveChanges();
        ///}

        public DepartmentRepository(MVCAppDbContext dbContext):base(dbContext)  //Ask CLR for creating object from DbContext
        { }

        public IQueryable<Department> SearchDepartmentByName(string name)
            => _dbContext.Departments.Where(e => e.Name.ToLower().Contains(name.ToLower()));
    }
}
