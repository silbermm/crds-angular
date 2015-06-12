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

    


});
