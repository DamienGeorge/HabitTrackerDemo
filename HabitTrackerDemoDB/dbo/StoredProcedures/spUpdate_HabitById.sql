CREATE PROCEDURE [dbo].[spUpdate_HabitById]
	@Id int,
	@HabitStreak int
AS
Begin
	set nocount on;
	Update Habit set HabitStreak = @HabitStreak
	where Id = @Id;
End
RETURN 0
