using System;
using System.Collections.Generic;
using System.Text;

namespace HabitTrackerLibrary.Models
{
    public class ProgressModel : IProgressModel
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public DateTime DatePerformed { get; set; } = DateTime.Now;
        public bool Completed { get; set; }
    }
}
