using HabitTrackerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTrackerUI.Models
{
    public class DisplayProgressModel : IProgressModel
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public DateTime DatePerformed { get; set; }
        public bool Completed { get; set; }
    }
}
