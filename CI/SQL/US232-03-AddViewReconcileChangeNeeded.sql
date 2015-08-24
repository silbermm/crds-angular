USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([View_Title]
           ,[Page_ID]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           ('Reconcile Change Needed'
           ,297
           ,'Donations.[Donation_Date] AS [Donation Date], Donor_ID_Table_Contact_ID_Table.[Display_Name] AS [Display Name], Donor_ID_Table_Contact_ID_Table.[Nickname] AS [Nickname], Donor_ID_Table_Contact_ID_Table.[Email_Address] AS [Email Address], Donations.[Donation_Amount] AS [Donation Amount], Payment_Type_ID_Table.[Payment_Type] AS [Payment Type], Donation_Status_ID_Table.[Donation_Status] AS [Donation Status], Donations.[Donation_Status_Date] AS [Donation Status Date], Donations.[Reconcile_Change_Needed] AS [Reconcile Change Needed], Donations.[Reconcile_Change_Complete] AS [Reconcile Change Complete]'
           ,'Donations.[Reconcile_Change_Complete] = 0')
GO
