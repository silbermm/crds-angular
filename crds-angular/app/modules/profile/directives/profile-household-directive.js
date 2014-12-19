(function () {
    angular.module('crdsProfile').directive('crdsProfileHousehold', ['$log', crdsProfile]);
    function crdsProfile($log) {
        return {
            restrict: 'EA',
            contoller: 'crdsProfileHouseholdCtrl',
            templateUrl: 'app/modules/profile/templates/profile_household.html',
            scope: true
        };
    }
})()