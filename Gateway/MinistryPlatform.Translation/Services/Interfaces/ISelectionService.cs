using System.Collections.Generic;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ISelectionService
    {
        IList<int> GetSelectionRecordIds(string authToken, int selectionId);
    }
}
