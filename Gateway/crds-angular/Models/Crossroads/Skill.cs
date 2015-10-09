using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using AutoMapper;

namespace crds_angular.Models.Crossroads
{
    public class SkillCategory
    {
        public string Name { get; set; }
        public List<Skill> Skills { get; set; }
    }
    public class Skill
    {
        public int SkillId { get; set; }
        public int RecordId { get; set; }
        public string Name { get; set; }
        [DefaultValue(false)]
        public bool Selected { get; set; }

        public MinistryPlatform.Models.SkillAttribute GetAttribute()
        {
            return new MinistryPlatform.Models.SkillAttribute
            {
                 Attribute_Name = this.Name,
                 Attribute_ID = this.SkillId
            };
        } 
    }
}