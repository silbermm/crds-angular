USE [MinistryPlatform]
GO

INSERT INTO [dbo].[Donation_Statuses]
            ([Donation_Status]
           ,[Display_On_Statements]
           ,[Display_On_Giving_History])
     VALUES
           ('Pending'
           ,1
           ,0)
           ,
          ('Declined'
           ,1
           ,1)
           ,
           ('Deposited'
           ,1
           ,0)
GO