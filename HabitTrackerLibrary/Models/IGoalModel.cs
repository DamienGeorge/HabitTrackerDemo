using System;
using System.Collections.Generic;

namespace HabitTrackerLibrary.Models
{
    public interface IGoalModel
    {
        int Id { get; set; }
        int HabitId { get; set; }
        string GoalName { get; set; }
        string GoalDescription { get; set; }
        DateTime StartDate { get; set; }
        DateTime TargetPeriod { get; set; }
        bool Active { get; set; }
        List<IProgressModel> ProgressStatus { get; set; }
    }
}