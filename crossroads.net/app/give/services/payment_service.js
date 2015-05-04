(function () {

  var getCookie = require('../../utilities/cookies');

  module.exports = PaymentService;

  function PaymentService($log, $http, $q, stripe) {
    var payment_service = {
      donor : {},
      createDonorWithCard : createDonorWithCard
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
    
    return payment_service;
  }

})();