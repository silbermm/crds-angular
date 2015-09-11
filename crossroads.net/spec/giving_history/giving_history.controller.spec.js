require('crds-core');
require('../../app/app');
require('../../app/giving_history');

describe('GivingHistoryController', function() {

  var httpBackend;
  var scope;
  var controllerConstructor;
  var sut;

  beforeEach(angular.mock.module('crossroads'));

  var mockDonationYearsResponse =
  {
    years: ['2015', '2014'],
    most_recent_giving_year: '2015'
  };

  var mockDonationResponse =
  {
    donations: [
      {
        amount: 78900,
        donation_id: '17588628',
        status: 'Succeeded',
        donation_date: '2015-05-14T09:52:44.873',
        distributions: [
          {
            program_name: 'Crossroads',
            amount: 78900
          }
        ],
        source_type: 'CreditCard',
        source_type_description: 'ending in 1234',
        card_type: 'Visa'
      },
      {
        amount: 10000,
        donation_id: '17599999',
        status: 'Succeeded',
        donation_date: '2015-05-21T10:25:32.923',
        distributions: [
          {
            program_name: 'Game Change',
            amount: 10000
          }
        ],
        source_type: 'CreditCard',
        source_type_description: 'ending in 2345',
        card_type: 'MasterCard'
      }
    ],
    donation_total_amount: 88900
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
      sut = controllerConstructor('GivingHistoryController', {$scope: scope});

      httpBackend.whenGET(/SiteConfig*/).respond('');

    });

    it('should retrieve most recent giving year donations for current user', function() {
      httpBackend.whenGET(/SiteConfig*/).respond('');
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donations/years')
                             .respond(mockDonationYearsResponse);
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donations?donationYear=2015')
                             .respond(mockDonationResponse);
      httpBackend.flush();

      expect(sut.donation_years.length).toBe(2);
      expect(sut.donation_years[0]).toEqual('2015');
      expect(sut.most_recent_giving_year).toEqual('2015');

      expect(sut.donations.length).toBe(2);
      expect(sut.donations[0].distributions[0].program_name).toEqual('Crossroads');
      expect(sut.donation_total_amount).toEqual(88900);
    });
  });
});
