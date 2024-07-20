using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RingoMediaApplication.Dtos;
using RingoMediaApplication.Models;
using RingoMediaApplication.Repo;
using RingoMediaApplication.Services;

namespace RingoMediaApplication.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _repo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DepartmentController(IDepartmentRepository repo, IWebHostEnvironment webHostEnvironment)
        {
            this._repo = repo;
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ViewModelDepartment viewModelDepartment, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModelDepartment);
            }

            if (image == null || image.Length == 0)
            {
                ModelState.AddModelError("Logo", "Image is required.");
                return View(viewModelDepartment);
            }

            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            string imageName = Guid.NewGuid().ToString() + "_" + image.FileName;
            string filePath = Path.Combine(uploadPath, imageName);

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                Department dept = new Department
                {
                    Name = viewModelDepartment.Name,
                    ParentDepartmentId = viewModelDepartment.ParentDepartmentId,
                    Logo = imageName 
                };

                await _repo.AddAsync(dept);
                TempData["SuccessMessage"] = "Department created successfully.";
                return RedirectToAction("Create"); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the department. Please try again.");
                return View(viewModelDepartment);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartmentWithListOfSubDepartment(int? departmentId)
        {
            if (!departmentId.HasValue || departmentId.Value <= 0)
            {
                ModelState.AddModelError("", "Invalid department ID.");
                return View(new List<Department>());
            }

            try
            {
                var subDepartments = await _repo.GetDepartmentWithListOfSubDepartment(departmentId.Value);
                if (subDepartments == null || !subDepartments.Any())
                {
                    ModelState.AddModelError("", "No sub-departments found for the given department ID.");
                    return View(new List<Department>());
                }
                return View(subDepartments);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving sub-departments. Please try again.");
                return View(new List<Department>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListOfAllParentDepartment(int departmentId)
        {
            if (departmentId <= 0)
            {
                ModelState.AddModelError("", "Invalid department ID.");
                return View(new List<Department>());
            }

            try
            {
                var parents = await _repo.GetListOfAllParentDepartment(departmentId);
                if (parents == null || !parents.Any())
                {
                    ModelState.AddModelError("", "No parent departments found for the given department ID.");
                }
                return View(parents);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving parent departments. Please try again.");
                return View(new List<Department>());
            }
        }


        internal Department MapToProduct(ViewModelDepartment departmentDto)
        {
            return new Department
            {
                Name = departmentDto.Name,
                ParentDepartmentId = departmentDto.ParentDepartmentId,
            };
        }

    }
}
