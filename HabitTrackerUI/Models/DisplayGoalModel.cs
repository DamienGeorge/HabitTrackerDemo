using HabitTrackerLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTrackerUI.Models
{
    public class DisplayGoalModel : IGoalModel
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        [Required(ErrorMessage = "Please enter a name for the goal")]
        [MinLength(3, ErrorMessage = "A goal name should contain at least 3 characters!")]
        [MaxLength(50, ErrorMessage = "A goal name cannot exceed 50 characters!")]
        public string GoalName { get; set; }

        [MaxLength(400, ErrorMessage = "Description cannot exceed 400 characters!")]
        public string GoalDescription { get; set; }

        [Required(ErrorMessage = "Please enter a start date for the goal!")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter a target end date for the goal!")]
        public DateTime TargetPeriod { get; set; } = (DateTime.Now).AddDays(30);
        public bool Active { get; set; }
        public List<IProgressModel> ProgressStatus { get; set; } = new List<IProgressModel>();
    }
}
