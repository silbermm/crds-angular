describe('Refine List Directive', function() {

  var $compile, $rootScope, element, scope;

  var mockJohn = { "name": "John", "contactId" : 12345678, "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ] };
  var mockTeam = [{ "name" : "Kids Club Nusery", "members" : [ mockJohn, { "name":  "Jane", "contactId": 1234567890, "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ], "signedup" : "yes" }, ] }];

  var mockOpportunity = { "time": "8:30am", "team": mockTeam  };
  var mockServingDays = [ {"day" : "03/29/2015", "teams" : [ mockTeam ] } ];

  beforeEach(function(){
    module('crossroads');
  });
 
  beforeEach(inject(function(_$compile_, _$rootScope_){
    $compile = _$compile_;
    $rootScope = _$rootScope_;
    scope = $rootScope.$new();
    element = "<refine-list serving-days='servingDays'></refine-list>";
    scope.servingDays = mockServingDays;
    element = $compile(element)(scope);
    scope.$digest();
  }));

  it("should have the serve data that was passed in", function(){ 
    var isolated = element.isolateScope();
    expect(isolated.servingDays).toBe( mockServingDays );
  });

  it("should filter out the family members", function(){
    var isolated = element.isolateScope();
    expect(isolated.filterFamily().length).toBe(2);
  });

  it("should filter out the family and contain John", function(){
     var isolated = element.isolateScope();
     expect(isolated.filterFamily()).toContain(mockJohn);
  });

}) 
