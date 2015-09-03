(function () {
  'use strict';
  module.exports = GiveCtrl;

  GiveCtrl.$inject = ['$rootScope', 
                      '$state', 
                      '$timeout', 
                      'Session', 
                      'PaymentService',
                      'DonationService',
                      'programList', 
                      'GiveTransferService',
                      'GiveFlow',
                      'AUTH_EVENTS',
                      'OneTimeGiving',
                      'CC_BRAND_CODES'];

  function DonationException(message) {
    this.message = message;
    this.name = 'DonationException';
  }

  function GiveCtrl($rootScope, $state, $timeout, Session, PaymentService, DonationService, programList, GiveTransferService, GiveFlow, AUTH_EVENTS, OneTimeGiving, CC_BRAND_CODES) {

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
   
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
       // Short-circuit this handler if we're not transitioning TO a give state
      if(toState && !/^give.*/.test(toState.name)) {
        return;
      }

       // vm.processing is used to set state and text on the "Give" button
       // Make sure to set the processing state to true whenever a state change begins
       vm.dto.processing = true;

       // If not initialized, initialize and go to default state
       if(!vm.dto.initialized || toState.name === 'give') {
         event.preventDefault();
         OneTimeGiving.initDefaultState();
         return;
       }

       vm.transitionForLoggedInUserBasedOnExistingDonor(event,toState);
    });

    $rootScope.$on(AUTH_EVENTS.logoutSuccess, function(event) {
      vm.dto.reset();
      $state.go('home');
    });


    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams) {
      // vm.processing is used to set state and text on the "Give" button
      // Make sure to reset the processing state to false whenever state change succeeds.
      vm.dto.processing = false;
      // Force the state to reset after successfully giving
      if(toState.name === GiveFlow.thankYou) {
        vm.dto.initialized = false;
      }
    });

    /// USE THE LOADING BUTTON DIRECTIVE TO HANDLE THIS
    $rootScope.$on('$stateChangeError', function (event, toState, toParams) {
      // vm.processing is used to set state and text on the "Give" button
      // Make sure to reset the processing state to false whenever state change fails.
      vm.dto.processing = false;
    });

   
    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////

    function activeSession(){
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
        var closeButton = document.querySelector("#existingEmail .close");
        if(closeButton) {
          closeButton.tabIndex = -1;
        }
      }, 11);
    }

    function onEmailNotFound() {
      // There isn't a way to close growl messages in code, outside of the growl
      // directive itself.  To work around this, we'll simply trigger the "click"
      // event on the close button, which has a close handler function.
      var closeButton = document.querySelector("#existingEmail .close");
      if(closeButton !== undefined) {
          $timeout(function() {
              angular.element(closeButton).triggerHandler('click');
          }, 0);
      }
    }

    //vm.submitChangedBankInfo = function() {
          //if (!Session.isActive()) {
             //$state.go("give.login");
          //}
          //vm.dto.bankinfoSubmitted = true;
          //vm.dto.amountSubmitted = true;
          //if(vm.dto.amount === "") {
           //$rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          //} else {
            //if (vm.dto.view == "cc") {
              //if(vm.dto.savedPayment == 'bank') {
                //vm.giveForm.creditCardForm.$setDirty();
              //}
              //if (!vm.giveForm.creditCardForm.$dirty){
                //var pgram = _.find(vm.programsInput, { ProgramId: vm.dto.program.ProgramId });
                //vm.dto.processing = true;
                //DonationService.donate(pgram);
              //} else {
                //vm.donationService.processCreditCardChange(vm.giveForm, vm.programsInput);
              //}
            //} else if (vm.dto.view == "bank"){
              //if(vm.dto.savedPayment == 'cc') {
                //vm.giveForm.bankAccountForm.$setDirty();
              //}
              //if(!vm.giveForm.bankAccountForm.$dirty) {
                //var pgram = _.find(vm.programsInput, { ProgramId: vm.dto.program.ProgramId });
                //vm.dto.processing = true;
                //DonationService.donate(pgram);
              //} else {
                //vm.donationService.processBankAccountChange(vm.giveForm, vm.programsInput);
              //}
            //};
          //};
        //};
//
    vm.transitionForLoggedInUserBasedOnExistingDonor = function(event, toState){
        if(toState.name === GiveFlow.account && Session.isActive() && vm.dto.donorError ) {
          vm.dto.processing = true;
          event.preventDefault();
          PaymentService.getDonor(vm.dto.email)
          .then(function(donor){
            vm.dto.donor = donor;
            if (vm.dto.donor.default_source.credit_card.last4 != null){
              vm.dto.last4 = vm.dto.donor.default_source.credit_card.last4;
              vm.dto.brand = CC_BRAND_CODES[vm.dto.donor.default_source.credit_card.brand];
            } else {
              vm.dto.last4 = vm.dto.donor.default_source.bank_account.last4;
              vm.dto.brand = '#library';
            };
            $state.go(GiveFlow.confirm);
          },function(error){
            // Go forward to account info if it was a 404 "not found" error,
            // the donor service returns a 404 when a donor doesn't exist
            if(error && error.httpStatusCode == 404) {
              vm.dto.donorError = true;
              $state.go(GiveFlow.account);
            } else {
              PaymentService.stripeErrorHandler(error);
            }
          });
        }

      }
     };

})();
