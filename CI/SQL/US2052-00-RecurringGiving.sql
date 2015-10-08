USE [MinistryPlatform]
GO

IF (NOT EXISTS (SELECT *
                 FROM INFORMATION_SCHEMA.TABLES
                 WHERE TABLE_NAME = 'Recurring_Gift_Days'))
BEGIN
	CREATE TABLE [dbo].[Recurring_Gift_Days](
		[Day_Of_Week_ID] [int] IDENTITY(1,1) NOT NULL,
		[Day_Of_Week] [nchar](10) NOT NULL,
		[Domain_ID] [int] NOT NULL CONSTRAINT [DF_Recurring_Gift_Days_Domain_ID]  DEFAULT ((1)),
	 CONSTRAINT [PK_Recurring_Gift_Days] PRIMARY KEY CLUSTERED
	(
		[Day_Of_Week_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];

	SET IDENTITY_INSERT [dbo].[Recurring_Gift_Days] ON;

	INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(1,'Monday',1);
	INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(2,'Tuesday',1);
	INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(3,'Wednesday',1);
	INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(4,'Thursday',1);
	INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(5,'Friday',1);
	INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(6,'Saturday',1);
	INSERT INTO [dbo].[Recurring_Gift_Days]([Day_Of_Week_ID],[Day_Of_Week],[Domain_ID]) VALUES(7,'Sunday',1);

	SET IDENTITY_INSERT [dbo].[Recurring_Gift_Days] OFF;
END

IF (NOT EXISTS (SELECT *
                 FROM [dbo].[dp_Pages]
                 WHERE [Page_ID] = 521))
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Pages] ON;

	INSERT INTO [dbo].[dp_Pages] (
		 [Page_ID]
		,[Display_Name]
		,[Singular_Name]
		,[Description]
		,[View_Order]
		,[Table_Name]
		,[Primary_Key]
		,[Display_Search]
		,[Default_Field_List]
		,[Selected_Record_Expression]
		,[Display_Copy]
)	VALUES(
	 521
	,'Recurring Gift Days'
	,'Recurring Gift Day'
	,'List of Recurring Gift Days (Monday-Sunday)'
	,10
	,'Recurring_Gift_Days'
	,'Day_Of_Week_ID'
	,NULL
	,'Day_Of_Week_ID, Day_Of_Week'
	,'Day_Of_Week'
	,0);

	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF;

	INSERT INTO [dbo].[dp_Page_Section_Pages]([Page_ID],[Page_Section_ID],[User_ID]) VALUES(521,4,NULL);

		-- Grant "Full" access on this page to "unauthenticatedCreate" role
		INSERT INTO [dbo].[dp_Role_Pages] (
		  [Role_ID],
		  [Page_ID],
		  [Access_Level],
		  [Scope_All],
		  [Approver],
		  [File_Attacher],
		  [Data_Importer],
		  [Data_Exporter],
		  [Secure_Records],
		  [Allow_Comments],
		  [Quick_Add]
		) VALUES (
		  62,
		  521,
		  3,
		  0,
		  0,
		  0,
		  0,
		  0,
		  0,
		  0,
		  0
		);

	-- Grant "Full" access on this page to "Stewardship Donation Processor" role
	INSERT INTO [dbo].[dp_Role_Pages] (
	  [Role_ID],
	  [Page_ID],
	  [Access_Level],
	  [Scope_All],
	  [Approver],
	  [File_Attacher],
	  [Data_Importer],
	  [Data_Exporter],
	  [Secure_Records],
	  [Allow_Comments],
	  [Quick_Add]
	) VALUES (
	  7,
	  521,
	  3,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0
	);
END

IF (NOT EXISTS (SELECT *
                 FROM INFORMATION_SCHEMA.TABLES
                 WHERE TABLE_NAME = 'Recurring_Gift_Frequencies'))
BEGIN
	CREATE TABLE [dbo].[Recurring_Gift_Frequencies](
		[Frequency_ID] [int] IDENTITY(1,1) NOT NULL,
		[Frequency] [nchar](10) NOT NULL,
		[Domain_ID] [int] NOT NULL CONSTRAINT [DF_Recurring_Gift_Frequencies_Domain_ID]  DEFAULT ((1)),
	 CONSTRAINT [PK_Recurring_Gift_Frequencies] PRIMARY KEY CLUSTERED
	(
		[Frequency_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];

	SET IDENTITY_INSERT [dbo].[Recurring_Gift_Frequencies] ON;

	INSERT INTO [dbo].[Recurring_Gift_Frequencies]([Frequency_ID],[Frequency],[Domain_ID]) VALUES(1,'Weekly',1);
	INSERT INTO [dbo].[Recurring_Gift_Frequencies]([Frequency_ID],[Frequency],[Domain_ID]) VALUES(2,'Monthly',1);

	SET IDENTITY_INSERT [dbo].[Recurring_Gift_Frequencies] OFF;
END

IF (NOT EXISTS (SELECT *
                 FROM [dbo].[dp_Pages]
                 WHERE [Page_ID] = 522))
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Pages] ON;

	INSERT INTO [dbo].[dp_Pages] (
		 [Page_ID]
		,[Display_Name]
		,[Singular_Name]
		,[Description]
		,[View_Order]
		,[Table_Name]
		,[Primary_Key]
		,[Display_Search]
		,[Default_Field_List]
		,[Selected_Record_Expression]
		,[Display_Copy]
)	VALUES(
	 522
	,'Recurring Gift Frequencies'
	,'Recurring Gift Frequency'
	,'List of Recurring Gift Frequencies (Weekly, Monthly, etc)'
	,20
	,'Recurring_Gift_Frequencies'
	,'Frequency_ID'
	,NULL
	,'Frequency_ID, Frequency'
	,'Frequency'
	,0);

	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF;

	INSERT INTO [dbo].[dp_Page_Section_Pages]([Page_ID],[Page_Section_ID],[User_ID]) VALUES(522,4,NULL);

		-- Grant "Full" access on this page to "unauthenticatedCreate" role
		INSERT INTO [dbo].[dp_Role_Pages] (
		  [Role_ID],
		  [Page_ID],
		  [Access_Level],
		  [Scope_All],
		  [Approver],
		  [File_Attacher],
		  [Data_Importer],
		  [Data_Exporter],
		  [Secure_Records],
		  [Allow_Comments],
		  [Quick_Add]
		) VALUES (
		  62,
		  522,
		  3,
		  0,
		  0,
		  0,
		  0,
		  0,
		  0,
		  0,
		  0
		);

	-- Grant "Full" access on this page to "Stewardship Donation Processor" role
	INSERT INTO [dbo].[dp_Role_Pages] (
	  [Role_ID],
	  [Page_ID],
	  [Access_Level],
	  [Scope_All],
	  [Approver],
	  [File_Attacher],
	  [Data_Importer],
	  [Data_Exporter],
	  [Secure_Records],
	  [Allow_Comments],
	  [Quick_Add]
	) VALUES (
	  7,
	  522,
	  3,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0
	);
END

IF (NOT EXISTS (SELECT *
                 FROM INFORMATION_SCHEMA.TABLES
                 WHERE TABLE_NAME = 'Recurring_Gifts'))
BEGIN
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
	) ON [PRIMARY];

	ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Congregations] FOREIGN KEY([Congregation_ID])
	REFERENCES [dbo].[Congregations] ([Congregation_ID]);

	ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Congregations];

	ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Donor_Accounts] FOREIGN KEY([Donor_Account_ID])
	REFERENCES [dbo].[Donor_Accounts] ([Donor_Account_ID]);

	ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Donor_Accounts];

	ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Donors] FOREIGN KEY([Donor_ID])
	REFERENCES [dbo].[Donors] ([Donor_ID]);

	ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Donors];

	ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_dp_Domains] FOREIGN KEY([Domain_ID])
	REFERENCES [dbo].[dp_Domains] ([Domain_ID]);

	ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_dp_Domains];

	ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Programs] FOREIGN KEY([Program_ID])
	REFERENCES [dbo].[Programs] ([Program_ID]);

	ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Programs];

	ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Recurring_Gift_Days] FOREIGN KEY([Day_Of_Week_ID])
	REFERENCES [dbo].[Recurring_Gift_Days] ([Day_Of_Week_ID]);

	ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Recurring_Gift_Days];

	ALTER TABLE [dbo].[Recurring_Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Gifts_Recurring_Gift_Frequencies] FOREIGN KEY([Frequency_ID])
	REFERENCES [dbo].[Recurring_Gift_Frequencies] ([Frequency_ID]);

	ALTER TABLE [dbo].[Recurring_Gifts] CHECK CONSTRAINT [FK_Recurring_Gifts_Recurring_Gift_Frequencies];
END

IF (NOT EXISTS (SELECT *
                 FROM [dbo].[dp_Pages]
                 WHERE [Page_ID] = 517))
BEGIN
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
	           (517
			   ,'Recurring Gifts'
	           ,'Recurring Gift'
	           ,'This is table that contains recurring gifts. '
	           ,100
	           ,'Recurring_Gifts'
	           ,'Recurring_Gift_ID'
	           ,1
	           ,'Donor_ID_Table_Contact_ID_Table.[Display_Name]
	, Donor_ID_Table_Contact_ID_Table_User_Account_Table.[User_Email]
	, Frequency_ID_Table.[Frequency]
	, CASE(Frequency_ID_Table.Frequency_ID)
	  WHEN 1
	    THEN
	      CONCAT(
	        ''Every '',
	        Day_Of_Week_ID_Table.Day_Of_Week
	      )
	  ELSE
	    CONCAT(
	      CAST(Day_Of_Month AS VARCHAR),
	      CASE(Day_Of_Month % 10)
	        WHEN 1 THEN ''st''
	        WHEN 2 THEN ''nd''
	        WHEN 3 THEN ''rd''
	        ELSE ''th''
	      END,
	      '' of the month''
	    )
	  END AS Recurrence
	, Recurring_Gifts.[Start_Date]
	, Recurring_Gifts.[End_Date]
	, Recurring_Gifts.[Amount]
	, Program_ID_Table.[Program_Name]
	, Congregation_ID_Table.[Congregation_Name]
	, CONCAT(Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type], ''/'', Donor_Account_ID_Table.[Account_Number]
	, ''/'', Donor_Account_ID_Table.[Institution_Name]) AS [Donor_Account]
	, Recurring_Gifts.[Subscription_ID]'
	           ,'Recurring_Gift_ID'
	           ,0);

	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF;

	INSERT INTO [dbo].[dp_Page_Section_Pages]([Page_ID],[Page_Section_ID],[User_ID]) VALUES(517,9,NULL);

	-- Grant "Full" access on this page to "unauthenticatedCreate" role
	INSERT INTO [dbo].[dp_Role_Pages] (
	  [Role_ID],
	  [Page_ID],
	  [Access_Level],
	  [Scope_All],
	  [Approver],
	  [File_Attacher],
	  [Data_Importer],
	  [Data_Exporter],
	  [Secure_Records],
	  [Allow_Comments],
	  [Quick_Add]
	) VALUES (
	  62,
	  517,
	  3,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0
	);

	-- Grant "Full" access on this page to "Stewardship Donation Processor" role
	INSERT INTO [dbo].[dp_Role_Pages] (
	  [Role_ID],
	  [Page_ID],
	  [Access_Level],
	  [Scope_All],
	  [Approver],
	  [File_Attacher],
	  [Data_Importer],
	  [Data_Exporter],
	  [Secure_Records],
	  [Allow_Comments],
	  [Quick_Add]
	) VALUES (
	  7,
	  517,
	  3,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0,
	  0
	);
END


IF (NOT EXISTS (SELECT *
                 FROM [dbo].[dp_Page_Views]
                 WHERE [Page_View_ID] = 2182))
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;

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
						 ,'Recurring_Gifts.[Recurring_Gift_ID],Subscription_ID,Donor_ID_Table.Donor_ID,Program_ID,Congregation_ID,Amount,Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type_ID],Donor_Account_ID_Table.Donor_Account_ID,Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type]'
	           ,'Subscription_ID IS NOT NULL' );

	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF;
END

IF (NOT EXISTS (SELECT *
                 FROM [dbo].[Account_Types]
                 WHERE [Account_Type] = 'Credit Card'))
BEGIN
	INSERT INTO [dbo].[Account_Types]
	           ([Account_Type])

	     VALUES
	           ('Credit Card');
END
