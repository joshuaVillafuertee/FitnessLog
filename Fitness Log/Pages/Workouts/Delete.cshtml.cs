using Microsoft.AspNetCore.Mvc.RazorPages;
using Fitness_Log.Repositories;
using Fitness_Log.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Log.Pages.Workouts
{
    public class DeleteModel : PageModel
    {
        private readonly IWorkoutRepository _repo;

        public DeleteModel(IWorkoutRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public Workout Workout { get; set; } = new();

        public async Task OnGetAsync(int id)
        {
            var w = await _repo.GetByIdAsync(id);
            if (w != null) Workout = w;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _repo.DeleteAsync(Workout.Id);
            return RedirectToPage("Index");
        }
    }
}
