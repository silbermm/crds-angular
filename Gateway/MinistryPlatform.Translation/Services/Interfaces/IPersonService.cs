using System.Collections.Generic;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IPersonService
    {
        void UpdateProfile(int contactId, Dictionary<string, object> profileDictionary, Dictionary<string, object> householdDictionary, Dictionary<string, object> addressDictionary  );
        
    }
}