using System;
using System.Collections.Generic;
using System.Text;

namespace HabitTrackerLibrary.Models
{
    public class HabitModel : IHabitModel
    {
        public int Id { get; set; }        
        public string HabitName { get; set; }
        //public DateTime EndDate { get; set; }
        public int HabitStreak { get; set; }
        public string HabitOwnerId { get; set; }
        public List<IGoalModel> Goals { get; set; } = new List<IGoalModel>();
    }
}
