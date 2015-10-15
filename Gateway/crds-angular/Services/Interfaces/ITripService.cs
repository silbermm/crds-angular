using System.Collections.Generic;
using crds_angular.Models.Crossroads.Trip;

namespace crds_angular.Services.Interfaces
{
    public interface ITripService
    {
        TripFormResponseDto GetFormResponses(int selectionId, int selectionCount, int recordId);
        List<TripGroupDto> GetGroupsByEventId(int eventId);
        MyTripsDto GetMyTrips(int contactId, string token);
        List<TripParticipantDto> Search(string search);
        TripCampaignDto GetTripCampaign(int pledgeCampaignId);
        List<FamilyMemberTripDto> GetFamilyMembers(int contactId, int pledgeId, string token);
        List<int> SaveParticipants(SaveTripParticipantsDto dto);
        int GeneratePrivateInvite(PrivateInviteDto dto, string token);
        bool ValidatePrivateInvite(int pledgeCampaignId, string guid, string token);
        int SaveApplication(TripApplicationDto dto);
    }
}