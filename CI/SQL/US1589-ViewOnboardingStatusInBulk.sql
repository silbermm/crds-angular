USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Onboarding_Statuses]    Script Date: 7/16/2015 3:51:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Onboarding_Statuses]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Onboarding_Statuses](
	[Onboarding_Status_ID] [int] IDENTITY(1,1) NOT NULL,
	[Onboarding_Status] [nvarchar](50) NOT NULL,
	[Final_Status] [bit] NOT NULL CONSTRAINT [DF_cr_Onboarding_Statuses_Final_Status]  DEFAULT ((0)),
 CONSTRAINT [PK_cr_Onboarding_Status] PRIMARY KEY CLUSTERED 
(
	[Onboarding_Status_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

UPDATE dp_Sub_Pages
SET Default_Field_List = 'Responses.Response_Date
,Participant_ID_Table_Contact_ID_Table.Display_Name
,(SELECT TOP 1 Follow_Up_Date FROM Response_Follow_Ups WHERE Response_Follow_Ups.Response_ID = Responses.Response_ID ORDER BY Follow_Up_Date DESC) AS Last_Follow_Up
,Responses.Closed
,Event_ID_Table.[Event_Start_Date] AS [Event Start Date], (SELECT TOP 1 a.Attribute_Name FROM Response_Attributes as ra JOIN [MinistryPlatform].[dbo].cr_Onboarding_Statuses as s on ra.Onboarding_Status_ID = s.Onboarding_Status_ID
  JOIN [MinistryPlatform].[dbo].[Attributes] as a on ra.Attribute_ID = a.Attribute_ID
  WHERE ra.Response_ID = Responses.Response_ID AND s.[Final_Status] = 0
  ORDER BY ra.[Order]) AS Current_Onboarding_Step'
WHERE Sub_Page_ID = 324
GO