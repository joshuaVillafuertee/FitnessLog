using Microsoft.AspNetCore.Mvc.RazorPages;
using Fitness_Log.Repositories;
using Fitness_Log.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Log.Pages.Workouts
{
    public class CreateModel : PageModel
    {
        private readonly IWorkoutRepository _repo;

        public CreateModel(IWorkoutRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public Workout Workout { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            Workout.UserId = User?.Identity?.Name;
            Workout.Date = DateTime.UtcNow;
            await _repo.AddAsync(Workout);
            return RedirectToPage("Index");
        }
    }
}
