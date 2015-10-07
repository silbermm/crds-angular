USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON

USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Pages]
			([Page_ID]
			,[Display_Name]
			,[Singular_Name]
			,[Description]
			,[View_Order]
			,[Table_Name]
			,[Primary_Key]
			,[Default_Field_List]
			,[Selected_Record_Expression]
			,[Display_Copy])
VALUES
			(508
			,'Payment Processor Event Errors'
			,'Payment Processor Event Error'
			,'Errors while processing asynchronous events sent from the Payment Processor (Stripe)'
			,90
			,'cr_Payment_Processor_Error_Events'
			,'Event_Error_ID'
			,'Event_Date_Time,Event_ID,Event_Type,Event_Message,Response_Message'
			,'Event_ID'
			,0)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO

INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID]
           ,[User_ID])
     VALUES
           (508
           ,9
           ,NULL)
GO

USE [MinistryPlatform]
GO

-- Grant "Edit" access on "Payment Processor Errors" Page to "Administrator"
INSERT [dbo].[dp_Role_Pages]
	([Role_ID],
	 [Page_ID],
	 [Access_Level],
	 [Scope_All],
	 [Approver],
	 [File_Attacher],
	 [Data_Importer],
	 [Data_Exporter],
	 [Secure_Records],
	 [Allow_Comments],
	 [Quick_Add])
VALUES
	(2,
	 508,
	 1,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0);
GO

-- Grant "Edit" access on "Payment Processor Errors" Page to "unauthenticatedCreate"
INSERT [dbo].[dp_Role_Pages]
	([Role_ID],
	 [Page_ID],
	 [Access_Level],
	 [Scope_All],
	 [Approver],
	 [File_Attacher],
	 [Data_Importer],
	 [Data_Exporter],
	 [Secure_Records],
	 [Allow_Comments],
	 [Quick_Add])
VALUES
	(62,
	 508,
	 1,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0);
GO
