require('crds-core');
require('../../../app/common/common.module');
require('../../../app/app');

describe('RecurringGiving Service', function() {
  var fixture;
  var GiveTransferService;
  var DonationService;
  var GiveFlow;
  var Session;
  var $state;
  var $filter;
  var $rootScope;

  beforeEach(angular.mock.module('crossroads'));

  beforeEach(angular.mock.module(function($provide) {
    DonationService = {
      success: true,
      setSuccess: function(b) {
        this.success = b;
      },

      getSuccess: function() {
        return (this.success);
      },

      adminCreateRecurringGift: function(donorId) {
        var vm = this;
        return ({
          then: function(success, failure) {
            if (vm.getSuccess()) {
              success();
            } else {
              failure();
            }
          }
        });
      }
    };

    $state = { get: function() {} };

    $provide.value('DonationService', DonationService);
    $provide.value('$state', $state);
  }));

  beforeEach(inject(function(_RecurringGiving_, _GiveTransferService_) {
    fixture = _RecurringGiving_;
    GiveTransferService = _GiveTransferService_;
  }));

  describe('Function createGift', function() {
    var callback;
    var recurringGiveForm;

    beforeEach(function() {
      GiveTransferService.processing = false;
      GiveTransferService.amountSubmitted = false;

      callback = jasmine.createSpyObj('callback', ['onSuccess', 'onFailure']);
      recurringGiveForm = {};
    });

    it('should set amountSubmitted to true', function() {
      fixture.createGift(recurringGiveForm, callback.onSuccess, callback.onFailure, null);
      expect(GiveTransferService.processing).toBeTruthy();
      expect(GiveTransferService.amountSubmitted).toBeTruthy();
      expect(callback.onSuccess).toHaveBeenCalled();
      expect(callback.onFailure).not.toHaveBeenCalled();
    });
  });

  describe('Function updateGift', function() {
    var callback;
    var recurringGiveForm;

    beforeEach(function() {
      GiveTransferService.processing = false;
      GiveTransferService.amountSubmitted = false;

      callback = jasmine.createSpyObj('callback', ['onSuccess', 'onFailure']);
      recurringGiveForm = {
        donationDetailsForm: {
          $dirty: false,
          amount: {
            $valid: true
          },
          recurringStartDate: {
            $dirty: true,
            $valid: true
          }
        }
      };
    });

    it('should set amountSubmitted to true', function() {
      fixture.updateGift(recurringGiveForm, callback.onSuccess, callback.onFailure, null);
      expect(GiveTransferService.processing).toBeTruthy();
      expect(GiveTransferService.amountSubmitted).toBeTruthy();
      expect(callback.onSuccess).toHaveBeenCalled();
      expect(callback.onFailure).not.toHaveBeenCalled();
    });
  });
});