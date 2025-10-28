using Microsoft.AspNetCore.Mvc;
using Fitness_Log.Repositories;
using Fitness_Log.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace Fitness_Log.Controllers
{
    [Authorize]
    public class WorkoutsController : Controller
    {
        private readonly IWorkoutRepository _repo;
        private readonly ILogger<WorkoutsController> _logger;

        public WorkoutsController(IWorkoutRepository repo, ILogger<WorkoutsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var userId = User.Identity?.Name ?? string.Empty;
                var items = await _repo.GetByUserAsync(userId);
                return View(items);
            }

            
            var all = await _repo.GetAllAsync();
            return View(all);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Workout model)
        {
            if (!ModelState.IsValid) return View(model);
            model.UserId = User?.Identity?.Name;
            model.Date = DateTime.UtcNow;
            await _repo.AddAsync(model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExportCsv()
        {
            var items = User?.Identity?.IsAuthenticated == true
                ? await _repo.GetByUserAsync(User.Identity?.Name ?? string.Empty)
                : await _repo.GetAllAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Id,Exercise,Reps,DurationSeconds,Date");
            foreach (var w in items)
            {
                sb.AppendLine($"{w.Id},\"{w.Exercise}\",{w.Reps},{w.DurationSeconds},{w.Date:O}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "workouts.csv");
        }
    }
}