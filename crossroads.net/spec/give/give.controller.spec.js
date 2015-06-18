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
      httpBackend = $injector.get('$httpBackend');
      Session = $injector.get('Session');


      mockGetResponse = {
        Processor_ID: "123456",
        default_source :  {
          credit_card : {
            brand : "Visa",
            last4  :"9876"
          },
          bank_account : {
            routing : "111100000",
            last4  :"987654321"
          }
        }
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
        donateToProgram: function() {},
        updateDonorWithCard: function() {},
        updateDonorWithBankAcct: function() {}
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
      controller.programsInput = [
        {ProgramId: 1, Name: "Crossroads"},
        {ProgramId: 2, Name: "Game Change"},
        {ProgramId: 3, Name: "Fuel"},
      ];
    })
  );

  describe('vm.confirmDonation() emits message in case of exception', function(){
    it('calls vm.donate with missing params', function(){
      spyOn($rootScope, "$emit");
      controller.confirmDonation();
      expect($rootScope.$emit).toHaveBeenCalledWith("notify", 15);
    });
  });

  describe('function submitChangedBankInfo-CreditCard', function() {
    var controllerGiveForm = {
      creditCardForm: {
        $dirty: true,
      },
      $valid: true,
    };

    var controllerDto = {
      amount: 987,
      program: {
        ProgramId: 1,
      },
      email: 'tim@kriz.net',
      donor: {
        id: 654,
        default_source: {
          name: 'Tim Startsgiving',
          last4: '98765',
          exp_date: '1213',
          address_zip: '90210',
        }
      }
    };

    var controllerCvc = '987';

    it('should call updateDonorWithCard with proper values when changing card info', function() {
      $scope.giveForm = controllerGiveForm;
      controller.dto = controllerDto;
      controller.cvc = controllerCvc;

      spyOn(mockPaymentService, 'updateDonorWithCard').and.callFake(function(donorId, donor) {
        var deferred = $q.defer();
        deferred.resolve(donor);
        return deferred.promise;
      });

      spyOn(controller, 'donate');

      controller.submitChangedBankInfo();
      // This resolves the promise above
      $rootScope.$apply();

      expect(controller.donate).toHaveBeenCalled();
      expect(mockPaymentService.updateDonorWithCard).toHaveBeenCalledWith(
        controllerDto.donor.id,
        {
          name: controllerDto.donor.default_source.name,
          number: controllerDto.donor.default_source.last4,
          exp_month: controllerDto.donor.default_source.exp_date.substr(0,2),
          exp_year: controllerDto.donor.default_source.exp_date.substr(2,2),
          cvc: controllerCvc,
          address_zip: controllerDto.donor.default_source.address_zip
        }
      );
    });
  });


describe('function submitChangedBankInfo-BankAcccount', function() {
    var controllerGiveForm = {
      bankAccountForm: {
        $dirty: true,
      },
      $valid: true,
    };

    var controllerDto = {
      amount: 987,
      program: {
        ProgramId: 1,
      },
      email: 'tim@kriz.net',
      donor: {
        id: 654,
        default_source: {
          last4: '12345698765',
          routing: '110000000',
        }
      }
    };

   
    it('should call updateDonorWithBankAcct with proper values when changing bank info', function() {
      $scope.giveForm = controllerGiveForm;
      controller.dto = controllerDto;
   
      spyOn(mockPaymentService, 'updateDonorWithBankAcct').and.callFake(function(donorId, donor) {
        var deferred = $q.defer();
        deferred.resolve(donor);
        return deferred.promise;
      });

      spyOn(controller, 'donate');

      controller.submitChangedBankInfo();
      // This resolves the promise above
      $rootScope.$apply();

      expect(controller.donate).toHaveBeenCalled();
      expect(mockPaymentService.updateDonorWithBankAcct).toHaveBeenCalledWith(
        controllerDto.donor.id,
        {
          number: controllerDto.donor.default_source.last4,
          routing: controllerDto.donor.default_source.routing
        }
      );
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
      $scope.give = {
        email: "test@test.com"
      };

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
      $scope.give = {
        email: "test@test.com"
      };

      controller.transitionForLoggedInUserBasedOnExistingDonor(mockEvent, mockToState);

      expect($state.go).toHaveBeenCalledWith("give.confirm");
      expect(mockEvent.preventDefault).toHaveBeenCalled();
      expect(mockPaymentService.donor).toHaveBeenCalled();
      expect(controller.donorError).toBeFalsy();
      expect(controller.donor.default_source.credit_card.last4).toBe("9876");
      expect(controller.donor.default_source.credit_card.brand).toBe("Visa");
    });

    it('should set brand and last 4 correctly when payment type is bank', function(){
      mockGetResponse = {
        Processor_ID: "123456",
        default_source :  {
          credit_card : {
            brand : null,
            last4  :null
          },
          bank_account: {
            routing: "111000222",
            last4: "6699"
          }
        }
      };
      $rootScope.username = "Shankar";

      var mockEvent = {
      preventDefault : function(){}
      };

      var mockToState = {
      name : "give.account"
      };
      $scope.give = {
        email: "test@test.com"
      };

      controller.transitionForLoggedInUserBasedOnExistingDonor(mockEvent, mockToState);
      expect(controller.last4).toBe("6699");
      expect(controller.brand).toBe("#library");
    });
  });

  describe('function goToChange', function() {
    it('should populate dto with appropriate values when going to the change page', function() {
      controller.dto = {};
      controller.goToChange(123, "donor", "test@here.com", "program", "view");
      expect(controller.dto.amount).toBe(123);
      expect(controller.dto.donor).toBe("donor");
      expect(controller.dto.email).toBe("test@here.com");
      expect(controller.dto.program).toBe("program");
      expect(controller.dto.view).toBe("cc");
      expect(controller.dto.changeAccountInfo).toBeTruthy();
    });
  });

  describe('function donate', function() {
    var callback = {
      onSuccess: function() { }
    };

    it('should call success callback if donation is successful', function() {
      spyOn(mockPaymentService, 'donateToProgram').and.callFake(function(programId, amount, donorId, email, pymtType) {
        var deferred = $q.defer();
        deferred.resolve({ amount: amount, });
        return deferred.promise;
      });

      spyOn(callback, 'onSuccess');

      controller.donate(1, 123, "2", "test@here.com", "cc", callback.onSuccess);
      // This resolves the promise above
      $rootScope.$apply();

      expect(controller.amount).toBe(123);
      expect(controller.program).toBeDefined();
      expect(controller.program_name).toBe("Crossroads");
      expect(callback.onSuccess).toHaveBeenCalled();
    });

    it('should not call success callback if donation fails', function() {
      spyOn(mockPaymentService, 'donateToProgram').and.callFake(function(programId, amount, donorId, email, pymtType) {
        var deferred = $q.defer();
        deferred.reject("Uh oh!");
        return deferred.promise;
      });

      spyOn(callback, 'onSuccess');

      controller.amount = undefined;
      controller.program = undefined;
      controller.program_name = undefined;

      controller.donate(1, 123, "2", "test@here.com", callback.onSuccess);
      try {
        // This resolves the promise above
        $rootScope.$apply();
        fail("Expected exception was not thrown");
      } catch(err) {
        expect(err.message).toBeDefined();
        expect(err.message).toMatch(/Uh oh!/);
        expect(err.name).toBe("DonationException");
      }

      expect(controller.amount).toBeUndefined();
      expect(controller.program).toBeUndefined();
      expect(controller.program_name).toBeUndefined();
      expect(callback.onSuccess).not.toHaveBeenCalled();
    });
  });
});
