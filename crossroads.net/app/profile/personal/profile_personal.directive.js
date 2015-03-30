'use strict';
(function() {
    module.exports = ProfilePersonalDirective;

    ProfilePersonalDirective.$inject = ['$log'];

    function ProfilePersonalDirective($log) {
        $log.debug("ProfilePersonalDirective");
        return {
                restrict: 'E',
                transclude: true,
                bindToController: true,
                scope: {
                    updatedPerson: '=person'
                },
                templateUrl: "personal/profile_personal.template.html",
                controller: "ProfilePersonalController as profile",
        };
    }
})()
