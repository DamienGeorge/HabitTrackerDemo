CREATE TABLE [dbo].[Goal]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [HabitId] INT NULL, 
    [GoalName] NVARCHAR(50) NULL, 
    [GoalDescription] NVARCHAR(400) NULL, 
    [StartDate] DATETIME2 NULL, 
    [TargetPeriod] DATETIME2 NULL, 
    [Active] BIT NULL, 
    CONSTRAINT [FK_Goal_ToHabit] FOREIGN KEY ([HabitId]) REFERENCES [Habit]([Id])
)
