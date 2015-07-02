(function () {

  angular.module('crossroads.give').factory('PaymentService',PaymentService);

  PaymentService.$inject = ['$log', '$http', '$resource','$q', 'stripe', 'MESSAGES'];

  function PaymentService($log, $http, $resource, $q, stripe, MESSAGES) {
    var payment_service = {
      createDonorWithBankAcct : createDonorWithBankAcct,
      createDonorWithCard : createDonorWithCard,
      donateToProgram : donateToProgram,
      donation : {},
      getDonor : getDonor,
      updateDonorWithBankAcct :updateDonorWithBankAcct,
      updateDonorWithCard :updateDonorWithCard,
      addGlobalErrorMessage: _addGlobalErrorMessage
    };

    stripe.setPublishableKey(__STRIPE_PUBKEY__);

    function createDonorWithBankAcct(bankAcct, email) {
      return(apiDonor(bankAcct, email, stripe.bankAccount, 'POST'));
    }

    function createDonorWithCard(card, email) {
      return(apiDonor(card, email, stripe.card, 'POST'));
    }

    function donateToProgram(program_id, amount, donor_id, email_address, pymt_type){
      var def = $q.defer();
      var donation_request = {
        "program_id" : program_id,
        "amount" : amount,
        "donor_id" : donor_id,
        "email_address": email_address,
        "pymt_type": pymt_type
      };
      $http({
        method: "POST",
        url: __API_ENDPOINT__ + 'api/donation',
        data: donation_request,
        headers: {
              'Authorization': crds_utilities.getCookie('sessionId')
            }
      }).success(function(data){
        payment_service.donation = data;
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
        method: "GET",
        url: __API_ENDPOINT__ + 'api/donor/?email=' + encodedEmail,
        headers: {
          'Authorization': crds_utilities.getCookie('sessionId')
        }
      }).success(function(data) {
        def.resolve(data);
      }).error(function(response, statusCode) {
        def.reject(_addGlobalErrorMessage(response.error, statusCode));
      });
      return(def.promise);
    }

    function updateDonorWithBankAcct(donorId, bankAcct, email){
      return(apiDonor(bankAcct, email, stripe.bankAccount, 'PUT'));
    }

    function updateDonorWithCard(donorId, card, email){
      return(apiDonor(card, email, stripe.card, 'PUT'));
    }

    function _addGlobalErrorMessage(error, httpStatusCode) {
      var e = error ? error : {};
      e.httpStatusCode = httpStatusCode;

      if(e.type == 'connection_error') {
        e.globalMessage = MESSAGES.paymentMethodProcessingError;
      } else if(e.type == 'card_error') {
        if(e.code == 'card_declined'
            || /^incorrect/.test(e.code)
            || /^invalid/.test(e.code)) {
          e.globalMessage = MESSAGES.paymentMethodDeclined;
        } else if(e.code == 'processing_error') {
          e.globalMessage = MESSAGES.paymentMethodProcessingError;
        }
      } else if(e.param == 'bank_account') {
        if(e.type == 'invalid_request_error') {
          e.globalMessage = MESSAGES.paymentMethodDeclined;
        }
      }
      return(e);
    }

    function apiDonor(donorInfo, email, stripeFunc, apiMethod) {
      var def = $q.defer();
      stripeFunc.createToken(donorInfo, function(status, response) {
        if(response.error) {
          def.reject(_addGlobalErrorMessage(response.error, status));
        } else {
          var donor_request = { stripe_token_id: response.id, email_address: email }
          $http({
            method: apiMethod,
            url: __API_ENDPOINT__ + 'api/donor',
            headers: {
              'Authorization': crds_utilities.getCookie('sessionId')
            },
            data: donor_request
          }).success(function(data) {
            payment_service.donor = data;
            def.resolve(data);
          }).error(function(response, statusCode) {
            def.reject(_addGlobalErrorMessage(response.error, statusCode));
          });
        }
      });
      return(def.promise);
    }

    return payment_service;
  }

})();
