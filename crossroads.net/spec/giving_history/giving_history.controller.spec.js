require('crds-core');
require('../../app/app');

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
        date: '2015-05-28T10:25:32.923',
        distributions: [
          {
            program_name: 'Crossroads',
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

  var mockSoftCreditDonationResponse =
  {
    donations: [
      {
        amount: 2000,
        status: 'Succeeded',
        date: '2015-03-31T09:52:44.873',
        distributions: [
          {
            program_name: 'Crossroads',
            amount: 2000
          }
        ],
        source:
        {
          type: 'SoftCredit',
          name: 'Fidelity Charity Account'
        }
      },
      {
        amount: 2000,
        status: 'Succeeded',
        date: '2015-06-30T10:25:32.923',
        distributions: [
          {
            program_name: 'Crossroads',
            amount: 2000
          }
        ],
        source:
        {
          type: 'SoftCredit',
          name: 'Fidelity Charity Account'
        }
      }
    ],
    donation_total_amount: 4000
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

      httpBackend.whenGET(/SiteConfig*/).respond({siteConfig: {title:'Crossroads'}});
      httpBackend.whenGET(/api\/Page*/).respond({ pages: [{}] });
    });

    it('should retrieve most recent giving year donations for current user', function() {
      var profile = {foo: 'bar'};
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/profile').respond(profile);
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donations/years')
                             .respond(mockDonationYearsResponse);
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donations/2015')
                             .respond(mockDonationResponse);
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donations/2015?softCredit=true')
                             .respond(mockSoftCreditDonationResponse);
      httpBackend.flush();

      expect(sut.donation_years.length).toBe(3);
      expect(sut.donation_years[0]).toEqual({key: '2015', value: '2015'});
      expect(sut.donation_years[2]).toEqual({key: '', value: 'All'});
      expect(sut.selected_giving_year).toEqual({key: '2015', value: '2015'});
      expect(sut.profile.foo).toEqual('bar');
      expect(sut.beginning_donation_date).toEqual(mockDonationResponse.beginning_donation_date);
      expect(sut.ending_donation_date).toEqual(mockDonationResponse.ending_donation_date);
      expect(sut.donation_history).toBeTruthy();
      expect(sut.donation_view_ready).toBeTruthy();
      expect(sut.soft_credit_donation_history).toBeTruthy();
      expect(sut.soft_credit_donation_view_ready).toBeTruthy();
      expect(sut.overall_view_ready).toBeTruthy();

      expect(sut.donations.length).toBe(3);
      expect(sut.donations[0].distributions[0].program_name).toEqual('Crossroads');
      expect(sut.donation_total_amount).toEqual(80000);
    });

    it('should not have history if there is no profile', function() {
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/profile').respond(404, {});
      httpBackend.flush();

      expect(Object.keys(sut.profile).length).toEqual(0);

      expect(sut.donation_years.length).toBe(0);
      expect(sut.selected_giving_year).not.toBeDefined();
      expect(sut.beginning_donation_date).not.toBeDefined();
      expect(sut.ending_donation_date).not.toBeDefined();
      expect(sut.donation_history).toBeFalsy();
      expect(sut.donation_view_ready).toBeFalsy();
      expect(sut.soft_credit_donation_history).toBeFalsy();
      expect(sut.soft_credit_donation_view_ready).toBeFalsy();
      expect(sut.overall_view_ready).toBeTruthy();

      expect(sut.donations.length).toBe(0);
    });

    it('should not have history if there are no giving years', function() {
      var profile = {foo: 'bar'};
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/profile').respond(profile);
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donations/years').respond(404, {});
      httpBackend.flush();

      expect(sut.profile.foo).toEqual('bar');

      expect(sut.donation_years.length).toBe(0);
      expect(sut.selected_giving_year).not.toBeDefined();
      expect(sut.beginning_donation_date).not.toBeDefined();
      expect(sut.ending_donation_date).not.toBeDefined();
      expect(sut.donation_history).toBeFalsy();
      expect(sut.donation_view_ready).toBeFalsy();
      expect(sut.soft_credit_donation_history).toBeFalsy();
      expect(sut.soft_credit_donation_view_ready).toBeFalsy();
      expect(sut.overall_view_ready).toBeTruthy();

      expect(sut.donations.length).toBe(0);
    });

    it('should not have history if there are no donations', function() {
      var profile = {foo: 'bar'};
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/profile').respond(profile);
      httpBackend
          .expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donations/years')
          .respond(mockDonationYearsResponse);
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donations/2015').respond(404, {});
      httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/donations/2015?softCredit=true').respond(404, {});
      httpBackend.flush();

      expect(sut.profile.foo).toEqual('bar');

      expect(sut.donation_years.length).toBe(3);
      expect(sut.donation_years[0]).toEqual({key: '2015', value: '2015'});
      expect(sut.donation_years[2]).toEqual({key: '', value: 'All'});
      expect(sut.selected_giving_year).toEqual({key: '2015', value: '2015'});
      expect(sut.beginning_donation_date).not.toBeDefined();
      expect(sut.ending_donation_date).not.toBeDefined();

      expect(sut.donation_history).toBeFalsy();
      expect(sut.donation_view_ready).toBeTruthy();
      expect(sut.soft_credit_donation_history).toBeFalsy();
      expect(sut.soft_credit_donation_view_ready).toBeTruthy();
      expect(sut.overall_view_ready).toBeTruthy();

      expect(sut.donations.length).toBe(0);
    });
  });
});
