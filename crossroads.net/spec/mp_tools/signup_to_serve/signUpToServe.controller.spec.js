describe('Signup To Serve Tool', function(){

  var expectedReturn = { 
    groupId: 23, 
    groupName: "Kids Club Nursery", 
    groupParticipants: [ 
     {
      contactId: 23456, 
      firstname: "Matt", 
      lastname: 'Silbernagel',
      nickname: 'Matt',
     },
     {
      contactId: 23457, 
      firstname: "Andy", 
      lastname: 'Canterbury',
      nickname: 'Andy',
     }]
  };

  var expectedDates = [1430382600,1430469000,1430555400,1430641800];
  
  beforeEach(module('crossroads'));
 
  beforeEach(inject(function(_$location_){
    $location = _$location_
    spyOn($location, 'search').and.returnValue({
      dg:'8b6242c9-ea32-40f7-97a2-e2bb3524ced2',
      'ug':'c29e64a5-820b-461f-a57c-5831d070d578', 
      pageID:'292',
      recordID:'2923',
      recordDescription: undefined,
      s:'11467', 
      sc:'1', 
      p:0, 
      v:387
    });
  }));

  var $controller, $log, mockSu2sResource, mockServeResource, $httpBackend, MPTools, $window;

  beforeEach(inject(function(_$controller_, _$log_, _MPTools_, _$window_, $injector){
    $controller = _$controller_;
    $log = _$log_;
    $window = _$window_;
    MPTools = _MPTools_;
    $httpBackend = $injector.get('$httpBackend');
    mockSu2sResource = $injector.get('Su2sData');
    mockServeResource = $injector.get('ServeOpportunities');
  }));

  describe('Signup To Serve Controller', function(){ 
    
    var $scope, controller;

    beforeEach(function(){
      $scope = {};
      controller = $controller('SignupToServeController', { $scope: $scope });
      $httpBackend.expectGET( window.__env__['CRDS_API_ENDPOINT'] + 'api/opportunity/getGroupParticipantsForOpportunity/2923').respond(expectedReturn);
      $httpBackend.expectGET( window.__env__['CRDS_API_ENDPOINT'] + 'api/opportunity/getAllOpportunityDates/2923').respond(expectedDates);
    }); 

    it("should get the correct query parameters", function(){
      expect(controller.params.userGuid).toBe('c29e64a5-820b-461f-a57c-5831d070d578');
    });

    it("should get a list of participants", function(){
      $httpBackend.flush();
      expect(controller.group.groupId).toBe(expectedReturn.groupId);
      expect(controller.group.groupName).toBe(expectedReturn.groupName);
      expect(controller.group.groupParticipants.length).toBe(2);
    });

    it("should show the error message", function(){
      $httpBackend.flush();
      expect(controller.showError()).toBe(true);
    });

  })
});
