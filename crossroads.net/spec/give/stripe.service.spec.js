describe ('StripeService', function () {
  var sut, httpBackend, stripe;

  beforeEach(function() {
    module('crossroads.give');
    
    module(function($provide) {
      $provide.value('stripe', {
        setPublishableKey: function() {},
        card :
          {
            createToken : function(card) {
              var last4 = card.number.slice(-4);
              return {
                then : function(callback) {return callback({id: "tok_test", card: { last4: last4}});}
              }
            } 
          }
      });
    });
    return null;
  })
  
  beforeEach(inject(function(_$injector_, $httpBackend, _StripeService_) {
      var $injector = _$injector_;

      sut = _StripeService_;
      httpBackend = $httpBackend;
      stripe = $injector.get('stripe');
    })
  );
  
  afterEach(function() {
    httpBackend.flush();
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
   });
  
  describe ('createCustomerWithCard', function() {
    beforeEach(function() {
      var card = {
        number : "4242424242424242",
        exp_month : "12",
        exp_year : "2016",
        cvc : "123"
      };
      
      var postData = {
        tokenId: "tok_test"
      }
      spyOn(stripe.card, 'createToken').and.callThrough();
      httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] +'api/donor', postData)
        .respond({
          id: "12345"
        });
      this.result = sut.createCustomerWithCard(card);
    });
    
    it('should create a single use token', function() {
      expect(stripe.card.createToken).toHaveBeenCalled();
    });
    
    xit('should create a new customer', function() {
      expect(this.result).toBeDefined();
      expect(this.result.id).toEqual("12345");
    });
  });
});