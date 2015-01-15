(function(){
    angular.module('crossroads').directive('registerDialog', ["$log", "AUTH_EVENTS", RegisterDialog]);

    function RegisterDialog($log, AUTH_EVENTS){
        return {
            restrict: 'EA',
            templateUrl: "app/crossroads.net/register/register_dialog.html",
            controller: "RegisterCtrl",
            link: function (scope) {
                $log.debug("in the registerdialog directive");
                var showDialog = function () {
                    $log.debug('not logged in');
                    scope.visible = true;
                };

                scope.visible = false;
               
            }
        };
    }
})()