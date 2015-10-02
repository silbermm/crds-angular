require('crds-core');
require('../../../app/common/common.module');
require('../../../app/app');

describe('Common Giving Donation Service', function() {
  var fixture;

  var Session;
  var PaymentService;
  var GiveTransferService;
  var GiveFlow;
  var $state;

  beforeEach(function() {
    angular.mock.module('crossroads', function($provide) {
      Session = {isActive: function() {return (true);}};

      $provide.value('Session', Session);
    });
  });

  beforeEach(function() {
    angular.mock.module('crossroads.common', function($provide) {
      GiveFlow = {account: 'give.account', confirm: 'give.confirm'};
      $provide.value('GiveFlow', GiveFlow);

      PaymentService = jasmine.createSpyObj('Mock Payment Service', ['getDonor']);
      $provide.value('PaymentService', PaymentService);

      $provide.constant('CC_BRAND_CODES', {Visa: '#cc_visa'});
    });
  });

  beforeEach(
      inject(function(DonationService, _GiveTransferService_, _$state_) {
        fixture = DonationService;
        GiveTransferService = _GiveTransferService_;
        $state = _$state_;
      })
  );

  describe('function transitionForLoggedInUserBasedOnExistingDonor', function() {
    var event;
    var toState;

    beforeEach(function() {
      event = jasmine.createSpyObj('event', ['preventDefault']);
      toState = {
        name: 'give.account'
      };
      GiveTransferService.donorError = false;
      GiveTransferService.processing = false;
      GiveTransferService.email = 'me@here.com';
    });

    it('should set transfer service card values for donor with default card', function() {
      var donor = {
        default_source: {
          credit_card: {
            last4: '9876',
            brand: 'Visa'
          }
        }
      };

      var getDonorPromise = {
        then: function(successCallback, errorCallback) {
          successCallback(donor);
        }
      }
      PaymentService.getDonor.and.returnValue(getDonorPromise);
      spyOn($state, 'go');

      fixture.transitionForLoggedInUserBasedOnExistingDonor(event, toState);

      expect(GiveTransferService.donorError).toBeFalsy();
      expect(GiveTransferService.processing).toBeTruthy();
      expect(event.preventDefault).toHaveBeenCalled();
      expect(PaymentService.getDonor).toHaveBeenCalledWith('me@here.com');
      expect($state.go).toHaveBeenCalledWith('give.confirm');
      expect(GiveTransferService.donor).toEqual(donor);
      expect(GiveTransferService.last4).toEqual('9876');
      expect(GiveTransferService.brand).toEqual('#cc_visa');
      expect(GiveTransferService.view).toEqual('cc');
    });

    it('should set transfer service bank values for donor with default bank account', function() {

    });
  });
});