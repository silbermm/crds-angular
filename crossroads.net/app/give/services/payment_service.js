(function () {

  module.exports = PaymentService;

  function PaymentService($log, $http, stripe) {
    var payment_service = {};
    
    stripe.setPublishableKey("pk_test_TR1GulD113hGh2RgoLhFqO0M");
    
    payment_service.createCustomerWithCard = function(card) {

      stripe.card.createToken(card)
        .then(function (token) {
          console.log('token created for card ending in ', token.card.last4);
          var donor_request = {
            tokenId: token.id
          }
          $http.post(__API_ENDPOINT__ + 'api/donor', donor_request);
        });
    }
    
    return payment_service;
  }

})();