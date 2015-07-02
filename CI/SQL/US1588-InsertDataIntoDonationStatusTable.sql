USE [MinistryPlatform]
GO

INSERT INTO [dbo].[Donation_Status]
            ([Donation_Status]
           ,[Display_On_Statements]
           ,[Display_On_Giving_History])
     VALUES
           ('Pending'
           ,0
           ,1)
           ,
          ('Declined'
           ,1
           ,1)
           ,
           ('Deposited'
           ,1
           ,0)
GO