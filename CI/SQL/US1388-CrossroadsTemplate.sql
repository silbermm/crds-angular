USE [MinistryPlatform]
GO

BEGIN TRANSACTION
GO 

DECLARE @TemplateId int;

INSERT INTO [dbo].[dp_Communications]
           ([Author_User_ID]
           ,[Subject]
           ,[Body]
           ,[Domain_ID]
           ,[Start_Date]
           ,[Expire_Date]
           ,[Communication_Status_ID]
           ,[From_Contact]
           ,[Reply_to_Contact]
           ,[_Sent_From_Task]
           ,[Selection_ID]
           ,[Template]
           ,[Active]
           ,[To_Contact])
     VALUES
       (1,
       'Thank you for your investment - Crossroads',
       '<p> Thank you for your investment!   </p><p></p><p>
       Because of you, people will encounter God for the first time. Because of you, people in our city will receive care and hope. Because of you, relationships will be restored and individual lives will be transformed.   
       </p><p></p><p> <b>You Gave to</b>: [Program_Name]</p><p>
       <b>Amount</b>: $[Donation_Amount]</p><p>
       <b>Date</b>: [Donation_Date]</p>
       <p>  <b>Payment Method</b>: [Payment_Method]</p><p>
       If at any point you have questions, please contact our Finance team at <a href="mailto:finance@crossroads.net"><span style="color:#1155CC; font-family:Arial;">finance@crossroads.net</span></a>.</p>
       <p>  No goods or services were exchanged for this gift.</p>
       <p>  Note:</p><p>If you logged in to give, this will be recorded as part of your regular giving and will be included on your quarterly statement for tax purposes. If you do not currently receive a quarterly statement, you will begin to receive one now.  For guest givers, this is your receipt but you can register at <a href="http://www.crossroads.net"><span style="color:#1155CC; font-family:Arial;">www.crossroads.net</span></a> to recieve quarterly statements on future giving.</p><p>Thanks again for being part of the team!</p>
       <p>  - Crossroads </p>',
       1,
       '2015-06-03 13:00:00.000',
       null,
       1,
       7,
       7,
       null,
       null,
       1,
       1,
       null)
  SET @TemplateId = SCOPE_IDENTITY()


IF EXISTS (SELECT 1 FROM [dbo].[Programs]
  WHERE [dbo].[Programs].[Program_ID] = 3)
  
  BEGIN 
    UPDATE [dbo].[Programs]
    SET [Communication_ID] = @TemplateId
    WHERE [dbo].[Programs].[Program_ID] = 3
  END

GO

COMMIT