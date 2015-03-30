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
      scope.applyTeamFilter = applyTeamFilter;
      scope.applyTimeFilter = applyTimeFilter;
      scope.filterAll = filterAll;
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
          filterAll();
        }); 
      }

      function applyFamilyFilter(){
        if(filterState.memberIds.length > 0){
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

      }

      function applyTimeFilter(){
        if(filterState.times.length > 0){
          var serveDay = [];
          _.each(scope.servingDays, function(day){
            var times = _.filter(day.serveTimes, function(serveTime){
              return _.find(filterState.times, function(ftimes){
                return ftimes === serveTime.time;
              });
            });
            if(times.length > 0) {
              serveDay.push({day: day.day, serveTimes: times}); 
            };
          });
          scope.servingDays = serveDay;
        }
      }

      function applyTeamFilter(){
        if(filterState.teams.length > 0){
          var serveDay = [];
          _.each(scope.servingDays, function(day){
            var serveTimes = [];
            _.each(day.serveTimes, function(serveTime){
              var servingTeams = _.filter(serveTime.servingTeams, function(team){
                return _.find(filterState.teams, function(t){
                  return team.groupId === t;
                });
              });
              if(servingTeams.length > 0) {
                serveTimes.push({time: serveTime.time, 'servingTeams':servingTeams }); 
              }
            });
            serveDay.push({day: day.day, serveTimes: serveTimes}); 
          });
          scope.servingDays = serveDay;
        }
      };

      function filterAll(){
        scope.servingDays = angular.copy(scope.original);
        applyFamilyFilter();
        applyTeamFilter();
        applyTimeFilter();
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

      function getUniqueTimes(){
        scope.uniqueTimes = _.chain(scope.times).map(function(time) {
          return {time: time.time};
        }).uniq("time").sortBy(function(n){
          return n.time;
        }).value();
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
        filterAll();
      }

      function toggleTeam(team){
        if(team.selected){
          filterState.addTeam(team.groupId);
        } else {
          filterState.removeTeam(team.groupId);
        }
        filterAll();
      }
 
      function toggleTime(time){
        if(time.selected){
          filterState.addTime(time.time);
        } else {
          filterState.removeTime(time.time);
        }
        filterAll(); 
      }
    }
  }


})()
