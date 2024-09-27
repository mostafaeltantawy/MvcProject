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

		public async Task AddAsync(T item)
		{
			 await _dbContext.AddAsync(item);
		}

		public  void Delete(T item)
		{
			  _dbContext.Remove(item);
		}

		public async Task<T> GetAsync(int id)
		{
			return  await _dbContext.FindAsync<T>(id);
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			if(typeof(T) == typeof(Employee))
			{
                return  (IEnumerable<T>)await _dbContext.Employees.Include(E => E.Department).ToListAsync();

            }
            return await _dbContext.Set<T>().ToListAsync();
		}

		public void Update(T item)
		{
			_dbContext.Update(item);
		}
	}
}
