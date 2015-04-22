describe('Signup To Serve Controller', function(){

  beforeEach(module('crossroads.mptools'));
  
  var $controller, $log, mockServeResource, $httpBackend;

  beforeEach(inject(function(_$controller_, _$log_, $injector){
    $controller = _$controller_;
    $log = _$log_;
    $httpBackend = $injector.get('$httpBackend');
  }));

  describe('Signup To Serve Controller', function(){ 
    var $scope, controller;

    beforeEach(inject(function($log, $httpBackend){
      $scope = {};
      controller = $controller('SignupToServeController', { $scope: $scope });
      //$httpBackend.expectGET( window.__env__['CRDS_API_ENDPOINT'] + 'api/serve/family-serve-days').respond([ retArray ]);
    })); 

    it("should ...", function(){

    });

  })
});
