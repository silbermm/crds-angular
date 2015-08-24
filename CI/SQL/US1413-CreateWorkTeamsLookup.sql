USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Work_Teams]    Script Date: 8/21/2015 10:19:09 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Work_Teams]') AND type in (N'U'))
DROP TABLE [dbo].[cr_Work_Teams]
GO

/****** Object:  Table [dbo].[cr_Work_Teams]    Script Date: 8/21/2015 10:19:09 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Work_Teams]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Work_Teams](
	[Work_Teams_ID] [int] IDENTITY(1,1) NOT NULL,
	[Work_Team] [nvarchar](50) NOT NULL
 CONSTRAINT [PK_cr_Work_Teams] PRIMARY KEY CLUSTERED 
(
	[Work_Teams_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET IDENTITY_INSERT [dbo].[cr_Work_Teams] ON;

INSERT INTO [dbo].[cr_Work_Teams]
(Work_Teams_ID, Work_Team)
VALUES
(1, 'Habitat (must be 16 or older)'),
(2, 'Vacation Bible Camp'),
(3, 'Hands On - Neighborhood Restoration')

SET IDENTITY_INSERT [dbo].[cr_Work_Teams] OFF;

GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Pages] WHERE [Page_ID] = 513)

SET IDENTITY_INSERT [dbo].[dp_Pages] ON;

INSERT INTO [dbo].[dp_Pages]
           ([Page_ID]
		   ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Display_Search]
		   ,[Default_Field_List]
		   ,[Selected_Record_Expression]
           ,[Display_Copy])
     VALUES
           (513
		   ,'Work Teams'
           ,'Work Team'
           ,'GO NOLA Work Teams'
           ,220
           ,'cr_Work_Teams'
           ,'Work_Teams_ID'
           ,1
		   ,'Work_Team'
		   ,'Work_Team'
           ,1)
SET IDENTITY_INSERT [dbo].[dp_Pages] OFF;

GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Section_Pages] WHERE [Page_ID] = 513)
INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (513
           ,4)
GO

