use MinistryPlatform;

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Child_Care_Available' AND Object_ID = Object_ID(N'Groups'))
BEGIN
    alter table [dbo].[Groups] add Child_Care_Available bit default 0 not null;
END

