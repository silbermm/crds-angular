USE [MinistryPlatform]
GO

IF EXISTS (SELECT * FROM [dbo].[dp_Communications] WHERE Communication_ID = 5386)
BEGIN
UPDATE [dbo].[dp_Communications]
   SET [Author_User_ID] = 1529662
      ,[Subject] = 'Thank you for signing up for the [Group_Name]!'
      ,[Body] = '<p class="MsoNormal"><font face="Arial, sans-serif"><span style="line-height: 17.1200008392334px;">Welcome [Nickname]!  You have successfully registered for the [Group_Name] which will be held at the [Congregation_Name] site.</span></font><br /></p><p class="MsoNormal"><font face="Arial, sans-serif"><span style="line-height: 17.12px;">[Childcare_Needed]</span><br /></font></p><p class="MsoNormal"><span style="font-size:12.0pt;line-height:107%;
font-family:"Arial","sans-serif""><o:p></o:p></span></p><p><span style="font-size:14.0pt; line-height:107%;">   </span>
</p>'
      ,[Domain_ID] = 1
      ,[Start_Date] = '2015-02-02'
      ,[Expire_Date] = null
      ,[Communication_Status_ID] = null
      ,[From_Contact] = 7
      ,[Reply_to_Contact] = 7
      ,[_Sent_From_Task] = null
      ,[Selection_ID] = null
      ,[Template] = 1
      ,[Active] = 1
      ,[To_Contact] = null
      ,[Time_Zone] = null
      ,[Locale] = null
 WHERE Communication_ID = 5386
END
ELSE
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Communications] ON
	INSERT INTO [dbo].[dp_Communications]
           ([Communication_ID]
		   ,[Author_User_ID]
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
           ,[To_Contact]
           ,[Time_Zone]
           ,[Locale])
     VALUES
           (5386
		   ,1529662
           ,'Thank you for signing up for the [Group_Name]!'
           ,'<p class="MsoNormal"><font face="Arial, sans-serif"><span style="line-height: 17.1200008392334px;">Welcome [Nickname]!  You have successfully registered for the [Group_Name] which will be held at the [Congregation_Name] site.</span></font><br /></p><p class="MsoNormal"><font face="Arial, sans-serif"><span style="line-height: 17.1200008392334px;"><br /></span></font></p><p class="MsoNormal"><font face="Arial, sans-serif"><span style="line-height: 17.12px;">[Childcare_Needed]</span><br /></font></p><p class="MsoNormal"><span style="font-size:12.0pt;line-height:107%;
font-family:"Arial","sans-serif""><o:p></o:p></span></p><p><span style="font-size:14.0pt; line-height:107%;">   </span>
</p>'
           ,1
           ,'2015-02-02'
           ,null
           ,null
           ,7
           ,7
           ,null
           ,null
           ,1
           ,1
           ,null
           ,null
           ,null)
	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END