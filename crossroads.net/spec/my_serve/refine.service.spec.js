describe('Refine Service', function() {

  var $rootScope, scope, filterState;

  beforeEach(module('crossroads'));

  beforeEach(inject(function(_filterState_) {
    filterState = _filterState_;
  }));

  it("should hold the selected family members", function(){
    filterState.addFamilyMember("0123456");
    filterState.addFamilyMember("6543210");
    expect(filterState.getFamilyMembers().length).toBe(2);
  });

  it("should remove an unselected family member", function(){
    filterState.addFamilyMember("0123456");
    filterState.addFamilyMember("6543210");
    expect(filterState.getFamilyMembers().length).toBe(2);
    filterState.removeFamilyMember("0123456");
    expect(filterState.getFamilyMembers().length).toBe(1);
  });

  it("should hold the selected times", function(){
    filterState.addTime("08:30:00");
    filterState.addTime("10:00:00");
    expect(filterState.getTimes().length).toBe(2);
  });

  it("should remove the unselected time", function(){
    filterState.addTime("08:30:00");
    filterState.addTime("10:00:00");
    expect(filterState.getTimes().length).toBe(2);
    filterState.removeTime("08:30:00");
    expect(filterState.getTimes().length).toBe(1);
  });

  it("should hold the selected teams", function(){
    filterState.addTeam("Nursery");
    filterState.addTeam("First Impressions");
    expect(filterState.getTeams().length).toBe(2);
  });

  it("should remove the unselected team", function(){
    filterState.addTeam("Nursery");
    filterState.addTeam("First Impressions");
    expect(filterState.getTeams().length).toBe(2);
    filterState.removeTeam("FirstImpressions");
    expect(filterState.getTeams().length).toBe(1);
  });

})
