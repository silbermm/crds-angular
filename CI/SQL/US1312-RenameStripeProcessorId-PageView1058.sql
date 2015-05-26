USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO
DELETE FROM [dbo].[dp_Page_Views]
WHERE Page_View_ID = 1058
GO


INSERT INTO [dbo].[dp_Page_Views]
           (Page_View_ID
           ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause]
           ,[Order_By]
           ,[User_ID]
           ,[User_Group_ID])
     VALUES
           (1058
           ,'DonorByContactId'
           ,299
           ,'Returns donors by contact ID'
           ,'Contact_ID_Table.[Contact_ID], Donors.[Donor_ID], Donors.[Processor_ID], Statement_Frequency_ID_Table.[Statement_Frequency], Statement_Type_ID_Table.[Statement_Type], Statement_Method_ID_Table.[Statement_Method], Donors.[Setup_Date]'
           ,'Donors.[Donor_ID] <> 0'
           ,'Donors.[Donor_ID]'
           ,NULL
           ,NULL)
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO
