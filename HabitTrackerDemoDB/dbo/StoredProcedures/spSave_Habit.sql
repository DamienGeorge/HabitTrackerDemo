CREATE PROCEDURE [dbo].[spSave_Habit]
@HabitName nvarchar(50),
@HabitOwnerId nvarchar(450),
@Id int = 0 output
AS
begin
set nocount on;

	Insert into Habit (HabitName,HabitOwnerId) values(@HabitName,@HabitOwnerId)

	Select @Id = SCOPE_IDENTITY();
End


RETURN 0
