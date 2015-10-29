(function() {
  'use strict()';

  module.exports = ProfileSkillsController;

  ProfileSkillsController.$inject = ['Skills', 'Session', 'Person'];

  function ProfileSkillsController(Skills, Session, Person) {
    var vm = this;

    var attributeTypeIds = require('crds-constants').ATTRIBUTE_TYPE_IDS;
    var personSkills = Person.attributeTypes[attributeTypeIds.SKILLS].attributes;

    vm.flatSkills = personSkills;
    vm.groupedSkills = groupSkills(personSkills);
    vm.removeSkill = removeSkill;
    vm.skillChange = skillChange;

    function groupSkills(attributes) {
      var skillsByCategory = {};
      _.forEach(attributes, function(attribute) {
        if (attribute.category in skillsByCategory === false) {
          skillsByCategory[attribute.category] = {
            name: attribute.category,
            description: attribute.categoryDescription,
            skills: []
          };
        }

        skillsByCategory[attribute.category].skills.push(attribute);
      });

      return convertHashValuesToArray(skillsByCategory);
    }

    function convertHashValuesToArray(obj) {
      var result = [];
      _.forEach(obj, function(item) {
        result.push(item);
      });

      return result;
    }

    function removeSkill(skill) {
      skill.selected = false;

      //call function to perform action, which is first?
      vm.skillChange(skill);
    }

    function skillChange(skill) {
      var newSkill = new Skills();
      newSkill.SkillId = skill.SkillId;
      newSkill.RecordId = skill.RecordId;

      if (skill.selected) {
        newSkill.$save({userId: Session.exists('userId')}, function(data) {
          skill.RecordId = data.RecordId;
        });
      } else {
        var removed = newSkill.$remove({userId: Session.exists('userId'), recordId: newSkill.RecordId});
      }
    }
  }
})()
