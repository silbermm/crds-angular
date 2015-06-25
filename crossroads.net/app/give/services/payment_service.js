(function () {

  angular.module('crossroads.give').factory('PaymentService',PaymentService);

  PaymentService.$inject = ['$log', '$http', '$resource','$q', 'stripe', 'MESSAGES'];

  function PaymentService($log, $http, $resource, $q, stripe, MESSAGES) {
    var payment_service = {
      createDonorWithBankAcct : createDonorWithBankAcct,
      createDonorWithCard : createDonorWithCard,
      donateToProgram : donateToProgram,
      donation : {},
      donor : getDonor,
      updateDonorWithBankAcct :updateDonorWithBankAcct,
      updateDonorWithCard :updateDonorWithCard
    };

    stripe.setPublishableKey(__STRIPE_PUBKEY__);

    function createDonorWithBankAcct(bankAcct, email) {
      return(_donor(bankAcct, email, stripe.bankAccount, 'POST'));
    }

    function createDonorWithCard(card, email) {
      return(_donor(card, email, stripe.card, 'POST'));
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
      }).error(function(response) {
        _addGlobalErrorMessage(response.error);
        def.reject(response.error);
      });

      return def.promise;
    }

    function getDonor(){
      return $resource(__API_ENDPOINT__ + 'api/donor/?email=:email',{email: '@_email'}, {
        get: {
          method : 'GET',
          headers: {'Authorization': crds_utilities.getCookie('sessionId')}
        }
      });
    }

    function updateDonorWithBankAcct(donorId, bankAcct, email){
      return(_donor(bankAcct, email, stripe.bankAccount, 'PUT'));
    }

    function updateDonorWithCard(donorId, card, email){
      return(_donor(card, email, stripe.card, 'PUT'));
    }

    function _addGlobalErrorMessage(error) {
      if(!error) {
        return;
      }

      if(error.type == 'card_error') {
        if(error.code == 'card_declined'
            || /^incorrect/.test(error.code)
            || /^invalid/.test(error.code)) {
          error.globalMessage = MESSAGES.paymentMethodDeclined;
        } else if(error.code == 'processing_error') {
          error.globalMessage = MESSAGES.paymentMethodProcessingError;
        }
      }
    }

    function _donor(donorInfo, email, stripeFunc, apiMethod) {
      var def = $q.defer();
      stripeFunc.createToken(donorInfo, function(status, response) {
        if(response.error) {
          _addGlobalErrorMessage(response.error);
          def.reject(response.error);
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
          }).error(function(response) {
            _addGlobalErrorMessage(response.error);
            def.reject(response.error);
          });
        }
      });
      return(def.promise);
    }

    return payment_service;
  }

})();
