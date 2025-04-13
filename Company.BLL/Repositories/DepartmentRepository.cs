using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Interfaces;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>,IDepartmentRepository
    {
        private readonly CompanyDbContext _context;

        public DepartmentRepository(CompanyDbContext context):base(context) 
        {
            _context = context;
        }


        public async Task<List<Department>> GetByNameAsync(string name)
        {
            return await _context.Departments.Where(E => E.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }   }
