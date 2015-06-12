describe("Volunteer Application Controller", function() {

  var controller, 
      $rootScope, 
      $scope, 
      httpBackend, 
      mockSession,
      Session,
      CmsInfo,
      Contact,
      Opportunity; 

  var mockVolunteer= {
    addressId: 99999,
    addressLine1: "9000 Observatory Lane",
    addressLine2: "",
    age: 35,
    anniversaryDate: "",
    city: "Cincinnati",
    congregationId: 5,
    contactId: '12345678',
    dateOfBirth: "04/03/2005",
    emailAddress: "matt.silbernagel@ingagepartners.com",
    employerName: null,
    firstName: "Miles",
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
    nickName: "Miles",
    postalCode: "45223-1231",
    state: "OH" 
  };

  var mockContactId = '12345678';
  var mockResponseId = 34567;

  var mockPageInfo = {
    pages : [
    {
    accessDenied: "<p>Oops! Looks like you are not authorized to access this information. If you think this is a mistake, please contact the system administrator.</p>",
    canEditType: "Inherit",
    canViewType: "Inherit",
    content: "<p>Please complete this application.</p>",
    extraMeta: null,
    group: "27705",
    hasBrokenFile: "0",
    hasBrokenLink: "0",
    id: 83,
    link: "/volunteer-application/kids-club/",
    menuTitle: null,
    metaDescription: null,
    noExistingResponse: "<p>Oops! Please contact the group leader of the team you are looking to serve. Looks like we don't have a request from you to join this team.</p>",
    opportunity: "115",
    pageType: "VolunteerApplicationPage",
    parent: 82,
    renderedContent: "<p>Please complete this application.</p>",
    reportClass: null,
    showInMenus: "1",
    showInSearch: "1",
    sort: "1",
    success: "<p>Default SUCCESS text for this page, see ApplicationPage.php to change</p>",
    title: "Kids Club",
    uRLSegment: "kids-club",
    version: "15"
    }]
  };
 
  beforeEach(module('crossroads'));
 
  beforeEach(module(function($provide){
    $provide.value('$stateParams', { id: '12345678'}); 
    mockSession= jasmine.createSpyObj('Session', ['exists', 'isActive']);
    mockSession.exists.and.callFake(function(something){
      return '12345678';
    });
    $provide.value('Session', mockSession);
    $provide.value('CmsInfo', mockPageInfo);
    $provide.value('Contact', mockVolunteer);
  }));

  var controller;

  beforeEach(inject(function(_$controller_, _$log_, $injector ){
    $controller = _$controller_;
    $log = _$log_;
     
    $httpBackend = $injector.get('$httpBackend');
    Opportunity = $injector.get('Opportunity');
    CmsInfo = $injector.get("CmsInfo");
    Contact = $injector.get("Contact");
    Session = $injector.get("Session");
   
    $scope = {};
    controller = $controller("VolunteerApplicationController", { $scope: $scope }); 

  }));

  it("should set up the tests correctly", function(){
    expect(true).toBe(true);
  });


});
