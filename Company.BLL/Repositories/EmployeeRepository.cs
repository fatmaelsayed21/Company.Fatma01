﻿using System;
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
    public class EmployeeRepository : GenericRepository<Employee>,IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base(context) //asl CLR to create obj from companydb context
        {
            _context = context;
        }

        public List<Employee> GetbyName(string name)
        {
            return _context.Employees.Include(E=>E.Department).Where(E=>E.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
