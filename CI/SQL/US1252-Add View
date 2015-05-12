USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
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
           (1098
           ,'Possible Guest Donor'
           ,292
           ,'Returns contacts without a user record'
           ,'Contacts.Contact_ID, Email_Address, Donor_Record, Donor_Record_Table.Stripe_Customer_ID'
           ,'User_Account is NULL'
           ,NULL
           ,NULL
           ,NULL)
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO
