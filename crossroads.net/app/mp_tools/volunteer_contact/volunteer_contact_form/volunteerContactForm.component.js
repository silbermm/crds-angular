(function() {
  'use strict';

  module.exports = VolunteerContactForm;

  VolunteerContactForm.$inject = ['$rootScope', '$window', 'Validation', 'Group'];

  function VolunteerContactForm($rootScope, $window, Validation, Group) {
    return {
      restrict: 'E',
      scope: {
        group: '='
      },
      bindToController: true,
      controller: VolunteerContactFormController,
      controllerAs: 'contactForm',
      templateUrl: 'volunteer_contact_form/volunteerContactForm.html'
    };

    function VolunteerContactFormController() {
      var vm = this;
      vm.cancel = cancel;
      vm.events = [];
      vm.formData = {};
      vm.processing = false;
      vm.save = save;
      vm.showForm = false;
      vm.validation = Validation;

      activate();

      function activate() {
        Group.Events.query({groupId: vm.group.groupId}, function(data) {
          vm.events = data;
          vm.showForm = true;
        },

        function(err) {
          console.error('Unable to get events');
          console.error(err);
        });
      }

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
