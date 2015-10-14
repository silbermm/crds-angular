require('crds-core');
require('../../../../app/app');

describe('RecurringGivingList Directive', function() {
  var $compile;
  var $rootScope;
  var $httpBackend;
  var scope;
  var templateString;
  var originalRecurringGifts;

  beforeEach(angular.mock.module('crossroads.profile'));

  beforeEach(angular.mock.module(function($provide) {
    $provide.value('$state', {});
  }));

  beforeEach(
      inject(function(_$compile_, _$rootScope_, _$httpBackend_) {
        $compile = _$compile_;
        $rootScope = _$rootScope_;
        $httpBackend = _$httpBackend_;

        originalRecurringGifts = [
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
          },
          {
            amount: 5000,
            recurrence: 'Tuesdays Weekly',
            program: 'Crossroads',
            source: {
              type: 'CreditCard',
              brand: 'DinersClub',
              last4: '5000',
              expectedViewBox: '0 0 160 100',
              expectedName: 'ending in 5000',
              expectedIcon: ''
            }
          },
          {
            amount: 6000,
            recurrence: 'Mondays Weekly',
            program: 'Crossroads',
            source: {
              type: 'Bank',
              last4: '7001',
              expectedViewBox: '0 0 32 32',
              expectedName: 'ending in 7001',
              expectedIcon: 'library'
            }
          },
          {
            amount: 7000,
            recurrence: '1st Montly',
            program: 'Crossroads',
            source: {
              type: 'Bank',
              last4: '7000',
              expectedViewBox: '0 0 32 32',
              expectedName: 'ending in 7000',
              expectedIcon: 'library'
            }
          },
          {
            amount: 8000,
            recurrence: 'Sundays Weekly',
            program: 'Crossroads',
            source: {
              type: 'Bank',
              last4: '8000',
              expectedViewBox: '0 0 32 32',
              expectedName: 'ending in 8000',
              expectedIcon: 'library'
            }
          }
        ];

        scope = $rootScope.$new();
        scope.recurringGiftsInput = _.cloneDeep(originalRecurringGifts);

        templateString =
            '<recurring-giving-list ' +
            ' recurring-gifts-input="recurringGiftsInput"></donation-list>';
      })
  );

  describe('scope.$watch(recurringGiftsInput)', function() {
    var element;
    beforeEach(function() {
      element = $compile(angular.element(templateString))(scope);
      scope.$digest();
    });

    it('should call postProcessDonations and set data appropriately', function() {
      var isolateScope = element.isolateScope();
      expect(isolateScope.recurringGifts).toBeDefined();
      expect(isolateScope.recurringGifts.length).toEqual(originalRecurringGifts.length);

      // Verify that the input recurring gift were not modified
      expect(isolateScope.recurringGiftsInput).toEqual(originalRecurringGifts);

      // Using the "expected*" properties on the mocked recurring gifts, make sure the actual properties are set correctly
      _.forEach(isolateScope.recurringGifts, function(recurringGift) {
        expect(recurringGift.source.name).toEqual(recurringGift.source.expectedName);
        expect(recurringGift.source.icon).toEqual(recurringGift.source.expectedIcon);
        expect(recurringGift.source.viewBox).toEqual(recurringGift.source.expectedViewBox);
      });
    });

    it('should call postProcessDonations when recurringGiftsInput model changes', function() {
      originalRecurringGifts.push(
          {
            amount: 9000,
            recurrence: '10th Montly',
            program: 'Crossroads',
            source: {
              type: 'CreditCard',
              brand: 'Discover',
              last4: '9000',
              expectedViewBox: '0 0 160 100',
              expectedName: 'ending in 9000',
              expectedIcon: 'cc_discover'
            }
          }
      );
      scope.recurringGiftsInput = _.cloneDeep(originalRecurringGifts);

      var isolateScope = element.isolateScope();
      isolateScope.$apply();

      expect(isolateScope.recurringGifts).toBeDefined();
      expect(isolateScope.recurringGifts.length).toEqual(scope.recurringGiftsInput.length);

      // Using the "expected*" properties on the mocked recurring gifts, make sure the actual properties are set correctly
      _.forEach(isolateScope.recurringGifts, function(recurringGift) {
        expect(recurringGift.source.name).toEqual(recurringGift.source.expectedName);
        expect(recurringGift.source.icon).toEqual(recurringGift.source.expectedIcon);
        expect(recurringGift.source.viewBox).toEqual(recurringGift.source.expectedViewBox);
      });
    });
  });

});
