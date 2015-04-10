var $compile, $rootScope, element, scope, mockSession, mockServeDate, $httpBackend;

var mockOpp = {"name": "NuseryA", "roleId": "145"};
var mockTeam = [{ "name" : "Kids Club Nusery", "eventTypeId": 100, "members" : [ { "name": "John", "contactId" : 12345678, "serveRsvp": {roleId: 145, attending: true}, "roles" : [ mockOpp, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ] }, { "name":  "Jane", "contactId": 1234567890, "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ], "signedup" : "yes" }, ] }];

var mockOpportunity = { "time": "8:30am", "team": mockTeam  };

var mockMatt = {"name":"Matt", "lastName": "Silbernagel", "contactId":1970611,"roles":[{"name":"Nursery A - Sunday 8:30 Member","capacity":100,"slotsTaken":0},{"name":"Nursery B - Sunday 8:30 Member","capacity":10,"slotsTaken":2},{"name":"Nursery C - Sunday 8:30 Member","capacity":0,"slotsTaken":1}]};

describe('Serve Teams Directive', function() {

  beforeEach(function(){
    module('crossroads');
  });

  beforeEach(inject(function(_$compile_, _$rootScope_, $injector){
    $compile = _$compile_;
    $rootScope = _$rootScope_;
    $httpBackend = $injector.get('$httpBackend');
    mockServeDate = $injector.get('ServeOpportunities');
    scope = $rootScope.$new();
    element = '<serve-team opp-serve-date="serveDate" opportunity="opp" team="team" tab-index="tabIndex" team-index="teamIndex" day-index="dayIndex" event-type-id="eventTypeId" > </serve-team>';
    scope.opp = mockOpportunity;
    scope.team = mockTeam[0];
    scope.dayIndex = 0;
    scope.tabIndex = 0;
    scope.teamIndex = 3;
    scope.eventTypeId = 100;
    scope.serveDate = "10/15/2015";
    element = $compile(element)(scope);
    scope.$digest();
  }));

  it("should set signedup to null", function(){
    var isolated = element.isolateScope();
    expect(isolated.signedup).toBe(null);
  });

  it("should handle the current active tab", function(){
    var isolated = element.isolateScope();
    expect(isolated.currentActiveTab).toBe(null);
    expect( isolated.isActiveTab(mockTeam[0].members[0])).toBe(false);
  });

  it("should set the current member to the loggedin user", function(){
    var isolated = element.isolateScope();
    isolated.openPanel(mockTeam[0].members);
    expect(isolated.currentMember).toBe(mockTeam[0].members[0]);
  });

  it("should handle changing the tab", function(){
     var isolated = element.isolateScope();
     isolated.setActiveTab(mockTeam[0].members[1]);
     expect(isolated.currentMember).toBe(mockTeam[0].members[1]);
     expect(isolated.currentActiveTab).toBe(mockTeam[0].members[1].name);
  });

  it("should have the correct ID for the panel", function(){
    var isolated = element.isolateScope();
    expect(isolated.panelId()).toBe("team-panel-003");
  });

  it("should get the last serving date for an opportunity", function() {
    var isolated = element.isolateScope();
    isolated.openPanel(mockTeam[0].members);
    expect(isolated.currentMember).toBe(mockTeam[0].members[0]);
    // scope.currentMember.serveRsvp.roleId
    isolated.currentMember.currentOpportunity = mockTeam[0].members[0].roles[0];
    isolated.currentMember.currentOpportunity.frequency = {value:1, text:"Every Week (Sundays 8:30am)"};
    $httpBackend.expect('GET', window.__env__['CRDS_API_ENDPOINT'] + 'api/opportunity/getLastOpportunityDate/145').respond({'date': '1444552200'});
    isolated.populateDates();
    $httpBackend.flush()
    expect(isolated.currentMember.currentOpportunity.toDt).toBe("10/11/2015");
  });

  it("should set the end date to the current opportunity when selecting 'once'", function(){
    var isolated = element.isolateScope();
    isolated.openPanel(mockTeam[0].members);
    expect(isolated.currentMember).toBe(mockTeam[0].members[0]);
    isolated.currentMember.currentOpportunity = mockTeam[0].members[0].roles[0];
    isolated.currentMember.currentOpportunity.frequency = {value:0, text:"Once"};
    isolated.populateDates();
    expect(isolated.currentMember.currentOpportunity.toDt).toBe(isolated.currentMember.currentOpportunity.fromDt);
  });

  it("should save the response of one time rsvping", function(){
    var isolated = element.isolateScope();
    isolated.openPanel(mockTeam[0].members);
    expect(isolated.currentMember).toBe(mockTeam[0].members[0]);
    isolated.currentMember.currentOpportunity = mockTeam[0].members[0].roles[0];
    isolated.currentMember.currentOpportunity.frequency = {value:0, text:"Once"};
    isolated.currentMember.serveRsvp = {roleId: mockOpp.roleId};

    var dateArr = "10/15/2015".split("/");
    var d = moment(dateArr[2] + "-" + dateArr[0] + "-" + dateArr[1]);
    var dFormated = d.format('X');

    var rsvp = {
      contactId: mockTeam[0].members[0].contactId,
      opportunityId: mockOpp.roleId,
      eventTypeId: 100,
      endDate: dFormated,
      startDate: dFormated,
      signUp: false,
      alternateWeeks: false
    };
    isolated.populateDates();
    $httpBackend.expect('POST', window.__env__['CRDS_API_ENDPOINT'] + 'api/serve/save-rsvp', rsvp ).respond(200, '');
    isolated.saveRsvp();
    $httpBackend.flush();
  });

});

describe("Serve Teams Directive Edit", function() {
  beforeEach(function(){
    module('crossroads', function($provide){
      mockSession= jasmine.createSpyObj('Session', ['exists']);
      mockSession.exists.and.callFake(function(something){
        return '12345678';
      });
      $provide.value('Session', mockSession);
    });
  });

  beforeEach(inject(function(_$compile_, _$rootScope_){
    $compile = _$compile_;
    $rootScope = _$rootScope_;
    scope = $rootScope.$new();
    element = '<serve-team opportunity="opp" team="team" tab-index="tabIndex" team-index="teamIndex" day-index="dayIndex"> </serve-team>';
    scope.opp = mockOpportunity;
    scope.team = mockTeam;
    scope.dayIndex = 0;
    scope.tabIndex = 0;
    scope.teamIndex = 3;
    element = $compile(element)(scope);
    scope.$digest();
  }));

  it("should show edit button for logged in user", function() {
    var isolated = element.isolateScope();
    isolated.setActiveTab(mockTeam[0].members[0]);
    expect(isolated.showEdit).toBe(true);
  });

  it("should not show edit button for not logged in user", function() {
    var isolated = element.isolateScope();
    isolated.setActiveTab(mockTeam[0].members[1]);
    expect(isolated.showEdit).toBe(false);
  });

});
