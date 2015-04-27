describe('MyServeController', function() {

  beforeEach(module('crossroads'));
  
  var $controller, $log, mockServeResource, $httpBackend;
 
  var retArray = { "day": "Saturday, November 16, 2014", "opportunities" : [ { "time": "8:30am", "name" : "Kids Club Nusery", "members" : [ { "name": "John", "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ] }, { "name":  "Jane", "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ], "signedup" : "yes" }, ] }, { "time": "8:30am", "name": "First Impressions", "members" : [ { "name": "John"}, {"name": "Jane" } ] } ] }
  
  beforeEach(inject(function(_$controller_, _$log_, $injector){
    $controller = _$controller_;
    $log = _$log_;
    $httpBackend = $injector.get('$httpBackend');
    mockServeResource = $injector.get('ServeOpportunities');
  }));

  describe('Serve Controller', function(){ 
    var $scope, controller;
   
    //TODO: Need to find a way to get the environment variable!
    beforeEach(inject(function($log, $httpBackend){
      $scope = {};
      controller = $controller('MyServeController', { $scope: $scope });
    })); 

    it('should get a list of groups', inject(function(ServeOpportunities){ 
      $httpBackend.expectGET( window.__env__['CRDS_API_ENDPOINT'] + 'api/serve/family-serve-days').respond([ retArray ]);
      expect(controller.groups.length).toBe(0);
      $httpBackend.flush(); 
      expect(controller.groups[0].day).toBe("Saturday, November 16, 2014"); 
    }));

    it('should default to todays date', function(){
      $httpBackend.expectGET( window.__env__['CRDS_API_ENDPOINT'] + 'api/serve/family-serve-days').respond([ retArray ]);
      var today = new Date();
      expect(controller.dt.getDate()).toBe(today.getDate());
      expect(controller.dt.getDay()).toBe(today.getDay());
      expect(controller.dt.getFullYear()).toBe(today.getFullYear());
    });

    it('should show the  opportunities message', function(){
      $httpBackend.expectGET( window.__env__['CRDS_API_ENDPOINT'] + 'api/serve/family-serve-days').respond([]);
      $httpBackend.flush();
      expect(controller.showNoOpportunitiesMsg()).toBe(true);
    });

    it('should not show opportunities message', function(){
      $httpBackend.expectGET( window.__env__['CRDS_API_ENDPOINT'] + 'api/serve/family-serve-days').respond([ retArray ]);
      $httpBackend.flush();
      expect(controller.showNoOpportunitiesMsg()).toBe(false);
    });

  });

})
