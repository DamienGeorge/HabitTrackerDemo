CREATE PROCEDURE [dbo].[spFetch_GoalsByHabitId]
	@Id int
AS
Begin
set nocount on;
	SELECT [Id], [HabitId], [GoalName], [GoalDescription], [StartDate], [TargetPeriod] , Active
	from dbo.Goal where HabitId = @Id;
	
End
RETURN 0
