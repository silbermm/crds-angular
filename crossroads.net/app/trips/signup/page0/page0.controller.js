(function() {
  'use strict';

  module.exports = Page0Controller;

  Page0Controller.$inject = ['TripsSignupService', 'Campaign'];

  function Page0Controller(TripsSignupService, Campaign) {

    var vm = this;
    vm.signupService = TripsSignupService;

    activate();

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////

    function activate() {
      vm.signupService.reset(Campaign);
      vm.signupService.activate();
    }

  }
})();
