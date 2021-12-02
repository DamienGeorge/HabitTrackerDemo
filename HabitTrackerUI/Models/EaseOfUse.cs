using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTrackerUI.Models
{
    public enum GoalMode
    {
        Add,
        Update
    }
    public static class EaseOfUse
    {
        public static Dictionary<int, string> cssStyleForBackground = new Dictionary<int, string>()
        {
            {1, "bg-danger" },
            {2, "bg-warning" },
            {3, "bg-primary" },
            {4, "bg-success" },
        };
    }
}