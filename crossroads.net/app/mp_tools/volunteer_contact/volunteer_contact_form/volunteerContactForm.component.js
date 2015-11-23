(function() {
  'use strict';

  module.exports = VolunteerContactForm;

  VolunteerContactForm.$inject = ['$rootScope', '$window', 'Validation'];

  function VolunteerContactForm($rootScope, $window, Validation) {
    return {
      restrict: 'E',
      scope: {

      },
      bindToController: true,
      controller: VolunteerContactFormController,
      controllerAs: 'contactForm',
      templateUrl: 'volunteer_contact_form/volunteerContactForm.html'
    };

    function VolunteerContactFormController() {
      var vm = this;
      vm.cancel = cancel;
      vm.formData = {};
      vm.processing = false;
      vm.save = save;
      vm.validation = Validation;

      function cancel() {
        $window.close();
      }

      function save() {
        if (!vm.data.$valid) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          return;
        }
      }

    }
  }
})();
