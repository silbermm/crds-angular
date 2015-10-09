require('crds-core');
require('../../../app/common/common.module');
require('../../../app/trips/trips.module');

var tripHelpers = require('../trips.helpers');

describe('GOTrip Signup Application Controller', function() {
  var controller;

  beforeEach(angular.mock.module('crossroads.trips'));

  beforeEach(angular.mock.module(function($provide) {
    // $provide.value('TripParticipant', tripHelpers.TripParticipant);
    $provide.value('Campaign', tripHelpers.Campaign);
    $provide.value('WorkTeams', tripHelpers.WorkTeams);
    $provide.value('contactId', 123456);
    $provide.value('Person', tripHelpers.Person);
    $provide.value('$state', { go: function() {} });

    mockSession = jasmine.createSpyObj('Session', ['exists', 'isActive', 'removeRedirectRoute', 'addRedirectRoute']);
    mockSession.exists.and.callFake(function(something) {
      return '12345678';
    });

    $provide.value('Session', mockSession);
  }));

  beforeEach(inject(function(_$rootScope_, _$controller_) {
    //Simulating isolate scope variables from the directive
    // var data = {
    //   volunteer: mockVolunteer,
    //   contactId: mockContactId,
    //   pageInfo: mockPageInfo,
    //   responseId: mockResponseId,
    //   showSuccess: true
    // };
    // $provide.value('Campaign', tripHelpers.Campaign);
    var scope = _$rootScope_.$new();
    controller = _$controller_('TripsSignupController', {$scope: scope});
  }));

  it('should require Nicaragua fields when destination is Nicaragua', function() {
    controller.destination = 'Nicaragua';
    expect(controller.nicaRequired()).toBe(true);
  });
});
