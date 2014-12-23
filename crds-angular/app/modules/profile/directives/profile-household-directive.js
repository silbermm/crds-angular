(function () {
    angular.module('crdsProfile').directive('crdsProfileHousehold', ['$log', crdsProfile]);
    function crdsProfile($log) {
        return {
            restrict: 'EA',
            require: '^crdsProfilePage',
            templateUrl: 'app/modules/profile/templates/profile_household.html',
            scope: true,
            link: (function (scope, el, attr, ctrl) {
                $log.debug(scope);
            })
        };
    }
})()