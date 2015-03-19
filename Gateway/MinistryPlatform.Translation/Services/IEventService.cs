namespace MinistryPlatform.Translation.Services
{
    public interface IEventService
    {
        int registerParticipantForEvent(int participantId, int eventId);
    }
}
