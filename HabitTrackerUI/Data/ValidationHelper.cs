using HabitTrackerLibrary.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTrackerUI.Data
{
    /// <summary>
    /// Class for common Validation
    /// </summary>
    public class ValidationHelper
    {
        /// <summary>
        /// Validate whether the goal should be active 
        /// </summary>
        /// <param name="goals"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public (IGoalModel,bool) ValidateGoalActivate(List<IGoalModel> goals,IGoalModel goal)
        {
            var result = false;

            if (goals.Count == 0)
            {
                if (goal.TargetPeriod.Date > DateTime.Now.Date && goal.StartDate.Date <= DateTime.Now.Date)
                {
                    goal.Active = true;
                    result = true;
                }
                else
                {
                    goal.Active = false;
                    result = false;
                }
            }
            else if (goals.Any(x => (x.Active) && (x.TargetPeriod.Date > goal.StartDate.Date && x.StartDate.Date < goal.TargetPeriod.Date)))
            {
                result = false;
            }

            return (goal,result);
        }

    }
}
