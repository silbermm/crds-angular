require('../../dependencies/dependencies');
require('../../core/core');
require('../../app/app');

describe('GiveController', function() {
  var controller, $rootScope, $scope, $state, $timeout, $q, httpBackend, Session, mockPaymentService, mockGetResponse, programList, mockPaymentServiceGetPromise, mockSession;

  beforeEach(angular.mock.module('crossroads', function($provide) {
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
    mockSession = jasmine.createSpyObj('Session', ['exists', 'isActive', 'removeRedirectRoute']);
    $provide.value('Session', mockSession);
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
      User = $injector.get('User');
      AUTH_EVENTS = $injector.get('AUTH_EVENTS');

      mockGetResponse = {
        id: "102030",
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
              errorCallback({httpStatusCode: this._httpStatusCode});
            }
          },
          _success: true,
          _httpStatusCode: 200,
        },
        setSuccess: function(success) {
          this.$promise._success = success;
        },
        setHttpStatusCode: function(code) {
          this.$promise._httpStatusCode = code;
        }
      };

      mockPaymentService = {
        getDonor: function(email){
          return(mockPaymentServiceGetPromise.$promise);
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
          'programList':programList,
          'User' : User,
          'AUTH_EVENTS': AUTH_EVENTS
        });

      controller.brand = "";
      controller.donor = {};
      controller.donorError = false;
      controller.last4 = "";
      controller.programsInput = programList;
    })
  );

  describe('function confirmDonation()', function() {
     beforeEach(function() {
       spyOn(controller, '_stripeErrorHandler');
       spyOn(controller,  'goToChange');
     });

    it('should go to the thank-you page if credit card payment was accepted', function() {
      var error = {error1: '1', error2: '2'};
      controller.dto = {
        view: 'cc'
      };
      controller.amount = 123;
      controller.donor = {donorinfo: 'blah'};
      controller.email = 'test@somewhere.com';
      controller.program = {id: 3};

      spyOn(controller, 'donate').and.callFake(function(programId, amount, donorId, email, pymtType, onSuccess, onFailure) {
        onSuccess(error);
      });
      spyOn($state, 'go');

      controller.confirmDonation();
      expect($state.go).toHaveBeenCalledWith('give.thank-you');
      expect(controller._stripeErrorHandler).not.toHaveBeenCalled();
      expect(controller.goToChange).not.toHaveBeenCalled();
    });

    it('should go to the change page if credit card payment was declined', function() {
      var error = {error1: '1', error2: '2'};
      controller.dto = {
        view: 'cc'
      };
      controller.amount = 123;
      controller.donor = {donorinfo: 'blah'};
      controller.email = 'test@somewhere.com';
      controller.program = {id: 3};

      mockSession.isActive.and.callFake(function(){
        return true;
      });
      spyOn(controller, 'donate').and.callFake(function(programId, amount, donorId, email, pymtType, onSuccess, onFailure) {
        controller.dto.declinedPayment = true;
        onFailure(error);
      });
      spyOn($state, 'go');

      controller.confirmDonation();
      //sr 08/03/15  ask jasmine jim why this doesn't fail
      expect($state.go).not.toHaveBeenCalled();
      expect(controller._stripeErrorHandler).toHaveBeenCalledWith(error);
      expect(controller.goToChange).toHaveBeenCalledWith(
        controller.amount,
        controller.donor,
        controller.email,
        controller.program,
        controller.dto.view);
    });

    it('should stay on the confirm page if there was a processing error', function() {
      var error = {error1: '1', error2: '2'};
      controller.dto = {
        view: 'cc'
      };
      controller.amount = 123;
      controller.donor = {donorinfo: 'blah'};
      controller.email = 'test@somewhere.com';
      controller.program = {id: 3};

      mockSession.isActive.and.callFake(function(){
        return true;
      });
      spyOn(controller, 'donate').and.callFake(function(programId, amount, donorId, email, pymtType, onSuccess, onFailure) {
        controller.dto.declinedPayment = false;
        onFailure(error);
      });
      spyOn($state, 'go');

      controller.confirmDonation();
      expect($state.go).not.toHaveBeenCalled();
      expect(controller._stripeErrorHandler).toHaveBeenCalledWith(error);
      expect(controller.goToChange).not.toHaveBeenCalled();
    });

    it('should emit a failure message if called with missing params', function(){
      spyOn($rootScope, "$emit");
      controller.confirmDonation();
      expect($rootScope.$emit).toHaveBeenCalledWith("notify", 15);
    });

    it('should goto give.login if session is not active', function() {
      mockSession.isActive.and.callFake(function(){
        return false;
      });
      spyOn($state, 'go');

      controller.confirmDonation();
      expect($state.go).toHaveBeenCalledWith('give.login');
    });
  });

  describe('function initDefaultState', function() {
    var controllerDto;

    beforeEach(function() {
      controllerDto = jasmine.createSpyObj('dto', ['reset']);

      spyOn($state, 'go');
      spyOn($scope, '$on').and.callFake(function(evt, handler) {
        handler();
      });
    });

    it('should go to give.amount if starting at give', function() {

      controller.initialized = false;
      controller.dto = controllerDto;
      controller.initDefaultState();

      expect($state.go).toHaveBeenCalledWith('give.amount');
      expect(controllerDto.reset).toHaveBeenCalled();
      expect(Session.removeRedirectRoute).toHaveBeenCalled();
    });

    it('should go to give.amount if starting at give II', function() {
      var states = {
        'give': true,
        'give.amount': false,
      };

      controller.initialized = false;
      controller.dto = controllerDto;
      controller.initDefaultState();

      expect($state.go).toHaveBeenCalledWith('give.amount');
      expect(controller.initialized).toBeTruthy();
      expect(controllerDto.reset).toHaveBeenCalled();
      expect(Session.removeRedirectRoute).toHaveBeenCalled();
    });

    it('should do nothing special if starting at give.amount', function() {
      var states = {
        'give': false,
        'give.amount': true,
      };

      controller.initialized = false;
      controller.dto = controllerDto;
      controller.initDefaultState();

      expect($state.go).toHaveBeenCalled();
      expect(controller.initialized).toBeTruthy();
      expect(controllerDto.reset).toHaveBeenCalled();
      expect(Session.removeRedirectRoute).toHaveBeenCalled();
    });

    it('should go to give.amount if starting at an unknown state and not initialized', function() {
      var states = {
        'give': true,
        'give.amount': false,
      };

      controller.initialized = false;
      controller.dto = controllerDto;
      controller.initDefaultState();

      expect($state.go).toHaveBeenCalledWith('give.amount');
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
      expect(controller.email).toBeUndefined();
    });

    it('should not initialize default state if already initialized', function() {
      $rootScope.email = "me2@here.com";
      controller.email = undefined;

      controller.initialized = true;
      var event = $scope.$broadcast('$stateChangeStart', {name: 'give.amount'});

      expect(event.defaultPrevented).toBeFalsy();
      expect(controller.initDefaultState).not.toHaveBeenCalled();
      expect(controller.transitionForLoggedInUserBasedOnExistingDonor).toHaveBeenCalled();
      expect(controller.processing).toBeTruthy();
      expect(controller.email).toBeUndefined();
    });

    it('should initialize default state if toState=give', function() {
      $rootScope.email = "me@here.com";
      controller.email = undefined;

      var event = $scope.$broadcast('$stateChangeStart', {name: 'give.amount'});

      expect(event.defaultPrevented).toBeTruthy();
      expect(controller.initDefaultState).toHaveBeenCalled();
      expect(controller.transitionForLoggedInUserBasedOnExistingDonor).not.toHaveBeenCalled();
      expect(controller.processing).toBeTruthy();
      expect(controller.email).toBeUndefined();
    });

    it('should not do anything if toState is not in the giving flow', function() {
      $rootScope.email = "me@here.com";
      controller.email = undefined;
      controller.processing = false;

      var event = $scope.$broadcast('$stateChangeStart', {name: 'Ohio'});

      expect(event.defaultPrevented).toBeFalsy();
      expect(controller.initDefaultState).not.toHaveBeenCalled();
      expect(controller.transitionForLoggedInUserBasedOnExistingDonor).not.toHaveBeenCalled();
      expect(controller.processing).toBeFalsy();
      expect(controller.email).not.toBeDefined();
    });

  });

  describe('AUTH_EVENTS.logoutSuccess event hook', function() {
    it('should reset data and go home', function() {
      spyOn(controller, 'reset');
      spyOn($state, 'go');

      var event = $scope.$broadcast(AUTH_EVENTS.logoutSuccess);
      expect(controller.reset).toHaveBeenCalled();
      expect($state.go).toHaveBeenCalledWith('home');
    });
  });

  describe('$stateChangeSuccess event hook', function() {
    var controllerDto;

    beforeEach(function() {
      controllerDto = jasmine.createSpyObj('dto', ['reset']);
      controller.dto = controllerDto;
      controller.processing = true;
      controller.initialized = true;
      spyOn(controller, 'reset').and.callThrough();
    });

    it('should not un-initialize controller if toState is not thank-you', function() {
      var event = $scope.$broadcast('$stateChangeSuccess', {name: 'give.amount'});

      expect(controller.processing).toBeFalsy();
      expect(controller.initialized).toBeTruthy();
      expect(controller.reset).not.toHaveBeenCalled();
      expect(controllerDto.reset).not.toHaveBeenCalled();
    });

    it('should un-initialize controller if toState is thank-you', function() {
      var event = $scope.$broadcast('$stateChangeSuccess', {name: 'give.thank-you'});

      expect(controller.processing).toBeFalsy();
      expect(controller.initialized).toBeFalsy();
      expect(controller.reset).not.toHaveBeenCalled();
      expect(controllerDto.reset).toHaveBeenCalled();
    });
  });

  describe('function reset', function() {
    it('should reset all appropriate values', function() {
      var controllerDto = jasmine.createSpyObj('dto', ['reset']);
      controller.amount = 123;
      controller.amountSubmitted = true;
      controller.bankinfoSubmitted = true;
      controller.changeAccountInfo = true;
      controller.donorError = true;
      controller.dto = controllerDto;
      controller.initialized = true;
      controller.processing = true;
      controller.program = 456;

      controller.reset();
      expect(controller.amount).not.toBeDefined();
      expect(controller.amountSubmitted).toBeFalsy();
      expect(controller.bankinfoSubmitted).toBeFalsy();
      expect(controller.changeAccountInfo).toBeFalsy();
      expect(controller.donorError).toBeFalsy();
      expect(controller.initialized).toBeFalsy();
      expect(controller.processing).toBeFalsy();
      expect(controller.program).not.toBeDefined();
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
          cc_number: '98765',
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
          bank_account_number: '753869',
          routing: '110000000',
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
          number: controllerDto.donor.default_source.cc_number,
          exp_month: controllerDto.donor.default_source.exp_date.substr(0,2),
          exp_year: controllerDto.donor.default_source.exp_date.substr(2,2),
          cvc: controllerDto.donor.default_source.cvc,
          address_zip: controllerDto.donor.default_source.address_zip
        },
        'tim@kriz.net'
      );
    });

   it('should call updateDonorWithBankAcct with proper values when bank account info is changed', function() {
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
          country: 'US',
          currency: 'USD',
          account_number: controllerBankDto.donor.default_source.bank_account_number,
          routing_number: controllerBankDto.donor.default_source.routing ,
        },
        'tim@kriz.net'
      );
    });

    it('should go to give.login if session is not active', function() {
       mockSession.isActive.and.callFake(function(){
         return false;
       });
       spyOn($state, "go");
       spyOn(controller, 'donate');

       controller.submitChangedBankInfo();
       expect(controller.donate).not.toHaveBeenCalled();
       expect($state.go).toHaveBeenCalledWith('give.login');
     });
  });

  describe('function submitBankInfo', function() {
    var controllerGiveForm = {
      accountForm: {
        $valid: true,
      },
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

    beforeEach(function() {
      $scope.giveForm = controllerGiveForm;
      controller.dto = controllerDto;
      controller.program = controllerDto.program;
      controller.amount = controllerDto.amount;
      controller.email = controllerDto.email;
    });

    it('should call updateDonorAndDonate when there is an existing donor', function() {
      spyOn(mockPaymentService, 'getDonor').and.callThrough();
      mockPaymentServiceGetPromise.setSuccess(true);
      $scope.give = {
        email: "test@test.com"
      };
      mockSession.isActive.and.callFake(function(){
        return true;
      });
      spyOn(controller, 'createDonorAndDonate');
      spyOn(controller, 'updateDonorAndDonate');
      controller.submitBankInfo();

      expect(mockPaymentService.getDonor).toHaveBeenCalledWith("test@test.com");
      expect(controller.updateDonorAndDonate).toHaveBeenCalledWith(mockGetResponse.id, controllerDto.program.ProgramId, controllerDto.amount, controllerDto.email, controllerDto.view);
      expect(controller.createDonorAndDonate).not.toHaveBeenCalled();
    });

    it('should call createDonorAndDonate when there is not an existing donor', function() {
      spyOn(mockPaymentService, 'getDonor').and.callThrough();
      mockPaymentServiceGetPromise.setSuccess(false);
      $scope.give = {
        email: "test@test.com"
      };
      mockSession.isActive.and.callFake(function(){
       return true;
      });
      spyOn(controller, 'createDonorAndDonate');
      spyOn(controller, 'updateDonorAndDonate');
      controller.submitBankInfo();

      expect(mockPaymentService.getDonor).toHaveBeenCalledWith("test@test.com");
      expect(controller.createDonorAndDonate).toHaveBeenCalledWith(controllerDto.program.ProgramId, controllerDto.amount, controllerDto.email, controllerDto.view);
      expect(controller.updateDonorAndDonate).not.toHaveBeenCalled();
    });

    it('should go to give.login if session is not active', function() {
       mockSession.isActive.and.callFake(function(){
         return false;
       });
       $scope.give = {
         email: "test@test.com"
       };
       spyOn($state, "go");

       spyOn(mockPaymentService, "getDonor").and.callThrough();
       spyOn(controller, 'updateDonorAndDonate');
       spyOn(controller, 'createDonorAndDonate');

       controller.submitBankInfo();

       expect(controller.createDonorAndDonate).not.toHaveBeenCalled();
       expect($state.go).toHaveBeenCalledWith('give.login');
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
       mockSession.isActive.and.callFake(function(){
         return false;
       });
       spyOn($state, "go");
       spyOn(mockEvent, "preventDefault");
       spyOn(mockPaymentService, "getDonor").and.callThrough();

       controller.transitionForLoggedInUserBasedOnExistingDonor(mockEvent, mockToState);

       expect($state.go).not.toHaveBeenCalled();
       expect(mockEvent.preventDefault).not.toHaveBeenCalled();
       expect(mockPaymentService.getDonor).not.toHaveBeenCalled();
       expect(controller.donorError).toBeFalsy();
     });

     it('should transition to give.account for a logged-in Giver without an existing donor', function(){
       mockSession.isActive.and.callFake(function(){
         return true;
       });
       spyOn($state, "go");
       spyOn(mockEvent, "preventDefault");
       spyOn(mockPaymentService, "getDonor").and.callThrough();
       mockPaymentServiceGetPromise.setSuccess(false);
       mockPaymentServiceGetPromise.setHttpStatusCode(404);
       $scope.give = {
         email: "test@test.com"
       };

       controller.transitionForLoggedInUserBasedOnExistingDonor(mockEvent, mockToState);

       expect($state.go).toHaveBeenCalledWith("give.account");
       expect(mockEvent.preventDefault).toHaveBeenCalled();
       expect(mockPaymentService.getDonor).toHaveBeenCalledWith("test@test.com");
       expect(controller.donorError).toBeTruthy();
     });

     it('should transition to give.confirm for a logged-in Giver with an existing donor', function(){
       mockSession.isActive.and.callFake(function(){
         return true;
       });
       spyOn($state, "go");
       spyOn(mockEvent, "preventDefault");
       spyOn(mockPaymentService, "getDonor").and.callThrough();
       mockPaymentServiceGetPromise.setSuccess(true);
       $scope.give = {
         email: "test@test.com"
       };

       controller.transitionForLoggedInUserBasedOnExistingDonor(mockEvent, mockToState);

       expect($state.go).toHaveBeenCalledWith("give.confirm");
       expect(mockEvent.preventDefault).toHaveBeenCalled();
       expect(mockPaymentService.getDonor).toHaveBeenCalledWith("test@test.com");
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

       var mockEvent = {
       preventDefault : function(){}
       };

       var mockToState = {
       name : "give.account"
       };
       $scope.give = {
         email: "test@test.com"
       };

       mockSession.isActive.and.callFake(function(){
         return true;
       });
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

    it('should transition to give.login is session is not active', function() {
      mockSession.isActive.and.callFake(function(){
        return false;
      });

      spyOn($state, "go");
      controller.goToChange();

      expect($state.go).toHaveBeenCalledWith('give.login');
    });
  });

  describe('function donate', function() {
    var callback;

    beforeEach(function() {
      callback = jasmine.createSpyObj('stripe callback', ['onSuccess', 'onFailure']);
    });

    it('should call success callback if donation is successful', function() {

      spyOn(mockPaymentService, 'donateToProgram').and.callFake(function(programId, amount, donorId, email, pymtType) {
        var deferred = $q.defer();
        deferred.resolve({ amount: amount, });
        return deferred.promise;
      });

      controller.donate(1, 123, "2", "test@here.com", "cc", callback.onSuccess, callback.onFailure);
      // This resolves the promise above
      $rootScope.$apply();

      expect(mockPaymentService.donateToProgram).toHaveBeenCalledWith(1, 123, "2", "test@here.com", "cc");
      expect(callback.onSuccess).toHaveBeenCalled();
      expect(callback.onFailure).not.toHaveBeenCalled();
    });

    it('should not call success callback if donation fails', function() {

      spyOn(mockPaymentService, 'donateToProgram').and.callFake(function(programId, amount, donorId, email, pymtType) {
        var deferred = $q.defer();
        deferred.reject("Uh oh!");
        return deferred.promise;
      });

      controller.amount = undefined;
      controller.program = undefined;
      controller.program_name = undefined;

      controller.donate(1, 123, "2", "test@here.com", "bank", callback.onSuccess, callback.onFailure);
      // This resolves the promise above
      $rootScope.$apply();

      expect(callback.onFailure).toHaveBeenCalledWith('Uh oh!');
      expect(controller.amount).toBeUndefined();
      expect(controller.program).toBeUndefined();
      expect(controller.program_name).toBeUndefined();
      expect(callback.onSuccess).not.toHaveBeenCalled();
    });
  });

  describe('function processChange', function() {
    it('should transition to give.login is session is not active', function() {
      mockSession.isActive.and.callFake(function(){
        return false;
      });

      spyOn($state, "go");
      controller.processChange();

      expect($state.go).toHaveBeenCalledWith('give.login');
    });
  });
});
