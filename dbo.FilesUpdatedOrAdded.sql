CREATE TABLE [dbo].[FilesUpdatedOrAdded] (
    [FileFullNamePath]   VARCHAR (5000) NOT NULL,
    [FileChangeType]     INT            NULL,
    [FileChangeDateTime] DATETIME       NULL,
    CONSTRAINT [PK_dbo.FileFullNamePath] PRIMARY KEY CLUSTERED ([FileFullNamePath] ASC)
);