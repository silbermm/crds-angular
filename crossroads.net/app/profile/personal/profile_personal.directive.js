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
                    updatedPerson: '=',
                    modalInstance: '=',
                },
                templateUrl: "personal/profile_personal.template.html",
                controller: "ProfilePersonalController as profile",
                link: link,
        };
    }

    function link(scope, el, attr, controller) {
        controller.closeModal = closeModal;

        function closeModal(success) {
            if(success) {
                controller.updatedPerson.emailAddress = controller.person.emailAddress;
                controller.updatedPerson.firstName = controller.person.firstName;
                controller.updatedPerson.nickName = controller.person.nickName;
            }

            controller.modalInstance.close();
        }
    }


})()
