require('crds-core');
require('../../../app/ang');

require('../../../app/common/common.module');
require('../../../app/app');

describe('AdminRecurringGift', function() {

  var $httpBackend;
  var scope;
  var $log;
  var $modal;
  var rootScope;
  var DonationService;
  var GiveTransferService = {};
  var vm;

  var mockRecurringGiftsResponse = [
    {
      amount: 1000,
      recurrence: 'Fridays Weekly',
      program: 'Crossroads',
      source:
      {
        type: 'CreditCard',
        brand: 'Visa',
        last4: '1000',
        expectedViewBox: '0 0 160 100',
        expectedName: 'ending in 1000',
        expectedIcon: 'cc_visa'
      }
    },
    {
      amount: 2000,
      recurrence: '8th Monthly',
      program: 'Crossroads',
      source: {
        type: 'CreditCard',
        brand: 'MasterCard',
        last4: '2000',
        expectedViewBox: '0 0 160 100',
        expectedName: 'ending in 2000',
        expectedIcon: 'cc_mastercard'
      }
    },
    {
      amount: 3000,
      recurrence: '30th Monthly',
      program: 'Crossroads',
      source: {
        type: 'CreditCard',
        brand: 'AmericanExpress',
        last4: '3000',
        expectedViewBox: '0 0 160 100',
        expectedName: 'ending in 3000',
        expectedIcon: 'cc_american_express'
      }
    },
    {
      amount: 4000,
      recurrence: '21st Monthly',
      program: 'Crossroads',
      source: {
        type: 'CreditCard',
        brand: 'Discover',
        last4: '4000',
        expectedViewBox: '0 0 160 100',
        expectedName: 'ending in 4000',
        expectedIcon: 'cc_discover'
      }
    }
  ];

  beforeEach(angular.mock.module('crossroads'));

  beforeEach(angular.mock.module(function($provide) {
    $modal = {
      open: function() {
      },

      name: 'test modal',
    };

    spyOn($modal, 'open').and.callFake(function() {
      return {
        result: {
          then: function(confirmCallback, cancelCallback) {
            //Store the callbacks for later when the user clicks on the OK or Cancel button of the dialog
            this.confirmCallBack = confirmCallback;
            this.cancelCallback = cancelCallback;
          }
        },
        close: function(item) {
          //The user clicked OK on the modal dialog, call the stored confirm callback with the selected item
          this.result.confirmCallBack(item);
        },

        dismiss: function(type) {
          //The user clicked cancel on the modal dialog, call the stored cancel callback
          this.result.cancelCallback(type);
        }
      };
    });

    $provide.value('$modal', $modal);
    $provide.value('$state', { get: function() {} });
  }));

  beforeEach(inject(function($injector, _$controller_) {
    var $rootScope = $injector.get('$rootScope');
    scope = $rootScope.$new();
    $httpBackend = $injector.get('$httpBackend');
    $log = $injector.get('$log');
    rootScope = $injector.get('$rootScope');
    DonationService = $injector.get('DonationService');
    GiveTransferService = $injector.get('GiveTransferService');

    scope = rootScope.$new();
    GiveTransferService.impersonateDonorId = 12;

    vm = _$controller_('AdminRecurringGiftController',
                           {$log: $log,
                             $modal: $modal,
                             $rootScope: rootScope,
                             DonationService: DonationService,
                             GiveTransferService: GiveTransferService});
  })
  );

  afterEach(function() {
    $httpBackend.verifyNoOutstandingExpectation();
    $httpBackend.verifyNoOutstandingRequest();
  });

  describe('On initialization', function() {
    it('should set impersonation error when user not allowed to impersonate', function() {
      var error = {};
      $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donor/recurrence?impersonateDonorId=12')
        .respond(403, error);
      $httpBackend.flush();

      expect(vm.impersonationError).toBeTruthy();
      expect(vm.impersonationErrorMessage).toEqual('User is not allowed to impersonate');
    });

    it('should set impersonation error when user to impersonate is not found', function() {
      var error = {};
      $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donor/recurrence?impersonateDonorId=12')
        .respond(409, error);
      $httpBackend.flush();

      expect(vm.impersonationError).toBeTruthy();
      expect(vm.impersonationErrorMessage).toEqual('Could not find user to impersonate');
    });

    it('should retrieve recurring gifts for impersonated user', function() {
      $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donor/recurrence?impersonateDonorId=12')
                             .respond(mockRecurringGiftsResponse);
      $httpBackend.flush();

      expect(vm.impersonateDonorId).toBe(12);
      expect(vm.recurring_giving).toBeTruthy();
      expect(vm.recurring_giving_view_ready).toBeTruthy();
      expect(vm.recurring_gifts.length).toBe(4);
      expect(vm.recurring_gifts[0].recurrence).toEqual('Fridays Weekly');
      expect(vm.recurring_gifts[1].recurrence).toEqual('8th Monthly');
      expect(vm.recurring_gifts[2].recurrence).toEqual('30th Monthly');
      expect(vm.recurring_gifts[3].recurrence).toEqual('21st Monthly');
    });

    it('should not have recurring gifts if there are no recurring gifts', function() {
      $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donor/recurrence?impersonateDonorId=12')
        .respond(404, {});
      $httpBackend.flush();

      expect(vm.impersonateDonorId).toBe(12);
      expect(vm.recurring_giving).toBeFalsy();
      expect(vm.recurring_giving_view_ready).toBeTruthy();
      expect(vm.recurring_gifts.length).toBe(0);
    });
  });

  describe('scope.openEditGiftModal(selectedDonation)', function() {
    beforeEach(function() {
      vm.openCreateGiftModal();
      $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donor/recurrence?impersonateDonorId=12')
                             .respond(mockRecurringGiftsResponse);
      $httpBackend.flush();

    });

    it('should open a modal', function() {
      expect($modal.open).toHaveBeenCalled();
    });
  });
});
