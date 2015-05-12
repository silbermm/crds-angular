describe('MyServeController', function() {


  var retArray = { "day": "Saturday, November 16, 2014", "serveTimes" : [ { "time": "8:30am", "name" : "Kids Club Nusery", "members" : [ { "name": "John", "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ] }, { "name":  "Jane", "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ], "signedup" : "yes" }, ] }, { "time": "8:30am", "name": "First Impressions", "members" : [ { "name": "John"}, {"name": "Jane" } ] } ] }

  beforeEach(module('crossroads'));
 
  beforeEach(module(function($provide){
    $provide.value('Groups', retArray);
  }));

  var $controller, $log, mockServeResource, $httpBackend;
  beforeEach(inject(function(_$controller_, _$log_, $injector){
    $controller = _$controller_;
    $log = _$log_;
    $httpBackend = $injector.get('$httpBackend');
    mockServeResource = $injector.get('ServeOpportunities');
  }));

  describe('Serve Controller', function(){
    var $scope, controller;
    beforeEach(inject(function($log, $httpBackend){
      $scope = {};
      $scope.groups = retArray;
      controller = $controller('MyServeController', { $scope: $scope });
    }));

    it('should show the opportunities message', function(){
      controller.groups = []; 
      expect(controller.showNoOpportunitiesMsg()).toBe(true);
    }); 

  });

})
