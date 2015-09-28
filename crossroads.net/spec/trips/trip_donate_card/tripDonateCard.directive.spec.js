require('crds-core');
require('../../../app/common/common.module');
require('../../../app/trips/trips.module');

(function() {
  'use strict';

  var tripHelpers = require('../trips.helpers');
  var $compile;
  var $rootScope;
  var $httpBackend;
  var scope;
  var element;
  var isolateScope;
  var tripDonations;

  describe('Trip Donate Card Directive', function() {

    beforeEach(angular.mock.module('crossroads.trips'));

    beforeEach(angular.mock.module(function($provide) {
      $provide.value('$state', {});
    }));

    beforeEach(inject(function(_$compile_, _$rootScope_, _$httpBackend_) {
      $compile = _$compile_;
      $rootScope = _$rootScope_;
      $httpBackend = _$httpBackend_;
      $httpBackend.whenGET(/SiteConfig*/).respond('');
      scope = $rootScope.$new();
      element = '<trip-donations donation=\'donation\'></trip-donations>';
      scope.donation = tripHelpers.MyTrips[0];
      element = $compile(element)(scope);
      scope.$digest();
      tripDonations = element.isolateScope().tripDonations;
    }));

    it('should have the donation information passed in', function() {
      expect(tripDonations.donation).toEqual(tripHelpers.MyTrips[0]);
    });

    it('should toggle the message', function() {
      expect(tripDonations.isMessageToggled).toBe(false);
      tripDonations.toggleMessage();
      expect(tripDonations.isMessageToggled).toBe(true);
    });

  });
})();
