describe('GiveController', function() {

  beforeEach(module('crossroads'));
  var $controller, $rootScope, $scope, $state, $timeout, httpBackend, Session, PaymentService, programList;


  beforeEach(
    inject(function($injector, $httpBackend) {
      $controller = $injector.get('$controller');
      $rootScope = $injector.get('$rootScope');
      $scope = $rootScope.$new();
      $state = $injector.get('$state');
      $timeout = $injector.get('$timeout');
      httpBackend = $httpBackend;
      Session = $injector.get('Session');
      PaymentService = $injector.get('PaymentService');
      
      spyOn(PaymentService.donor, "get").and.callFake(function(donor) {
        return {
          success: function() {
            return {Processor_ID: "123456"};
          },
          error: function() {
            
          }
        }
      });
      
      $controller = $controller('GiveCtrl', 
        { '$rootScope': $rootScope, 
          '$scope': $scope, 
          '$state': $state, 
          '$timeout': $timeout,
          'Session': Session, 
          'PaymentService': PaymentService, 
          'programList':programList 
        });
    })
  );
  
  describe('Credit Card type checking', function() {

    it('should have the visa credit card class', function(){
      $controller.ccNumber = '4242424242424242';
      $controller.ccCardType();
      expect($controller.ccNumberClass).toBe("cc-visa");
    });

    it('should have the mastercard credit card class', function(){
      $controller.ccNumber = '5105105105105100';
      $controller.ccCardType();
      expect($controller.ccNumberClass).toBe("cc-mastercard");
    });

    it('should have the discover credit card class', function(){
      $controller.ccNumber = '6011111111111117';
      $controller.ccCardType();
      expect($controller.ccNumberClass).toBe("cc-discover");
    });

    it('should have the amex credit card class', function(){
      $controller.ccNumber = '378282246310005';
      $controller.ccCardType();
      expect($controller.ccNumberClass).toBe("cc-american-express");
    });

    it('should not a credit card class', function(){
      $controller.ccNumber = '';
      $controller.ccCardType();
      expect($controller.ccNumberClass).toBe("");
    });
  });
  
  describe('Amount to Login/Account/Confirm state transition', function() {
    it('should fill in donor when going to Account state', function() {
      $scope.giveForm = { amountForm : {$valid : true}};
      $rootScope.username = "Tester";
      $controller.goToAccount();
      expect(PaymentService.donor.get).toHaveBeenCalled();
      expect($controller.donor.Processor_ID).toBe("123456");
    });
  });

});
