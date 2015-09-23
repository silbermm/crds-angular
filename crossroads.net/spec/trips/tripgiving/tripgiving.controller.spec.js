require('crds-core');
require('../../../app/common/common.module');
require('../../../app/trips/trips.module');

var tripHelpers = require('../trips.helpers');

(function() {
  'use strict';

  describe('Trip Giving Controller', function() {

    beforeEach(angular.mock.module('crossroads.trips'));

    beforeEach(angular.mock.module(function($provide) {
      $provide.value('TripParticipant', tripHelpers.TripParticipant);
      $provide.value('$state', { go: function() {} });
      mockSession = jasmine.createSpyObj('Session', ['exists', 'isActive', 'removeRedirectRoute', 'addRedirectRoute']);
      mockSession.exists.and.callFake(function(something){
        return '12345678';
      });
      $provide.value('Session', mockSession);
    }));

    var $controller;
    var $scope;
    var $log;
    var TripParticipant;
    var $httpBackend;
    var Session;
    var DonationService;
    var GiveTransferService;
    var GiveFlow;
    var TripGiving;
    var $state;
    var Session;
    var $rootScope;
    var AUTH_EVENTS;

    beforeEach(inject(function(_$controller_, _$log_, $injector) {
      $rootScope = $injector.get('$rootScope'); 
      $scope = $rootScope.$new();
      $controller = _$controller_('TripGivingController', {$scope: $scope});
      $log = _$log_;
      Session = $injector.get('Session');
      $state = $injector.get('$state');
      $httpBackend = $injector.get('$httpBackend');
      TripParticipant = $injector.get('TripParticipant');
      DonationService = $injector.get('DonationService');
      GiveTransferService = $injector.get('GiveTransferService');
      AUTH_EVENTS = $injector.get('AUTH_EVENTS');
      GiveFlow = $injector.get('GiveFlow');
      TripGiving = $injector.get('TripGiving');

      $controller.initDefaultState();
    }));

    it('should set the program correctly', function() {
      expect($controller.dto.program)
        .toEqual( {
          ProgramId: tripHelpers.Trip.programId, 
          Name: tripHelpers.Trip.programName
        });
    });
    
    it('should not show the give button', function() {
      expect($controller.tripParticipant.showGiveButton).toBe(false);       
    });

    it('should show the share buttons', function() {
      expect($controller.tripParticipant.showShareButtons).toBe(true);
    });

    it('should not be processing', function() {
      expect($controller.dto.processing).toBe(false);
    });
    
  });

})();
