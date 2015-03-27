'use strict';
(function() {
    module.exports = ProfilePersonalDirective;

    ProfilePersonalDirective.$inject = ['$http', '$log', '$controller', 'Session', 'User'];

    function ProfilePersonalDirective($http, $log, $controller, Session, User) {
        $log.debug("ProfilePersonalDirective");

        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'personal/profile_personal.template.html',
            controller: "ProfilePersonalController as profile",
            link: link,
            data: {
                isProtected: true
            },
        };
    }

    function link(scope, el, attr, controller) {
        debugger;
        console.log('Linking: ' + controller);
    }
})()
