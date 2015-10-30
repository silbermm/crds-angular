(function() {
  'use strict()';
  module.exports = ProfileSkillsController;

  ProfileSkillsController.$inject = ['$rootScope', 'Skills', 'Session', '$log'];

  function ProfileSkillsController($rootScope, Skills, Session, $log) {

    var vm = this;
    vm.skills = [];
    vm.skillChange = skillChange;
    vm.skillTrashCan = skillTrashCan;

    activate();

    /////////////////
    function activate() {
      vm.skills = Skills.query({userId:Session.exists('userId')}, function() {
        vm.myskills = function() {
          var flat = [];
          vm.skills.forEach(function(item) {
            flat.push.apply(flat, item.Skills);
          });

          return flat;
        };
      });
    }

    function skillTrashCan(skill) {
      //toggle Selected
      skill.Selected = !skill.Selected;

      //call function to perform action, which is first?
      vm.skillChange(skill);
    }

    function skillChange(skill) {
      var newSkill = new Skills();
      newSkill.SkillId = skill.SkillId;
      newSkill.RecordId = skill.RecordId;

      if (skill.Selected) {
        newSkill.$save({userId:Session.exists('userId')}, function(data) {
          skill.RecordId = data.RecordId;
        });
      } else {
        var removed = newSkill.$remove({ userId: Session.exists('userId'), recordId: newSkill.RecordId });
      }
    }
  }

})();
