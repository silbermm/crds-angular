(function(){  
  'use strict';

  module.exports = TripGivingController;

  TripGivingController.$inject = ['$rootScope', 
                      '$scope', 
                      '$state', 
                      '$timeout', 
                      'Session', 
                      'PaymentService',
                      'GiveTransferService', 
                      'User', 
                      'AUTH_EVENTS',
                      'CC_BRAND_CODES',
                      'TripParticipant'];

  function TripGivingController($rootScope, $scope, $state, $timeout, 
                                Session, PaymentService, GiveTransferService,
                                User, AUTH_EVENTS, CC_BRAND_CODES, TripParticipant){
    var vm = this;
    vm._stripeErrorHandler = _stripeErrorHandler;
    vm.activeSession = activeSession;
    vm.amount = undefined;
    vm.amountSubmitted = false;
    vm.bank= {};
    vm.bankinfoSubmitted = false;
    vm.card = {};
    vm.changeAccountInfo = false;
    vm.confirmDonation = confirmDonation;
    vm.createBank = createBank;
    vm.createCard = createCard;
    vm.createDonorAndDonate = createDonorAndDonate;
    vm.donate = donate;
    vm.donor = {};
    vm.donorError = false;
    vm.dto = GiveTransferService;
    vm.email = null;
    vm.emailAlreadyRegisteredGrowlDivRef = 1000;
    vm.emailPrefix = 'give';
    vm.goToAccount = goToAccount;
    vm.goToChange = goToChange;
    vm.goToLogin = goToLogin;
    vm.initDefaultState = initDefaultState;
    vm.initialized = false;
    vm.last4 = '';
    vm.onEmailFound = onEmailFound;
    vm.onEmailNotFound = onEmailNotFound;
    vm.processBankAccountChange = processBankAccountChange;
    vm.processChange = processChange;
    vm.processingChange = false;
    vm.processing = false;
    vm.program = { 
      'ProgramId': TripParticipant.trips[0].programId, 
      'ProgramName': TripParticipant.trips[0].programName 
    };
    vm.programInput = [{ 
      'ProgramId': TripParticipant.trips[0].programId, 
      'ProgramName': TripParticipant.trips[0].programName 
    }]; 
    vm.reset = reset;
    vm.showMessage = 'Where?';
    vm.showCheckClass = 'ng-hide';
    vm.showInitiative = false;
    vm.showFrequency = false;
    vm.submitBankInfo = submitBankInfo;
    vm.submitChangedBankInfo = submitChangedBankInfo;
    vm.togglePaymentInfo = togglePaymentInfo;
    vm.transitionForLoggedInUserBasedOnExistingDonor = transitionForLoggedInUserBasedOnExistingDonor;
    vm.tripParticipant = TripParticipant;
    vm.updateDonorAndDonate = updateDonorAndDonate;
    
    //////////////////////////////// 
    //// State Change Listeners ////
    ////////////////////////////////
    $scope.$on('$stateChangeStart', function (event, toState, toParams) {
      if(toState && !/^give.*/.test(toState.name)) {
        return;
      }
      vm.processing = true;
      if(!vm.initialized || toState.name === 'tripgiving') {
       event.preventDefault();
       vm.initDefaultState();
       return;
      }
      vm.transitionForLoggedInUserBasedOnExistingDonor(event,toState);
    });

    $scope.$on(AUTH_EVENTS.logoutSuccess, function(event) {
      vm.reset();
      $state.go('tripsearch');
    });

    $scope.$on('$stateChangeSuccess', function (event, toState, toParams) {
      vm.processing = false;
      if(toState.name === 'tripgiving.account' && Session.isActive()) {
        vm.togglePaymentInfo();
      }

      if(toState.name === 'tripgiving.thank-you') {
        vm.initialized = false;
        vm.dto.reset();
      }
    });

    $scope.$on('$stateChangeError', function (event, toState, toParams) {
      vm.processing = false;
    });

    

    activate();
    ////////////////////////////
    // IMPLEMENTATION DETAILS //
    ////////////////////////////
    
    function activate(){
      vm.tripParticipant.showGiveButton = false;
      vm.tripParticipant.showShareButtons = true;
      if (!vm.dto.view ){
        vm.dto.view = 'bank';
      }
      //Set up the programs in
      

    }

    function activeSession(){
      return (Session.isActive());
    }

    function confirmDonation(){
      if (!Session.isActive()) {
        $state.go('tripgiving.login');
      }
      try
      {
        vm.processing = true;
        vm.donate(vm.program.ProgramId, vm.amount, vm.donor.id, vm.email, vm.dto.view, function() {
          $state.go('tripgiving.thank-you');
        }, function(error) {
          vm._stripeErrorHandler(error);
          if(vm.dto.declinedPayment) {
            vm.goToChange(vm.amount, vm.donor, vm.email, vm.program, vm.dto.view);
          }
        });
      }
      catch(DonationException)
      {
        $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
      }
    }

    function createBank(){
      vm.bank = {
        country: 'US',
        currency: 'USD',
        routing_number: vm.dto.donor.default_source.routing,
        account_number: vm.dto.donor.default_source.bank_account_number
      };
    }

    function createCard(){
      vm.card = {
        name: vm.dto.donor.default_source.name,
        number: vm.dto.donor.default_source.cc_number,
        exp_month: vm.dto.donor.default_source.exp_date.substr(0,2),
        exp_year: vm.dto.donor.default_source.exp_date.substr(2,2),
        cvc: vm.dto.donor.default_source.cvc,
        address_zip: vm.dto.donor.default_source.address_zip
      };
    }

    function createDonorAndDonate(programId, amount, email, view) {
      // The vm.email below is only required for guest giver, however, there
      // is no harm in sending it for an authenticated user as well,
      // so we'll keep it simple and send it in all cases.
      if (view === 'cc') {
        vm.createCard();
        PaymentService.createDonorWithCard(vm.card, email) .then(function(donor) {
          vm.donate(programId, amount, donor.id, email, view, function() {
            $state.go('tripgiving.thank-you');
          }, vm._stripeErrorHandler);
        }, vm._stripeErrorHandler);
      } else if (view === 'bank') {
        vm.createBank();
        PaymentService.createDonorWithBankAcct(vm.bank, email) .then(function(donor) {
          vm.donate(programId, amount, donor.id, email, view, function() {
           $state.go('tripgiving.thank-you');
          }, vm._stripeErrorHandler);
        }, vm._stripeErrorHandler);
      }
    }
   
    function donate(programId, amount, donorId, email, pymtType, onSuccess, onFailure){
      PaymentService.donateToProgram(programId, amount, donorId, email, pymtType) .then(function(confirmation){
        vm.amount = confirmation.amount;
        vm.program = _.find(vm.programsInput, {'ProgramId': programId});
        vm.program_name = vm.program.Name;
        vm.email = confirmation.email;
        onSuccess(confirmation);
      }, function(error) {
        onFailure(error);
      });
    }

    function goToAccount() {
      vm.amountSubmitted = true;
      if($scope.giveForm.amountForm.$valid) {
          if(!vm.dto.view) {
            vm.dto.view = 'bank';
          }
          vm.processing = true;
          if (!Session.isActive() && vm.processingChange === false) {
              Session.addRedirectRoute('tripgiving.account', '');
              $state.go('tripgiving.login');
          } else {
              $state.go('tripgiving.account');
          }
      } else {
         $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      }
    }

    function goToChange(amount, donor, email, program) {
      if (!Session.isActive()) {
        $state.go('tripgivig.login');
      }
      vm.dto.amount = amount;
      vm.dto.donor = donor;
      vm.dto.email = email;
      vm.dto.program = program;
      if (vm.brand === '#library'){
        vm.dto.view = 'bank';
      } else {
        vm.dto.view = 'cc';
      }
      vm.dto.savedPayment = vm.dto.view;
      vm.dto.changeAccountInfo = true;
      vm.amountSubmitted = false;
      $state.go('tripgiving.change');
    }

    function goToLogin() {
      vm.processing = true;
      Session.addRedirectRoute('tripgiving.account', '');
      $state.go('tripgiving.login');
    }

    function initDefaultState() {
      vm.reset();
      vm.initialized = true;
      Session.removeRedirectRoute();
      $state.go('tripgiving.amount');
    }

    function onEmailFound() {
      $rootScope.$emit( 'notify',
        $rootScope.MESSAGES.donorEmailAlreadyRegistered,
        vm.emailAlreadyRegisteredGrowlDivRef,
        -1 // Indicates that this message should not time out
      );

      // This is a hack to keep from tabbing on the close button on the growl message.
      // There is no option in Growl to make the close button not tabbable...
      $timeout(function() {
        var closeButton = document.querySelector('#existingEmail .close');
        if(closeButton) {
          closeButton.tabIndex = -1;
        }
      }, 11);
    }

    // Callback from email-field on guest giver page.  This closes any
    // growl notification left over from the onEmailFound callback.
    function onEmailNotFound() {
      // There isn't a way to close growl messages in code, outside of the growl
      // directive itself.  To work around this, we'll simply trigger the "click"
      // event on the close button, which has a close handler function.
      var closeButton = document.querySelector('#existingEmail .close');
      if(closeButton !== undefined) {
          $timeout(function() {
              angular.element(closeButton).triggerHandler('click');
          }, 0);
      }
    }

    function _stripeErrorHandler(error) {
      vm.processing = false;
      if(error && error.globalMessage) {
        vm.dto.declinedPayment = error.globalMessage.id === $rootScope.MESSAGES.paymentMethodDeclined.id;
        $rootScope.$emit('notify', error.globalMessage);
      } else {
        $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
      }
    }

    function processBankAccountChange(){
     if ($scope.giveForm.$valid) {
         vm.processing = true;
         vm.createBank();
         PaymentService.updateDonorWithBankAcct(vm.dto.donor.id,vm.bank,vm.dto.email)
         .then(function(donor) {
           vm.donate(vm.dto.program.ProgramId, vm.dto.amount, vm.dto.donor.id, vm.dto.email, vm.dto.view, function() {
             $state.go('tripgiving.thank-you');
           }, vm._stripeErrorHandler);
         },
         vm._stripeErrorHandler);
       }
       else {
         $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
       }
    }

    function processChange(){
      if (!Session.isActive()) {
        $state.go('tripgiving.login');
      }
      vm.processingChange = true;
      vm.amountSubmitted = false;
      $state.go('tripgiving.amount');
    }

    function processCreditCardChange(){
      if ($scope.giveForm.$valid) {
        vm.processing = true;
        vm.dto.declinedCard = false;
        vm.createCard();
         PaymentService.updateDonorWithCard(vm.dto.donor.id, vm.card, vm.dto.email)
         .then(
          function(donor) {
            vm.donate(
              vm.dto.program.ProgramId,
              vm.dto.amount,
              vm.dto.donor.id,
              vm.dto.email,
              vm.dto.view,
              function() {
                $state.go('tripgiving.thank-you');
              },
              vm._stripeErrorHandler
            );
          },
          vm._stripeErrorHandler);
      } else {
        vm.processing = false;
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      }
    }

    function reset() {
      vm.amount = undefined;
      vm.amountSubmitted = false;
      vm.bankinfoSubmitted = false;
      vm.changeAccountInfo = false;
      vm.email = undefined;
      vm.initialized = false;
      vm.processing = false;
      vm.processingChange = false;
      vm.program = undefined;
      vm.donorError = false;
      if (!Session.isActive()) {
        User.email = '';
      }
      vm.dto.reset();
    }

    function submitBankInfo(){
      vm.bankinfoSubmitted = true;
      if ($scope.giveForm.accountForm.$valid) {
        vm.processing = true;
        PaymentService.getDonor($scope.give.email)
        .then(function(donor){
            vm.updateDonorAndDonate(donor.id, vm.program.ProgramId, vm.amount, vm.email, vm.dto.view);
        },
        function(error){
          vm.createDonorAndDonate(vm.program.ProgramId, vm.amount, vm.email, vm.dto.view);
        });
      } else {
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      }
    }
    
    function submitChangedBankInfo() {
      if (!Session.isActive()) {
         $state.go('tripgiving.login');
      }
      vm.bankinfoSubmitted = true;
      vm.amountSubmitted = true;
      if(vm.dto.amount === '') {
       $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      } else {
        if (vm.dto.view === 'cc') {
          if(vm.dto.savedPayment === 'bank') {
            $scope.giveForm.creditCardForm.$setDirty();
          }
          if (!$scope.giveForm.creditCardForm.$dirty){
            vm.processing = true;
            vm.donate(vm.dto.program.ProgramId, vm.dto.amount, vm.dto.donor.id, vm.dto.email, vm.dto.view, function() {
             $state.go('tripgiving.thank-you');
            }, vm._stripeErrorHandler);
           } else {
             vm.processCreditCardChange();
           }
         } else if (vm.dto.view === 'bank'){
            if(vm.dto.savedPayment === 'cc') {
              $scope.giveForm.bankAccountForm.$setDirty();
            }
            if(!$scope.giveForm.bankAccountForm.$dirty) {
               vm.processing = true;
               vm.donate(vm.dto.program.ProgramId, 
                         vm.dto.amount, vm.dto.donor.id, 
                         vm.dto.email, vm.dto.view, function() {
                $state.go('tripgiving.thank-you');
               }, vm._stripeErrorHandler);
            } else { 
              vm.processBankAccountChange(); 
            }
         }
      }
    }
    
    function transitionForLoggedInUserBasedOnExistingDonor(event, toState){
      if(toState.name === 'tripgiving.account' && Session.isActive() && !vm.donorError ) {
        vm.processing = true;
        event.preventDefault();
        PaymentService.getDonor($scope.give.email).then(function(donor){
          vm.donor = donor;
          vm.email = vm.donor.email;
          if (vm.donor.default_source.credit_card.last4 != null){
            vm.last4 = donor.default_source.credit_card.last4;
            vm.brand = CC_BRAND_CODES[donor.default_source.credit_card.brand];
            vm.expYear =  donor.exp_year;
            vm.exp_month = donor.exp_month;
          } else {
            vm.routing = donor.default_source.bank_account.routing;
            vm.account = donor.default_source.bank_account.last4;
            vm.last4 = donor.default_source.bank_account.last4;
            vm.brand = '#library';
          }
          $state.go('tripgiving.confirm');
        },function(error){
          // Go forward to account info if it was a 404 "not found" error,
          // the donor service returns a 404 when a donor doesn't exist
          if(error && error.httpStatusCode === 404) {
            vm.donorError = true;
            $state.go('tripgiving.account');
          } else {
            vm._stripeErrorHandler(error);
          }
        });
      }
    }


    function togglePaymentInfo() {
      $timeout(function() {
        var e = vm.dto.view === 'cc' ? 
          document.querySelector('[name=\'ccNumber\']') : 
          document.querySelector('[name=\'routing\']');
        e.focus();
      }, 0);
    }

    
    function updateDonorAndDonate(donorId, programId, amount, email, view) {
      // The vm.email below is only required for guest giver, however, there
      // is no harm in sending it for an authenticated user as well,
      // so we'll keep it simple and send it in all cases.
      if (view === 'cc') {
        vm.createCard();
        PaymentService.updateDonorWithCard(donorId, vm.card, email).then(function(donor) {
          vm.donate(programId, amount, donor.id, email, view, function() {
            $state.go('tripgiving.thank-you');
          }, vm._stripeErrorHandler);
        },
        vm._stripeErrorHandler);
     } else if (view === 'bank') {
        vm.createBank();
        PaymentService.updateDonorWithBankAcct(donorId, vm.bank, email).then(function(donor) {
          vm.donate(programId, amount, donor.id, email, view, function() {
           $state.go('tripgiving.thank-you');
          }, vm._stripeErrorHandler);
        }, vm._stripeErrorHandler);
     }
    }
    
    function DonationException(message) {
      this.message = message;
      this.name = 'DonationException';
    }
 }

})();
