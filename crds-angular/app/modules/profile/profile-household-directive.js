(function () {
    angular.module('crdsProfile').directive('crdsProfileHousehold', ['$log', crdsProfile]);
    function crdsProfile($log) {
        return {
            restrict: 'EA',
            contoller: 'crdsProfileCtrl as profile',
            templateUrl: 'app/modules/profile/profile_household.html',
            scope: true,
            link: (function (scope, el, attr) {

            })
        };
    }
})()