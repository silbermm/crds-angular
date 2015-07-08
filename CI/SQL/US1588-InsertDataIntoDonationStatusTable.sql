USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[Donation_Statuses] ON
GO
INSERT INTO [dbo].[Donation_Statuses]
            ([Donation_Status_ID]
           ,[Donation_Status]
           ,[Display_On_Statements]
           ,[Display_On_Giving_History])
     VALUES
           (1
           ,'Pending'
           ,1
           ,0)
           ,
           (2
           ,'Deposited'
           ,1
           ,0)
           ,
           (3
           ,'Declined'
           ,1
           ,1)
           ,
           (4
           ,'Succeeded'
           ,1
           ,1)
GO
SET IDENTITY_INSERT [dbo].[Donation_Statuses] OFF
GO
