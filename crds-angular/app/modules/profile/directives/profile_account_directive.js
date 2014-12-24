(function () {
    angular.module("crdsProfile").directive("crdsProfileAccount", ['$log','Profile', CrdsAccount]);
    function CrdsAccount($log, Profile){
        return {
            restrict: 'E',
            replace: true,
            require: "^crdsProfilePage",
            templateUrl: 'app/modules/profile/templates/profile_account.html',
            controllerAs: true,
            link: (function (scope, el, attr, ctrl) {
                
            })
        }
    }
})()