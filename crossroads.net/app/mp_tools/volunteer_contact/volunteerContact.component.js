(function() {
  'use strict()';

  module.exports = VolunteerContact;

  VolunteerContact.$inject = ['$rootScope', '$window', '$log', 'MPTools', 'Group'];

  function VolunteerContact($rootScope, $window, $log, MPTools, Group) {

    return {
      restrict: 'E',
      scope: {},
      controller: VolunteerContactController,
      controllerAs: 'contact',
      bindToController: true,
      templateUrl: 'volunteer_contact/contact.html'
    };

    function VolunteerContactController() {

      var vm = this;
      vm.errorMessage = $rootScope.MESSAGES.toolsError;
      vm.group = {};
      vm.multipleRecordsSelected = true;
      vm.params = MPTools.getParams();
      vm.showError = showError;
      vm.viewReady = false;

      activate();

      //////////////////////

      function activate() {
        vm.multipleRecordsSelected = showError();
        Group.Detail.get({groupId: vm.params.recordId}, function(data) {
          vm.group = data;
          Group.Events.query({groupId: vm.group.groupId}, function(events) {
            vm.group.events = events;
            vm.viewReady = true;
          });
        });
      }

      function showError() {
        if (vm.params.selectedCount > 1 ||
          vm.params.recordDescription === undefined ||
          vm.params.recordId === '-1') {
          vm.viewReady = true;
          return true;
        }
        return false;
      }
    }
  }
})();
