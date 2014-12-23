(function(){
    angular.module('crdsProfile').directive('crdsProfile', ['$log', crdsProfile]);
    angular.module('crdsProfile').directive('crdsProfilePage', ['$log', crdsProfilePage]);

    function crdsProfile($log) {
        return {
            restrict: 'E',
            templateUrl: 'app/modules/profile/templates/profile.html'
        };
    }

    function crdsProfilePage($log) {
        return {
            restrict: 'EA',
            transclude: true,
            replace: true,
            controller: 'crdsProfileCtrl as profile',
            controllerAs: true,
            template: '<div ng-transclude></div>',
            scope: {},
            link: (function (scope, el, attr) {

            })
        };
    }

})()