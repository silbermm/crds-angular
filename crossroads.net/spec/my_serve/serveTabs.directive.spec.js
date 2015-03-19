describe('Serve Tabs Directive', function() {

  var $compile, $rootScope, element, scope, mockSession;
 
  var mockOpportunity = { "time": "8:30am", "name" : "Kids Club Nusery", "members" : [ { "name": "John", "contactId" : 12345678, "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ] }, { "name":  "Jane", "contactId": 1234567890, "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ], "signedup" : "yes" }, ] };


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
    element = '<serve-tabs opportunity="opp"> </serve-tabs>';
    scope.opp = mockOpportunity;
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
    
    expect(
        isolated.isActiveTab(mockOpportunity.members[0].name)
    ).toBe(false); 
  });

  it("should set the current member to the loggedin user", function(){
    var isolated = element.isolateScope();
    debugger; 
    isolated.openPanel(mockOpportunity.members);
    expect(isolated.currentMember).toBe(mockOpportunity.members[0]);
  });

  it("should handle changing the tab", function(){
     var isolated = element.isolateScope();
     isolated.setActiveTab(mockOpportunity.members[1]);
     expect(isolated.currentMember).toBe(mockOpportunity.members[1]);
     expect(isolated.currentActiveTab).toBe(mockOpportunity.members[1].name);
  });

  describe("markup to be correct", function(){
    it("should have a highlighted tab", function(){
      expect(element.html()).toContain("<div class=\"serve-day-time row push-top\">")
    });
  });

  
});
