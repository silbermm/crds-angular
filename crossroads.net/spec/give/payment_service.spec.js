describe ('PaymentService', function () {
  var sut, httpBackend, stripe;
  
  var card = {
    number : "4242424242424242",
    exp_month : "12",
    exp_year : "2016",
    cvc : "123"
  };

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
  
  beforeEach(inject(function(_$injector_, $httpBackend, _PaymentService_) {
      var $injector = _$injector_;

      sut = _PaymentService_;
      httpBackend = $httpBackend;
      stripe = $injector.get('stripe');
    })
  );
  
  afterEach(function() {
    httpBackend.flush();
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
   });
  
  describe ('createDonorWithCard', function() {
    var result;
    
    beforeEach(function() {
      spyOn(stripe.card, 'createToken').and.callThrough();
      
      var postData = {
        stripe_token_id: "tok_test"
      }
      httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] +'api/donor', postData)
        .respond({
          id: "12345",
          stripe_customer_id: "cust_test"
        });
      sut.createDonorWithCard(card)
        .then(function(donor) {
          result = donor;
        });
    });
    
    it('should create a single use token', function() {
      expect(stripe.card.createToken).toHaveBeenCalledWith(card);
    });
    
    it('should create a new donor', function() {
      expect(result).toBeDefined();
      expect(result.id).toEqual("12345");
      expect(result.stripe_customer_id).toEqual("cust_test");
    });
  });
   
  describe('createDonorWithCard Error', function() {
    it('should return error if there is problem calling donor service', function() {
      var postData = {
        stripe_token_id: "tok_test"
      }
      httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] +'api/donor', postData)
        .respond(400,{
          message: "Token not found"
        } );
      sut.createDonorWithCard(card)
        .then(function(donor) {
          result = donor;
        }, 
        function(error) {
          expect(error).toBeDefined();
          expect(error.message).toEqual("Token not found");
        });
    });
  });

  describe('donateToProgram', function(){

    var postData = {
        programId: "Program",
        amount: "1234",
        donor_id: "Donor"
      }

    httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] +'api/donation', postData)
        .respond({
          id: "12345",
          stripe_customer_id: "cust_test"
        });
  });
  
});