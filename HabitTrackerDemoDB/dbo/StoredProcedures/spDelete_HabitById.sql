CREATE PROCEDURE [dbo].[spDelete_HabitById]
	@Id int
AS
BEGIN
	set nocount on;
	Delete From Progress where GoalId IN (Select Id from Goal where HabitId = @Id);
	Delete from Goal where HabitId= @Id;
	Delete from Habit where Id = @Id;
END
RETURN 0
