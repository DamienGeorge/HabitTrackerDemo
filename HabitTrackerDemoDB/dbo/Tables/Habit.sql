CREATE TABLE [dbo].[Habit]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [HabitName] NVARCHAR(50) NOT NULL, 
    [HabitOwnerId] NVARCHAR(450) NULL, 
    [HabitStreak] INT NULL, 
    CONSTRAINT [FK_Habit_ToAspNetUsers] FOREIGN KEY ([HabitOwnerId]) REFERENCES [dbo].[AspNetUsers]([Id])
)
