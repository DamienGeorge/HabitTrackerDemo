CREATE TABLE [dbo].[Progress]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [GoalId] INT NOT NULL, 
    [DatePerformed] DATETIME2 NULL, 
    [Completed] BIT NULL, 
    CONSTRAINT [FK_Progress_ToGoal] FOREIGN KEY (GoalId) REFERENCES dbo.Goal([Id])
)
