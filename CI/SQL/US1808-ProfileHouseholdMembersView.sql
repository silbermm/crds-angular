USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Sub_Page_Views] WHERE [Sub_Page_View_ID] = 102)
BEGIN
SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
INSERT INTO [dbo].[dp_Sub_Page_Views]
           ([Sub_Page_View_ID]
		   ,[View_Title]
           ,[Sub_Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (102
		   ,'Profile Household Member'
           ,307
           ,'View used by profile page in CR.net'
           ,'Contacts.[Contact_ID], Contacts.[First_Name], Contacts.[Nickname], Contacts.[Last_Name], Household_Position_ID_Table.[Household_Position_ID]
		   , Household_Position_ID_Table.[Household_Position], Contacts.[Date_of_Birth], Contacts.[__Age]'
           ,'(1=1)')
SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF
END
GO