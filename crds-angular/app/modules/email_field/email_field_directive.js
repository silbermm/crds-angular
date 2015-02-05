(function () {
    angular.module("email_field",[]).directive("emailField", ['$parse','$compile','$log', 'Profile', EmailField]);

    function EmailField($parse,$compile,$log, Profile) {
        return {
            restrict: 'EA',
            replace: true,
            scope: {
                email: "=",
                submitted: "=submitted"
            },
            templateUrl: 'app/modules/email_field/email_field.html',
            link: (function (scope, el, attr, ctrl) {
                //scope.$watch(attr['ng-bind-html'], function () {
                //    el.html($parse(attr['ng-bind-html'])(scope));
                //    $compile(el.contents())(scope);
                //}, true);
            })
        }
    }
})()