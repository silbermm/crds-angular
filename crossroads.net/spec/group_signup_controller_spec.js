
// testing controller
describe('EventsController', function() {
 var $httpBackend, scope;

 var userGetResponse = {

   "Contact_Id" : "contactJson.Contact_Id",
   "Email_Address" : "test@test.com",
   "NickName" : "contactJson.Nickname",
   "First_Name" : "Shankar",
   "Middle_Name" : "contactJson.Middle_Name",
   "Last_Name" : "Poncelet",
   "Maiden_Name" : "contactJson.Maiden_Name",
   "Mobile_Phone" : "contactJson.Mobile_Phone",
   "Mobile_Carrier" : "contactJson.Mobile_Carrier_ID",
   "Date_of_Birth" : "contactJson.Date_of_Birth",
   "Marital_Status_Id" : "contactJson.Marital_Status_ID",
   "Gender_Id" : "contactJson.Gender_ID",
   "Employer_Name" : "contactJson.Employer_Name",
   "Address_Line_1" : "contactJson.Address_Line_1",
   "Address_Line_2" : "contactJson.Address_Line_2",
   "City" : "contactJson.City",
   "State" : "contactJson.State",
   "Postal_Code" : "contactJson.Postal_Code",
   "Anniversary_Date" : "contactJson.Anniversary_Date",
   "Foreign_Country" : "contactJson.Foreign_Country",
   "County" : "contactJson.County",
   "Home_Phone" : "contactJson.Home_Phone",
   "Congregation_ID" : "contactJson.Congregation_ID",
   "Household_ID" : "contactJson.Household_ID",
   "Address_Id" : "contactJson.Address_ID"

 };

 var pageGetResponse = {"pages":[{
  "group":"1"
}]};


   //var endpoint = JSON.stringify(process.env.CRDS_API_ENDPOINT || "http://localhost:49380/");

   // Set up the module
   beforeEach(module('crossroads'));

   beforeEach(
    inject(
      function($injector) {
       // Set up the mock http service responses
       $httpBackend = $injector.get('$httpBackend');
       // Get hold of a scope (i.e. the root scope)
       $rootScope = $injector.get('$rootScope');
       scope = $rootScope.$new();
       // The $controller service is used to create instances of controllers
       var $controller = $injector.get('$controller');
       var $stateParams = {"link" : "test"}

       groupSignupController = function() {
         return $controller('GroupSignupController', {'$scope' : scope, '$stateParams' : $stateParams });
       };
     }));


   afterEach(function() {
     $httpBackend.verifyNoOutstandingExpectation();
     $httpBackend.verifyNoOutstandingRequest();
   });

   it('should get logged-in person when instantiated', function(){
     $httpBackend.when('GET', window.__env__['CRDS_API_ENDPOINT'] +'api/profile')
     .respond(userGetResponse);

     $httpBackend.when('GET', 'http://content.crossroads.net//api/Page/?link=test')
     .respond(pageGetResponse);

     var controller = groupSignupController();
     $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] +'api/profile');
     $httpBackend.expectGET('http://content.crossroads.net//api/Page/?link=test');
     $httpBackend.flush();
     var person = controller.person;
     expect(person).toBeDefined();
     expect(person["First_Name"]).toEqual("Shankar");
     expect(person["Last_Name"]).toEqual("Poncelet");
     expect(person["Email_Address"]).toEqual("test@test.com");

   });

   it('should signup a person for a community group', function(){
    $httpBackend.when('GET', window.__env__['CRDS_API_ENDPOINT'] + 'api/profile')
    .respond(userGetResponse);

    $httpBackend.when('GET', 'http://content.crossroads.net//api/Page/?link=test')
    .respond(pageGetResponse);

    var controller = groupSignupController();
    $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/profile');
    $httpBackend.flush();

    expect(controller.signup).toBeDefined();

    $httpBackend.when('POST', window.__env__['CRDS_API_ENDPOINT'] + 'api/group/1/user')
    .respond("200");
    controller.signup();                      
    $httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] +'api/group/1/user').respond('200');
    $httpBackend.flush();          

  });
 });