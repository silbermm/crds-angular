describe ('StripeService', function () {
  var sut, stripe;

  beforeEach(function() {
    module('crossroads.give');
    
    module(function($provide) {
      $provide.value('stripe', {
        setPublishableKey: function() {},
        card :
          {
            createToken : function(card) {
              return {
                then : function(callback) {return callback({card: { last4: "1234"}});}
              }
            } 
          }
      });
    });
    return null;
  })
  
  beforeEach(inject(function(_$injector_, _stripe_) {
      var $injector = _$injector_;
      
      //$httpBackend = $injector.get('$httpBackend');
      stripe = $injector.get('stripe');
      
      sut = $injector.get('StripeService');
    })
  );
  
  // afterEach(function() {
  //   $httpBackend.flush();
  //   $httpBackend.verifyNoOutstandingExpectation();
  //   $httpBackend.verifyNoOutstandingRequest();
  //  });
  
  describe ('createCustomer', function () {
    beforeEach(function() {
      spyOn(stripe.card, 'createToken').and.callThrough();
      sut.createCustomer();
    });
    it('should create a single use token', function () {
      expect(stripe.card.createToken).toHaveBeenCalled();
    });
  });
});