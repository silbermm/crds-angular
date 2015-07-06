USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Donation_Statuses](
	[Donation_Status_ID] [int] IDENTITY(1,1) NOT NULL,
	[Donation_Status] [nvarchar](50) NOT NULL,
	[Display_On_Statements] [bit] NULL,
	[Display_On_Giving_History] [bit] NULL,
 CONSTRAINT [PK_Donation_Status] PRIMARY KEY CLUSTERED 
(
	[Donation_Status_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


GO
