(function() {
  'use strict';

  module.exports = DonationService;

  DonationService.$inject = ['$rootScope', 'Session', 'GiveTransferService', 'PaymentService', 'GiveFlow', '$state'];

  function DonationService($rootScope, Session, GiveTransferService, PaymentService, GiveFlow, $state) {
    var donationService = {
      bank: {},
      card: {},
      createBank: createBank,
      createCard: createCard,
      createDonorAndDonate: createDonorAndDonate,
      confirmDonation: confirmDonation,
      donate: donate,
      processBankAccountChange: processBankAccountChange,
      processChange: processChange,
      processCreditCardChange: processCreditCardChange,
    };

    function createBank() {
      donationService.bank = {
        country: 'US',
        currency: 'USD',
        routing_number: GiveTransferService.donor.default_source.routing,
        account_number: GiveTransferService.donor.default_source.bank_account_number
      };
    }

    function createCard() {
      donationService.card = {
        name: GiveTransferService.donor.default_source.name,
        number: GiveTransferService.donor.default_source.cc_number,
        exp_month: GiveTransferService.donor.default_source.exp_date.substr(0,2),
        exp_year: GiveTransferService.donor.default_source.exp_date.substr(2,2),
        cvc: GiveTransferService.donor.default_source.cvc,
        address_zip: GiveTransferService.donor.default_source.address_zip
      };
    }

    function createDonorAndDonate(programsInput) {
      var pgram;
      if (programsInput !== undefined) {
        pgram = _.find(programsInput, { ProgramId: GiveTransferService.program.programId });
      } else {
        pgram = GiveTransferService.program;
      }
      if (GiveTransferService.view === 'cc') {
        donationService.createCard();
        PaymentService.createDonorWithCard(donationService.card, GiveTransferService.email)
          .then(function(donor) {
            donationService.donate(pgram);
          }, PaymentService.stripeErrorHandler);
      } else if (view === 'bank') {
        vm.donationService.createBank();
        PaymentService.createDonorWithBankAcct(donationService.bank, GiveTransferService.email)
          .then(function(donor) {
            donationService.donate(pgram);
          }, PaymentService.stripeErrorHandler);
      }
    }

    function confirmDonation(programsInput) {
      if (!Session.isActive()) {
        $state.go(GiveFlow.login);
      }

      GiveTransferService.processing = true;
      try {
        var pgram;
        if (programsInput !== undefined) {
          pgram = _.find(programsInput, { ProgramId: GiveTransferService.program.ProgramId });
        } else {
          pgram = GiveTransferService.program;
        }

        donationService.donate(pgram, function(confirmation) {

        }, function(error) {

          if (GiveTransferService.declinedPayment) {
            GiveFlow.goToChange();
          }
        });
      } catch (DonationException) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
      }
    }

    function donate(program, onSuccess, onFailure) {
      PaymentService.donateToProgram(program.programId,
          GiveTransferService.amount,
          GiveTransferService.donor.donorId,
          GiveTransferService.email,
          GiveTransferService.pymtType).then(function(confirmation) {
            GiveTransferService.amount = confirmation.amount;
            GiveTransferService.program = program;
            GiveTransferService.program_name = GiveTransferService.program.Name;
            GiveTransferService.email = confirmation.email;
            if (onSuccess !== undefined) {
              onSuccess(confirmation);
            }

            $state.go(GiveFlow.thankYou);
          }, function(error) {

            GiveTransferService.processing = false;
            PaymentService.stripeErrorHandler(error);
            if (onSuccess !== undefined && onFailure !== undefined) {
              onFailure(error);
            }
          });
    }

    function processBankAccountChange(giveForm, programsInput) {
      if (giveForm.$valid) {
        GiveTransferService.processing = true;
        donationService.createBank();
        PaymentService.updateDonorWithBankAcct(GiveTransferService.donor.id,donationService.bank,GiveTransferService.email)
         .then(function(donor) {
           var pgram;
           if (programsInput !== undefined) {
             pgram = _.find(programsInput, { ProgramId: GiveTransferService.program.ProgramId });
           } else {
             pgram = GiveTransferService.program;  
           }
           donationService.donate(pgram);
        }, PaymentService.stripeErrorHandler);
      } else {
         $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
       }

    }

    function processChange() {
      if (!Session.isActive()) {
        $state.go(GiveFlow.login);
      }

      GiveTransferService.processingChange = true;
      GiveTransferService.amountSubmitted = false;
      $state.go(GiveFlow.amount);
    }

    function processCreditCardChange(giveForm, programsInput) {
      if (giveForm.$valid) {
        GiveTransferService.processing = true;
        GiveTransferService.declinedCard = false;
        donationService.createCard();
        var pgram;
        if (programsInput !== undefined) {
          pgram = _.find(programsInput, { ProgramId: GiveTransferService.program.ProgramId });
        } else {
          pgram = GiveTransferService.program;
        }
        PaymentService.updateDonorWithCard(GiveTransferService.donor.id, donationService.card, GiveTransferService.email)
          .then(function(donor) {
            donate(pgram, function() {
            
            }, function(error) {
              GiveTransferService.processing = false;
              PaymentService.stripeErrorHandler(error);
            });
          },function(error) {
            GiveTransferService.processing = false;
            PaymentService.stripeErrorHandler(error);
          });

      } else {
        GiveTransferService.processing = false;
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      }
    }

    return donationService;
  }
})();
