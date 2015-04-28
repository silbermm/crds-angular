(function () {

  module.exports = StripeService;

  function StripeService($log, $http, stripe) {
    var stripe_service = {};
    
    stripe.setPublishableKey("pk_test_TR1GulD113hGh2RgoLhFqO0M");
    
    stripe_service.createCustomer = function (account_info) {
      
      var card = 
        {
          number: '4242424242424242',
          cvc: '123',
          exp_month: '12',
          exp_year: '2016'
        }
      
      stripe.card.createToken(card)
        .then(function (token) {
          console.log('token created for card ending in ', token.card.last4);
        });
      
      
    }
    
    return stripe_service;
  }

})();