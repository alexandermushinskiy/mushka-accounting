USE [accounting]
GO

/****** Object:  Table [dbo].[category]    Script Date: 05/06/2018 15:43:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[category](
	[id] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](250) NOT NULL,
	[isSizesRequired] [bit] NOT NULL,
	[sizes] [nvarchar](max) NULL,
 CONSTRAINT IX_category_name UNIQUE([name]),
 CONSTRAINT [PK_category] PRIMARY KEY CLUSTERED
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

INSERT INTO [dbo].[category]
           ([id]
           ,[name]
           ,[isSizesRequired]
           ,[sizes])
     VALUES
           ('88CD0F34-9D4A-4E45-BE97-8899A97FB82C'
           ,N'Носки'
           ,1
           ,'36-39;39-42;41-45;43-46')
GO

INSERT INTO [dbo].[category]
           ([id]
           ,[name]
           ,[isSizesRequired]
           ,[sizes])
     VALUES
           ('0E7BE1DE-267C-4C0A-8EE9-ABA0A267F27A'
           ,N'Упаковка'
           ,1
           ,'Single;Triple;Big')
GO

INSERT INTO [dbo].[category]
           ([id]
           ,[name]
           ,[isSizesRequired])
     VALUES
           ('B425D75B-2E72-45F0-A55D-3BA400051E5F'
           ,N'Вспомогательные'
           ,0)
GO

