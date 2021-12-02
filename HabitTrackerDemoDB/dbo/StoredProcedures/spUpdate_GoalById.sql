CREATE PROCEDURE [dbo].[spUpdate_GoalById]
@Id int,
	@GoalName varchar(50), 
	@GoalDescription nvarchar(400),
	@StartDate datetime2(7), 
	@TargetPeriod datetime2(7),
	@Active bit
AS
Begin
	set nocount on;
	Update Goal set GoalName=@GoalName , 
			GoalDescription = @GoalDescription,
			StartDate = @StartDate,
			TargetPeriod = @TargetPeriod,
			Active = @Active
			where Id = @Id
End
RETURN 0
