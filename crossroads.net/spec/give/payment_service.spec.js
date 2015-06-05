describe ('PaymentService', function () {
  var sut, httpBackend, stripe;

  var card = {
    number : "4242424242424242",
    exp_month : "12",
    exp_year : "2016",
    cvc : "123"
  };
  
  var bank = {
    account_number: "1234567890",
    routing_number: "111122223"
  }
  
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
              };
            }
          },
        bankAccount :
          {
            createToken : function(bank) {
              return {
                then: function(callback) { return callback({id: "tok_test"})}
              }
            }
          }
      });
    });
    return null;
  });

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
        stripe_token_id: "tok_test",
        email_address: "me@here.com"
      };
      httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] +'api/donor', postData)
        .respond({
          id: "12345",
          stripe_customer_id: "cust_test"
        });
      sut.createDonorWithCard(card, "me@here.com")
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
      };
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
  
  describe('createDonorWithBankAcct', function() {
    var result;
    
    beforeEach(function() {
      spyOn(stripe.bankAccount, 'createToken').and.callThrough();
      
      var postData = {
        stripe_token_id: "tok_test",
        email_address: "me@here.com"
      };
      httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] + 'api/donor', postData)
        .respond({
          id: "12345",
          stripe_customer_id: "cust_test"
        });
      sut.createDonorWithBankAcct(bank, "me@here.com")
        .then(function(donor){
          result = donor;
        });
    });
    
    it('should create a single use token', function() {
      expect(stripe.bankAccount.createToken).toHaveBeenCalledWith(bank);
    });
    
    it('should create a new donor', function() {
      expect(result).toBeDefined();
      expect(result.id).toEqual("12345");
      expect(result.stripe_customer_id).toEqual("cust_test");
    });
  });
  
  describe('donateToProgram', function(){
    it('should successfully create a donation', function(){

    var postData = {
        program_id: "Program",
        amount: "1234",
        donor_id: "Donor"
      };

    httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] +'api/donation', postData)
        .respond({
          amount: "1234",
          program_id: "Program"
        });

    sut.donateToProgram("Program", "1234", "Donor")
      .then(function(confirmation){
        expect(confirmation.program_id).toEqual("Program");
        expect(confirmation.amount).toEqual("1234");
      });
    });
  });

  describe ('updateDonorWithCard', function() {
     var result;

     var card = {
        number : "5555555555554444",
        exp_month : "06",
        exp_year : "2020",
        cvc : "987"
      };

    beforeEach(function() {
      spyOn(stripe.card, 'createToken').and.callThrough();

      var putData = {
        stripe_token_id: "tok_test"
      };
      httpBackend.expectPUT(window.__env__['CRDS_API_ENDPOINT'] +'api/donor', putData)
        .respond({
          id: "12345",
          stripe_customer_id: "cust_test"
        });
      sut.updateDonorWithCard('12345', card)
        .then(function(donor) {
          result = donor;
        });
    });

     it('should create a single use token', function() {
      expect(stripe.card.createToken).toHaveBeenCalledWith(card);
    });

    it('should update the existing donor', function() {
      expect(result).toBeDefined();
      expect(result.id).toEqual("12345");
      expect(result.stripe_customer_id).toEqual("cust_test");
    });
  });

  describe('createDonorWithCard Error', function() {
    it('should return error if there is problem calling donor service', function() {
      var putData = {
        stripe_token_id: "tok_test"
      };
      httpBackend.expectPUT(window.__env__['CRDS_API_ENDPOINT'] +'api/donor', putData)
        .respond(400,{
          message: "Token not found"
        } );
      sut.updateDonorWithCard('12345',card)
        .then(function(donor) {
          result = donor;
        },
        function(error) {
          expect(error).toBeDefined();
          expect(error.message).toEqual("Token not found");
        });
    });
  });


});
