'use strict()';
(function(){

  module.exports = RefineDirective;

  RefineDirective.$inject = ['filterState']

  function RefineDirective(filterState){
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
    
      scope.applyFamilyFilter = applyFamilyFilter;
      scope.getUniqueMembers = getUniqueMembers;
      scope.getUniqueTeams = getUniqueTeams;
      scope.getUniqueTimes = getUniqueTimes;
      scope.resolvedData = [];
      scope.serveMembers = [];
      scope.serveTeams = [];
      scope.times = [];
      scope.toggleFamilyMember = toggleFamilyMember;
      scope.toggleTeam = toggleTeam;
      scope.toggleTime = toggleTime;
      scope.uniqueDays = [];
      scope.uniqueMembers = [];
      scope.uniqueTeams = [];
      scope.uniqueTimes = [];

      activate();
      //////////////////////////////////
    
      function activate(){
       scope.servingDays.$promise.then(function(data) {
          filterTimes();
          filterTeams();
          filterFamily();
          getUniqueMembers();
          getUniqueTeams();
          getUniqueTimes();
          initCheckBoxes();
          scope.original = angular.copy(data);
        }); 
      }

      function applyFamilyFilter(){
        scope.servingDays = angular.copy(scope.original);
        if(filterState.memberIds.length === 0){
          console.log("nothing to filter, returning all"); 
        } else { 
          var serveDay = [];
          _.each(scope.servingDays, function(day){
            var serveTimes = [];
            _.each(day.serveTimes, function(serveTime){
              var servingTeams = [];      
              _.each(serveTime.servingTeams, function(team){
                var theTeam = team;
                var members = _.filter(team.members, function(m){
                  return _.find(filterState.memberIds, function(familyMember){
                    return familyMember === m.contactId; 
                  }); 
                }); 
                if(members.length > 0) {
                  theTeam.members = members;
                  servingTeams.push(theTeam);
                }
              });
              serveTimes.push({time: serveTime.time, 'servingTeams':servingTeams }); 
            });
            serveDay.push({day: day.day, serveTimes: serveTimes}); 
          });
          scope.servingDays = serveDay;
        }

      };

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

      function getUniqueTimes(){
        scope.uniqueTimes = _.chain(scope.times).map(function(time) {
          return {time: time.time};
        }).uniq("time").value();
      }  

      function initCheckBoxes(){
        _.each(scope.uniqueMembers, function(member){
          var found = filterState.findMember(member.contactId);
          if (found !== undefined)
          {
            member.selected = true;
          }
        });
        _.each(scope.uniqueTeams, function(team){
          var found = filterState.findTeam(team.groupId);
          if (found !== undefined)
          {
            team.selected = true;
          }
        });
        _.each(scope.uniqueTimes, function(time){
          var found = filterState.findTime(time.time);
          if (found !== undefined)
          {
            time.selected = true;
          }
        });

      }

      function toggleFamilyMember(member){
        if(member.selected){
          filterState.addFamilyMember(member.contactId);
        } else {
          filterState.removeFamilyMember(member.contactId);
        }
        applyFamilyFilter();
      }

      function toggleTeam(team){
        if(team.selected){
          filterState.addTeam(team.groupId);
        } else {
          filterState.removeTeam(team.groupId);
        }
      }
 
      function toggleTime(time){
        if(time.selected){
          filterState.addTime(time.time);
        } else {
          filterState.removeTime(time.time);
        }
      }
    }
  }


})()
