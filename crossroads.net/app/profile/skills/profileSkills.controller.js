(function() {
  'use strict()';

  module.exports = ProfileSkillsController;

  ProfileSkillsController.$inject = ['Skills', 'Session', 'Person'];

  function ProfileSkillsController(Skills, Session, Person) {
    var vm = this;

    debugger;

    var attributeTypeIds = require('crds-constants').ATTRIBUTE_TYPE_IDS;

    vm.Person = Person;
    vm.removeSkill = RemoveSkill;
    vm.skillChange = SkillChange;

    var attributes = Person.attributeTypes[attributeTypeIds.SKILLS].attributes;
    vm.flatSkills = attributes;

    vm.categories = GroupSkillsByCategory();
    vm.otherSkills = _.groupBy(attributes, 'category');
    debugger;

    function GroupSkillsByCategory() {
      return _.chain(attributes)
        .groupBy('category')
        .pairs()
        .map(function (currentItem) {
          var zipped = _.zip(['name', 'skills'], currentItem);
          return _.object(zipped);
        })
        .value();
    }

    function RemoveSkill(skill) {
      skill.selected = false;

      //call function to perform action, which is first?
      vm.skillChange(skill);
    }

    function SkillChange(skill) {
      var newSkill = new Skills();
      newSkill.SkillId = skill.SkillId;
      newSkill.RecordId = skill.RecordId;

      if (skill.Selected) {
        newSkill.$save({userId: Session.exists('userId')}, function(data) {
          skill.RecordId = data.RecordId;
        });
      } else {
        var removed = newSkill.$remove({userId: Session.exists('userId'), recordId: newSkill.RecordId});
      }
    }
  }
})()
