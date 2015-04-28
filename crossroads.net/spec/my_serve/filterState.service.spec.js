describe('Filter State Service', function() {

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

  it("should hold the selected rsvp option", function() {
    filterState.addSignUp({
      'name': 'Yes',
      'id': 1,
      'selected': true,
      'attending': true
    });
    filterState.addSignUp({
      'name': 'No',
      'id': 2,
      'selected': true,
      'attending': false
    });
    expect(filterState.getSignUps().length).toBe(2);
  })

  it("should remove a selected rsvp option", function() {
    filterState.addSignUp({
      'name': 'Yes',
      'id': 1,
      'selected': true,
      'attending': true
    });
    filterState.addSignUp({
      'name': 'No',
      'id': 2,
      'selected': true,
      'attending': false
    });
    expect(filterState.getSignUps().length).toBe(2);

    filterState.removeSignUp({
      'name': 'No',
      'id': 2,
      'selected': true,
      'attending': false
    });
    expect(filterState.getSignUps().length).toBe(1);
  })

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
    filterState.removeTeam("First Impressions");
    expect(filterState.getTeams().length).toBe(1);
  });

  it("should find a family member", function (){
    filterState.addFamilyMember("0123456");
    filterState.addFamilyMember("6543210");
    expect(filterState.findMember("0123456")).toBe("0123456");
  });
  it("should not find a family member", function (){
    filterState.addFamilyMember("0123456");
    filterState.addFamilyMember("6543210");
    expect(filterState.findMember("8675309")).not.toBeDefined();
  });

  it("should find a team", function(){
    filterState.addTeam("Nursery");
    filterState.addTeam("First Impressions");
    expect(filterState.findTeam("Nursery")).toBe("Nursery");
  });

  it("should not find a team", function(){
    filterState.addTeam("Nursery");
    filterState.addTeam("First Impressions");
    expect(filterState.findTeam("A-Team")).not.toBeDefined();
  });

  it("should find a time", function(){
    filterState.addTime("08:30:00");
    filterState.addTime("10:00:00");
    expect(filterState.findTime("08:30:00")).toBe("08:30:00");
  });

  it("should not find a time", function(){
    filterState.addTime("08:30:00");
    filterState.addTime("10:00:00");
    expect(filterState.findTime("12:30:00")).not.toBeDefined();
  });

  it("should clear the list of filters", function(){
    filterState.addTeam("Nursery");
    filterState.addTime("08:30:00");
    filterState.addFamilyMember("0123456");
    filterState.clearAll();
    expect(filterState.getTeams().length).toBe(0);
    expect(filterState.getTimes().length).toBe(0);
    expect(filterState.getFamilyMembers().length).toBe(0);
  });
})
