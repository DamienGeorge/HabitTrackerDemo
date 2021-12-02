using HabitTrackerLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HabitTrackerLibrary.Data
{
    public interface IHabitData
    {
        Task<int> AddGoal(IGoalModel goal);
        Task AddGoals(List<IGoalModel> goals);
        Task SaveProgress(IProgressModel progress);
        Task<List<IGoalModel>> FetchGoalsByHabitId(int Id);
        Task<List<IHabitModel>> FetchHabitsByOwnerId(string Id);
        Task<List<IProgressModel>> FetchProgressByGoalId(int Id);
        Task SaveNewHabit(IHabitModel habit);
        Task DeleteHabitById(int Id);
        Task UpdateGoal(IGoalModel goal);
        Task UpdateHabit(IHabitModel habit);
        Task DeleteGoalById(int id);
        Task<IGoalModel> FetchGoalById(int Id);
    }
}