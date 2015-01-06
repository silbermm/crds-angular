(function () {
    angular.module("crdsProfile").controller("ProfileSkillsController", ['$rootScope', 'Lookup', '$log', ProfileSkillsController]);

    function ProfileSkillsController($rootScope, Lookup, $log) {

        _this = this;

        _this.initSkills = function () {
            _this.skills = Lookup.Skills.query(function () {
                _this.myskills = function () {
                    var flat = [];
                    _this.skills.forEach(function (item) {
                        flat.push.apply(flat, item.Skills);
                    })
                    return flat;
                };
            });
        }

        _this.skillTrashCan = function (skill) {
            //toggle Selected
            skill.Selected = !skill.Selected;

            //call function to perform action, which is first?
            _this.skillChange(skill);
        }

        _this.skillChange = function (skill) {
            var newSkill = new Lookup.Skills();
            newSkill.SkillId = skill.SkillId;
            newSkill.RecordId = skill.RecordId;

            if (skill.Selected) {
                newSkill.$save(function (data) {
                    skill.RecordId = data.RecordId;
                });
            }
            else {
                var removed = newSkill.$remove({ recordId: newSkill.RecordId });
            }
        }
    }



})()