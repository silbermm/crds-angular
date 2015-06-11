USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Onboarding_Statuses]    Script Date: 6/9/2015 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Onboarding_Statuses](
	[Onboarding_Status_ID] [int] IDENTITY(1,1) NOT NULL,
	[Onboarding_Status] [nvarchar](50) NOT NULL,
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

/****** Add Lookup Table to MP Page ******/
INSERT INTO [dbo].[dp_Pages]
           ([Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Display_Copy])
     VALUES
           ('Onboarding Statuses'
           ,'Onboarding Status'
           ,'Status of an Onboarding Step'
           ,305
           ,'cr_Onboarding_Statuses'
           ,'Onboarding_Status_ID'
           ,'Onboarding_Status'
           ,'Onboarding_Status'
           ,0)
GO

INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (SCOPE_IDENTITY()
           ,11)
GO

INSERT INTO [dbo].[dp_Sub_Pages]
           ([Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
           ,[Link_To_Page_ID]
           ,[Link_From_Field_Name]
           ,[Select_To_Page_ID]
           ,[Select_From_Field_Name]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Key]
           ,[Relation_Type_ID])
     VALUES
           ('Onboarding'
           ,'Onboarding'
           ,382
           ,6
           ,277
           ,'Attribute_ID'
           ,277
           ,'Response_Attributes.Attribute_ID'
           ,'Response_Attributes'
           ,'Response_Attribute_ID'
           ,'Attribute_ID_Table.[Attribute_Name] AS Step, Attribute_ID_Table.[Description] AS [Description] , Response_Attributes.[Start_Date] AS [Start Date] , Response_Attributes.[End_Date] AS [End Date], Onboarding_Status_ID_Table.[Onboarding_Status] AS [Onboarding Status] , Response_Attributes.[Notes] AS [Notes] , [dp_Updated].[Date_Time] AS [Date Updated] , [dp_Updated].[User_Name] AS [Updated By]'
           ,'Attribute_ID_Table.Attribute_Name'
           ,'Response_ID'
           ,2)
GO
