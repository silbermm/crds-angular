using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class FormResponse
    {
        public int FormId { get; set; }
        public int ContactId { get; set; }
        public int? OpportunityId { get; set; }
        public int? OpportunityResponseId { get; set; }
        public int? PledgeCampaignId { get; set; }
        public List<FormAnswer> FormAnswers { get; set; }

        public FormResponse()
        {
            FormAnswers = new List<FormAnswer>();
        }
    }

    public class FormAnswer
    {
        public int FieldId { get; set; }
        public int FormResponseId { get; set; }
        public string Response { get; set; }
        public int? OpportunityResponseId { get; set; }
    }
}