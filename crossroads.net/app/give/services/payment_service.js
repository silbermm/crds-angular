(function () {

  var getCookie = require('../../utilities/cookies');
  angular.module('crossroads.give').factory('PaymentService',PaymentService);

  PaymentService.$inject = ['$log', '$http', '$resource','$q', 'stripe'];

  function PaymentService($log, $http, $resource, $q, stripe) {
    var payment_service = {
      donor : $resource(__API_ENDPOINT__ + 'api/donor/?email=:email',{email: '@_email'}, {
        get: {
          method : 'GET',
          headers: {'Authorization': getCookie('sessionId')}
        }
      }),
      donation : {},
      createDonorWithCard : createDonorWithCard,
      donateToProgram : donateToProgram
    };

    stripe.setPublishableKey(__STRIPE_PUBKEY__);

    function createDonorWithCard(card) {
      var def = $q.defer();
      stripe.card.createToken(card)
        .then(function (token) {
          var donor_request = { stripe_token_id: token.id }
          $http({
            method: "POST",
            url: __API_ENDPOINT__ + 'api/donor',
            headers: {
              'Authorization': getCookie('sessionId')
            },
            data: donor_request
            }).success(function(data) {
              payment_service.donor = data;
              def.resolve(data);
            }).error(function(error) {
              def.reject(error);
            });
        });
       return def.promise;
    }

    function donateToProgram(program_id, amount, donor_id){
      var def = $q.defer();
      var donation_request = {
        "program_id" : program_id,
        "amount" : amount,
        "donor_id" : donor_id
      };
      $http({
        method: "POST",
        url: __API_ENDPOINT__ + 'api/donation',
        data: donation_request,
        headers: {
              'Authorization': getCookie('sessionId')
            }
        }).success(function(data){
          payment_service.donation = data;
          def.resolve(data);
        }).error(function(error) {
          def.reject(error);
        });

      return def.promise;

    }

    return payment_service;
  }

})();
