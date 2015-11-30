USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Staff_Only_Feedback]    Script Date: 11/18/2015 08:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Staff_Only_Feedback]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Staff_Only_Feedback](
	[Staff_Only_Feedback_ID] [int] IDENTITY(1,1) NOT NULL,
	[Contact_ID] [int] NOT NULL,
	[Date_Submitted] [date] NOT NULL CONSTRAINT [DF_Staff_Only_Feedback_Date_Submitted]  DEFAULT (getdate()),
	[Description] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_cr_Staff_Only_Feedback] PRIMARY KEY CLUSTERED 
(
	[Staff_Only_Feedback_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Staff_Only_Feedback_Contacts]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Staff_Only_Feedback]'))
ALTER TABLE [dbo].[cr_Staff_Only_Feedback]  WITH CHECK ADD  CONSTRAINT [FK_cr_Staff_Only_Feedback_Contacts] FOREIGN KEY([Contact_ID])
REFERENCES [dbo].[Contacts] ([Contact_ID])
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Sub_Pages] WHERE [Sub_Page_ID] = 544)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON
	INSERT INTO [dbo].[dp_Sub_Pages]
           ([Sub_Page_ID]
		   ,[Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Key]
           ,[Relation_Type_ID]
           ,[On_Quick_Add]
           ,[Contact_ID_Field]
           ,[Display_Copy])
     VALUES
           (544
		   ,'Staff Only Feedback'
           ,'Staff Only Feedback'
           ,292
           ,11
           ,'cr_Staff_Only_Feedback'
           ,'Staff_Only_Feedback_ID'
           ,'Date_Submitted, Description, dp_Created.User_Name as Created_By'
           ,'Description'
           ,'Contact_ID'
           ,1
           ,0
           ,NULL
           ,0)
	SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF
END
ELSE 
	BEGIN
		UPDATE dp_Sub_Pages set Contact_ID_Field = NULL where Sub_Page_ID = 544
	END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Role_Sub_Pages] WHERE [Role_Sub_Page_ID] = 16105)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Role_Sub_Pages] ON
	INSERT INTO [dbo].[dp_Role_Sub_Pages]
           ([Role_Sub_Page_ID]
		   ,[Role_ID]
           ,[Sub_Page_ID]
           ,[Access_Level])
     VALUES
           (16105
		   ,2
           ,544
           ,3)
	SET IDENTITY_INSERT [dbo].[dp_Role_Sub_Pages] OFF
END
GO