describe('Refine List Directive', function() {

  var $compile, $rootScope, element, scope, isolateScope, filterState;

  var mockLeslie = {"name":"Leslie", "lastName": "Silbernagel", "contactId":1670885,"roles":[{"name":"First Grade Room A - Sunday 8:30 Member","capacity":0,"slotsTaken":0},{"name":"First Grade Room B - Sunday 8:30 Member","capacity":0,"slotsTaken":0}]};

  var mockMatt = {"name":"Matt", "lastName": "Silbernagel", "contactId":1970611,"roles":[{"name":"Nursery A - Sunday 8:30 Member","capacity":100,"slotsTaken":0},{"name":"Nursery B - Sunday 8:30 Member","capacity":10,"slotsTaken":2},{"name":"Nursery C - Sunday 8:30 Member","capacity":0,"slotsTaken":1}]};

  var serveTeam830 = [{"name":"KC First Grade Oakley MP","groupId":34911,"members":[mockLeslie]},{"name":"KC Oakley Nursery MP","groupId":6329,"members":[{"name":"Leslie","contactId":1670885,"roles":[{"name":"Nursery A - Sunday 8:30 Member","capacity":100,"slotsTaken":0},{"name":"Nursery B - Sunday 8:30 Member","capacity":10,"slotsTaken":2},{"name":"Nursery C - Sunday 8:30 Member","capacity":0,"slotsTaken":1}]},mockMatt]}]

  var serveTeam10 = [{"name":"KC Oakley Nursery MP","groupId":6329,"members":[{"name":"Leslie","contactId":1670885,"roles":[{"name":"Nursery A - Sunday 10:00 Member","capacity":100,"slotsTaken":1},{"name":"Nursery B - Sunday 10:00 Member","capacity":0,"slotsTaken":0},{"name":"Nursery C - Sunday 10:00 Member","capacity":3,"slotsTaken":1}]},{"name":"Matt","contactId":1970611,"roles":[{"name":"Nursery A - Sunday 10:00 Member","capacity":100,"slotsTaken":1},{"name":"Nursery B - Sunday 10:00 Member","capacity":0,"slotsTaken":0},{"name":"Nursery C - Sunday 10:00 Member","capacity":3,"slotsTaken":1}]}]}];

  var expectedTeam830 = [{"name":"KC Oakley Nursery MP","groupId":6329,"members":[mockMatt]}];

  var expectedTeam10 = [{"name":"KC Oakley Nursery MP","groupId":6329,"members":[{"name":"Matt","contactId":1970611,"roles":[{"name":"Nursery A - Sunday 10:00 Member","capacity":100,"slotsTaken":1},{"name":"Nursery B - Sunday 10:00 Member","capacity":0,"slotsTaken":0},{"name":"Nursery C - Sunday 10:00 Member","capacity":3,"slotsTaken":1}]}]}];

  var expectedSaturdayTimes = [{"time":"08:30:00","servingTeams": expectedTeam830 },{"time":"10:00:00","servingTeams": expectedTeam10 }];

  var expectedSundayTimes = [{"time":"08:30:00","servingTeams": expectedTeam830 },{"time":"10:00:00","servingTeams": expectedTeam10 }];
  
  var expectedServingDays = [{"day":"3/28/2015", "serveTimes": expectedSaturdayTimes},{"day":"3/29/2015","serveTimes": expectedSundayTimes }]; 

  var mockSaturdayTimes = [{"time":"08:30:00","servingTeams": serveTeam830 },{"time":"10:00:00","servingTeams": serveTeam10 }];
  var mockSundayTimes = [{"time":"08:30:00","servingTeams": serveTeam830 },{"time":"10:00:00","servingTeams": serveTeam10 }];

  var mockServingDays = [{"day":"3/28/2015", "serveTimes": mockSaturdayTimes}, {"day":"3/29/2015","serveTimes": mockSundayTimes }];


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
    filterState.memberIds = [];
    filterState.teams = [];
    filterState.times = [];
    isolateScope.filterAll();
    expect(JSON.stringify(isolateScope.servingDays)).toBe(JSON.stringify(mockServingDays));
  });

  it("should have two serve dates", function(){ 
    expect(isolateScope.servingDays.length).toBe(2);
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
  });

  it("should filter out only the teams selected", function(){
    filterState.memberIds = [];
    filterState.teams = [];
    filterState.times = [];
    filterState.addTeam(34911);
    isolateScope.filterAll();
    expect(isolateScope.servingDays[0].serveTimes[0].servingTeams.length).toBe(1);
  });

   it("should not filter teams when teams are not selected", function(){
    filterState.memberIds = [];
    filterState.teams = [];
    filterState.times = [];
    isolateScope.filterAll();
    expect(isolateScope.servingDays[0].serveTimes[0].servingTeams.length).toBe(2);
  });

  it("should filter out only days where I and only I  have a team and serving opportunity available", function(){
      filterState.teams = [];
      filterState.times = [];
      filterState.memberIds = [];
      filterState.addFamilyMember(1970611);
      isolateScope.filterAll();
      expect(isolateScope.servingDays[0].serveTimes[0].servingTeams[0].members.length).toBe(1);
      expect(isolateScope.servingDays[0].serveTimes[0].servingTeams[0].members[0].contactId).toBe(1970611);
  });
 
  it("should filter out only times when opportunities are available", function(){
      filterState.teams = [];
      filterState.times = [];
      filterState.memberIds = [];
      filterState.addTime("08:30:00");
      isolateScope.filterAll();
      expect(isolateScope.servingDays[0].serveTimes.length).toBe(1);
      expect(isolateScope.servingDays[1].serveTimes.length).toBe(1);
      expect(isolateScope.servingDays[1].serveTimes[0].time).toBe("08:30:00");
  });

  it("should not filter out times when times are not checked", function(){
      filterState.teams = [];
      filterState.times = [];
      filterState.memberIds = [];
      isolateScope.filterAll();
      expect(isolateScope.servingDays[0].serveTimes.length).toBe(2);
      expect(isolateScope.servingDays[1].serveTimes.length).toBe(2);
  });

  it("should clear all filters and return to the default list", function(){
    filterState.addFamilyMember(1970611);
    filterState.addTime("08:30:00");
    filterState.addTeam(34911);
    isolateScope.filterAll(); 
    isolateScope.clearFilters(); 
    expect(isolateScope.servingDays[0].serveTimes.length).toBe(2);
    expect(isolateScope.servingDays[1].serveTimes.length).toBe(2);
    expect(isolateScope.servingDays[0].serveTimes[0].servingTeams.length).toBe(2);
    expect(isolateScope.servingDays[0].serveTimes[1].servingTeams.length).toBe(1);
  });

}); 