USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Letters] ON
GO

INSERT INTO [dbo].[Letters]
           ([Letter_ID]
		   ,[Letter_Title]
           ,[Page_ID]
           ,[Letter_Opening]
           ,[Letter_Body]
           ,[Letter_From]
           ,[Active]
           ,[Domain_ID])
     VALUES
           (6
		   ,'New Donor Letter'
           ,299
           ,'<p class="MsoNormal" style="margin-top:5.0pt;margin-right:0in;margin-bottom:5.0pt;
margin-left:0in;line-height:normal;mso-pagination:none;mso-layout-grid-align:
none;text-autospace:none"><span style="font-size:12.0pt;font-family:"Times New Roman",serif"> Thank-you
for supporting the ministry God is doing through our church. Because you have
given to the Lord through our church we would like to send you this book.<o:p></o:p></span></p>'
,'<p class="MsoNormal" style="margin-top:5.0pt;margin-right:0in;margin-bottom:5.0pt;
margin-left:0in;line-height:normal;mso-pagination:none;mso-layout-grid-align:
none;text-autospace:none"><span style="font-size:12.0pt;font-family:"Times New Roman",serif">You will receive
credit for a gift as long as you give by check or write your name and address
on one of the blank envelopes in the seat pockets. Also, giving online is super fast and easy!! Either way, we will send you
a statement of your giving each quarter to review and another one at the end of
the year for your tax records.  You can also call the office with any questions you may have.</span></p><p class="MsoNormal" style="margin-top:5.0pt;margin-right:0in;margin-bottom:5.0pt;
margin-left:0in;line-height:normal;mso-pagination:none;mso-layout-grid-align:
none;text-autospace:none"><span style="font-size:12.0pt;font-family:"Times New Roman",serif"> Finally, if you are “high tech” you may choose to setup a recurring
gift online at our website. If you do this, let us know and we will remove you
from the envelope mailing list. Thank you for your commitment as a disciple of Christ.
Your stewardship is helping us reach our community for God’s Kingdom.<o:p></o:p></span></p>'

           ,'<p class="MsoNormal" style="margin-top:5.0pt;margin-right:0in;margin-bottom:5.0pt;
margin-left:0in;line-height:normal;mso-pagination:none;mso-layout-grid-align:
none;text-autospace:none"><span style="font-size:12.0pt;font-family:"Times New Roman",serif"> Crossroads <o:p></o:p></span></p>'
           ,1
           ,1)
GO