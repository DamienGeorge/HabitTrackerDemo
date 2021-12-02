CREATE PROCEDURE [dbo].[spAdd_Goal]
	@GoalName nvarchar(50),
	@GoalDescription nvarchar(400),
	@HabitId int,
	@StartDate datetime2(7),
	@TargetPeriod datetime2(7),
	@Active bit,
	@Id int=0 output
AS
Begin
	set nocount on;

	Insert into Goal (GoalName,GoalDescription, HabitId, StartDate,TargetPeriod,Active) values (@GoalName,@GoalDescription,@HabitId, @StartDate, @TargetPeriod,@Active);

	Select  @Id= SCOPE_IDENTITY();
END
