describe('GiveController', function() {

  beforeEach(module('crossroads'));
  var controller, $rootScope, $scope, $state, $timeout, $q, httpBackend, Session, mockPaymentService, mockGetResponse, programList, mockPaymentServiceGetPromise;


  beforeEach(
    inject(function($injector, $controller, $httpBackend, _$q_) {
      $rootScope = $injector.get('$rootScope');
      $scope = $rootScope.$new();
      $state = $injector.get('$state');
      $timeout = $injector.get('$timeout');
      $q = _$q_;
      httpBackend = $httpBackend;
      Session = $injector.get('Session');

      mockGetResponse = {
        Processor_ID: "123456",
        last4: "9876",
        brand: "Visa"
      };

      mockPaymentServiceGetPromise = {
        $promise: {
          then: function(successCallback, errorCallback) {
            if(this._success) {
              successCallback(mockGetResponse);
            } else {
              errorCallback();
            }
          },
          _success: true
        },
        setSuccess: function(success) {
          this.$promise._success = success;
        }
      };

      mockPaymentService = {
          donor: function(){ return {
            get: function(parms) {
              return(mockPaymentServiceGetPromise);
            }
          };
        },
      };

      controller = $controller('GiveCtrl',
        { '$rootScope': $rootScope,
          '$scope': $scope,
          '$state': $state,
          '$timeout': $timeout,
          'Session': Session,
          'PaymentService': mockPaymentService,
          'programList':programList
        });

      controller.brand = "";
      controller.donor = {};
      controller.donorError = false;
      controller.last4 = "";
    })
  );

  describe('Credit Card type checking', function() {

    it('should have the visa credit card class', function(){
      controller.ccNumber = '4242424242424242';
      controller.ccCardType();
      expect(controller.ccNumberClass).toBe("cc-visa");
    });

    it('should have the mastercard credit card class', function(){
      controller.ccNumber = '5105105105105100';
      controller.ccCardType();
      expect(controller.ccNumberClass).toBe("cc-mastercard");
    });

    it('should have the discover credit card class', function(){
      controller.ccNumber = '6011111111111117';
      controller.ccCardType();
      expect(controller.ccNumberClass).toBe("cc-discover");
    });

    it('should have the amex credit card class', function(){
      controller.ccNumber = '378282246310005';
      controller.ccCardType();
      expect(controller.ccNumberClass).toBe("cc-american-express");
    });

    it('should not a credit card class', function(){
      controller.ccNumber = '';
      controller.ccCardType();
      expect(controller.ccNumberClass).toBe("");
    });
  });

  describe('vm.confirmDonation() emits message in case of exception', function(){
    it('calls vm.donate with missing params', function(){
      spyOn($rootScope, "$emit");
      controller.confirmDonation();
      expect($rootScope.$emit).toHaveBeenCalledWith("notify", 15);
    });
  });

  describe('function transitionForLoggedInUserBasedOnExistingDonor', function(){

    var mockEvent = {
      preventDefault : function(){}
    };

    var mockToState = {
      name : "give.account"
    };

    it('should not perform any transitions for an unauthenticated user', function(){
      $rootScope.username = undefined;

      spyOn($state, "go");
      spyOn(mockEvent, "preventDefault");
      spyOn(mockPaymentService, "donor").and.callThrough();

      controller.transitionForLoggedInUserBasedOnExistingDonor(mockEvent, mockToState);

      expect($state.go).not.toHaveBeenCalled();
      expect(mockEvent.preventDefault).not.toHaveBeenCalled();
      expect(mockPaymentService.donor).not.toHaveBeenCalled();
      expect(controller.donorError).toBeFalsy();
    });

    it('should transition to give.account for a logged-in Giver without an existing donor', function(){
      $rootScope.username = "Shankar";

      spyOn($state, "go");
      spyOn(mockEvent, "preventDefault");
      spyOn(mockPaymentService, "donor").and.callThrough();
      mockPaymentServiceGetPromise.setSuccess(false);

      controller.transitionForLoggedInUserBasedOnExistingDonor(mockEvent, mockToState);

      expect($state.go).toHaveBeenCalledWith("give.account");
      expect(mockEvent.preventDefault).toHaveBeenCalled();
      expect(mockPaymentService.donor).toHaveBeenCalled();
      expect(controller.donorError).toBeTruthy();
    });

    it('should transition to give.confirm for a logged-in Giver with an existing donor', function(){
      $rootScope.username = "Shankar";

      spyOn($state, "go");
      spyOn(mockEvent, "preventDefault");
      spyOn(mockPaymentService, "donor").and.callThrough();
      mockPaymentServiceGetPromise.setSuccess(true);

      controller.transitionForLoggedInUserBasedOnExistingDonor(mockEvent, mockToState);

      expect($state.go).toHaveBeenCalledWith("give.confirm");
      expect(mockEvent.preventDefault).toHaveBeenCalled();
      expect(mockPaymentService.donor).toHaveBeenCalled();
      expect(controller.donorError).toBeFalsy();
      expect(controller.last4).toBe("9876");
      expect(controller.brand).toBe("#cc_visa");
    });
  });



  // describe('Amount to Login/Account/Confirm state transition', function() {
  //   beforeEach(function() {
  //     //getDeferred.resolve(mockGetResponse);
  //     successCallback(mockGetResponse);
  //     $rootScope.$apply();
  //   });
  //
  //   it('should fill in donor when going to Account state', function() {
  //     $scope.giveForm = { amountForm : {$valid : true}};
  //     $rootScope.username = "Tester";
  //     $scope.give = {email: ''};
  //     controller.goToAccount();
  //     expect(mockPaymentService.donor.get).toHaveBeenCalled();
  //     expect(controller.donor.Processor_ID).toBe("123456");
  //     expect($state.$current.name).toBe("give.confirm");
  //   });
  // });

});
