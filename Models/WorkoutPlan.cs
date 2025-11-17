using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.WPF.Models
{
    [Table("WorkoutPlans")]
    public class WorkoutPlan
    {
        [Key]
        public int WorkoutPlanId { get; set; }

        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string PlanName { get; set; } // Bắt buộc, giữ nguyên

        // THÊM DẤU ? ĐỂ CHO PHÉP NULL
        public string? Description { get; set; }

        public FitnessGoal Goal { get; set; }

        public PreferredEnvironment Environment { get; set; }

        public int DurationWeeks { get; set; } = 4;

        public int DaysPerWeek { get; set; } = 3;

        public bool IsAIGenerated { get; set; } = false;

        // THÊM DẤU ? ĐỂ CHO PHÉP NULL (Vì AI có thể chưa tạo prompt hoặc DB cũ null)
        public string? AIPrompt { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<WorkoutPlanExercise> Exercises { get; set; }
        public virtual ICollection<WorkoutSession> WorkoutSessions { get; set; }
    }
}