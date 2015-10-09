using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactAttributeService
    {
        List<ContractAttribute> GetCurrentContractAttributes(int contactId);
    }
}