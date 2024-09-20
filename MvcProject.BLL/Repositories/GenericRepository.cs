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
	public class GenericRepository<T>  :IGenericRepository<T> where T : class
	{
		private readonly ApplicationDbContext _dbContext;

		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public void Add(T item)
		{
			_dbContext.Add(item);
		}

		public void Delete(T item)
		{
			_dbContext.Remove(item);
		}

		public T Get(int id)
		{
			return _dbContext.Find<T>(id);
		}

		public IEnumerable<T> GetAll()
		{
			if(typeof(T) == typeof(Employee))
			{
                return(IEnumerable<T>) _dbContext.Employees.Include(E => E.Department).ToList();

            }
            return _dbContext.Set<T>().ToList();
		}

		public void Update(T item)
		{
			_dbContext.Update(item);
		}
	}
}
