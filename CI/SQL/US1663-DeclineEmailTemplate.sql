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
          (12200, 5, 'Thank you for your investment, but there was a problem.',
            '<p>Oh no!  The Gift you recently initiated was declined! </p>
Details can be found below:
<p></p><p></p><p><b>You attempted to give to</b>: [Program_Name]<br /></p><p>
<b>Amount</b>: $[Donation_Amount]</p><p>
<b>Date</b>: [Donation_Date]</p>
<p> <b>Payment Method</b>: [Payment_Method]</p>
<b>Decline Reason</b>: [Decline_Reason]<div><br /></div><div>
Please contact the Finance team at <a href="mailto:finance@crossroads.net"><span style="color:#1155CC; font-family:Arial;">finance@crossroads.net</span></a> or submit a new check.</p>
<p>Thanks again for being part of the team!<br /></p>
<p> - Crossroads </p></div>'
            ,1
            ,'08/14/2015'
            ,7
            ,7
            ,1
            ,1)
SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO
