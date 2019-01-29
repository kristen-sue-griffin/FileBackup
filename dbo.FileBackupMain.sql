CREATE TABLE [dbo].[FileBackupMain] (
    [PK_FileId]              INT           IDENTITY (1, 1) NOT NULL,
    [FileName]               VARCHAR (500) NULL,
    [FileLastEditedDateTime] DATETIME      NULL,
    CONSTRAINT [PK_FileBackupMain] PRIMARY KEY CLUSTERED ([PK_FileId] ASC)
);