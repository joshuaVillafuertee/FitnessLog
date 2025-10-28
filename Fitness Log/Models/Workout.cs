using System.ComponentModel.DataAnnotations;

namespace Fitness_Log.Models
{
    public class Workout
    {
        public int Id { get; set; }

        [Required]
        public string Exercise { get; set; } = string.Empty;

        public int? Reps { get; set; }

        
        public int? DurationSeconds { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
    }
}