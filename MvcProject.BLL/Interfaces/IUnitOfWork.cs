using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Signature for property for each and every repository interface
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        Task<int> CompleteAsync();


    }
}
