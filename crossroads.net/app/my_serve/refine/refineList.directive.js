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
      
      scope.getUniqueMembers = getUniqueMembers;
      scope.serveMembers = [];
      scope.serveTeams = [];
      scope.times = [];
      scope.uniqueDays = [];
      scope.uniqueMembers = [];

      activate();
      //////////////////////////////////
    
      function activate(){
        debugger;
        filterTimes();
        filterTeams();
        filterFamily();
        getUniqueMembers();
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

      function getUniqueMembers(){
        var uniqueMembers = [];
        _.each(scope.serveMembers, function(member){
          if (uniqueMembers.length < 1){
            uniqueMembers.push(member);
          } else {
            var el = _.find(uniqueMembers, function(f){
              return member.contactId === f.contactId;
            });
            if(el === undefined) 
              uniqueMembers.push(member);
          }
        });
        scope.uniqueMembers = _.map(uniqueMembers, function(m){
          return {name: m.name, contactId: m.contactId};
        });
      };
      
    }
  }


})()
