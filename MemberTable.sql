USE [DEMO]
GO

/****** Object:  Table [dbo].[Member]    Script Date: 2020/9/12 ¤U¤È 05:18:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Member](
	[ID] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Salt] [varchar](5) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Birthday] [date] NULL,
	[AreaCode] [varchar](5) NULL,
	[Mobile] [varchar](50) NULL,
	[Country] [varchar](20) NULL,
	[City] [varchar](20) NULL,
	[District] [varchar](20) NULL,
	[Address] [varchar](50) NULL,
	[Status] [smallint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Member] ADD  CONSTRAINT [DF_Member_ID]  DEFAULT (newid()) FOR [ID]
GO

ALTER TABLE [dbo].[Member] ADD  CONSTRAINT [DF_Member_Status]  DEFAULT ((1)) FOR [Status]
GO

ALTER TABLE [dbo].[Member] ADD  CONSTRAINT [DF_Member_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

CREATE UNIQUE INDEX email ON [Member] ([Email] ASC);


