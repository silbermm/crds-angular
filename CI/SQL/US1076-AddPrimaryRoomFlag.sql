USE [MinistryPlatform]
GO

INSERT INTO [dbo].[Event_Rooms]
           ([Event_ID]
           ,[Room_ID]
           ,[Room_Layout_ID]
           ,[Chairs]
           ,[Tables]
           ,[Notes]
           ,[Domain_ID]
           ,[_Approved]
           ,[__Start_Time_Offset]
           ,[__End_Time_Offset]
           ,[Cancelled]
           ,[__ExternalStartTime]
           ,[__ExternalFinishTime]
           ,[__ExternalSetupTime]
           ,[__ExternalTeardownTime]
           ,[__ExternalAudioVisual]
           ,[Primary_Room])
     VALUES
           (<Event_ID, int,>
           ,<Room_ID, int,>
           ,<Room_Layout_ID, int,>
           ,<Chairs, smallint,>
           ,<Tables, smallint,>
           ,<Notes, nvarchar(max),>
           ,<Domain_ID, int,>
           ,<_Approved, bit,>
           ,<__Start_Time_Offset, smallint,>
           ,<__End_Time_Offset, smallint,>
           ,<Cancelled, bit,>
           ,<__ExternalStartTime, time(7),>
           ,<__ExternalFinishTime, time(7),>
           ,<__ExternalSetupTime, int,>
           ,<__ExternalTeardownTime, int,>
           ,<__ExternalAudioVisual, nvarchar(1),>
           ,<Primary_Room, bit,>)
GO


