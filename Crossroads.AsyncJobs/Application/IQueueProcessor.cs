namespace Crossroads.AsyncJobs.Application
{
    public interface IQueueProcessor
    {
        void Start();
        void Pause();
    }
}
