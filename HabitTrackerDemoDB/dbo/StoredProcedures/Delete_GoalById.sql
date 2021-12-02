CREATE PROCEDURE [dbo].[Delete_GoalById]
	@id int 
AS
begin
set nocount on;

	Delete from dbo.Progress
	where GoalId = @id;
	
	Delete from dbo.Goal
	where Id= @id; 
END
RETURN 0
