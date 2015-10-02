require('crds-core');
require('../../../app/common/common.module');
require('../../../app/profile/profile.module');
require('../../../app/app');

describe('ProfileGivingController', function() {

  var httpBackend;
  var scope;
  var controllerConstructor;
  var sut;

  beforeEach(angular.mock.module('crossroads'));

  beforeEach(angular.mock.module(function($provide) {
    $provide.value('$state', {});
  }));

  var mockDonationResponse =
  {
    donations: [
      {
        amount: 78900,
        status: 'Succeeded',
        date: '2015-05-14T09:52:44.873',
        distributions: [
          {
            program_name: 'Crossroads',
            amount: 78900
          }
        ],
        source:
        {
          type: 'CreditCard',
          last4: '1234',
          brand: 'Visa',
          payment_processor_id: 'ch_16jvzDEldv5NE53smr7Mwi4x'
        }
      },
      {
        amount: 1200,
        status: 'Succeeded',
        date: '2015-05-21T10:25:32.923',
        distributions: [
          {
            program_name: 'Game Change',
            amount: 1200
          }
        ],
        source:
        {
          type: 'Bank',
          last4: '4321',
          payment_processor_id: 'py_16YS8wEldv5NE53s770upiHw'
        },
      },
      {
        amount: 100,
        status: 'Deposited',
        date: '2015-03-28T10:25:32.923',
        distributions: [
          {
            program_name: 'Beans & Rice',
            amount: 100
          }
        ],
        source:
        {
          type: 'Cash',
          name: 'Cash'
        }
      }
    ],
    donation_total_amount: 80000,
    beginning_donation_date: '123',
    ending_donation_date: '456'
  };

  beforeEach(inject(function(_$injector_, $httpBackend, _$controller_, $rootScope) {
      var $injector = _$injector_;

      httpBackend = $httpBackend;

      controllerConstructor = _$controller_;

      scope = $rootScope.$new();
    })
  );

  afterEach(function() {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });

  describe('On initialization', function() {
    beforeEach(function() {
      sut = controllerConstructor('ProfileGivingController', {$scope: scope});
    });

    it('should retrieve most recent giving year donations for current user', function() {
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donations?limit=3')
                             .respond(mockDonationResponse);
      httpBackend.flush();

      expect(sut.donation_history).toBeTruthy();
      expect(sut.donation_view_ready).toBeTruthy();

      expect(sut.donations.length).toBe(3);
      expect(sut.donations[0].distributions[0].program_name).toEqual('Crossroads');
      expect(sut.donations[1].distributions[0].program_name).toEqual('Game Change');
      expect(sut.donations[2].distributions[0].program_name).toEqual('Beans & Rice');
    });

    it('should not have history if there are no donations', function() {
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donations?limit=3').respond(404, {});
      httpBackend.flush();

      expect(sut.donation_history).toBeFalsy();
      expect(sut.donation_view_ready).toBeTruthy();
      expect(sut.donations.length).toBe(0);
    });
  });
});
