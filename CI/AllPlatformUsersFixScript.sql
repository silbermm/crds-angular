--script from Kevin McCord, June 29, 2015

DECLARE @DomainID INT = 1

DECLARE @AllPlatformUsersRoleID INT 
SELECT @AllPlatformUsersRoleID = Role_ID FROM dp_Roles R WHERE R.Role_Name = 'All Platform Users' AND R.Domain_ID = @DomainID

INSERT INTO [dbo].[dp_Role_Pages]
           ([Role_ID]
           ,[Page_ID]
           ,[Access_Level]
           ,[Scope_All]
           ,[Approver]
           ,[File_Attacher]
           ,[Data_Importer]
           ,[Data_Exporter]
           ,[Secure_Records]
           ,[Allow_Comments]
           ,[Quick_Add])
SELECT @AllPlatformUsersRoleID, Page_ID, 3,0,0,1,1,1,0,1,0
FROM dp_Pages 
WHERE Display_Name IN ('My Tasks','My Messages','My Selections')
 AND NOT EXISTS (SELECT 1 FROM dp_Role_Pages RP WHERE RP.Page_ID = dp_Pages.Page_ID AND RP.Role_ID = @AllPlatformUsersRoleID)