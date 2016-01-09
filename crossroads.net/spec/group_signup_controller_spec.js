require('crds-core');
require('../app/ang');
require('../app/ang2');

require('../app/app');

// testing controller
describe('GroupSignupController', function() {
  var $httpBackend;
  var scope;
  var groupSignupController;

  var userGetResponse = {
    Contact_Id: 'contactJson.Contact_Id',
    Email_Address: 'test@test.com',
    NickName: 'contactJson.Nickname',
    First_Name: 'Shankar',
    Middle_Name: 'contactJson.Middle_Name',
    Last_Name: 'Poncelet',
    Maiden_Name: 'contactJson.Maiden_Name',
    Mobile_Phone: 'contactJson.Mobile_Phone',
    Mobile_Carrier: 'contactJson.Mobile_Carrier_ID',
    Date_of_Birth: 'contactJson.Date_of_Birth',
    Marital_Status_Id: 'contactJson.Marital_Status_ID',
    Gender_Id: 'contactJson.Gender_ID',
    Employer_Name: 'contactJson.Employer_Name',
    Address_Line_1: 'contactJson.Address_Line_1',
    Address_Line_2: 'contactJson.Address_Line_2',
    City: 'contactJson.City',
    State: 'contactJson.State',
    Postal_Code: 'contactJson.Postal_Code',
    Anniversary_Date: 'contactJson.Anniversary_Date',
    Foreign_Country: 'contactJson.Foreign_Country',
    County: 'contactJson.County',
    Home_Phone: 'contactJson.Home_Phone',
    Congregation_ID: 'contactJson.Congregation_ID',
    Household_ID: 'contactJson.Household_ID',
    Address_Id: 'contactJson.Address_ID'

  };

  var pageGetResponse = {
    pages: [{
      group: '1'
    }]
  };

  var groupGetDetailResponse = {
    groupID: '1',
    groupFullInd: 'True',
    waitListInd: 'True',
    waitListGroupId: '1',
    userInGroup: true,
    SignUpFamilyMembers: [
      {
        First_Name: 'Shankar',
        Email_Address: 'shankx@test.com',
        userInGroup: false,
        participantId: '1234',
        newAdd: '1234',
        childCareNeeded: false
      },
      {
        First_Name: 'Luisa',
        Email_Address: 'Luisa@test.com',
        userInGroup: true,
        participantId: '1234',
        childCareNeeded: false
      }
    ]
  };

  var successResponse = [
    {
      success: 1897699,
      enrolledEvents: [
        '871912'
      ]
    },
    {
      success: 994377,
      enrolledEvents: [
        '871912'
      ]
    }
  ];

  //var endpoint = JSON.stringify(process.env.CRDS_API_ENDPOINT || "http://localhost:49380/");

  // Set up the module
  beforeEach(angular.mock.module('crossroads'));

  beforeEach(angular.mock.module(function($provide) {
    $provide.value('$state', { get: function() {} });

    $provide.value('$stateParams', { link: 'test' });
  }));

  beforeEach(
    inject(
      function($injector) {
        // Set up the mock http service responses
        $httpBackend = $injector.get('$httpBackend');

        // Get hold of a scope (i.e. the root scope)
        var $rootScope = $injector.get('$rootScope');
        scope = $rootScope.$new();

        // The $controller service is used to create instances of controllers
        var $controller = $injector.get('$controller');
        var $stateParams = {link: 'test'};

        groupSignupController = function() {
          return $controller('CommunityGroupSignupController', {$scope: scope, $stateParams: $stateParams});
        };

        $httpBackend.when('GET', window.__env__['CRDS_API_ENDPOINT'] + 'api/profile')
          .respond(userGetResponse);

        $httpBackend.when('GET', window.__env__['CRDS_CMS_ENDPOINT'] + '/api/Page/?link=test%2F')
          .respond(pageGetResponse);

        $httpBackend.when('GET', window.__env__['CRDS_API_ENDPOINT'] + 'api/group/1')
          .respond(groupGetDetailResponse);

      }));

  afterEach(function() {
    $httpBackend.verifyNoOutstandingExpectation();
    $httpBackend.verifyNoOutstandingRequest();
  });

  it('should signup a person for a community group', function() {
    $httpBackend.when('POST', window.__env__['CRDS_API_ENDPOINT'] + 'api/group/1/participants')
      .respond(successResponse);

    var controller = groupSignupController();
    verifyExpectations();
    var person = controller.person;
    expect(controller.signup).toBeDefined();
    controller.signup();
    $httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] + 'api/group/1/participants').respond(successResponse);
    $httpBackend.flush();

  });

  it('should give error when signing up and group is full', function() {
    $httpBackend.when('POST', window.__env__['CRDS_API_ENDPOINT'] + 'api/group/1/participants')
      .respond(function() {
        return [422, {}, {}];
      });

    var controller = groupSignupController();
    verifyExpectations();
    var person = controller.person;
    expect(controller.signup).toBeDefined();
    controller.signup();
    $httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] + 'api/group/1/participants');
    $httpBackend.flush();
    expect(controller.showFull).toEqual(true);
    expect(controller.showContent).toEqual(false);

  });

  it('should give error when signing up and HTTP error is returned', function() {
    $httpBackend.when('POST', window.__env__['CRDS_API_ENDPOINT'] + 'api/group/1/participants')
      .respond(function() {
        return [400, {}, {}];
      });

    var controller = groupSignupController();
    verifyExpectations();
    var person = controller.person;
    expect(controller.signup).toBeDefined();
    controller.signup();
    $httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] + 'api/group/1/participants');
    $httpBackend.flush();
    expect(controller.showFull).toEqual(false);
    expect(controller.showContent).toEqual(true);

  });

  it('should set the alreadySignedUp flag to TRUE ', function() {
    $httpBackend.when('POST', window.__env__['CRDS_API_ENDPOINT'] + 'api/group/1/participants')
      .respond(successResponse);

    var controller = groupSignupController();
    verifyExpectations();
    var response = {
      groupID: '1',
      groupFullInd: 'True',
      waitListInd: 'True',
      waitListGroupId: '1',
      userInGroup: true,
      SignUpFamilyMembers: [
        {
          First_Name: 'Shankar',
          Email_Address: 'shankx@test.com',
          userInGroup: true,
          participantId: '1234',
          newAdd: '1234',
          childCareNeeded: false
        },
        {
          First_Name: 'Luisa',
          Email_Address: 'Luisa@test.com',
          userInGroup: true,
          participantId: '1234',

          childCareNeeded: false
        }
      ]
    };
    var test = controller.allSignedUp(response);
    expect(test).toEqual(true);
  });

  it('should retun object containing newAdd value(s)', function() {
    $httpBackend.when('POST', window.__env__['CRDS_API_ENDPOINT'] + 'api/group/1/participants')
      .respond(successResponse);

    var controller = groupSignupController();
    verifyExpectations();
    var response =
      [
        {
          First_Name: 'Shankar',
          Email_Address: 'shankx@test.com',
          userInGroup: true,
          participantId: '1234',
          newAdd: '1234',
          childCareNeeded: false
        },
        {
          First_Name: 'Luisa',
          Email_Address: 'Luisa@test.com',
          userInGroup: false,
          participantId: '1234',
          childCareNeeded: false
        }
      ];
    var result = controller.hasParticipantID(response);
    expect(result.partId[0]).toEqual({ participantId: '1234', childCareNeeded: false });

    response =
      [
        {
          First_Name: 'Shankar',
          Email_Address: 'shankx@test.com',
          userInGroup: true,
          participantId: '2222',
          childCareNeeded: false
        }
      ];
    result = controller.hasParticipantID(response);
    expect(result.partId[0]).toEqual({ participantId: '2222', childCareNeeded: false });

    response =
      [
        {
          First_Name: 'Shankar',
          Email_Address: 'shankx@test.com',
          userInGroup: true,
          participantId: '1234',
          childCareNeeded: false
        },
        {
          First_Name: 'Luisa',
          Email_Address: 'Luisa@test.com',
          userInGroup: false,
          participantId: '1234',
          childCareNeeded: false
        }
      ];
    result = controller.hasParticipantID(response);
    expect(result.partId[0]).toBeUndefined();

  });

  function verifyExpectations() {
    $httpBackend.expectGET(window.__env__['CRDS_CMS_ENDPOINT'] + '/api/Page/?link=test%2F');

    $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/group/1');
    $httpBackend.flush();
  }
});
