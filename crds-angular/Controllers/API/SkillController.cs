//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
using crds_angular.Security;
using crds_angular.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;

namespace crds_angular.Controllers.API
{
    public class SkillController : LookupController
    {
        [ResponseType(typeof(List<Models.Crossroads.SkillCategory>))]
        [Route("api/skill")]
        public IHttpActionResult Get()
        {
            var pageId = 277;
            return Authorized(t =>
            {
                var mpObject = TranslationService.GetLookup(pageId, t);
                var attributes = JsonConvert.DeserializeObject<List<MinistryPlatform.Models.Attribute>>(mpObject);
                var skills = ConvertToSkills(attributes);

                return this.Ok(skills);
            });
        }

        //public IHttpActionResult Get(int id)
        //{
        //    return Authorized(token =>
        //    {

        //    });
        //}

        private List<Models.Crossroads.SkillCategory> ConvertToSkills(List<MinistryPlatform.Models.Attribute> attributes)
        {
            //init our return variable
            var skillCategories = new List<Models.Crossroads.SkillCategory>();

            //filter out attributes that are not skills
            //order the remaining
            //group by category
            var categories = attributes                
                .Where(a => a.Attribute_Category != null && a.Attribute_Type.StartsWith("Skill"))
                .OrderBy(a => a.Attribute_Category).ThenBy(a => a.Attribute_Name)
                .GroupBy(g => g.Attribute_Category);

            //iterate over the groups, assign skills to each category
            foreach (var category in categories)
            {
                var skillCategory = new Models.Crossroads.SkillCategory {Name = category.Key};
                var skills = new List<Models.Crossroads.Skill>();
                foreach (var skill in category)
                {
                    var s = new Models.Crossroads.Skill
                    {
                        Id = skill.dp_RecordID,
                        Name = skill.Attribute_Name
                    };
                    skills.Add(s);
                }
                skillCategory.Skills = skills;
                skillCategories.Add(skillCategory);
            }
            return skillCategories;
        }
    }
}
