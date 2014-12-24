using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.Crossroads
{
    public class SkillCategory
    {
        public string Name { get; set; }
        public List<Skill> Skills { get; set; }
    }
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}