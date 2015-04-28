describe ('StripeService', function () {
  var sut;
  
  beforeEach(module('crossroads'));
  
  beforeEach(inject(function(_$injector_) {
      var $injector = _$injector_;
      
      $httpBackend = $injector.get('$httpBackend');
      sut = $injector.get('StripeService');
    })
  );
  
  afterEach(function() {
    $httpBackend.flush();
    $httpBackend.verifyNoOutstandingExpectation();
    $httpBackend.verifyNoOutstandingRequest();
   });
    
  it('should create a single use token', function () {
    sut.createCustomer();
    $httpBackend.expectPOST('http://api.stripe.com/v1/tokens');
  });
});