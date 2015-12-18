use [MinistryPlatform]
GO

IF EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2181)

BEGIN

UPDATE [dbo].[dp_Page_Views]

SET
	[View_Title]='Users API Lookup'
	,[Page_ID]=401
	,[Description]='Users view for API UserService'
	,[Field_List]='dp_Users.[User_Name]
, Contact_ID_Table.[Contact_ID]
, dp_Users.[Can_Impersonate]
, dp_Users.[User_GUID]
, dp_Users.[User_Email]
, dp_Users.[PasswordResetToken]
'
	,[View_Clause]='1 = 1'
	,[Order_By]=NULL
	,[User_ID]=NULL
	,[User_Group_ID]=NULL

WHERE [Page_View_ID]=2181

END

ELSE

BEGIN

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dbo].[dp_Page_Views] (
     [Page_View_ID]
	,[View_Title]
	,[Page_ID]
	,[Description]
	,[Field_List]
	,[View_Clause]
	,[Order_By]
	,[User_ID]
	,[User_Group_ID]
) VALUES (
     2181
	,'Users API Lookup'
	,401
	,'Users view for API UserService'
	,'dp_Users.[User_Name]
, Contact_ID_Table.[Contact_ID]
, dp_Users.[Can_Impersonate]
, dp_Users.[User_GUID]
, dp_Users.[User_Email]
, dp_Users.[PasswordResetToken]
'
    ,'1 = 1'
	,NULL
	,NULL
	,NULL
);

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
GO
