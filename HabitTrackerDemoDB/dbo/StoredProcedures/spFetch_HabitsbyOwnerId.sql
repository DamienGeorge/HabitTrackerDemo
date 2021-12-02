CREATE PROCEDURE [dbo].[spFetch_HabitsbyOwnerId]
	@Id nvarchar(450)
AS
Begin
set nocount on;
	SELECT [Id], [HabitName], [HabitOwnerId], [HabitStreak] from Habit where HabitOwnerId = @Id
End
RETURN 0
