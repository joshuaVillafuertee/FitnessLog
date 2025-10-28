using Fitness_Log.Data;
using Fitness_Log.Models;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Log.Repositories
{
    public class EfWorkoutRepository : IWorkoutRepository
    {
        private readonly ApplicationDbContext _db;

        public EfWorkoutRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Workout workout)
        {
            _db.Workouts.Add(workout);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var w = await _db.Workouts.FindAsync(id);
            if (w != null)
            {
                _db.Workouts.Remove(w);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Workout>> GetAllAsync(string? userId = null)
        {
            return await _db.Workouts
                .Where(w => userId == null || w.UserId == userId)
                .OrderByDescending(w => w.Date)
                .ToListAsync();
        }

        public async Task<Workout?> GetByIdAsync(int id) => await _db.Workouts.FindAsync(id);

        public async Task<List<Workout>> GetByUserAsync(string userId) =>
            await _db.Workouts.Where(w => w.UserId == userId).OrderByDescending(w => w.Date).ToListAsync();
    }
}