using System;
using System.Collections.Generic;
using System.Text;

namespace HabitTrackerLibrary.Models
{
    public class GoalModel : IGoalModel
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public string GoalName { get; set; }
        public string GoalDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetPeriod { get; set; }
        public bool Active { get; set; }
        public List<IProgressModel> ProgressStatus { get; set; } = new List<IProgressModel>();
    }
}
