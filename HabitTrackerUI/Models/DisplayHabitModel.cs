using HabitTrackerLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTrackerUI.Models
{
    public class DisplayHabitModel : IHabitModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name for the Habit")]
        [MaxLength(50, ErrorMessage = "A Habit name can only be 50 characters long!")]
        [MinLength(4, ErrorMessage = "A Habit name needs to be at least 4 characters long!")]
        public string HabitName { get; set; }
        public int HabitStreak { get; set; }
        public string HabitOwnerId { get; set; }
        public List<IGoalModel> Goals { get; set; } = new List<IGoalModel>();
    }
}
