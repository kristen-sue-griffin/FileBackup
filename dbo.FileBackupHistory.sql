CREATE TABLE [dbo].[FileBackupHistory] (
    [PK_FileHistoryId]          INT            IDENTITY (1, 1) NOT NULL,
    [FileHistoryZipPathName]    VARCHAR (5000) NULL,
    [FileHistoryDescription]    VARCHAR (5000) NULL,
    [FileHistoryOriginPath]     VARCHAR (500)  NULL,
    [FileHistoryBackupDateTime] DATETIME       NULL,
    [FileHistoryUserName]       VARCHAR (50)   NULL,
    [FileHistoryEditedDateTime] DATETIME       NULL,
    [FK_FileId]                 INT            NULL,
    CONSTRAINT [PK_FileBackupHistory] PRIMARY KEY CLUSTERED ([PK_FileHistoryId] ASC),
    CONSTRAINT [FK_FileBackupHistory_FileBackupMain] FOREIGN KEY ([FK_FileId]) REFERENCES [dbo].[FileBackupMain] ([PK_FileId])
);