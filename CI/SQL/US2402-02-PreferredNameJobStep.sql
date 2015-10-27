USE msdb;
GO
EXEC sp_add_jobstep
    @job_name = N'Crossroads.Daily',
    @step_name = N'Populate Preferred Name',
    @subsystem = N'TSQL',
    @command = N'EXEC [dbo].[crds_di_PopulatePreferredName]', 
    @retry_attempts = 0,
    @retry_interval = 0 ;
GO