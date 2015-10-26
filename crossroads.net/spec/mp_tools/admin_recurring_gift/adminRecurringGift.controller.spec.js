require('crds-core');
require('../../../app/common/common.module');
require('../../../app/app');

describe('AdminRecurringGift', function() {

  var vm;
  var rootScope;
  var scope;
  var DonationService;
  var GiveTransferService;
  var RecurringGiving;
  var donation;
  var programList;
  var httpBackend;

  var mockProgramList = [
    {
      ProgramId: 1,
      Name: 'Crossroads'
    },
    {
      ProgramId: 2,
      Name: 'Beans & Rice'
    }
  ];

  var mockRecurringGift =
  {
    recurring_gift_id: 12,
    donor_id: 123,
    amount: 1000,
    recurrence: '8th Monthly',
    interval: 'month',
    program: 1,
    source:
    {
      type: 'CreditCard',
      brand: 'Visa',
      last4: '1000',
      icon: 'cc_visa',
      address_zip: '41983',
      exp_date: '2029-08-01T00:00:00',
      expectedViewBox: '0 0 160 100',
      expectedBrand: '#cc_visa',
      expectedCCNumberClass: 'cc_visa',
    },
  };

  beforeEach(angular.mock.module('crossroads'));

  beforeEach(angular.mock.module(function($provide) {
    $provide.value('$state', { get: function() {} });
  }));

  describe('When Admin performing an update', function() {
    beforeEach(inject(function(_$controller_, $injector) {
      httpBackend = $injector.get('$httpBackend');
      rootScope = $injector.get('$rootScope');

      scope = rootScope.$new();
      DonationService = $injector.get('DonationService');
      GiveTransferService = $injector.get('GiveTransferService');
      RecurringGiving = $injector.get('RecurringGiving');

      GiveTransferService.impersonateDonorId = 12;
      spyOn(rootScope, '$emit').and.callFake(function() {});

      vm = _$controller_('AdminRecurringGiftController',
                             {$rootScope: rootScope,
                               DonationService: DonationService,
                               GiveTransferService: GiveTransferService,
                               donation: mockRecurringGift,
                               programList: mockProgramList});
    }));

    describe('On initialization', function() {
      it('should populate the dto with the correct information', function() {
        expect(vm.impersonateDonorId).toBe(12);
        expect(vm.dto.amount).toBe(mockRecurringGift.amount);
        expect(vm.dto.amountSubmitted).toBeFalsy();
        expect(vm.dto.bankinfoSubmitted).toBeFalsy();
        expect(vm.dto.changeAccountInfo).toBeTruthy();
        expect(vm.dto.brand).toBe(mockRecurringGift.source.expectedBrand);
        expect(vm.dto.ccNumberClass).toBe(mockRecurringGift.source.expectedCCNumberClass);
        expect(vm.dto.donor.id).toBe(12);
        expect(vm.dto.last4).toBe(mockRecurringGift.source.last4);
        expect(vm.dto.givingType).toBe(mockRecurringGift.interval);
        expect(vm.dto.initialized).toBeTruthy();
        expect(vm.dto.program).toBe(mockProgramList[0]);
        expect(vm.dto.view).toBe('cc');
        expect(vm.dto.interval).toBe('Monthly');
        expect(vm.dto.donor.default_source.credit_card.last4).toBe(mockRecurringGift.source.last4);
        expect(vm.dto.donor.default_source.credit_card.brand).toBe(mockRecurringGift.source.brand);
        expect(vm.dto.donor.default_source.credit_card.address_zip).toBe(mockRecurringGift.source.address_zip);
        expect(vm.dto.donor.default_source.credit_card.exp_date).toBe('0829');
      });

    });

    var recurringGiveForm;
    describe('On actual edit', function() {
      beforeEach(function() {
        recurringGiveForm = {
          donationDetailsForm: {
            amount: {
              $valid: true,
            },
            recurringStartDate: {
              $dirty: false,
            },
            $dirty: undefined,
          },
        };
      });

      it('should call the emit on rootScope when edit is called', function() {
        httpBackend.expectPUT(window.__env__['CRDS_API_ENDPOINT'] + 'api/donor/recurrence/12?impersonateDonorId=12').respond(200);
        vm.update(recurringGiveForm);

        expect(rootScope.$emit).toHaveBeenCalled();
      });

      it('should call the emit on rootScope when edit is called', function() {
        httpBackend.expectPUT(window.__env__['CRDS_API_ENDPOINT'] + 'api/donor/recurrence/12?impersonateDonorId=12').respond(404);
        vm.update(recurringGiveForm);

        expect(rootScope.$emit).toHaveBeenCalled();
      });
    });
  });

  describe('When Admin performing a create', function() {
    beforeEach(inject(function(_$controller_, $injector) {
      httpBackend = $injector.get('$httpBackend');
      rootScope = $injector.get('$rootScope');

      scope = rootScope.$new();
      DonationService = $injector.get('DonationService');
      GiveTransferService = $injector.get('GiveTransferService');
      RecurringGiving = $injector.get('RecurringGiving');

      GiveTransferService.impersonateDonorId = 12;
      spyOn(rootScope, '$emit').and.callFake(function() {});

      var mockDonation = null;

      vm = _$controller_('AdminRecurringGiftController',
                             {$rootScope: rootScope,
                               DonationService: DonationService,
                               GiveTransferService: GiveTransferService,
                               donation: mockDonation,
                               programList: mockProgramList});
    }));

    describe('On initialization', function() {
      it('should populate the dto with the correct information', function() {
        expect(vm.impersonateDonorId).toBe(12);
        expect(vm.dto.amount).toBe(undefined);
        expect(vm.dto.amountSubmitted).toBeFalsy();
        expect(vm.dto.bankinfoSubmitted).toBeFalsy();
        expect(vm.dto.changeAccountInfo).toBeTruthy();
        expect(vm.dto.brand).toBe('');
        expect(vm.dto.ccNumberClass).toBe('');
        expect(vm.dto.donor.id).toBe(12);
        expect(vm.dto.last4).toBe('');
        expect(vm.dto.givingType).toBe(undefined);
        expect(vm.dto.initialized).toBeTruthy();
        expect(vm.dto.program).toBe(undefined);
        expect(vm.dto.view).toBe('bank');
        expect(vm.dto.interval).toBe(undefined);
      });

    });
  });
});
