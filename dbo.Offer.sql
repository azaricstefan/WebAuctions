CREATE TABLE [dbo].[Offer](
	[Id] [int] NOT NULL,
	[user_Id] [nvarchar](128) NOT NULL Foreign key References AspNetUsers(Id),
	[auction_Id] INT NOT NULL Foreign key References Auction(Id),
	[value] DECIMAL(18, 2) NOT NULL,
	[time] DATETIME NOT NULL
	)