USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Onboarding_Statuses]    Script Date: 6/9/2015 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Onboarding_Statuses](
	[Onboarding_Status_ID] [int] IDENTITY(1,1) NOT NULL,
	[Onboarding_Status] [int] NOT NULL,
 CONSTRAINT [PK_cr_Onboarding_Status] PRIMARY KEY CLUSTERED 
(
	[Onboarding_Status_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Response_Attributes]    Script Date: 6/9/2015 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Response_Attributes](
	[Response_Attribute_ID] [int] IDENTITY(1,1) NOT NULL,
	[Attribute_ID] [int] NOT NULL,
	[Response_ID] [int] NOT NULL,
	[Domain_ID] [int] NOT NULL,
	[Start_Date] [datetime] NOT NULL,
	[End_Date] [datetime] NULL,
	[Notes] [nvarchar](255) NULL,
	[Order] [int] NULL,
	[Onboarding_Status_ID] [int] NOT NULL,
 CONSTRAINT [PK_Response_Attributes] PRIMARY KEY CLUSTERED 
(
	[Response_Attribute_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Response_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Response_Attributes_Attributes] FOREIGN KEY([Attribute_ID])
REFERENCES [dbo].[Attributes] ([Attribute_ID])
GO

ALTER TABLE [dbo].[Response_Attributes] CHECK CONSTRAINT [FK_Response_Attributes_Attributes]
GO

ALTER TABLE [dbo].[Response_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Response_Attributes_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[Response_Attributes] CHECK CONSTRAINT [FK_Response_Attributes_dp_Domains]
GO

ALTER TABLE [dbo].[Response_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Response_Attributes_Responses] FOREIGN KEY([Response_ID])
REFERENCES [dbo].[Responses] ([Response_ID])
GO

ALTER TABLE [dbo].[Response_Attributes] CHECK CONSTRAINT [FK_Response_Attributes_Responses]
GO

ALTER TABLE [dbo].[Response_Attributes] WITH CHECK ADD CONSTRAINT [FK_Response_Attributes_cr_Onboarding_Statuses] FOREIGN KEY([Onboarding_Status_ID])
REFERENCES [dbo].[cr_Onboarding_Statuses] ([Onboarding_Status_ID])

ALTER TABLE [dbo].[Response_Attributes] CHECK CONSTRAINT [FK_Response_Attributes_cr_Onboarding_Statuses]
GO

