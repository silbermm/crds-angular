describe('MyServeController', function() {

  beforeEach(module('crossroads'));
  
  var $controller, $log, ServeOpportunities;
   
  var retArray = { "day": "Saturday, November 16, 2014", "opportunities" : [ { "time": "8:30am", "name" : "Kids Club Nusery", "members" : [ { "name": "John", "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ] }, { "name":  "Jane", "roles" : [ {"name": "NuseryA"}, {"name": "NuseryB"}, {"name": "NuseryC"}, {"name": "NuseryD"} ], "signedup" : "yes" }, ] }, { "time": "8:30am", "name": "First Impressions", "members" : [ { "name": "John"}, {"name": "Jane" } ] } ] }
  
  beforeEach(inject(function(_$controller_, _$log_, _ServeOpportunities_){
    $controller = _$controller_;
    $log = _$log_;
    ServeOpportunities = _ServeOpportunities_;
  }));

  describe('Serve Controller', function(){ 
    var $scope, controller, ServeOpportunities;
    beforeEach(inject(function($log){
      $scope = {};
      controller = $controller('MyServeController', { $scope: $scope });
    }));


    it('should get a list of groups', function(){ 
      expect(controller.groups[0].day).toBe("Saturday, November 16, 2014"); 
    });

    it('should default to todays date', function(){
      var today = new Date();
      expect(controller.dt.getDate()).toBe(today.getDate());
      expect(controller.dt.getDay()).toBe(today.getDay());
      expect(controller.dt.getFullYear()).toBe(today.getFullYear());
    });



  });

})
