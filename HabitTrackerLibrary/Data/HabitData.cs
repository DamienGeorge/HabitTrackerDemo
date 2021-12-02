using Dapper;
using HabitTrackerLibrary.DataAccess;
using HabitTrackerLibrary.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace HabitTrackerLibrary.Data
{
    public class HabitData : IHabitData
    {
        private readonly IDataAccess _dataAccess;
        private const string DatabaseConfigName = "DefaultConnection";
        public HabitData(IDataAccess DataAccess)
        {
            _dataAccess = DataAccess;
        }

        public async Task SaveNewHabit(IHabitModel habit)
        {
            var habitparameters = new DynamicParameters();
            habitparameters.Add("@HabitName", habit.HabitName);
            habitparameters.Add("@HabitOwnerId", habit.HabitOwnerId);
            habitparameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);


            await _dataAccess.SaveData("dbo.spSave_Habit", habitparameters, DatabaseConfigName);

            foreach (IGoalModel goal in habit.Goals)
            {
                goal.HabitId = habitparameters.Get<int>("@Id");
            }

            await AddGoals(habit.Goals);
        }

        public async Task AddGoals(List<IGoalModel> goals)
        {
            foreach (IGoalModel goal in goals)
            {
                await AddGoal(goal);
            }
        }

        public async Task<int> AddGoal(IGoalModel goal)
        {
            var goalParameters = new DynamicParameters();
            goalParameters.Add("@GoalName", goal.GoalName);
            goalParameters.Add("@GoalDescription", goal.GoalDescription);
            goalParameters.Add("@HabitId", goal.HabitId);
            goalParameters.Add("@StartDate", goal.StartDate);
            goalParameters.Add("@TargetPeriod", goal.TargetPeriod);
            goalParameters.Add("@Active", goal.Active);
            goalParameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("dbo.spAdd_Goal", goalParameters, DatabaseConfigName);

            return goalParameters.Get<int>("@Id");
        }

        public async Task SaveProgress (IProgressModel progress)
        {
            var progressparameters = new DynamicParameters();

            progressparameters.Add("@GoalId", progress.GoalId);
            progressparameters.Add("@DatePerformed", progress.DatePerformed);
            progressparameters.Add("@Completed", progress.Completed);

            await _dataAccess.SaveData("dbo.spSave_Progress", progressparameters, DatabaseConfigName);
        }

        public async Task<List<IGoalModel>> FetchGoalsByHabitId(int Id)
        {
           var rows = await _dataAccess.LoadData<GoalModel,dynamic>("dbo.spFetch_GoalsByHabitId", new { Id }, DatabaseConfigName);

            foreach (var row in rows)
            {
                row.ProgressStatus = await FetchProgressByGoalId(row.Id);
            }

            return rows.ToList<IGoalModel>();
        }

        public async Task<List<IHabitModel>> FetchHabitsByOwnerId(string Id)
        {
            var rows = await _dataAccess.LoadData<HabitModel, dynamic>("dbo.spFetch_HabitsbyOwnerId", new { Id }, DatabaseConfigName);

            List<IHabitModel> habits = new List<IHabitModel>();

            foreach(var row in rows)
            {
                row.Goals = await FetchGoalsByHabitId(row.Id);
            }

            return rows.ToList<IHabitModel>();
        }

        public async Task<List<IProgressModel>> FetchProgressByGoalId(int Id)
        {
            var rows = await _dataAccess.LoadData<ProgressModel, dynamic>("dbo.spFetch_ProgressByGoalId", new { Id }, DatabaseConfigName);

            return rows.ToList<IProgressModel>();
        }

        //TODO- Create a delete habit, goal and progress method
        public async Task DeleteHabitById(int Id)
        {
            await _dataAccess.SaveData("dbo.spDelete_HabitById", new { Id }, DatabaseConfigName);
        }

        public async Task UpdateGoal(IGoalModel goal)
        {
            var goalParameters = new DynamicParameters();

            goalParameters.Add("@Id", goal.Id);
            goalParameters.Add("@GoalName", goal.GoalName);
            goalParameters.Add("@GoalDescription", goal.GoalDescription);
            goalParameters.Add("@StartDate", goal.StartDate);
            goalParameters.Add("@TargetPeriod", goal.TargetPeriod);
            goalParameters.Add("@Active", goal.Active);

            await _dataAccess.SaveData("dbo.spUpdate_GoalById", goalParameters, DatabaseConfigName);
        }
        public async Task UpdateHabit(IHabitModel habit)
        {
            var habitParameters = new DynamicParameters();

            habitParameters.Add("@Id", habit.Id);
            habitParameters.Add("@HabitStreak", habit.HabitStreak);

            await _dataAccess.SaveData("dbo.spUpdate_HabitById", habitParameters, DatabaseConfigName);
        }

        public async Task DeleteGoalById(int id)
        {
            await _dataAccess.SaveData("dbo.Delete_GoalById", new { id }, DatabaseConfigName);
        }

        public async Task<IGoalModel> FetchGoalById(int Id)
        {
            var rows =  await _dataAccess.LoadData<GoalModel, dynamic>("dbo.spFetchGoalById", new { Id }, DatabaseConfigName);

            return rows.ToList<IGoalModel>().FirstOrDefault();
        }
    }
}
