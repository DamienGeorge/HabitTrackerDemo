CREATE PROCEDURE [dbo].[spFetch_ProgressByGoalId]
	@Id int
AS
Begin
	set nocount on;
	SELECT * from dbo.Progress where GoalId= @Id;
End
RETURN 0
