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
            return Authorized(token =>
            {
                var mySkills = GetMySkills(token);

                var mpObject = TranslationService.GetLookup(pageId, token);
                var attributes = JsonConvert.DeserializeObject<List<MinistryPlatform.Models.Attribute>>(mpObject);
                var skills = ConvertToSkills(attributes, mySkills);

                return this.Ok(skills);
            });
        }

        [ResponseType(typeof(int))]
        [Route("api/skill")]
        public IHttpActionResult Post([FromBody] Models.Crossroads.Skill skill)
        {
            logger.Debug("Skill Post");

            return Authorized(token =>
            {
                //where to get the parent record id from?  i think this is the contact id
                var contactId = GetUserIdCookie();
                if (contactId == 0)
                {
                    return Unauthorized();
                }

                var returnVal = SkillService.Add(skill, contactId, token);
                return this.Ok(returnVal );
                //return this.Ok(x);
            });
        }

        [ResponseType(typeof(bool))]
        [Route("api/skill/{recordId?}")]
        [HttpDelete]
        public IHttpActionResult Delete(int recordId)
        {
            logger.Debug("Skill Delete");

            return Authorized(token => 
            {
                var contactId = GetUserIdCookie();
                if (contactId == 0) {
                    return Unauthorized();
                }

                if (SkillService.Delete(recordId, token))
                {
                    return this.Ok();
                }
                else
                {
                    //which one is appropriate?
                    return this.BadRequest();
                    //return this.NotFound();
                }
            });
        }

        private List<Models.Crossroads.Skill> GetMySkills(string token)
        {
            var contactId = GetUserIdCookie();
            if (contactId != 0 )
                {
                var personService = new PersonService();
                var skills = personService.getLoggedInUserSkills(contactId, token);
                    return skills;
                }
            return null;
        }

        private int GetUserIdCookie()
        {
             var cookie = Request.Headers.GetCookies("userId").FirstOrDefault();
             if (cookie != null && (cookie["userId"].Value != "null" || cookie["userId"].Value != null))
             {
                 var contactId = int.Parse(cookie["userId"].Value);
                 return contactId;
             }
             return 0;
        }

        private List<Models.Crossroads.SkillCategory> ConvertToSkills(List<MinistryPlatform.Models.Attribute> attributes, List<Models.Crossroads.Skill> mySkills)
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
                        SkillId = skill.dp_RecordID,
                        Name = skill.Attribute_Name
                    };
                    var selectedRecordId = mySkills.Where(m => m.Name == skill.Attribute_Name).Select(m => m.SkillId).FirstOrDefault();
                    if (selectedRecordId != 0)
                    {
                        s.Selected = true;
                        s.RecordId = selectedRecordId;
                    }
                    skills.Add(s);
                }

                skillCategory.Skills = skills;
                skillCategories.Add(skillCategory);
            }
            return skillCategories;
        }

       
    }
}
