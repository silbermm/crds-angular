use [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO

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
'
    ,'1 = 1'
	,NULL
	,NULL
	,NULL
);

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO
