USE [accounting]
GO

/****** Object:  Table [dbo].[supplier]    Script Date: 05/10/2018 23:49:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[supplier](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Address] [nvarchar](500) NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](200) NULL,
	[WebSite] [nvarchar](200) NULL,
	[ContactPerson] [nvarchar](100) NULL,
	[PaymentConditions] [nvarchar](max) NULL,
	[Services] [nvarchar](max) NULL,
	[Comments] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT IX_supplier_name UNIQUE([name]),
 CONSTRAINT [PK_supplier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


INSERT INTO [dbo].[supplier]
           ([Id]
           ,[Name]
           ,[Address]
           ,[Phone]
           ,[Email]
           ,[WebSite]
           ,[ContactPerson]
           ,[PaymentConditions]
           ,[Services]
           ,[Comments]
           ,[CreatedOn])
     VALUES
           ('FE5570E0-FE4E-492E-933E-EACD6A31E22D'
           ,N'ТОВ "Новая Линия"'
           ,N'ул. Центральная 11/3, г.Тернополь, УКРАИНА'
           ,'+380(98)412-121'
           ,'info@socks.com'
           ,'socks.com.ua'
           ,N'Иванов Иван Иванович'
           ,N'Наличный, безналичный'
           ,N'Носки'
		   ,NULL
           ,GETDATE() )
GO

INSERT INTO [dbo].[supplier]
           ([Id]
           ,[Name]
           ,[Address]
           ,[Phone]
           ,[Email]
           ,[WebSite]
           ,[ContactPerson]
           ,[PaymentConditions]
           ,[Services]
           ,[Comments]
           ,[CreatedOn])
     VALUES
           ('FE557110-FE4E-492E-933E-EACD6A31E22D'
           ,N'Вова-Зи-Львов'
           ,N'ул. Шевченка 41, г.Львов, УКРАИНА'
           ,'+380(50)921-7654'
           ,'hello@vova-zi-lvova.com'
           ,'vova-zi-lvova.com.ua'
           ,N'Сахаров Владимир Сергеевич'
           ,N'Безналичный'
           ,N'Бирки'
		   ,NULL
           ,GETDATE() )
GO


