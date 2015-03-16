describe('Serve Tabs Directive', function() {

  var $compile, $rootScope; 
  var element, scope;

  var mockOpportunity = { "time": "8:30am", "name" : "Kids Club Nusery", "members" : [ { "name": "John", "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ] }, { "name":  "Jane", "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ], "signedup" : "yes" }, ] }


  beforeEach(module('crossroads'));
  
  beforeEach(inject(function(_$compile_, _$rootScope_){
    $compile = _$compile_;
    $rootScope = _$rootScope_;
    
    scope = $rootScope.$new();

    element = '<serve-tabs opportunity="opp"> </serve-tabs>';
    scope.opp = mockOpportunity;

    element = $compile(element)(scope);
    scope.$digest();
  }));


  describe("when given an opportunity", function(){

    it("should set signedup to null", function(){
      var isolated = element.isolateScope();
      expect(isolated.signedup).toBe(null);
    });

    it("should handle the current active tab", function(){
      var isolated = element.isolateScope();
      expect(isolated.currentActiveTab).toBe(mockOpportunity.members[0].name);
      
      expect(
          isolated.isActiveTab(mockOpportunity.members[0].name)
      ).toBe(true); 
    });

    it("should set the current member to the first member", function(){
      var isolated = element.isolateScope();
      expect(isolated.currentMember).toBe(mockOpportunity.members[0]);
    });
  
    it("should handle changing the tab", function(){
       var isolated = element.isolateScope();
       expect(isolated.currentMember).toBe(mockOpportunity.members[0]);
       isolated.setActiveTab(mockOpportunity.members[1]);
       expect(isolated.currentMember).toBe(mockOpportunity.members[1]);
       expect(isolated.currentActiveTab).toBe(mockOpportunity.members[1].name);
    });
  });

  describe("markup to be correct", function(){
    it("should have a highlighted tab", function(){
      expect(element.html()).toContain("<div class=\"serve-day-time row push-top\">")
    });
  });

  
});
