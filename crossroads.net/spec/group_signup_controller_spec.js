 //require('angular');

// testing controller
describe('EventsController', function() {
   var $httpBackend, $rootScope, eventsController;

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
   
   var endpoint = 'http://silbervm:49380/';
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
       // The $controller service is used to create instances of controllers
       var $controller = $injector.get('$controller');

       groupSignupController = function(site) {
     $rootScope.site = site;
         return $controller('GroupSignupController', {'$scope' : $rootScope });
       };
   }));


   afterEach(function() {
     $httpBackend.verifyNoOutstandingExpectation();
     $httpBackend.verifyNoOutstandingRequest();
   });

      it('should get logged-in person when instantiated', function(){
       $httpBackend.when('GET', endpoint + 'api/profile')
                              .respond(userGetResponse);

        var controller = groupSignupController("1");
        $httpBackend.expectGET(endpoint + 'api/profile');
        $httpBackend.flush();
        var person = $rootScope.person;
        expect(person).toBeDefined();
        expect(person["First_Name"]).toEqual("Shankar");
        expect(person["Last_Name"]).toEqual("Poncelet");
        expect(person["Email_Address"]).toEqual("test@test.com");

      });
});