describe('GiveController', function() {
  var controller, $rootScope, $scope, $state, $timeout, $q, httpBackend, Session, mockPaymentService, mockGetResponse, programList, mockPaymentServiceGetPromise;

  beforeEach(module('crossroads', function($provide) {
    programList = [
      {ProgramId: 1, Name: "Crossroads"},
      {ProgramId: 2, Name: "Game Change"},
      {ProgramId: 3, Name: "Old St George Building"},
    ];
    $provide.value('getPrograms', {
      Programs: function() {
        return({
          get: function() {
            var deferred = $q.defer();
            deferred.resolve(programList);
            return deferred.promise
          },
        });
      },
    });
  }));

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
        updateDonorWithBankAcct: function() {},
        updateDonorWithCard: function() {},
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
      controller.programsInput = programList;
    })
  );

  describe('vm.confirmDonation() emits message in case of exception', function(){
    it('calls vm.donate with missing params', function(){
      spyOn($rootScope, "$emit");
      controller.confirmDonation();
      expect($rootScope.$emit).toHaveBeenCalledWith("notify", 15);
    });
  });

  describe('function initDefaultState', function() {
    var controllerDto;

    beforeEach(function() {
      controllerDto = jasmine.createSpyObj('dto', ['reset']);

      spyOn(Session, 'removeRedirectRoute');

      spyOn($state, 'go');
      spyOn($scope, '$on').and.callFake(function(evt, handler) {
        handler();
      });
    });

    it('should go to give.amount if starting at give', function() {
      spyOn($state, 'is').and.returnValue(true);

      controller.initialized = false;
      controller.dto = controllerDto;
      controller.initDefaultState();

      expect($state.is).toHaveBeenCalledWith('give');
      expect($state.go).toHaveBeenCalledWith('give.amount');
      expect(controllerDto.reset).not.toHaveBeenCalled();
      expect(Session.removeRedirectRoute).not.toHaveBeenCalled();
    });

    it('should go to give.amount if starting at give', function() {
      var states = {
        'give': true,
        'give.amount': false,
      };
      spyOn($state, 'is').and.callFake(function(stateName) {
        return(states[stateName]);
      });

      controller.initialized = false;
      controller.dto = controllerDto;
      controller.initDefaultState();

      expect($state.is).toHaveBeenCalledWith('give');
      expect($state.go).toHaveBeenCalledWith('give.amount');
      expect(controller.initialized).toBeTruthy();
      expect(controllerDto.reset).not.toHaveBeenCalled();
      expect(Session.removeRedirectRoute).not.toHaveBeenCalled();
    });

    it('should do nothing special if starting at give.amount', function() {
      var states = {
        'give': false,
        'give.amount': true,
      };
      spyOn($state, 'is').and.callFake(function(stateName) {
        return(states[stateName]);
      });

      controller.initialized = false;
      controller.dto = controllerDto;
      controller.initDefaultState();

      expect($state.is).toHaveBeenCalledWith('give');
      expect($state.is).toHaveBeenCalledWith('give.amount');
      expect($state.go).not.toHaveBeenCalled();
      expect(controller.initialized).toBeTruthy();
      expect(controllerDto.reset).not.toHaveBeenCalled();
      expect(Session.removeRedirectRoute).not.toHaveBeenCalled();
    });

    it('should go to give.amount if starting at an unknown state and not initialized', function() {
      var states = {
        'give': false,
        'give.amount': false,
      };
      spyOn($state, 'is').and.callFake(function(stateName) {
        return(states[stateName]);
      });

      controller.initialized = false;
      controller.dto = controllerDto;
      controller.initDefaultState();

      expect($state.is).toHaveBeenCalledWith('give');
      expect($state.go).toHaveBeenCalledWith('give.amount');
      expect($scope.$on).not.toHaveBeenCalled();
      expect(controller.initialized).toBeTruthy();
      expect(controllerDto.reset).toHaveBeenCalled();
      expect(Session.removeRedirectRoute).toHaveBeenCalled();
    });
  });

  describe('$stateChangeStart event hook', function() {
    beforeEach(function() {
      controller.processing = false;
      spyOn(controller, 'initDefaultState');
      spyOn(controller, 'transitionForLoggedInUserBasedOnExistingDonor');
    });

    it('should initialize default state if not already initialized', function() {
      $rootScope.email = "me@here.com";
      controller.email = undefined;

      controller.initialized = false;
      var event = $scope.$broadcast('$stateChangeStart');

      expect(event.defaultPrevented).toBeTruthy();
      expect(controller.initDefaultState).toHaveBeenCalled();
      expect(controller.transitionForLoggedInUserBasedOnExistingDonor).not.toHaveBeenCalled();
      expect(controller.processing).toBeTruthy();
      expect(controller.email).toBeDefined();
      expect(controller.email).toBe("me@here.com");
    });

    it('should not initialize default state if already initialized', function() {
      $rootScope.email = undefined;
      controller.email = "me2@here.com";

      controller.initialized = true;
      var event = $scope.$broadcast('$stateChangeStart');

      expect(event.defaultPrevented).toBeFalsy();
      expect(controller.initDefaultState).not.toHaveBeenCalled();
      expect(controller.transitionForLoggedInUserBasedOnExistingDonor).toHaveBeenCalled();
      expect(controller.processing).toBeTruthy();
      expect(controller.email).toBeDefined();
      expect(controller.email).toBe("me2@here.com");
    });

  });

  describe('$stateChangeSuccess event hook', function() {
    var controllerDto;

    beforeEach(function() {
      controllerDto = jasmine.createSpyObj('dto', ['reset']);
      controller.dto = controllerDto;
      controller.processing = true;
      controller.initialized = true;
    });

    it('should not un-initialize controller if toState is not thank-you', function() {
      var event = $scope.$broadcast('$stateChangeSuccess', {name: 'give.amount'});

      expect(controller.processing).toBeFalsy();
      expect(controller.initialized).toBeTruthy();
      expect(controllerDto.reset).not.toHaveBeenCalled();
    });

    it('should un-initialize controller if toState is thank-you', function() {
      var event = $scope.$broadcast('$stateChangeSuccess', {name: 'give.thank-you'});

      expect(controller.processing).toBeFalsy();
      expect(controller.initialized).toBeFalsy();
      expect(controllerDto.reset).toHaveBeenCalled();
    });
  });

  describe('function submitChangedBankInfo', function() {
    var controllerGiveForm = {
      creditCardForm: {
        $dirty: true,
      },
      $valid: true,
    };

    var controllerGiveFormBank = {
      bankAccountForm: {
        $dirty: true,
      },
      $valid: true,
    };

    var controllerDto = {
      amount: 987,
      view: 'cc',
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
          cvc: '987',
        }
      },
      reset: function() {},
    };

    var controllerBankDto = {
      amount: 858,
      view: 'bank',
      program: {
        ProgramId: 2,
      },
      email: 'tim@kriz.net',
      donor: {
        id: 654,
        default_source: {          
          last4: '753869',
          routing: '110000000'          
        }
      },
      reset: function() {},
    };


    it('should call updateDonorWithCard with proper values when changing card info', function() {
      $scope.giveForm = controllerGiveForm;
      controller.dto = controllerDto;

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
          cvc: controllerDto.donor.default_source.cvc,
          address_zip: controllerDto.donor.default_source.address_zip
        }
      );
    });

   it('should call updateDonorWithBankAcct with proper values when bank account info in changed', function() {
      $scope.giveForm = controllerGiveFormBank;
      controller.dto = controllerBankDto;

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
          account: controllerBankDto.donor.default_source.account,
          routing: controllerBankDto.donor.default_source.routing      
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
    it('should populate dto with appropriate values when going to the credit card change page', function() {
      controller.dto = { 
        reset: function() {},
      };
      controller.brand = "#visa";
      controller.goToChange(123, "donor", "test@here.com", "program");
      expect(controller.dto.amount).toBe(123);
      expect(controller.dto.donor).toBe("donor");
      expect(controller.dto.email).toBe("test@here.com");
      expect(controller.dto.program).toBe("program");
      expect(controller.dto.view).toBe("cc");
      expect(controller.dto.changeAccountInfo).toBeTruthy();
    });

    it('should populate dto with appropriate values when going to the bank account change page', function() {
      controller.dto = { 
        reset: function() {},
      };
      controller.brand = "#library";
      controller.goToChange(123, "donor", "test@here.com", "program");
      expect(controller.dto.amount).toBe(123);
      expect(controller.dto.donor).toBe("donor");
      expect(controller.dto.email).toBe("test@here.com");
      expect(controller.dto.program).toBe("program");
      expect(controller.dto.view).toBe("bank");
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
