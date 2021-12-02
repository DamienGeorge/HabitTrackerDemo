using System.Collections.Generic;

namespace HabitTrackerLibrary.Models
{
    public interface IHabitModel
    {
        int Id { get; set; }
        string HabitName { get; set; }
        int HabitStreak { get; set; }
        string HabitOwnerId { get; set; }
        List<IGoalModel> Goals { get; set; }
    }
}