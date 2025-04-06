using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Models;

namespace Company.BLL.Interfaces
{
    public interface IEmployeeRepository :IGenericRepository<Employee>
    {
        Task<List<Employee>> GetbyNameAsync(string name);
    }
}
