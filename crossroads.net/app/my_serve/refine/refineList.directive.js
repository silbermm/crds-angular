'use strict()';
(function(){

  module.exports = RefineDirective;

  function RefineDirective(){
    return {
      restrict: "E",
      replace: true,
      templateUrl: "refine/refineList.html",
      scope: {
        "servingDays": "=servingDays"
      },
      link : link
    }

    function link(scope, el, attr){
        
      scope.serveMembers = [];
      scope.serveTeams = [];
      scope.times = [];
      scope.uniqueDays = [];


      activate();
      //////////////////////////////////
    
      function activate(){
        filterTimes();
        filterTeams();
        filterFamily();
      }

      function filterFamily(){
        _.each(scope.serveTeams, function(serveTeam){
          _.each(serveTeam.members, function(member){
            scope.serveMembers.push(member);
          });
        });
      }

      function filterTeams(){
        _.each(scope.times, function(serveTime){
          _.each(serveTime.servingTeams, function(serveTeam){
            scope.serveTeams.push(serveTeam);
          });
        });
      }

      function filterTimes(){
        _.each(scope.servingDays, function(servingDay){
          _.each(servingDay.serveTimes, function(serveTime){
            scope.times.push(serveTime);
          });
        }); 
      }

      
    }
  }


})()
