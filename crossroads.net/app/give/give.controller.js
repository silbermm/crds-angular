(function() {
  'use strict';
  module.exports = GiveCtrl;

  GiveCtrl.$inject = ['$rootScope',
                      '$state',
                      '$timeout',
                      'Session',
                      'DonationService',
                      'programList',
                      'GiveTransferService',
                      'GiveFlow',
                      'AUTH_EVENTS',
                      'OneTimeGiving',
                      ];

  function DonationException(message) {
    this.message = message;
    this.name = 'DonationException';
  }

  function GiveCtrl($rootScope,
    $state,
    $timeout,
    Session,
    DonationService,
    programList,
    GiveTransferService,
    GiveFlow,
    AUTH_EVENTS,
    OneTimeGiving) {

    var vm = this;
    vm.activeSession = activeSession;
    vm.donationService = DonationService;
    vm.dto = GiveTransferService;
    vm.emailAlreadyRegisteredGrowlDivRef = 1000;
    vm.emailPrefix = 'give';
    vm.giveFlow = GiveFlow;
    vm.initDefaultState = OneTimeGiving.initDefaultState;
    vm.onEmailFound = onEmailFound;
    vm.onEmailNotFound = onEmailNotFound;
    vm.programsInput = programList;

    $rootScope.$on('$stateChangeStart', function(event, toState, toParams) {

      // Short-circuit this handler if we're not transitioning TO a give state
      if (toState && !/^give.*/.test(toState.name)) {
        return;
      }

      GiveTransferService.processing = true;

      if (!vm.dto.initialized || toState.name === 'give') {
        event.preventDefault();
        OneTimeGiving.initDefaultState();
        return;
      }

      vm.donationService.transitionForLoggedInUserBasedOnExistingDonor(event, toState);
    });

    $rootScope.$on(AUTH_EVENTS.logoutSuccess, function(event) {
      vm.dto.reset();
      $state.go('home');
    });

    $rootScope.$on('$stateChangeSuccess', function(event, toState, toParams) {
      vm.dto.processing = false;
      if (toState.name === GiveFlow.thankYou) {
        vm.dto.initialized = false;
      }
    });

    $rootScope.$on('$stateChangeError', function(event, toState, toParams) {
      GiveTransferService.processing = false;
    });

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////

    function activeSession() {
      return (Session.isActive());
    }

    // Callback from email-field on guest giver page.  Emits a growl
    // notification indicating that the email entered may already be a
    // registered user.
    function onEmailFound() {
      $rootScope.$emit(
          'notify',
          $rootScope.MESSAGES.donorEmailAlreadyRegistered,
          vm.emailAlreadyRegisteredGrowlDivRef,
          -1 // Indicates that this message should not time out
          );

      // This is a hack to keep from tabbing on the close button on the growl message.
      // There is no option in Growl to make the close button not tabbable...
      $timeout(function() {
        var closeButton = document.querySelector('#existingEmail .close');
        if (closeButton) {
          closeButton.tabIndex = -1;
        }
      }, 11);
    }

    function onEmailNotFound() {
      // There isn't a way to close growl messages in code, outside of the growl
      // directive itself.  To work around this, we'll simply trigger the "click"
      // event on the close button, which has a close handler function.
      var closeButton = document.querySelector('#existingEmail .close');
      if (closeButton !== undefined) {
        $timeout(function() {
          angular.element(closeButton).triggerHandler('click');
        }, 0);
      }
    }

    function branchOnGivingType() {
    }
  }

})();
