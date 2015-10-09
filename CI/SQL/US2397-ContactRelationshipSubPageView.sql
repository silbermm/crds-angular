USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Sub_Page_Views] WHERE [Sub_Page_View_ID] = 110)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
	INSERT INTO [dbo].[dp_Sub_Page_Views]
			   ([Sub_Page_View_ID]
			   ,[View_Title]
			   ,[Sub_Page_ID]
			   ,[Description]
			   ,[Field_List]
			   ,[View_Clause]
			   ,[Order_By])
		 VALUES
			   (110
			   ,'Relationships With IDs'
			   ,265
			   ,'Page View used to find/create relationships from CR.net'
			   ,'Relationship_ID_Table.[Relationship_ID], Related_Contact_ID_Table.[Contact_ID] AS ''Related_Contact_ID''
					, Related_Contact_ID_Table.[Nickname], Related_Contact_ID_Table.[Last_Name]
					, Contact_Relationships.[Start_Date], Contact_Relationships.[End_Date]'
			   ,'1=1'
			   ,'')
	SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF
END
