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
        
      scope.filterDays = filterDays;
      scope.filterFamily = filterFamily; 
      scope.filterTimes = filterTimes;   

      activate();
      //////////////////////////////////
    
      function activate(){
        filterDays();
        filterTimes();
      }


      function filterFamily(){
        var family = _.each(scope.servingDays, function(servingDay){ 
          _.each(servingDay.serveTimes, function(serveTime){
            _.each(serveTime.servingTeams, function(serveTeam){
              _.each(serveTeam.members, function(member){
                return { name: member.name, contactId: member.contactId }
              });
            });
          }); 
        }); 
        return family;
      }

      function filterDays(){
        scope.days = _.each(scope.servingDays, function(servingDay){
          return servingDay;
        }); 
      }

      function filterTimes(){
        scope.times = _.each(scope.days, function(servingTime){
          return servingTime;
        });
      }

    }
  }


})()
