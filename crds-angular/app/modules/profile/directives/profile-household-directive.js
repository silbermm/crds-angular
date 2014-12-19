(function () {
    angular.module('crdsProfile').directive('crdsProfileHousehold', ['$log', crdsProfile]);
    function crdsProfile($log) {
        return {
            restrict: 'EA',
            templateUrl: 'app/modules/profile/templates/profile_household.html',
            scope: true
        };
    }
})()