USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Sign_Up_Deadline]    Script Date: 4/22/2015 8:51:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Sign_Up_Deadline](
	[Sign_Up_Deadline_ID] [int] IDENTITY(1,1) NOT NULL,
	[Sign_Up_Deadline] [int] NOT NULL,
 CONSTRAINT [PK_cr_Sign_Up_Deadline] PRIMARY KEY CLUSTERED 
(
	[Sign_Up_Deadline_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO