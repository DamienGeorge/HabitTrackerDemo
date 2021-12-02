CREATE PROCEDURE [dbo].[spFetchGoalById]
	@Id int
AS
Begin 
	set nocount on;

	SELECT * from dbo.Goal 
	Where Id = @Id;
End
