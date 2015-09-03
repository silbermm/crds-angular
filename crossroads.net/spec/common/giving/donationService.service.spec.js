require('crds-core');

describe('Donation Service', function() {

  var $rootScope;
  var GiveTransferService;
  var PaymentService;
  var GiveFlow;
  var $state;
  var CC_BRAND_CODES;
  var mockSession;
  var Session;
  var donationService;
  var PaymentService;  
 
  var fakeDonor = createFakeDonor();
   
  beforeEach(angular.mock.module('crossroads.give'));

  beforeEach(angular.mock.module(function($provide){
    mockSession= jasmine.createSpyObj('Session', ['exists', 'isActive']);
    mockSession.exists.and.callFake(function(something){
      return '12345678';
    });
    $provide.value('Session', mockSession);
    
    mockPaymentService = jasmine.createSpyObj('PaymentService', ['getDonor', 'updateDonorWithBankAccount', 'updateDonorWithCard', 'donateToProgram']);
    mockPaymentService.getDonor.and.callFake(function(n){
      return(createMockPaymentServiceGetPromise().$promise);
    });
    mockPaymentService.updateDonorWithBankAccount.and.callFake(function(){});
    mockPaymentService.updateDonorWithCard.and.callFake(function(){});
    mockPaymentService.donateToProgram.and.callFake(function(){});

    $provide.service('PaymentService' mockPaymentService); 
  }));

  beforeEach(inject(function(__GiveTransferService__, __PaymentService__, __GiveFlow__, __$state__, __CC_BRAND_CODES__, __DonationService__) {
    GiveTransferService = __GiveTransferService__;
    GiveFlow = __GiveFlow__;
    $state = __$state__;
    CC_BRAND_CODES = __CC_BRAND_CODES__;
    Session = mockSession;
    donationService = __DonationService__;
  })); 

  describe('function objects', function() {  
    it('should create a bank', function() {
      GiveTransferService.donor = fakeDonor;
      donationService.createBank();
      expect(donationService.bank).toBe({country: 'US', currency: 'USD', routing_number: fakeDonor.routing, account_number: fakeDonor.bank_account_number});  
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
     
      controller.donationService.donate({programId: 1, Name: 'Game Change'}, callback.onSuccess, callback.onFailure);
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

      controller.dto.amount = undefined;
      controller.dto.program = undefined;
      controller.dto.program_name = undefined;

      controller.donate({programId: 1, Name: 'Game Change'}, callback.onSuccess, callback.onFailure);
      // This resolves the promise above
      $rootScope.$apply();

      expect(callback.onFailure).toHaveBeenCalledWith('Uh oh!');
      expect(controller.amount).toBeUndefined();
      expect(controller.program).toBeUndefined();
      expect(controller.program_name).toBeUndefined();
      expect(callback.onSuccess).not.toHaveBeenCalled();
    });
  });



  function createFakeDonor(){
    return {
      default_source: {
        routing: 110000000,
        bank_account_number: 000123456789
      } 
    };
  }

  function createMockPaymentServiceGetPromise() {
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
    }


});
