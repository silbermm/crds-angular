(function() {
  'use strict';

  module.exports = GiveRun;

  GiveRun.$inject = ['Session', 'OneTimeGiving',  'GiveFlow', 'GiveTransferService', '$rootScope', 'AUTH_EVENTS'];

  function GiveRun(Session, OneTimeGiving, GiveFlow, GiveTransferService, $rootScope, AUTH_EVENTS) {

    $rootScope.$on('$stateChangeStart', function(event, toState, toParams) {
      // Short-circuit this handler if we're not transitioning TO a give state
      if (toState && !/^give.*/.test(toState.name)) {
        return;
      }

      // vm.processing is used to set state and text on the "Give" button
      // Make sure to set the processing state to true whenever a state change begins
      GiveTransferService.processing = true;

      // If not initialized, initialize and go to default state
      OneTimeGiving.initDefaultState();


      //vm.transitionForLoggedInUserBasedOnExistingDonor(event,toState);
    });

  }

})();
