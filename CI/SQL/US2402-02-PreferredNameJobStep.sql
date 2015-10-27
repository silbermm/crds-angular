USE msdb;
GO
EXEC sp_add_jobstep
    @job_name = N'Crossroads.Daily',
    @step_name = N'Populate Preferred Name',
    @subsystem = N'TSQL',
	@cmdexec_success_code=0, 
	@on_success_action=1, 
	@on_success_step_id=0, 
	@on_fail_action=3, 
	@on_fail_step_id=0, 
	@retry_attempts=0, 
	@retry_interval=0, 
	@os_run_priority=0, 
	@subsystem=N'TSQL', 
	@command=N'EXEC [dbo].[crds_di_PopulatePreferredName]', 
	@database_name=N'master', 
	@flags=0
GO