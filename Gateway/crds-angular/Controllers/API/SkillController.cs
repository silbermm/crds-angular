using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Newtonsoft.Json;
using Attribute = MinistryPlatform.Models.Attribute;

namespace crds_angular.Controllers.API
{
    public class SkillController : LookupController
    {
        private IPersonService _personService;
        private ISkillService _skillService;

        public SkillController(IPersonService personService, ISkillService skillService)
        {
            _personService = personService;
            _skillService = skillService;
        }

        [ResponseType(typeof(List<SkillCategory>))]
        [Route("api/skill/{userid}")]
        public IHttpActionResult Get(int userid)
        {
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["Attributes"]);
            return Authorized(token =>
            {
                var mySkills = GetMySkills(token, userid);

                var mpObject = TranslationService.GetSkills(pageId, token);
                var attributes = JsonConvert.DeserializeObject<List<Attribute>>(mpObject);
                var skills = ConvertToSkills(attributes, mySkills);

                return this.Ok(skills);
            });
        }

        [ResponseType(typeof(Skill))]
        [Route("api/skill/{userid}")]
        public IHttpActionResult Post(int userid, [FromBody] Skill skill)
        {
            logger.Debug("Skill Post");

            return Authorized(token =>
            {
                if (userid == 0)
                {
                    return Unauthorized();
                }

                var recordId = _skillService.Add(skill, userid, token);
                skill.RecordId = recordId;
                return this.Ok(skill );
            });
        }

        [ResponseType(typeof(bool))]
        [Route("api/skill/{userId}/{recordId?}")]
        [HttpDelete]
        public IHttpActionResult Delete(int userId, int recordId)
        {
            logger.Debug("Skill Delete");

            return Authorized(token => 
            {
                var contactId = userId;
                if (contactId == 0) {
                    return Unauthorized();
                }

                if (_skillService.Delete(recordId, token))
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

        private List<Skill> GetMySkills(string token, int contactId)
        {
            if (contactId != 0 )
                {
                //var personService = new PersonService();
                var skills = _personService.GetLoggedInUserSkills(contactId, token);
                    return skills;
                }
            return null;
        }

        //private int GetUserIdCookie()
        //{
        //     var cookie = Request.Headers.GetCookies("userId").FirstOrDefault();
        //     if (cookie != null && (cookie["userId"].Value != "null" || cookie["userId"].Value != null))
        //     {
        //         var contactId = int.Parse(cookie["userId"].Value);
        //         return contactId;
        //     }
        //     return 0;
        //}

        private List<SkillCategory> ConvertToSkills(List<Attribute> attributes, List<Skill> mySkills)
        {
            //init our return variable
            var skillCategories = new List<SkillCategory>();

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
                var skillCategory = new SkillCategory {Name = category.Key};
                var skills = new List<Skill>();
                foreach (var skill in category)
                {
                    
                    var s = new Skill
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
