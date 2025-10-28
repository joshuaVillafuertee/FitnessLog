using Microsoft.AspNetCore.Mvc.RazorPages;
using Fitness_Log.Repositories;
using Fitness_Log.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Fitness_Log.Pages.Workouts
{
    public class IndexModel : PageModel
    {
        private readonly IWorkoutRepository _repo;

        public IndexModel(IWorkoutRepository repo)
        {
            _repo = repo;
        }

        public List<Workout> Workouts { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (User?.Identity?.IsAuthenticated == true)
                Workouts = await _repo.GetByUserAsync(User.Identity?.Name ?? string.Empty);
            else
                Workouts = await _repo.GetAllAsync();
        }

        public async Task<IActionResult> OnGetExportAsync()
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
