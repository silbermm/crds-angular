namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IEventService
    {
        int registerParticipantForEvent(int participantId, int eventId);
    }
}
