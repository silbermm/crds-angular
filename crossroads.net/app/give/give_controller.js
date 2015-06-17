(function () {
  'use strict';
  module.exports = GiveCtrl;

  GiveCtrl.$inject = ['$rootScope', '$scope', '$state', '$timeout', 'Session', 'PaymentService','programList', 'GiveTransferService'];

  function DonationException(message) {
    this.message = message;
    this.name = "DonationException";
  };

  function GiveCtrl($rootScope, $scope, $state, $timeout, Session, PaymentService, programList, GiveTransferService) {

        $scope.$on('$stateChangeStart', function (event, toState, toParams) {
           // vm.processing is used to set state and text on the "Give" button
           // Make sure to set the processing state to true whenever a state change begins
           vm.processing = true;
           if ($rootScope.email) {
               vm.email = $rootScope.email;
               //what if email is not found for some reason??
             }
             vm.transitionForLoggedInUserBasedOnExistingDonor(event,toState);
        });

        $scope.$on('$stateChangeSuccess', function (event, toState, toParams) {
          // vm.processing is used to set state and text on the "Give" button
          // Make sure to reset the processing state to false whenever state change succeeds.
          vm.processing = false;
        });

        $scope.$on('$stateChangeError', function (event, toState, toParams) {
          // vm.processing is used to set state and text on the "Give" button
          // Make sure to reset the processing state to false whenever state change fails.
          vm.processing = false;
        });

        var vm = this;
        vm.amountSubmitted = false;
        vm.bankinfoSubmitted = false;
        vm.changeAccountInfo = false;
        vm.donor = {};
        vm.donorError = false;
        vm.dto = GiveTransferService;
        vm.email = null;
        vm.emailAlreadyRegisteredGrowlDivRef = 1000;
        vm.emailPrefix = "give";
        vm.last4 = '';
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
            });
          }
          catch(DonationException)
          {
            $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
          }

        };

        vm.donate = function(programId, amount, donorId, email, pymtType, onSuccess){
          PaymentService.donateToProgram(programId, amount, donorId, email, pymtType)
            .then(function(confirmation){
              vm.amount = confirmation.amount;
              vm.program = _.find(vm.programsInput, {'ProgramId': programId});
              vm.program_name = vm.program.Name;
              onSuccess();
            },
            function(reason){
              throw new DonationException("Failed: " + reason);
            });
        };

        vm.goToAccount = function() {
          vm.amountSubmitted = true;
          if($scope.giveForm.amountForm.$valid) {
              if(!vm.dto.view) {
                vm.dto.view = 'bank';
              }
              vm.processing = true;
              if ($rootScope.username === undefined) {
                  Session.addRedirectRoute("give.account", "");
                  $state.go("give.login");
              } else {
                  $state.go("give.account");
              }
          } else {
             $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          }
        };

        vm.goToChange = function(amount, donor, email, program, view) {
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
          $state.go("give.change")
        };


        vm.goToLogin = function () {
          vm.processing = true;
          Session.addRedirectRoute("give.account", "");
          $state.go("give.login");
        };

        // Invoked from the initial "/give" state to get us to the first page
        vm.initDefaultState = function() {
            $scope.$on('$viewContentLoaded', function() {
                if($state.is("give")) {
                    $state.go("give.amount");
                }
            });
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

        vm.submitBankInfo = function() {
            vm.bankinfoSubmitted = true;
            if ($scope.giveForm.accountForm.$valid) {
              vm.processing = true;
              PaymentService.donor().get({email: $scope.give.email})
             .$promise
              .then(function(donor){
                vm.donate(vm.program.ProgramId, vm.amount, donor.id, vm.email, vm.dto.view, function() {
                  $state.go("give.thank-you");
                });

                },
                function(error){
                  // The vm.email below is only required for guest giver, however, there
                  // is no harm in sending it for an authenticated user as well,
                  // so we'll keep it simple and send it in all cases.
                  if (vm.dto.view == "cc") {
                    PaymentService.createDonorWithCard({
                      name: vm.dto.donor.default_source.name,
                      number: vm.dto.donor.default_source.last4,
                      exp_month: vm.dto.donor.default_source.exp_date.substr(0,2),
                      exp_year: vm.dto.donor.default_source.exp_date.substr(2,2),
                      cvc: vm.cvc
                    }, vm.email)
                  .then(function(donor) {
                    vm.donate(vm.program.ProgramId, vm.amount, donor.id, vm.email, vm.dto.view, function() {
                      $state.go("give.thank-you");
                    });
                  },
                  function() {
                    vm.processing = false;
                    $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
                  });
                 };

                 if (vm.dto.view == "bank") {
                    PaymentService.createDonorWithBankAcct({
                       country: 'US',
                       currency: 'USD',
                       routing_number: vm.dto.routing,
                       account_number: vm.dto.account
                    }, vm.email)
                  .then(function(donor) {
                    vm.donate(vm.program.ProgramId, vm.amount, donor.id, vm.email, vm.dto.view, function() {
                     $state.go("give.thank-you");
                    });
                  },
                  function() {
                    vm.processing = false;
                    $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
                  });
                };


                });
            }
            else {
                  // The vm.email below is only required for gu
              $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
            }
        };

        vm.submitChangedBankInfo = function() {
            vm.bankinfoSubmitted = true;
            if($scope.giveForm.creditCardForm.$dirty) {
              // If dirty, it means we changed the bank info, so we'll
              // need to update it at the payment processor
              if ($scope.giveForm.$valid) {
               vm.processing = true;
               PaymentService.updateDonorWithCard(
                 vm.dto.donor.id,
                 {
                   name: vm.dto.donor.default_source.name,
                   number: vm.dto.donor.default_source.last4,
                   exp_month: vm.dto.donor.default_source.exp_date.substr(0,2),
                   exp_year: vm.dto.donor.default_source.exp_date.substr(2,2),
                   cvc: vm.cvc,
                   address_zip: vm.dto.donor.default_source.address_zip
                 })
               .then(function(donor) {
                 vm.donate(vm.dto.program.ProgramId, vm.dto.amount, vm.dto.donor.id, vm.dto.email, vm.dto.view, function() {
                   $state.go("give.thank-you");
                 });
               }),
               function() {
                 $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
               };
             }
             else {
               $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
             }
           } else {
             // If pristine, it means we did not change the bank info, so we'll
             // simply make the payment using the existing info
             vm.processing = true;
             vm.donate(vm.dto.program.ProgramId, vm.dto.amount, vm.dto.donor.id, vm.dto.email, vm.dto.view, function() {
               $state.go("give.thank-you");
             });
           }
        };

        vm.transitionForLoggedInUserBasedOnExistingDonor = function(event, toState){
          if(toState.name == "give.account" && $rootScope.username && !vm.donorError ) {
            vm.processing = true;
            event.preventDefault();
            PaymentService.donor().get({email: $scope.give.email})
            .$promise
            .then(function(donor){
              vm.donor = donor;
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

    }

})();
