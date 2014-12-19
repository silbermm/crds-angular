(function () {
    angular.module("crdsProfile").directive("crdsProfileAccount", ['$log',CrdsAccount]);
    function CrdsAccount($log){
        return {
            restrict: 'E',
            replace: true,
            contoller: 'crdsProfileCtrl as profile',
            templateUrl: 'app/modules/profile/templates/profile_account.html',
            link: (function (scope, el, attr) {
                
            })
        }
    }
})()