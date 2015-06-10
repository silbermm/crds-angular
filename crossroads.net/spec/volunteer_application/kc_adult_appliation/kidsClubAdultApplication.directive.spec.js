describe("KidsClub Adult Application Directive", function() {

  var $compile, $rootScope, $log, element, scope, isolateScope;
  
  var mockVolunteer= {
    addressId: 99999,
    addressLine1: "9000 Observatory Lane",
    addressLine2: "",
    age: 35,
    anniversaryDate: "",
    city: "Cincinnati",
    congregationId: 5,
    contactId: 2186211,
    dateOfBirth: "02/21/1980",
    emailAddress: "matt.silbernagel@ingagepartners.com",
    employerName: null,
    firstName: "Matt",
    foreignCountry: "United States",
    genderId: 1,
    homePhone: "513-555-5555",
    householdId: 1709940,
    lastName: "Silbernagel",
    maidenName: null,
    maritalStatusId: 2,
    middleName: null,
    mobileCarrierId: null,
    mobilePhone: null,
    nickName: "Matt",
    postalCode: "45223-1231",
    state: "OH" 
  };


  beforeEach(function() {
    module('crossroads');
  });

  beforeEach(inject(function(_$compile_, _$rootScope_, _$q_, _$log_){
    $compile = _$compile_;
    $rootScope = _$rootScope_;
    $q = _$q_;
    $log = _$log_;
    scope = $rootScope.$new();
    element = "<kids-club-adult-application volunteer='volunteer'></kids-club-adult-application>";
    scope.volunteer = mockVolunteer;
    element = $compile(element)(scope);
    scope.$digest();
    isolateScope = element.isolateScope();
  }));

  it("should be false when checking if availability has been selected", function(){
    expect(isolateScope.availabilitySelected()).toBe(false); 
  });

  it("should be true if an availability has been selected", function(){
    isolateScope.volunteer.availabilityWeek = "week";
    expect(isolateScope.availabilitySelected()).toBe(true); 
  });

  it("should be false when checking if gradeLevel has been selected", function(){
    expect(isolateScope.gradeLevelSelected()).toBe(false);
  });

  it("should be true when gradeLevel has been selected", function(){
    isolateScope.volunteer.birthToTwo = "some string";
    expect(isolateScope.gradeLevelSelected()).toBe(true);
  });

  it("should be false when checking if availability has been selected", function(){
    expect(isolateScope.locationSelected()).toBe(false);
  });

  it("should be true when a location has been selected", function(){
    isolateScope.volunteer.availabilityFlorence = "some value";
    expect(isolateScope.locationSelected()).toBe(true);
  });

  it("should set the status of the datepicker to open", function(){
    isolateScope.open('childDob1', null);
    expect(isolateScope.datePickers['childDob1']).toBe(true);
    isolateScope.open('childDob2', null);
    expect(isolateScope.datePickers['childDob2']).toBe(true);
    isolateScope.open('signatureDate', null);
    expect(isolateScope.datePickers['signatureDate']).toBe(true);
  });

  it("should fail to save the form if there are errors in the form", function(){
    expect(isolateScope.save()).toBe(false);
  });

  it("should be false when checking if religion has been selected", function(){
    expect(isolateScope.religionSelected()).toBe(false);
  });

  it("should be true when a religion has been selected", function(){
    isolateScope.volunteer.exploring = "some value";
    expect(isolateScope.religionSelected()).toBe(true);
  });



});
