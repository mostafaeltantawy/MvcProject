using Microsoft.EntityFrameworkCore;
using MvcProject.BLL.Interfaces;
using MvcProject.DAL.Data;
using MvcProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.BLL.Repositories
{
	public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public EmployeeRepository(ApplicationDbContext context):base(context)
        {
			_dbContext = context;
		}
        public IQueryable<Employee> GetEmployeesByAddress(string address)
		{
			return _dbContext.Employees.Where(e => e.Address == address);
		}

        public IQueryable<Employee> GetEmployeesByName(string name)
        {
            return _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower() )).Include(E => E.Department);
        }
    }
}
