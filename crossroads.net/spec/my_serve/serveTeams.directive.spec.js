describe('Serve Teams Directive', function() {

  var $compile, $rootScope, element, scope, mockSession, mockServeDate, $httpBackend;

  var mockOpp = {"name": "NuseryA", "roleId": "145"};
  var mockTeam = [{ "name" : "Kids Club Nusery", "members" : [ { "name": "John", "contactId" : 12345678, "roles" : [ mockOpp, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ] }, { "name":  "Jane", "contactId": 1234567890, "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ], "signedup" : "yes" }, ] }];

  var mockOpportunity = { "time": "8:30am", "team": mockTeam  };

  var mockMatt = {"name":"Matt", "lastName": "Silbernagel", "contactId":1970611,"roles":[{"name":"Nursery A - Sunday 8:30 Member","capacity":100,"slotsTaken":0},{"name":"Nursery B - Sunday 8:30 Member","capacity":10,"slotsTaken":2},{"name":"Nursery C - Sunday 8:30 Member","capacity":0,"slotsTaken":1}]};



  beforeEach(function(){
    module('crossroads', function($provide){
      mockSession= jasmine.createSpyObj('Session', ['exists']);
      mockSession.exists.and.callFake(function(something){
        return '12345678';
      });
      $provide.value('Session', mockSession);
    });
  });

  beforeEach(inject(function(_$compile_, _$rootScope_, $injector){
    $compile = _$compile_;
    $rootScope = _$rootScope_;
    $httpBackend = $injector.get('$httpBackend');
    mockServeDate = $injector.get('ServeOpportunities');    
    $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/opportunity/getLastOpportunityDate/145').respond({"date": 1444552200});
    $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/authenticated').respond({userId: 12345678, userToken: "12345", username: "Laks"},{'Authorization': "12345"});
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

  /*it("should get the last serving date for an opportunity", function() {   
    var isolated = element.isolateScope();
    isolated.openPanel(mockTeam[0].members);
    expect(isolated.currentMember).toBe(mockTeam[0].members[0]);
    isolated.currentMember.currentOpportunity = mockTeam[0].members[0].roles[0];
    isolated.getLastDate();
    debugger;
    $httpBackend.flush();
    expect(isolated.toDt).toBe("10/11/2015");
  });
  */
});
