(function() {
  'use strict';
  module.exports = PaymentService;

  PaymentService.$inject = ['$http','$q', 'stripe', '$cookies', '$rootScope', 'GiveTransferService', 'MESSAGES'];

  function PaymentService($http, $q, stripe, $cookies, $rootScope, GiveTransferService, MESSAGES) {
    var paymentService = {
      createDonorWithBankAcct: createDonorWithBankAcct,
      createDonorWithCard: createDonorWithCard,
      donateToProgram: donateToProgram,
      donation: {},
      getDonor: getDonor,
      updateDonorWithBankAcct:updateDonorWithBankAcct,
      updateDonorWithCard:updateDonorWithCard,
      addGlobalErrorMessage: _addGlobalErrorMessage,
      stripeErrorHandler: stripeErrorHandler
    };

    stripe.setPublishableKey(__STRIPE_PUBKEY__);

    function createDonorWithBankAcct(bankAcct, email) {
      return apiDonor(bankAcct, email, stripe.bankAccount, 'POST');
    }

    function createDonorWithCard(card, email) {
      return apiDonor(card, email, stripe.card, 'POST');
    }

    function donateToProgram(program_id, campaignId, amount, donor_id, email_address, pymt_type){
      var def = $q.defer();
      var donationRequest = {
        program_id: program_id,
        campaign_id: campaignId,
        amount: amount,
        donor_id: donor_id,
        email_address: email_address,
        pymt_type: pymt_type
      };
      $http({
        method: 'POST',
        url: __API_ENDPOINT__ + 'api/donation',
        data: donationRequest,
        headers: {
              Authorization: $cookies.get('sessionId')
            }
      }).success(function(data) {
        paymentService.donation = data;
        def.resolve(data);
      }).error(function(response, statusCode) {
        def.reject(_addGlobalErrorMessage(response.error, statusCode));
      });

      return def.promise;
    }

    function getDonor(email) {
      var encodedEmail = email ?
          encodeURI(email).replace(/\+/g, '%2B')
          :
          '';
      var def = $q.defer();
      $http({
        method: 'GET',
        url: __API_ENDPOINT__ + 'api/donor/?email=' + encodedEmail,
        headers: {
          Authorization: $cookies.get('sessionId')
        }
      }).success(function(data) {
        def.resolve(data);
      }).error(function(response, statusCode) {
        def.reject(_addGlobalErrorMessage(response.error, statusCode));
      });

      return def.promise;
    }

    function stripeErrorHandler(error){
      if(error && error.globalMessage) {
        GiveTransferService.declinedPayment =
              error.globalMessage.id == $rootScope.MESSAGES.paymentMethodDeclined.id;
        $rootScope.$emit('notify', error.globalMessage);
      } else {
        $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
      }
    }

    function updateDonorWithBankAcct(donorId, bankAcct, email) {
      return apiDonor(bankAcct, email, stripe.bankAccount, 'PUT');
    }

    function updateDonorWithCard(donorId, card, email) {
      return apiDonor(card, email, stripe.card, 'PUT');
    }

    function _addGlobalErrorMessage(error, httpStatusCode) {
      var e = error ? error : {};
      e.httpStatusCode = httpStatusCode;

      if (e.globalMessage && e.globalMessage > 0) {
        // Short-circuit the logic below, as the API should have
        // already determined the message to display
        return e;
      }

      // This same logic exists on the .Net side in crds-angular/Services/StripeService.cs
      // This is because of the Stripe "tokens" call, which goes directly to Stripe, not via our API.  We
      // are implementing the same here in the interest of keeping our application somewhat agnostic to
      // the underlying payment processor.
      if (e.type === 'abort' || e.code === 'abort') {
        e.globalMessage = MESSAGES.paymentMethodProcessingError;
      } else if (e.type === 'card_error') {
        return processCardError(e);
      } else if (e.param === 'bank_account') {
        if (e.type === 'invalid_request_error') {
          e.globalMessage = MESSAGES.paymentMethodDeclined;
        }
      }

      return e;
    }

    function processCardError(e) {
      if (e.code === 'card_declined' ||
          /^incorrect/.test(e.code) ||
          /^invalid/.test(e.code)) {
        e.globalMessage = MESSAGES.paymentMethodDeclined;
      } else if (e.code === 'processing_error') {
        e.globalMessage = MESSAGES.paymentMethodProcessingError;
      }

      return e;
    }

    function apiDonor(donorInfo, email, stripeFunc, apiMethod) {
      var def = $q.defer();
      stripeFunc.createToken(donorInfo, function(status, response) {
        if (response.error) {
          def.reject(_addGlobalErrorMessage(response.error, status));
        } else {
          var donorRequest = { stripe_token_id: response.id, email_address: email };
          $http({
            method: apiMethod,
            url: __API_ENDPOINT__ + 'api/donor',
            headers: {
              Authorization:  $cookies.get('sessionId')
            },
            data: donorRequest
          }).success(function(data) {
            paymentService.donor = data;
            def.resolve(data);
          }).error(function(response, statusCode) {
            def.reject(_addGlobalErrorMessage(response.error, statusCode));
          });
        }
      });

      return def.promise;
    }

    return paymentService;
  }

})();
