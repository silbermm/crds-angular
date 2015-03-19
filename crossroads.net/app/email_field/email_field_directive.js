
require('./email_field.html');
(function () {
    angular.module("email_field",[]).directive("emailField", ['$log', EmailField]);

    function EmailField($log) {
        return {
            restrict: 'EA',
            replace: true,
            scope: {
                email: "=",
                submitted: "=submitted",
                prefix: "=prefix"
            },
            templateUrl: 'email_field/email_field.html',
            link: function (scope, el, attr, ctrl) {
                if(document.location.hash === "#/register"){
                    scope.redirectTo = "#/login"
                }else{
                    scope.redirectTo = "#"
                }
                scope.checkEmail = function () {
                    //TODO Put this logic in a method that is globally accessible
                    return (scope.email_field.email.$error.required && scope.submitted && scope.email_field.email.$dirty ||
                        scope.email_field.email.$error.required && scope.submitted && !scope.email_field.email.$touched ||
                        scope.email_field.email.$error.required && scope.submitted && scope.email_field.email.$touched ||
                        scope.email_field.email.$error.unique && scope.email_field.email.$dirty ||
                        !scope.email_field.email.$error.required && scope.email_field.email.$dirty && !scope.email_field.email.$valid);
                };
            }
        }
    }
})()