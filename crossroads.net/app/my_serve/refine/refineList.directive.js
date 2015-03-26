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
      scope.getUniqueTeams = getUniqueTeams;
      scope.resolvedData = [];
      scope.serveMembers = [];
      scope.serveTeams = [];
      scope.times = [];
      scope.uniqueDays = [];
      scope.uniqueMembers = [];
      scope.uniqueTeams = [];

      activate();
      //////////////////////////////////
    
      function activate(){
       scope.servingDays.$promise.then(function(data) {
          filterTimes();
          filterTeams();
          filterFamily();
          getUniqueMembers();
          getUniqueTeams();
        }); 
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
        scope.uniqueMembers = _.chain(scope.serveMembers).map(function(m){
          return {name: m.name, lastName: m.lastName, contactId: m.contactId};
        }).uniq('contactId').value();
      }

      function getUniqueTeams(){
        scope.uniqueTeams = _.chain(scope.serveTeams).map(function(team){
          return { 'name': team.name, 'groupId': team.groupId };
        }).uniq('groupId').value();
      } 
    }
  }


})()
