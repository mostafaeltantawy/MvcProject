using MvcProject.BLL.Interfaces;
using MvcProject.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get ; set; }

        public UnitOfWork(ApplicationDbContext dbContext ) // Ask CLR 
        {
            EmployeeRepository = new EmployeeRepository(dbContext);
            DepartmentRepository = new DepartmentRepository(dbContext);
            _dbContext = dbContext;
        }

        public int Complete()
        {
            return _dbContext.SaveChanges();    
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
