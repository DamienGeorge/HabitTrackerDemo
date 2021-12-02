using HabitTrackerLibrary.Models;
using HabitTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTrackerUI.Pages
{
    public partial class HabitDashboard
    {
        List<IHabitModel> habits = new List<IHabitModel>();
        List<IGoalModel> goalsByHabit = new List<IGoalModel>();

        List<IGoalModel> filteredGoalsByHabit => habitIdSelected == 0 ?
            goalsByHabit : goalsByHabit.Where(x => ((habitIdSelected > 0) && (x.HabitId == habitIdSelected))).ToList<IGoalModel>();

        List<IProgressModel> progress = new List<IProgressModel>();

        int habitIdSelected = 0;
        int habitIdToEdit = 0;
        int habitIdToDelete = 0;
        string fluidHabitTableCss = "col-md-12";
        IGoalModel goalToEdit = new DisplayGoalModel();
        string currentMode = String.Empty;
        int goalIdToDelete = 0;


        /// <summary>
        /// Sets up the parameters when passed.
        /// Fetches the logged in user and all data associated with the user
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            string userMailId = _stateProvider.GetAuthenticationStateAsync().Result.User.Identity.Name;

            if (userMailId != null)
            {
                var user = _userManager.FindByEmailAsync(userMailId);
                //fetch all habits
                habits = await _HabitDataAccess.FetchHabitsByOwnerId(user.Result.Id);
                //Add all goals and append to list
                habits.ForEach(h => { goalsByHabit.AddRange(h.Goals); });
                //Fetch progress and append to list
                habits.ForEach(h =>
                {
                    h.Goals.ForEach(g =>
                    {
                        progress.AddRange(g.ProgressStatus);
                    });
                }
                );
            }

            await StartUpTask();
        }

        /// <summary>
        /// Runs Startup checks to deactivate any expired goals
        /// </summary>
        /// <returns></returns>
        public async Task StartUpTask()
        {
            int currentHabitId = 0;

            foreach (var habit in habits)
            {
                if (habit.Id != currentHabitId)
                {
                    currentHabitId = habit.Id;

                    if (habit.Goals.Any(x => x.Active))
                    {
                        foreach (var goal in habit.Goals)
                        {
                            if (goal.TargetPeriod.Date < DateTime.Today.Date)
                            {
                                goal.Active = false;
                                await _HabitDataAccess.UpdateGoal(goal);
                            }
                        }
                    }
                    else
                    {
                        //TODO- check if validations for adding goals/updating goals work as expected
                        foreach (var goal in habit.Goals)
                        {
                            if (goal.StartDate.Date <= DateTime.Today.Date && goal.TargetPeriod.Date > DateTime.Today)
                            {
                                goal.Active = true;
                                await _HabitDataAccess.UpdateGoal(goal);
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Updates the Habit Id to filter by
        /// and fetches all the goals for the selected Id
        /// </summary>
        /// <param name="Id"></param>
        //TODO- Complete as single line action within the element onclick
        private void FetchGoals(int Id)
        {
            //TODO - Filterable habitsGoals
            habitIdSelected = Id;
        }


        #region Add Goals Mode Trigger Methods

        /// <summary>
        /// Trigger to Show the AddGoals Component
        /// </summary>
        /// <param name="Id"></param>
        private void AddGoalsTrigger(int Id)
        {
            goalToEdit = null;
            ValidateTriggerByMode("Add", Id);
        }

        /// <summary>
        /// Trigger to show the AddGoals Component in Update Mode
        /// </summary>
        /// <param name="Id"></param>
        private void UpdateGoalsTrigger(int Id)
        {
            ValidateTriggerByMode("Update", Id);
        }

        /// <summary>
        /// Validate whether mode should be Add or Update 
        /// </summary>
        /// <param name="mode">Enter either Add or Update</param>
        /// <param name="Id"></param>
        private void ValidateTriggerByMode(string mode, int Id)
        {

            if (habitIdToEdit == Id && currentMode == mode)
            {
                habitIdToEdit = 0;
                fluidHabitTableCss = "col-md-12";
            }
            else
            {
                currentMode = mode;
                habitIdToEdit = Id;
                fluidHabitTableCss = "col-md-6";
            }
        }

        #endregion

        #region EventCallback
        /// <summary>
        /// Handles EventCall Back when a new Goal is created
        /// </summary>
        /// <param name="goal"></param>
        private async Task HandleGoalCreated(IGoalModel goal)
        {
            var result = false;

            goal.HabitId = habitIdToEdit;
            //TODO - Calculate whether to make active
            var goals = habits.Where(x => x.Id == goal.HabitId).ToList().FirstOrDefault().Goals;

            if (goals.Count>0)
            {
                (goal, result) = validation.ValidateGoalActivate(goals, goal);
                if (result == true)
                {
                    goal.Id = await _HabitDataAccess.AddGoal(goal);
                }
            }


            goalsByHabit.Add(goal);
        }

        /// <summary>
        /// Handles the Event Callback once a goal has been updated
        /// </summary>
        /// <param name="updatedGoal"></param>
        /// <returns></returns>
        private async Task HandleGoalUpdated(IGoalModel updatedGoal)
        {
            if ((goalsByHabit.Any(x => x.StartDate >= updatedGoal.TargetPeriod) && goalsByHabit.Any(x => x.TargetPeriod > updatedGoal.StartDate && DateTime.Now.Date >= x.TargetPeriod)) == false)
            {
                return;
            }

            //var result = ValidateGoalSpan(updatedGoal);

            //if (result)
            //{
            //    return;
            //}

            await _HabitDataAccess.UpdateGoal(updatedGoal);

            var goalToUpdate = goalsByHabit.Where(x => x.Id == updatedGoal.Id).FirstOrDefault();
            goalToUpdate = updatedGoal;

            UpdateGoalsTrigger(updatedGoal.HabitId);

            habitIdToEdit = 0;
            goalToEdit = new DisplayGoalModel();
        }

        #endregion

        #region Button Click

        /// <summary>
        /// Handles the events to occur when a goal is completed
        /// </summary>
        /// <param name="goal"></param>
        //TODO - Allow entering of previous data as well
        private async Task HandleCompletedClick(IGoalModel goal)
        {
            IHabitModel habitToUpdate = habits.Where(x => x.Id == goal.HabitId).FirstOrDefault();
            CalculateHabitStreak(habitToUpdate);

            await CreateProgressEntry(goal);
            await _HabitDataAccess.UpdateHabit(habitToUpdate);

            //TODO - update to view as well?
        }

        /// <summary>
        /// Handles the events to occur when when Edit Button is clicked on a goal
        /// </summary>
        /// <param name="goal"></param>
        private void HandleEditGoal(IGoalModel goal)
        {
            //TODO - Add method to edit existing Goals. Description, name and dates. Progress?

            goalToEdit = goal;
            UpdateGoalsTrigger(goal.HabitId);
            //habitIdToEdit = goal.HabitId;
        }

        /// <summary>
        /// Handles the events to occur when Delete button is Clicked on a goal
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        private async Task HandleGoalDelete(IGoalModel goal)
        {
            await _HabitDataAccess.DeleteGoalById(goal.Id);

            goalsByHabit.Remove(goal);
            progress.RemoveAll(x => x.GoalId == goal.Id);
            goalIdToDelete = 0;
        }
        #endregion

        #region Custom Methods

        /// <summary>
        /// Add New Entry to Progress Table
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        private async Task CreateProgressEntry(IGoalModel goal)
        {
            IProgressModel progressToSubmit = new DisplayProgressModel();

            progressToSubmit.GoalId = goal.Id;
            progressToSubmit.DatePerformed = DateTime.Now;
            progressToSubmit.Completed = true;

            await _HabitDataAccess.SaveProgress(progressToSubmit);
            progress.Add(progressToSubmit);
            goal.ProgressStatus.Add(progressToSubmit);
        }

        /// <summary>
        /// Method removes a habit and all data associated with it
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private async Task HandleHabitDelete(IHabitModel habit)
        {
            await _HabitDataAccess.DeleteHabitById(habit.Id);

            goalsByHabit.RemoveAll(x => x.HabitId == habit.Id);
            habits.Remove(habit);

            //TODO - cleanup ID set variables
            habitIdSelected = 0;
        }

        /// <summary>
        /// Determines the background animation for progress bar
        /// </summary>
        /// <param name="goal"></param>
        /// <returns>string</returns>
        private string DecideProgressBarBackground(IGoalModel goal)
        {
            double progressPercentage = CalculatePercentageProgress(goal);

            if (progressPercentage >= 0 && progressPercentage < 25)
            {
                return EaseOfUse.cssStyleForBackground[1];
            }
            else if (progressPercentage >= 25 && progressPercentage < 50)
            {
                return EaseOfUse.cssStyleForBackground[2];
            }
            else if (progressPercentage >= 50 && progressPercentage < 80)
            {
                return EaseOfUse.cssStyleForBackground[3];
            }
            else
            {
                return EaseOfUse.cssStyleForBackground[4];
            }
        }

        /// <summary>
        /// Calculates percentage progress of a particular goal
        /// </summary>
        /// <param name="goal"></param>
        /// <returns>double</returns>
        private double CalculatePercentageProgress(IGoalModel goal)
        {
            int currentCount = progress.Where(x => x.GoalId == goal.Id).Count();
            double TotalCount = (goal.TargetPeriod - goal.StartDate).TotalDays;

            return currentCount / TotalCount * 100;
        }

        /// <summary>
        /// Calculate progress Streak for a habit
        /// </summary>
        /// <param name="habit"></param>
        /// <returns></returns>
        private int CalculateHabitStreak(IHabitModel habit)
        {
            //TODO - Condition to check if habit streak is empty, then calculate the habit streak. Else find out if current date is date+1. Else Reset to 0
            if (habit.HabitStreak != 0)
            {
                //CalculateLargestStreakFromCurrentDate(habit.Goals);
                var result = CalculateDateDifference(habit.Goals);
                return habit.HabitStreak = result ? habit.HabitStreak + 1 : 0;
            }
            else
            {
                habit.HabitStreak = habit.HabitStreak + 1;
                return habit.HabitStreak;
            }
        }

        /// <summary>
        /// Method to check if Difference in date between two goals is more than a day
        /// </summary>
        /// <param name="goals"></param>
        /// <returns></returns>
        private bool CalculateDateDifference(List<IGoalModel> goals)
        {
            bool result = false;
            foreach (var goal in goals)
            {
                if (goal.TargetPeriod >= DateTime.Now && goal.ProgressStatus.Count > 0)
                {
                    var orderedProgress = goal.ProgressStatus.OrderByDescending(x => x.DatePerformed);

                    if ((DateTime.Now.Date - orderedProgress.FirstOrDefault().DatePerformed.Date).TotalDays == 1)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        #endregion












    }
}
