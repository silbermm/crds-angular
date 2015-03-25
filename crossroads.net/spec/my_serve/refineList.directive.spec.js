describe('Refine List Directive', function() {

  var $compile, $rootScope, element, scope;

  var mockServingDays = [{"day":"3/29/2015","serveTimes": mockTimes}];

  var mockTimes = [{"time":"08:30:00","servingTeams": serveTeam830 },{"time":"10:00:00","servingTeams": serveTeam10 }];

  var serveTeam830 = [{"name":"KC First Grade Oakley MP","groupId":34911,"members":[{"name":"Leslie","contactId":1670885,"roles":[{"name":"First Grade Room A - Sunday 8:30 Member","capacity":0,"slotsTaken":0},{"name":"First Grade Room B - Sunday 8:30 Member","capacity":0,"slotsTaken":0}]}]},{"name":"KC Oakley Nursery MP","groupId":6329,"members":[{"name":"Leslie","contactId":1670885,"roles":[{"name":"Nursery A - Sunday 8:30 Member","capacity":100,"slotsTaken":0},{"name":"Nursery B - Sunday 8:30 Member","capacity":10,"slotsTaken":2},{"name":"Nursery C - Sunday 8:30 Member","capacity":0,"slotsTaken":1}]},{"name":"Matt","contactId":1970611,"roles":[{"name":"Nursery A - Sunday 8:30 Member","capacity":100,"slotsTaken":0},{"name":"Nursery B - Sunday 8:30 Member","capacity":10,"slotsTaken":2},{"name":"Nursery C - Sunday 8:30 Member","capacity":0,"slotsTaken":1}]}]}]

  var serveTeam10 = [{"name":"KC Oakley Nursery MP","groupId":6329,"members":[{"name":"Leslie","contactId":1670885,"roles":[{"name":"Nursery A - Sunday 10:00 Member","capacity":100,"slotsTaken":1},{"name":"Nursery B - Sunday 10:00 Member","capacity":0,"slotsTaken":0},{"name":"Nursery C - Sunday 10:00 Member","capacity":3,"slotsTaken":1}]},{"name":"Matt","contactId":1970611,"roles":[{"name":"Nursery A - Sunday 10:00 Member","capacity":100,"slotsTaken":1},{"name":"Nursery B - Sunday 10:00 Member","capacity":0,"slotsTaken":0},{"name":"Nursery C - Sunday 10:00 Member","capacity":3,"slotsTaken":1}]}]}];

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

  it("should filter the array of days", function(){
    var isolated = element.isolateScope();
    isolated.filterDays();
    expect(isolated.days.length).toBe(1);
  });

  it("should get the correct dates", function(){
    var isolated = element.isolateScope();
    isolated.filterDays();
    expect(isolated.days[0].day).toBe("3/29/2015");
  });

  it("should filter the array of times", function(){
    var isolated = element.isolateScope();
    isolated.filterTimes();
    expect(isolated.times.length).toBe(2);
  });

  it("should have the correct times", function(){
    var isolated = element.isolateScope();
    isolated.filterTimes();
    expect(isolated.times[0].time).toBe("08:30:00");
    expect(isolated.times[1].time).toBe("10:00:00");
  });

  it("should filter out the family members", function(){
    var isolated = element.isolateScope();
    debugger;
    var family = isolated.filterFamily();
    expect(family.length).toBe(2);
  });

  it("should filter out the family and contain John", function(){
     var isolated = element.isolateScope();
     expect(isolated.filterFamily()).toContain(mockJohn);
  });

}) 
