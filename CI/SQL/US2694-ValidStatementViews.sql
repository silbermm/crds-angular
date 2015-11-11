USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		       ,[View_Title]
           ,[Page_ID]
           ,[View_Clause])
     VALUES
           (2195
		       ,'Donors Without Email Address'
           ,299
           ,'Contact_ID_Table.[Email_Address] IS NULL AND
		        Statement_Method_ID_Table.[Statement_Method_ID] = 2 AND
			      Contact_ID_Table_Contact_Status_ID_Table.[Contact_Status_ID] = 1' )

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF

GO


SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		       ,[View_Title]
           ,[Page_ID]
           ,[View_Clause])
     VALUES
           (2196
		   ,'Donors Without Mailing Address'
           ,299
           ,'(Contact_ID_Table_Household_ID_Table_Address_ID_Table.[Address_Line_1] IS NULL OR
		         Contact_ID_Table_Household_ID_Table_Address_ID_Table.[City] IS NULL OR
             Contact_ID_Table_Household_ID_Table_Address_ID_Table.[State/Region] IS NULL OR
             Contact_ID_Table_Household_ID_Table_Address_ID_Table.[Postal_Code] IS NULL) AND
             Statement_Method_ID_Table.[Statement_Method_ID] = 1 AND
             Contact_ID_Table_Contact_Status_ID_Table.[Contact_Status_ID] = 1' )

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF

GO


