require('crds-core');
require('../../../app/app');

describe('Signup To Serve Tool', function(){

  var expectedReturn = {
    groupId: 23,
    groupName: 'Kids Club Nursery',
    groupParticipants: [
     {
      contactId: 23456,
      firstname: 'Matt',
      lastname: 'Silbernagel',
      nickname: 'Matt',
     },
     {
      contactId: 23457,
      firstname: 'Andy',
      lastname: 'Canterbury',
      nickname: 'Andy',
     }]
  };

  var singleParticipant = [{
    'participantId':4218214,
    'contactId':768379,
    'nickname':'Tony',
    'lastname':'Maddox',
    'groupRoleId':22,
    'groupRoleTitle':'Leader'
  }];

  var expectedSingleRSVP = {
    'contactId':768379,
    'opportunityId':'2923',
    'opportunityIds': ['2923'],
    'eventTypeId':142,
    'endDate':'1430452800',
    'startDate':'1430452800',
    'signUp':true,
    'alternateWeeks':false
  };

  var multiParticipant = [
  { participantId:4218214,
    contactId: 768379,
    nickname: 'Tony',
    lastname: 'Maddox',
    groupRoleId: 22,
    groupRoleTitle: 'Leader'
  },
  {
    participantId: 2346790,
    contactId:23457890,
    nickname: 'Andy',
    lastname:'Canterbury',
    groupRoleId:22,
    groupRoleTitle :'Leader'
  }];

  var expectedMultiRSVP1 = {
    'alternateWeeks':false,
    'contactId':768379,
    'endDate':'1430452800',
    'eventTypeId':142,
    'opportunityId':'2923',
    'opportunityIds': ['2923'],
    'signUp':true,
    'startDate':'1430452800'
  };

  var expectedMultiRSVP2 = {
    'alternateWeeks':false,
    'contactId':23457890,
    'endDate':'1430452800',
    'eventTypeId':142,
    'opportunityId':'2923',
    'opportunityIds': ['2923'],
    'signUp':true,
    'startDate':'1430452800'
  };

  var expectedDates = [1430382600,1430469000,1430555400,1430641800];

  beforeEach(angular.mock.module('crossroads'));

  beforeEach(angular.mock.module(function($provide) {
    $provide.value('$state', { get: function() {} });
  }));


  beforeEach(inject(function(_$location_){
    var $location = _$location_;
    spyOn($location, 'search').and.returnValue({
      dg:'8b6242c9-ea32-40f7-97a2-e2bb3524ced2',
      'ug':'c29e64a5-820b-461f-a57c-5831d070d578',
      pageID:'292',
      recordID:'2923',
      recordDescription: undefined,
      s:'11467',
      sc:'1',
      p:0,
      v:387
    });
  }));

  var $controller, $log, mockSu2sResource, mockServeResource, $httpBackend, MPTools, $window;

  beforeEach(inject(function(_$controller_, _$log_, _MPTools_, _$window_, $injector){
    $controller = _$controller_;
    $log = _$log_;
    $window = _$window_;
    MPTools = _MPTools_;
    $httpBackend = $injector.get('$httpBackend');
    mockSu2sResource = $injector.get('Su2sData');
    mockServeResource = $injector.get('ServeOpportunities');
  }));

  describe('Signup To Serve Controller', function(){

    var $scope, controller;
    beforeEach(function(){
      $scope = {};
      controller = $controller('SignupToServeController', { $scope: $scope });
      $httpBackend.expectGET( window.__env__['CRDS_API_ENDPOINT'] + 'api/opportunity/getGroupParticipantsForOpportunity/2923').respond(expectedReturn);
      $httpBackend.expectGET( window.__env__['CRDS_API_ENDPOINT'] + 'api/opportunity/getAllOpportunityDates/2923').respond(expectedDates);
    });

    describe('Initial Load', function(){
      it('should get the correct query parameters', function(){
        expect(controller.params.userGuid).toBe('c29e64a5-820b-461f-a57c-5831d070d578');
      });

      it('should get a list of participants', function(){

        $httpBackend.flush();
        expect(controller.group.groupId).toBe(expectedReturn.groupId);
        expect(controller.group.groupName).toBe(expectedReturn.groupName);
        expect(controller.group.groupParticipants.length).toBe(2);
      });

      it('should show the error message', function(){
        expect(controller.showError()).toBe(true);
      });
    });

    describe('Save RSVP', function() {
      it('should save RSVP for one participant', function(){

        $httpBackend.flush();
        $httpBackend.expectPOST(
          window.__env__['CRDS_API_ENDPOINT'] + 'api/serve/save-rsvp', expectedSingleRSVP
        ).respond(201, '');
        controller.group.eventTypeId = 142;
        controller.participants = singleParticipant;
        controller.selectedFrequency = {'value':0,'text':'Once'};
        controller.selectedEvent = '5/1/2015';
        controller.attending = true;
        controller.saveRsvp(true);
        $httpBackend.flush(true);
      });

      it('should save RSVP for multiple participants', function(){

        $httpBackend.flush();
        $httpBackend.expectPOST( window.__env__['CRDS_API_ENDPOINT'] + 'api/serve/save-rsvp', expectedMultiRSVP1).respond(201, '');
        $httpBackend.expectPOST( window.__env__['CRDS_API_ENDPOINT'] + 'api/serve/save-rsvp', expectedMultiRSVP2).respond(201, '');
        controller.group.eventTypeId = 142;
        controller.participants = multiParticipant;
        controller.selectedFrequency = {'value':0,'text':'Once'};
        controller.selectedEvent = '5/1/2015';
        controller.attending = true;
        controller.saveRsvp(true);
        $httpBackend.flush();
      });
    });

  });
});
