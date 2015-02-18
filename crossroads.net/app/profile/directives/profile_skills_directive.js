(function () {
    angular.module('crdsProfile').directive('crdsProfileSkills', ['$log', crdsProfile]);
    function crdsProfile($log) {
        return {
            restrict: 'EA',
            require: "^crdsProfilePage",
            templateUrl: 'app/modules/profile/templates/profile_skills.html',
            scope: true
        };
    }
})()