using Fitness_Log.Models;

namespace Fitness_Log.Repositories
{
    public interface IWorkoutRepository
    {
        Task<List<Workout>> GetAllAsync(string? userId = null);
        Task<Workout?> GetByIdAsync(int id);
        Task AddAsync(Workout workout);
        Task DeleteAsync(int id);
        Task<List<Workout>> GetByUserAsync(string userId);
    }
}