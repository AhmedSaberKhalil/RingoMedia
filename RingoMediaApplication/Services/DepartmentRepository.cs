using Microsoft.EntityFrameworkCore;
using RingoMediaApplication.Data;
using RingoMediaApplication.Models;
using RingoMediaApplication.Repo;

namespace RingoMediaApplication.Services
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        public async Task<List<Department>> GetDepartmentWithListOfSubDepartment(int departmentId)
        {
            return await _context.Departments
                .Where(d => d.ParentDepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<List<Department>> GetListOfAllParentDepartment(int departmentId)
        {
            var department = await _context.Departments
                .Include(d => d.ParentDepartment)
                .FirstOrDefaultAsync(d => d.Id == departmentId);

            var parents = new List<Department>();

            while (department != null && department.ParentDepartment != null)
            {
                department = await _context.Departments
                    .Include(d => d.ParentDepartment) // Ensure we load parent department
                    .FirstOrDefaultAsync(d => d.Id == department.ParentDepartment.Id);

                if (department != null)
                {
                    parents.Add(department);
                }
            }

            return parents;
        }

    }
}
