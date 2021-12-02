CREATE PROCEDURE [dbo].[spSave_Progress]
	@GoalId int ,
	@DatePerformed datetime2(7),
	@Completed bit
AS
BEGIN
set nocount on;
	INSERT into dbo.Progress (GoalId,DatePerformed,Completed) values (@GoalId,@DatePerformed,@Completed);
END

