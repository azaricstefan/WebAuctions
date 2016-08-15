CREATE TABLE [dbo].[Auction](
	[Id] [int] NOT NULL,
	[user_Id] [nvarchar](128) NOT NULL Foreign key References AspNetUsers(Id),
	[name] [nvarchar](100) NOT NULL,
	[length] [int] NOT NULL,
	[price] [decimal](18, 2) NOT NULL,
	[creation] [datetime] NOT NULL,
	[opening] [datetime] NOT NULL,
	[closing] [datetime] NOT NULL,
	[details] [nvarchar](500) NOT NULL,
	[img] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Auction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO


