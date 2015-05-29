(function () {
  'use strict';
  module.exports = GiveCtrl;

  GiveCtrl.$inject = ['$rootScope', '$scope', '$state', '$timeout', 'Session', 'PaymentService','programList'];

  function DonationException(message) {
    this.message = message;
    this.name = "DonationException";
  };

  function GiveCtrl($rootScope, $scope, $state, $timeout, Session, PaymentService, programList) {

        $scope.$on('$stateChangeStart', function (event, toState, toParams) {
           if ($rootScope.email) {
               vm.email = $rootScope.email;
               //what if email is not found for some reason??
             }
             vm.transitionForLoggedInUserBasedOnExistingDonor(event,toState);
        });
        
        var vm = this;
        vm.amountSubmitted = false;
        vm.bankinfoSubmitted = false;
       
        vm.donor = {};
        vm.donorError = false;
        vm.email = null;
        vm.emailAlreadyRegisteredGrowlDivRef = 1000;
        vm.emailPrefix = "give";
        vm.last4 = '';
        vm.showMessage = "Where?";
        vm.showCheckClass = "ng-hide";
        vm.view = 'bank';
        vm.programsInput = programList;

        //Credit Card RegExs
        var americanExpressRegEx = /^3[47][0-9]{13}$/;
        var discoverRegEx = /^6(?:011|5[0-9]{2})/;
        var mastercardRegEx = /^5[1-5][0-9]/;
        var visaRegEx = /^4[0-9]{12}(?:[0-9]{3})?$/;

        var brandCode = [];
        brandCode['Visa'] = "#cc_visa";
        brandCode['MasterCard'] = '#cc_mastercard';
        brandCode['American Express'] = '#cc_american_express';
        brandCode['Discover'] = '#cc_discover';

        vm.transitionForLoggedInUserBasedOnExistingDonor = function(event, toState){
          if(toState.name == "give.account" && $rootScope.username && !vm.donorError ) {
            event.preventDefault();
            PaymentService.donor().get({email: $scope.give.email})
            .$promise
            .then(function(donor){
              vm.donor = donor;
              vm.last4 = donor.last4;
              vm.brand = brandCode[donor.brand];
              vm.expYear =  donor.exp_year;
              vm.exp_month = donor.exp_month;
              $state.go("give.confirm");
            },function(error){
            //  create donor record
              vm.donorError = true;
              $state.go("give.account");
            });
          }
        }
             
        vm.goToAccount = function() {
            vm.amountSubmitted = true;
            if($scope.giveForm.amountForm.$valid) {
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

        vm.confirmDonation = function(){
          try
          {
            vm.donate(vm.program.ProgramId, vm.amount, vm.donor.id, vm.email);
            $state.go("give.thank-you");
          }
          catch(DonationException)
          {
            $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
          }

        };

        vm.goToChange = function() {
          $state.go("give.change")
        };

        vm.goToLogin = function () {
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
             if ($scope.giveForm.$valid) {
              PaymentService.donor().get({email: $scope.give.email})
             .$promise
              .then(function(donor){
                vm.donate(vm.program.ProgramId, vm.amount, donor.id, vm.email);
                $state.go("give.thank-you");
                },
                function(error){
                  // The vm.email below is only required for guest giver, however, there
                  // is no harm in sending it for an authenticated user as well,
                  // so we'll keep it simple and send it in all cases.
                  PaymentService.createDonorWithCard({
                    name: vm.nameOnCard,
                    number: vm.ccNumber,
                    exp_month: vm.expDate.substr(0,2),
                    exp_year: vm.expDate.substr(2,2),
                    cvc: vm.cvc
                  }, vm.email)
                  .then(function(donor) {
                    vm.donate(vm.program.ProgramId, vm.amount, donor.id, vm.email);
                    $state.go("give.thank-you");
                  },
                  function() {
                    $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
                  });

                });
            }
            else {
              $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
            }
        };

        vm.submitChangedBankInfo = function() {
            vm.bankinfoSubmitted = true;
             if ($scope.giveForm.$valid) {
             
                  PaymentService.updateDonorWithCard(
                    donor.id,
                    {
                      name: vm.nameOnCard,
                      number: vm.ccNumber,
                      exp_month: vm.expDate.substr(0,2),
                      exp_year: vm.expDate.substr(2,2),
                      cvc: vm.cvc
                    })
                  .then(function(donor) {
                    vm.donate(vm.program.ProgramId, vm.amount, donor.id, vm.email);
                    $state.go("give.thank-you");
                  },
                  function() {
                    $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
                  });

                });
            }
            else {
              $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
            }
        };

        vm.donate = function(programId, amount, donorId, email){
          PaymentService.donateToProgram(programId, amount, donorId, email)
            .then(function(confirmation){
              vm.program_name = _.result(_.find(vm.programsInput,
              {'ProgramId': confirmation.program_id}), 'Name');
              vm.amount = confirmation.amount;
            },
            function(reason){
              throw new DonationException("Failed: " + reason);
            });
        };
        
    }

})();
