(function () {
  'use strict';
  module.exports = GiveCtrl;

  GiveCtrl.$inject = ['$rootScope', '$scope', '$state', '$timeout', 'Session', 'PaymentService','programList', 'GiveTransferService', 'User', 'AUTH_EVENTS'];

  function DonationException(message) {
    this.message = message;
    this.name = "DonationException";
  };

  function GiveCtrl($rootScope, $scope, $state, $timeout, Session, PaymentService, programList, GiveTransferService, User, AUTH_EVENTS) {

        $scope.$on('$stateChangeStart', function (event, toState, toParams) {
           // Short-circuit this handler if we're not transitioning TO a give state
           if(toState && !/^give.*/.test(toState.name)) {
             return;
           }

           // vm.processing is used to set state and text on the "Give" button
           // Make sure to set the processing state to true whenever a state change begins
           vm.processing = true;

           // If not initialized, initialize and go to default state
           if(!vm.initialized || toState.name == "give") {
             event.preventDefault();
             vm.initDefaultState();
             return;
           }

           vm.transitionForLoggedInUserBasedOnExistingDonor(event,toState);
        });

        $scope.$on(AUTH_EVENTS.logoutSuccess, function(event) {
          vm.reset();
          $state.go('home');
        });

        $scope.$on('$stateChangeSuccess', function (event, toState, toParams) {
          // vm.processing is used to set state and text on the "Give" button
          // Make sure to reset the processing state to false whenever state change succeeds.
          vm.processing = false;

          // Force the state to reset after successfully giving
          if(toState.name == 'give.thank-you') {
            vm.initialized = false;
            vm.dto.reset();
          }
        });

        $scope.$on('$stateChangeError', function (event, toState, toParams) {
          // vm.processing is used to set state and text on the "Give" button
          // Make sure to reset the processing state to false whenever state change fails.
          vm.processing = false;
        });

        var vm = this;
        vm.amountSubmitted = false;
        vm.bank= {};
        vm.bankinfoSubmitted = false;
        vm.card = {};
        vm.changeAccountInfo = false;
        vm.donor = {};
        vm.donorError = false;
        vm.dto = GiveTransferService;
        vm.email = null;
        vm.emailAlreadyRegisteredGrowlDivRef = 1000;
        vm.emailPrefix = "give";
        vm.initialized = false;
        vm.last4 = '';
        vm.processingChange = false;
        vm.processing = false;
        vm.programsInput = programList;
        vm.showMessage = "Where?";
        vm.showCheckClass = "ng-hide";
        if (!vm.dto.view ){
          vm.dto.view = "bank";
        };

        var brandCode = [];
        brandCode['Visa'] = "#cc_visa";
        brandCode['MasterCard'] = '#cc_mastercard';
        brandCode['American Express'] = '#cc_american_express';
        brandCode['Discover'] = '#cc_discover';

        vm.confirmDonation = function(){
          try
          {
            vm.processing = true;
            vm.donate(vm.program.ProgramId, vm.amount, vm.donor.id, vm.email, vm.dto.view, function() {
              $state.go("give.thank-you");
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

        };

        vm.createBank = function(){
          vm.bank = {
             country: 'US',
             currency: 'USD',
             routing_number: vm.dto.donor.default_source.routing,
             account_number: vm.dto.donor.default_source.last4
          }
        };

        vm.createCard = function(){
          vm.card = {
           name: vm.dto.donor.default_source.name,
           number: vm.dto.donor.default_source.last4,
           exp_month: vm.dto.donor.default_source.exp_date.substr(0,2),
           exp_year: vm.dto.donor.default_source.exp_date.substr(2,2),
           cvc: vm.dto.donor.default_source.cvc,
           address_zip: vm.dto.donor.default_source.address_zip
           }
        };

        vm.donate = function(programId, amount, donorId, email, pymtType, onSuccess, onFailure){
          PaymentService.donateToProgram(programId, amount, donorId, email, pymtType)
            .then(function(confirmation){
              vm.amount = confirmation.amount;
              vm.program = _.find(vm.programsInput, {'ProgramId': programId});
              vm.program_name = vm.program.Name;
              vm.email = confirmation.email;
              onSuccess(confirmation);
            }, function(error) {
              onFailure(error)
            });
        };

        vm.goToAccount = function() {
          vm.amountSubmitted = true;
          if($scope.giveForm.amountForm.$valid) {
              if(!vm.dto.view) {
                vm.dto.view = 'bank';
              }
              vm.processing = true;
              if ($rootScope.username === undefined && vm.processingChange === false) {
                  Session.addRedirectRoute("give.account", "");
                  $state.go("give.login");
              } else {
                  $state.go("give.account");
              }
          } else {
             $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          }
        };

        vm.goToChange = function(amount, donor, email, program) {
          vm.dto.amount = amount;
          vm.dto.donor = donor;
          vm.dto.email = email;
          vm.dto.program = program;
          if (vm.brand == "#library"){
            vm.dto.view = "bank"
          } else {
            vm.dto.view = "cc";
          }
          vm.dto.changeAccountInfo = true;
          vm.amountSubmitted = false;
          $state.go("give.change")
        };

        vm.goToLogin = function () {
          vm.processing = true;
          Session.addRedirectRoute("give.account", "");
          $state.go("give.login");
        };

        // Invoked from the initial "/give" state to get us to the first page
        vm.initDefaultState = function() {
          // If we have not initialized (meaning we came in via a deep-link, refresh, etc),
          // reset state and redirect to start page (/give/amount).
          vm.reset();
          vm.initialized = true;
          Session.removeRedirectRoute();
          $state.go("give.amount");
        };

        // Callback from email-field on guest giver page.  Emits a growl
        // notification indicating that the email entered may already be a
        // registered user.
        vm.onEmailFound = function() {
            $rootScope.$emit(
                'notify',
                $rootScope.MESSAGES.donorEmailAlreadyRegistered,
                vm.emailAlreadyRegisteredGrowlDivRef,
                -1 // Indicates that this message should not time out
                );
        };

        // Callback from email-field on guest giver page.  This closes any
        // growl notification left over from the onEmailFound callback.
        vm.onEmailNotFound = function() {
            // There isn't a way to close growl messages in code, outside of the growl
            // directive itself.  To work around this, we'll simply trigger the "click"
            // event on the close button, which has a close handler function.
            var closeButton = document.querySelector("#existingEmail .close");
            if(closeButton !== undefined) {
                $timeout(function() {
                    angular.element(closeButton).triggerHandler("click");
                }, 0);
            }
        };

        vm._stripeErrorHandler = function(error) {
          vm.processing = false;
          if(error && error.globalMessage) {
            vm.dto.declinedPayment =
              error.globalMessage == $rootScope.MESSAGES.paymentMethodDeclined;

            $rootScope.$emit('notify', error.globalMessage);
          } else {
            $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
          }
        };

        vm.processBankAccountChange = function(){
         if ($scope.giveForm.$valid) {
             vm.processing = true;
             vm.createBank();
             PaymentService.updateDonorWithBankAcct(vm.dto.donor.id,vm.bank,vm.dto.email)
             .then(function(donor) {
               vm.donate(vm.dto.program.ProgramId, vm.dto.amount, vm.dto.donor.id, vm.dto.email, vm.dto.view, function() {
                 $state.go("give.thank-you");
               }, vm._stripeErrorHandler);
             },
             vm._stripeErrorHandler);
           }
           else {
             $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
           }
        };

        vm.processChange = function(){
          if (vm.setValidCard == false){
            vm.dto.donor.default_source.last4 = "";
          };
          if (vm.setValidCvc == false){
            vm.dto.donor.default_source.cvc = "";
          };
          vm.processingChange = true;
          vm.amountSubmitted = false;
          $state.go("give.amount");
        };

        vm.processCreditCardChange = function (){
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
                    $state.go("give.thank-you");
                  },
                  vm._stripeErrorHandler
                );
              },
              vm._stripeErrorHandler);
          } else {
            vm.processing = false;
            $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          }
        };

        vm.reset = function() {
          vm.amount = undefined;
          vm.amountSubmitted = false;
          vm.bankinfoSubmitted = false;
          vm.changeAccountInfo = false;
          vm.email = undefined;
          vm.initialized = false;
          vm.processing = false;
          vm.program = undefined;
          vm.donorError = false;
          if ($rootScope.username === undefined) {
            User.email = "";
          };

          vm.dto.reset();
        }

        vm.submitBankInfo = function() {
            vm.bankinfoSubmitted = true;
            if ($scope.giveForm.accountForm.$valid) {
              vm.processing = true;
              PaymentService.donor().get({email: $scope.give.email})
              .$promise
              .then(function(donor){
                  vm.updateDonorAndDonate(donor.id, vm.program.ProgramId, vm.amount, vm.email, vm.dto.view);
              },
              function(error){
                vm.createDonorAndDonate(vm.program.ProgramId, vm.amount, vm.email, vm.dto.view);
              });
            } else {
              $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
            }
        };

        vm.createDonorAndDonate = function(programId, amount, email, view) {
          // The vm.email below is only required for guest giver, however, there
          // is no harm in sending it for an authenticated user as well,
          // so we'll keep it simple and send it in all cases.
          if (view == "cc") {
            vm.createCard();
            PaymentService.createDonorWithCard(vm.card, email)
          .then(function(donor) {
            vm.donate(programId, amount, donor.id, email, view, function() {
              $state.go("give.thank-you");
            }, vm._stripeErrorHandler);
          },
          vm._stripeErrorHandler);
         } else if (view == "bank") {
            vm.createBank();
            PaymentService.createDonorWithBankAcct(vm.bank, email)
          .then(function(donor) {
            vm.donate(programId, amount, donor.id, email, view, function() {
             $state.go("give.thank-you");
           }, vm._stripeErrorHandler);
          },
          vm._stripeErrorHandler);
         };
        }

        vm.updateDonorAndDonate = function(donorId, programId, amount, email, view) {
          // The vm.email below is only required for guest giver, however, there
          // is no harm in sending it for an authenticated user as well,
          // so we'll keep it simple and send it in all cases.
          if (view == "cc") {
            vm.createCard();
            PaymentService.updateDonorWithCard(donorId, vm.card, email)
          .then(function(donor) {
            vm.donate(programId, amount, donor.id, email, view, function() {
              $state.go("give.thank-you");
            }, vm._stripeErrorHandler);
          },
          vm._stripeErrorHandler);
         } else if (view == "bank") {
            vm.createBank();
            PaymentService.updateDonorWithBankAcct(donorId, vm.bank, email)
          .then(function(donor) {
            vm.donate(programId, amount, donor.id, email, view, function() {
             $state.go("give.thank-you");
           }, vm._stripeErrorHandler);
          },
          vm._stripeErrorHandler);
         };
        }

        vm.submitChangedBankInfo = function() {
            vm.bankinfoSubmitted = true;
            vm.amountSubmitted = true;
           if(vm.dto.amount === "") {
             $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
           } else {
            if (vm.dto.view == "cc") {
              if (!$scope.giveForm.creditCardForm.$dirty){
                vm.processing = true;
                vm.donate(vm.dto.program.ProgramId, vm.dto.amount, vm.dto.donor.id, vm.dto.email, vm.dto.view, function() {
                 $state.go("give.thank-you");
               }, vm._stripeErrorHandler);
              } else {
                vm.processCreditCardChange();
             }
           } else if (vm.dto.view == "bank"){
              if(!$scope.giveForm.bankAccountForm.$dirty) {
                 vm.processing = true;
                 vm.donate(vm.dto.program.ProgramId, vm.dto.amount, vm.dto.donor.id, vm.dto.email, vm.dto.view, function() {
                $state.go("give.thank-you");
              }, vm._stripeErrorHandler);
              } else {
                vm.processBankAccountChange();
             }
           };
        };
      };

        vm.transitionForLoggedInUserBasedOnExistingDonor = function(event, toState){
          if(toState.name == "give.account" && $rootScope.username && !vm.donorError ) {
            vm.processing = true;
            event.preventDefault();
            PaymentService.donor().get({email: $scope.give.email})
            .$promise
            .then(function(donor){
              vm.donor = donor;
              vm.email = vm.donor.email;
              if (vm.donor.default_source.credit_card.last4 != null){
                vm.last4 = donor.default_source.credit_card.last4;
                vm.brand = brandCode[donor.default_source.credit_card.brand];
                vm.expYear =  donor.exp_year;
                vm.exp_month = donor.exp_month;
              } else {
                vm.routing = donor.default_source.bank_account.routing;
                vm.account = donor.default_source.bank_account.last4
                vm.last4 = donor.default_source.bank_account.last4;
                vm.brand = '#library';
              };
              $state.go("give.confirm");
            },function(error){
            //  create donor record
              vm.donorError = true;
              $state.go("give.account");
            });
          }

        }
       };

})();
