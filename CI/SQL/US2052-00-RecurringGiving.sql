USE [MinistryPlatform]
GO

CREATE TABLE [dbo].[Recurring_Gift_Days](
	[Day_Of_Week_ID] [int] IDENTITY(1,1) NOT NULL,
	[Day_Of_Week] [nchar](10) NOT NULL,
	[Domain_ID] [int] NOT NULL CONSTRAINT [DF_Recurring_Gift_Days_Domain_ID]  DEFAULT ((1)),
 CONSTRAINT [PK_Recurring_Gift_Days] PRIMARY KEY CLUSTERED
(
	[Day_Of_Week_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[Recurring_Gift_Days] ON
GO

INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(1,'Monday',1);
INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(2,'Tuesday',1);
INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(3,'Wednesday',1);
INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(4,'Thursday',1);
INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(5,'Friday',1);
INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(6,'Saturday',1);
INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(7,'Sunday',1);

SET IDENTITY_INSERT [dbo].[Recurring_Gift_Days] OFF
GO

CREATE TABLE [dbo].[Recurring_Gift_Frequencies](
	[Frequency_ID] [int] IDENTITY(1,1) NOT NULL,
	[Frequency] [nchar](10) NOT NULL,
	[Domain_ID] [int] NOT NULL CONSTRAINT [DF_Recurring_Gift_Frequencies_Domain_ID]  DEFAULT ((1)),
 CONSTRAINT [PK_Recurring_Gift_Frequencies] PRIMARY KEY CLUSTERED
(
	[Frequency_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[Recurring_Gift_Frequencies] ON
GO

INSERT INTO [dbo].[Recurring_Gift_Frequencies]([Frequency_ID],[Frequency],[Domain_ID]) VALUES(1,'Weekly',1);
INSERT INTO [dbo].[Recurring_Gift_Frequencies]([Frequency_ID],[Frequency],[Domain_ID]) VALUES(2,'Monthly',1);

SET IDENTITY_INSERT [dbo].[Recurring_Gift_Frequencies] OFF
GO

CREATE TABLE [dbo].[Recurring_Gifts](
	[Recurring_Gift_ID] [int] IDENTITY(1,1) NOT NULL,
	[Donor_ID] [int] NOT NULL,
	[Donor_Account_ID] [int] NOT NULL,
	[Frequency_ID] [int] NOT NULL,
	[Day_Of_Month] [int] NULL,
	[Day_Of_Week_ID] [int] NULL,
	[Amount] [money] NOT NULL,
	[Start_Date] [datetime] NOT NULL,
	[End_Date] [datetime] NULL,
	[Program_ID] [int] NOT NULL,
	[Congregation_ID] [int] NOT NULL,
	[Subscription_ID] [varchar](50) NOT NULL,
	[Domain_ID] [int] NOT NULL CONSTRAINT [DF_Recurring_Gifts_Domain_ID]  DEFAULT ((1)),
 CONSTRAINT [PK_Recurring_Gifts] PRIMARY KEY CLUSTERED
(
	[Recurring_Gift_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Congregations] FOREIGN KEY([Congregation_ID])
REFERENCES [dbo].[Congregations] ([Congregation_ID])
GO

ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Congregations]
GO

ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Donor_Accounts] FOREIGN KEY([Donor_Account_ID])
REFERENCES [dbo].[Donor_Accounts] ([Donor_Account_ID])
GO

ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Donor_Accounts]
GO

ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Donors] FOREIGN KEY([Donor_ID])
REFERENCES [dbo].[Donors] ([Donor_ID])
GO

ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Donors]
GO

ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_dp_Domains]
GO

ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Programs] FOREIGN KEY([Program_ID])
REFERENCES [dbo].[Programs] ([Program_ID])
GO

ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Programs]
GO

ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Recurring_Gift_Days] FOREIGN KEY([Day_Of_Week_ID])
REFERENCES [dbo].[Recurring_Gift_Days] ([Day_Of_Week_ID])
GO

ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Recurring_Gift_Days]
GO

ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Recurring_Gift_Frequencies] FOREIGN KEY([Frequency_ID])
REFERENCES [dbo].[Recurring_Gift_Frequencies] ([Frequency_ID])
GO

ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Recurring_Gift_Frequencies]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO

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
           (517
		   ,'Recurring Gifts'
           ,'Recurring Gift'
           ,'This is table that contains recurring gifts. '
           ,100
           ,'Recurring_Gifts'
           ,'Recurring_Gift_ID'
           ,1
           ,'Donor_ID_Table_Contact_ID_Table.Display_Name ,Donor_ID_Table_Contact_ID_Table.Email_Address ,Frequency_ID_Table.Frequency,Day_Of_Month, Amount,Start_Date,End_Date,Subscription_ID'
           ,'Recurring_Gift_ID'
           ,0)
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause] )
     VALUES
           (2182
		   ,'Recurring Gifts By Subscription ID'
           ,517
           ,'Recurring gifts by subscription'
					 ,'Subscription_ID,Donor_ID_Table.Donor_ID,Program_ID,Congregation_ID,Amount,Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type_ID],Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type]'
           ,'Subscription_ID IS NOT NULL' )
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO



INSERT INTO [dbo].[Account_Types]
           ([Account_Type])

     VALUES
           ('Credit Card')
GO
