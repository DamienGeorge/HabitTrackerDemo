using System;

namespace HabitTrackerLibrary.Models
{
    public interface IProgressModel
    {
        int Id { get; set; }
        int GoalId { get; set; }
        DateTime DatePerformed { get; set; }
        bool Completed { get; set; }
    }
}