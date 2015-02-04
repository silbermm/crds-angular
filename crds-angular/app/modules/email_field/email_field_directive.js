(function () {
    angular.module("email_field",[]).directive("emailField", ['$log', 'Profile', EmailField]);

    function EmailField($log, Profile) {
        return {
            restrict: 'EA',
            replace: true,
            scope: {
                email: "=email",
                submitted: "=submitted"
            },
            templateUrl: 'app/modules/email_field/email_field.html',
            link: (function (scope, el, attr, ctrl) {

            })
        }
    }
})()