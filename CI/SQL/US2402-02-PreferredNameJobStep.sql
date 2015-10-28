USE msdb;
GO

DECLARE @existing_step_id INT

SELECT @existing_step_id = sjs.step_id
FROM sysjobs sj
INNER JOIN sysjobsteps sjs ON sj.job_id = sjs.job_id
WHERE sj.name = 'Crossroads.Daily' AND sjs.step_name='Populate Preferred Name'

IF @existing_step_id IS NULL

BEGIN
	EXEC sp_add_jobstep
	@step_id = @existing_step_id,
    @job_name = N'Crossroads.Daily',
    @step_name = N'Populate Preferred Name',
    @subsystem = N'TSQL',
	@cmdexec_success_code=0, 
	@on_success_action=3, 
	@on_success_step_id=0, 
	@on_fail_action=3, 
	@on_fail_step_id=0, 
	@retry_attempts=0, 
	@retry_interval=0, 
	@os_run_priority=0,  
	@command=N'EXEC [dbo].[crds_di_PopulatePreferredName]', 
	@database_name=N'master', 
	@flags=0
END
GO

