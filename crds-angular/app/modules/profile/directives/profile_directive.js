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
            restrict: 'A',
            transclude: true,
            controller: 'crdsProfileCtrl as profile',
            template: '<div ng-transclude></div>',
            scope: {},
            link: (function (scope, el, attr) {

            })
        };
    }

})()