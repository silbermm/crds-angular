using System.Collections.Generic;
using System.Linq;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services;

namespace crds_angular.Services
{
    public interface ILookupService
    {
        List<Lookup> Lookup(string token, string type);
    }

    public class LookupService : ILookupService
    {
        private readonly MinistryPlatform.Translation.Services.ILookupServiceImpl _lookupService;

        public LookupService(ILookupServiceImpl lookupService)
        {
            _lookupService = lookupService;
        }

        public List<Lookup> Lookup(string token, string type)
        {
            // use enum for type
            List<Dictionary<string, object>> dict = null;

            if (type == "eventtype")
            {
                dict = EventTypes(token);
            }

            if (dict == null)
            {
                return null;
            }
            var list = dict.Select(d => new Lookup
            {
                Id = d.ToInt("dp_RecordID"),
                Value = d.ToString("dp_RecordName")
            }).ToList();

            return list.OrderBy(o => o.Value).ToList();
        }

        private List<Dictionary<string, object>> EventTypes(string token)
        {
            return _lookupService.EventTypes(token);
        }
    }

    public class Lookup
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}