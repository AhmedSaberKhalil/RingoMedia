using RingoMediaApplication.Models;

namespace RingoMediaApplication.Repo
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<List<Department>> GetDepartmentWithListOfSubDepartment(int departmentId);
        Task<List<Department>> GetListOfAllParentDepartment(int departmentId);
    }
}
