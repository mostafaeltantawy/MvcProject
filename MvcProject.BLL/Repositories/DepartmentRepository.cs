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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
		public DepartmentRepository(ApplicationDbContext context) : base(context)
		{

		}

	}
}
