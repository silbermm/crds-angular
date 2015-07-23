USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Communications] ON 

INSERT INTO [dbo].[dp_Communications]
           ([Communication_ID]
		   ,[Author_User_ID]
           ,[Subject]
           ,[Body]
           ,[Domain_ID]
           ,[Start_Date]
           ,[From_Contact]
           ,[Reply_to_Contact]
           ,[Template]
           ,[Active])
     VALUES
           (11402
		   ,1
           ,'Thank you for your investment - Crossroads'
           ,'<p>Thank you for your investment!</p>
			Because of you, people will encounter God for the first time. Because of you, people in our city will receive care and hope. Because of you, relationships will be restored and individual lives will be transformed.
			</p><p></p><p><b>You gave to</b>: [Program_Name]<br /></p><p>
			<b>Amount</b>: $[Donation_Amount]</p><p>
			<b>Date</b>: [Donation_Date]</p>
			<p>	<b>Payment Method</b>: [Payment_Method]</p>
			<p>If at any point you have questions, please contact our Finance team at<a href="mailto:finance@crossroads.net"><span style="color:#1155CC; font-family:Arial;">finance@crossroads.net</span></a>.</p>
			<p>No goods or services were exchanged for this gift. </p>
			<p>Note:
			If you logged in to give, this will be recorded as part of your regular giving and will be included on your quarterly statement for tax purposes. If you do not currently receive a quarterly statement, you will begin to receive one now.  For guest givers, this is your receipt but you can register at www.crossroads.net to recieve quarterly statements on future giving.</p>

		<p>Thanks again for being part of the team!<br /></p>
			<p>	- Crossroads </p>'
           ,1
           ,2015-06-03
           ,7
           ,7
           ,1
           ,1 )

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO



