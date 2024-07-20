using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RingoMediaApplication.Models;
using RingoMediaApplication.Repo;

namespace RingoMediaApplication.Controllers
{
    public class ReminderController : Controller
    {
        private readonly IRepository<Reminder> _context;

        public ReminderController(IRepository<Reminder> contect)
        {
            this._context = contect;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var reminders = await _context.GetAllAsync();
                return View(reminders);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving reminders. Please try again.");
                return View(new List<Reminder>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.AddAsync(reminder);
                    TempData["SuccessMessage"] = "Reminder created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the reminder. Please try again.");
                }
            }
            return View(reminder);
        }
    }
}
