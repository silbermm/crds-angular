(function() {
  'use strict';

  module.exports = TripGivingController;

  TripGivingController.$inject = [
    '$rootScope',
    '$state',
    '$timeout',
    'Session',
    'DonationService',
    'GiveTransferService',
    'GiveFlow',
    'AUTH_EVENTS',
    'TripGiving',
    'TripParticipant' ];

  function DonationException(message) {
    this.message = message;
    this.name = 'DonationException';
  }

  function TripGivingController(
      $rootScope,
      $state,
      $timeout,
      Session,
      DonationService,
      GiveTransferService,
      GiveFlow,
      AUTH_EVENTS,
      TripGiving, 
      TripParticipant ) {

    var vm = this;
    vm.activeSession = activeSession;
    vm.donationService = DonationService;
    vm.dto = GiveTransferService;
    vm.emailAlreadyRegisteredGrowlDivRef = 1000;
    vm.emailPrefix = 'give';
    vm.giveFlow = GiveFlow;
    vm.initDefaultState = TripGiving.initDefaultState;
    vm.program = null;
    vm.tripParticipant = TripParticipant;
    //vm.onEmailFound = onEmailFound;
    //vm.onEmailNotFound = onEmailNotFound;
  
    activate();
    ////////////////////////////////
    //// IMPLEMENTATION DETAILS //// 
    ////////////////////////////////

    function activate() {
      vm.dto.program = { ProgramId: 1, Name: "2015 December GO NOLA" }  
    }

    function activeSession() {
      return (Session.isActive());
    }


    
  }

})();
