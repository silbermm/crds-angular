(function(){
    angular.module('crdsProfile').directive('crdsProfile', ['$log', crdsProfile]);
    function crdsProfile($log) {
        return {
            restrict: 'EA',
            contoller: 'crdsProfileCtrl as p',
            templateUrl: 'app/modules/profile/templates/profile.html',
            scope: true
        };
    }
})()