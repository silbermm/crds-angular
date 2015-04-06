describe('Serve Teams Directive', function() {

  var $compile, $rootScope, element, scope, mockSession;

  var mockTeam = [{ "name" : "Kids Club Nusery", "members" : [ { "name": "John", "contactId" : 12345678, "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ] }, { "name":  "Jane", "contactId": 1234567890, "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ], "signedup" : "yes" }, ] }];

  var mockOpportunity = { "time": "8:30am", "team": mockTeam  };


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

});
