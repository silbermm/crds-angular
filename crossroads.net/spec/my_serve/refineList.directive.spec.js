describe('Refine List Directive', function() {

  var $compile, $rootScope, element, scope, isolateScope, filterState;

  var mockLeslie = {"name":"Leslie", "lastName": "Silbernagel", "contactId":1670885,"roles":[{"name":"First Grade Room A - Sunday 8:30 Member","capacity":0,"slotsTaken":0},{"name":"First Grade Room B - Sunday 8:30 Member","capacity":0,"slotsTaken":0}]};

  var mockMatt = {"name":"Matt", "lastName": "Silbernagel", "contactId":1970611,"roles":[{"name":"Nursery A - Sunday 8:30 Member","capacity":100,"slotsTaken":0},{"name":"Nursery B - Sunday 8:30 Member","capacity":10,"slotsTaken":2},{"name":"Nursery C - Sunday 8:30 Member","capacity":0,"slotsTaken":1}]};

  var serveTeam830 = [{"name":"KC First Grade Oakley MP","groupId":34911,"members":[mockLeslie]},{"name":"KC Oakley Nursery MP","groupId":6329,"members":[{"name":"Leslie","contactId":1670885,"roles":[{"name":"Nursery A - Sunday 8:30 Member","capacity":100,"slotsTaken":0},{"name":"Nursery B - Sunday 8:30 Member","capacity":10,"slotsTaken":2},{"name":"Nursery C - Sunday 8:30 Member","capacity":0,"slotsTaken":1}]},mockMatt]}]

  var serveTeam10 = [{"name":"KC Oakley Nursery MP","groupId":6329,"members":[{"name":"Leslie","contactId":1670885,"roles":[{"name":"Nursery A - Sunday 10:00 Member","capacity":100,"slotsTaken":1},{"name":"Nursery B - Sunday 10:00 Member","capacity":0,"slotsTaken":0},{"name":"Nursery C - Sunday 10:00 Member","capacity":3,"slotsTaken":1}]},{"name":"Matt","contactId":1970611,"roles":[{"name":"Nursery A - Sunday 10:00 Member","capacity":100,"slotsTaken":1},{"name":"Nursery B - Sunday 10:00 Member","capacity":0,"slotsTaken":0},{"name":"Nursery C - Sunday 10:00 Member","capacity":3,"slotsTaken":1}]}]}];


  var mockSaturdayTimes = [{"time":"08:30:00","servingTeams": serveTeam830 },{"time":"10:00:00","servingTeams": serveTeam10 }];
  var mockSundayTimes = [{"time":"08:30:00","servingTeams": serveTeam830 },{"time":"10:00:00","servingTeams": serveTeam10 }];

  var mockServingDays = [{"day":"3/28/2015", "serveTimes": mockSaturdayTimes}, {"day":"3/29/2015","serveTimes": mockSundayTimes}];


  beforeEach(function(){
    module('crossroads');
  });
 
  beforeEach(inject(function(_$compile_, _$rootScope_, _$q_, _filterState_){
    $compile = _$compile_;
    $rootScope = _$rootScope_;
    $q = _$q_;
    filterState = _filterState_;
    filterState.addFamilyMember(1670885);
    filterState.addTeam(34911);
    filterState.addTime("08:30:00");
    scope = $rootScope.$new();
    element = "<refine-list serving-days='servingDays'></refine-list>";
    scope.servingDays = mockServingDays;
    scope.servingDays.$promise = promiseServeDates(mockServingDays);
    element = $compile(element)(scope);
    scope.$digest();
    isolateScope = element.isolateScope();
   
  
    function promiseServeDates(serveDates) {
      var deferred = $q.defer();
      deferred.resolve(serveDates);
      return deferred.promise;
    } 
 
  }));

  it("should have the serve data that was passed in", function(){ 
    var servingDays = isolateScope.servingDays;
    expect(isolateScope.servingDays).toBe( mockServingDays );
  });

  it("should filter the array of times", function(){
    expect(isolateScope.times.length).toBe(4);
  });

  it("should have the correct times", function(){
    var times = isolateScope.times;
    expect(isolateScope.times[0].time).toBe("08:30:00");
    expect(isolateScope.times[1].time).toBe("10:00:00");
    expect(isolateScope.times[2].time).toBe("08:30:00");
    expect(isolateScope.times[3].time).toBe("10:00:00");
  });

  it("should filter times to a unique list of times", function(){
    expect(isolateScope.uniqueTimes.length).toBe(2);
  });

  it("should have the correct times in unique list", function(){
    var times = isolateScope.uniqueTimes;
    expect(isolateScope.times[0].time).toBe("08:30:00");
    expect(isolateScope.times[1].time).toBe("10:00:00");
  });

  it("should filter the array of teams", function(){
    expect(isolateScope.serveTeams.length).toBe(6);
  });

  it("should have the correct teams", function(){
    expect(isolateScope.serveTeams[0].name).toBe("KC First Grade Oakley MP");
  });

  it("should filter out the family members", function(){
    expect(isolateScope.serveMembers.length).toBe(10);
  });

  it("should filter out the family and contain Leslie", function(){
     expect(isolateScope.serveMembers).toContain(mockLeslie);
  });

  it("should filter out the family and contain Leslie", function(){
     expect(isolateScope.serveMembers).toContain(mockMatt);
  });
  
  it("should filter family to a unique list of contacts", function(){
    isolateScope.getUniqueMembers();
    expect(isolateScope.uniqueMembers.length).toBe(2);
  });

  it("should find the correct members in the unique list", function(){
    isolateScope.getUniqueMembers();
    expect(isolateScope.uniqueMembers).toContain({name: "Leslie", lastName: "Silbernagel", contactId: 1670885});
    expect(isolateScope.uniqueMembers).toContain({name: "Matt", lastName: "Silbernagel", contactId: 1970611 });

  });

  it("should filter out unique teams", function(){
    isolateScope.getUniqueTeams();
    expect(isolateScope.uniqueTeams.length).toBe(2); 
  });

  it("should have the correct teams in the unique team list", function(){
    isolateScope.getUniqueTeams();
    expect(isolateScope.uniqueTeams).toContain({"name":"KC First Grade Oakley MP","groupId":34911});
    expect(isolateScope.uniqueTeams).toContain({"name":"KC Oakley Nursery MP","groupId":6329});
  });

  it("should set selected on correct family member", function() {
    _.each(isolateScope.uniqueMembers, function(member){
      if (member.contactId === 1670885){
        expect(member.selected).toBe(true);
      } else {
        expect(member.selected).toBeFalsy();
      }
    });
  });

  it("should set selected on correct team", function() {
    _.each(isolateScope.uniqueTeams, function(team){
      if (team.groupId === 34911){
        expect(team.selected).toBe(true);
      } else {
        expect(team.selected).toBeFalsy();
      }
    });
  });

  it("should set selected on correct time", function() {
    _.each(isolateScope.uniqueTimes, function(time){
      if (time.time === "08:30:00"){
        expect(time.selected).toBe(true);
      } else {
        expect(time.selected).toBeFalsy();
      }
    });
  })
}) 
