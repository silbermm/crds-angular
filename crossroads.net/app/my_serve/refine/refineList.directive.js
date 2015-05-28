'use strict()';
(function() {

  var moment = require('moment');

  module.exports = RefineDirective;

  RefineDirective.$inject = ['$rootScope', 'filterState', 'screenSize']

  function RefineDirective($rootScope, filterState, screenSize) {
    return {
      restrict: "E",
      replace: true,
      templateUrl: "refine/refineList.html",
      scope: {
        "servingDays": "=servingDays",
        "original": "=?original",
        "filterBoxes": "=?filterBoxes",
        "lastDate": "=lastDate"
      },
      link: link
    }

    function link(scope, el, attr) {

      scope.applyFamilyFilter = applyFamilyFilter;
      scope.applySignUpFilter = applySignUpFilter;
      scope.applyTeamFilter = applyTeamFilter;
      scope.applyTimeFilter = applyTimeFilter;
      scope.clearFilters = clearFilters;
      scope.dateOptions = {
        formatYear: 'yy',  
        startingDay: 1,
        showWeeks: 'false'
      };
      scope.datePickers = { fromOpened : false, toOpened: false };
      scope.filterAll = filterAll;
      scope.filterFromDate = formatDate(new Date());
      scope.format = 'MM/dd/yyyy';
      scope.fromDateError = false;
      scope.getUniqueMembers = getUniqueMembers;
      scope.getUniqueSignUps = getUniqueSignUps;
      scope.getUniqueTeams = getUniqueTeams;
      scope.getUniqueTimes = getUniqueTimes;
      scope.isCollapsed = $rootScope.mobile;
      scope.isFilterSet = isFilterSet;
      scope.isFromError = isFromError;
      scope.isToError = isToError;
      scope.openFromDate = openFromDate;
      scope.openToDate = openToDate;
      scope.readyFilterByDate = readyFilterByDate;
      scope.toDateError = false;
      scope.toggleCollapse = toggleCollapse;
      scope.toggleFamilyMember = toggleFamilyMember;
      scope.toggleSignedUp = toggleSignedUp;
      scope.toggleTeam = toggleTeam;
      scope.toggleTime = toggleTime;
      scope.uniqueDays = [];
      scope.uniqueMembers = [];
      scope.uniqueSignUps = [];
      scope.uniqueTeams = [];
      scope.uniqueTimes = [];

      activate();

      screenSize.on('xs, sm', function(match) {
        scope.isCollapsed = match;
      })

      $rootScope.$on("rerunFilters", function(event, data) {
        // Update the entire data with the new data
        scope.servingDays = data;
        initServeArrays();
        filter(data, false);
        $rootScope.$emit("filterDone", scope.servingDays);
      });

      //////////////////////////////////

      function activate() {
        initServeArrays();
        filter(scope.servingDays);
      }

      function applyFamilyFilter() {
        if (filterState.memberIds.length > 0) {
          var serveDay = [];
          _.each(scope.servingDays, function(day) {
            var serveTimes = [];
            _.each(day.serveTimes, function(serveTime) {
              var servingTeams = [];
              _.each(serveTime.servingTeams, function(team) {
                var theTeam = team;
                var members = _.filter(team.members, function(m) {
                  return _.find(filterState.memberIds, function(familyMember) {
                    return familyMember === m.contactId;
                  });
                });

                if (members.length > 0) {
                  theTeam.members = members;
                  servingTeams.push(theTeam);
                }
              });
              if (servingTeams.length > 0) {
                serveTimes.push({
                  time: serveTime.time,
                  'servingTeams': servingTeams
                });
              }
            });
            if (serveTimes.length > 0) {
              serveDay.push({
                day: day.day,
                eventType: day.eventType,
                eventTypeId: day.eventTypeId,
                serveTimes: serveTimes
              });
            }
          });
          if (serveDay.length > 0) {
            scope.servingDays = serveDay;
          }
        }
      }

      function applySignUpFilter() {
        if (filterState.signUps.length > 0) {
          var serveDay = [];
          _.each(scope.servingDays, function(day) {
            var serveTimes = [];
            _.each(day.serveTimes, function(serveTime) {
              var servingTeams = [];
              _.each(serveTime.servingTeams, function(team) {
                var theTeam = team;
                var members = _.filter(team.members, function(m) {
                  return _.find(filterState.signUps, function(signUp) {
                    if ((m.serveRsvp == null) && (signUp.attending == null)) {
                      return true;
                    } else {
                      if (m.serveRsvp != null) {
                        return signUp.attending === m.serveRsvp.attending;
                      } else {
                        return false;
                      }
                    }
                  });
                });
                if (members.length > 0) {
                  theTeam.members = members;
                  servingTeams.push(theTeam);
                }
              });
              if (servingTeams.length > 0) {
                serveTimes.push({
                  time: serveTime.time,
                  'servingTeams': servingTeams
                });
              }
            });
            if (serveTimes.length > 0) {
              serveDay.push({
                day: day.day,
                eventType: day.eventType,
                eventTypeId: day.eventTypeId,
                serveTimes: serveTimes
              });
            }
          });
          scope.servingDays = serveDay;
        }
      }

      function applyTimeFilter() {
        if (filterState.times.length > 0) {
          var serveDay = [];
          _.each(scope.servingDays, function(day) {
            var times = _.filter(day.serveTimes, function(serveTime) {
              return _.find(filterState.times, function(ftimes) {
                return ftimes === serveTime.time;
              });
            });
            if (times.length > 0) {
              serveDay.push({
                day: day.day,
                eventType: day.eventType,
                eventTypeId: day.eventTypeId,
                serveTimes: times
              });
            };
          });
          scope.servingDays = serveDay;
        }
      }

      function applyTeamFilter() {
        if (filterState.teams.length > 0) {
          var serveDay = [];
          _.each(scope.servingDays, function(day) {
            var serveTimes = [];
            _.each(day.serveTimes, function(serveTime) {
              var servingTeams = _.filter(serveTime.servingTeams, function(team) {
                return _.find(filterState.teams, function(t) {
                  return team.groupId === t;
                });
              });
              if (servingTeams.length > 0) {
                serveTimes.push({
                  time: serveTime.time,
                  'servingTeams': servingTeams
                });
              }
            });
            serveDay.push({
              day: day.day,
              eventType: day.eventType,
              eventTypeId: day.eventTypeId,
              serveTimes: serveTimes
            });
          });
          scope.servingDays = serveDay;
        }
      };

      function clearFilters() {
        filterState.clearAll();
        _.each(scope.uniqueMembers, function(member) {
          member.selected = false;
        });
        _.each(scope.uniqueTeams, function(team) {
          team.selected = false;
        });
        _.each(scope.uniqueTimes, function(time) {
          time.selected = false;
        });
        filterAll();
      }

      function filter(data, copyScope = true) {
        filterTimes();
        filterTeams();
        filterFamily();
        getUniqueMembers();
        getUniqueSignUps();
        getUniqueTeams();
        getUniqueTimes();
        initCheckBoxes();
        if (copyScope) {
          scope.original = angular.copy(data);
        }
        filterAll(copyScope);
      }

      function filterAll(copyScope = true) {
        if (copyScope) {
          scope.servingDays = angular.copy(scope.original);
        }
        applyFamilyFilter();
        applySignUpFilter();
        applyTeamFilter();
        applyTimeFilter();
      }

      function filterFamily() {
        _.each(scope.serveTeams, function(serveTeam) {
          _.each(serveTeam.members, function(member) {
            scope.serveMembers.push(member);
          });
        });
      }

      function filterTeams() {
        _.each(scope.times, function(serveTime) {
          _.each(serveTime.servingTeams, function(serveTeam) {
            scope.serveTeams.push(serveTeam);
          });
        });
      }

      function filterTimes() {
        _.each(scope.servingDays, function(servingDay) {
          _.each(servingDay.serveTimes, function(serveTime) {
            scope.times.push(serveTime);
          });
        });
      }

      function getUniqueMembers() {
        scope.uniqueMembers = _.chain(scope.serveMembers).map(function(m) {
          return {
            name: m.name,
            lastName: m.lastName,
            contactId: m.contactId
          };
        }).uniq('contactId').value();
      }

      function getUniqueSignUps() {
        //var signUps = [];
        var yes = {
          'name': 'Yes',
          'id': 1,
          'selected': false,
          'attending': true
        };
        var no = {
          'name': 'No',
          'id': 2,
          'selected': false,
          'attending': false
        };
        var nada = {
          'name': 'Nada',
          'id': 3,
          'selected': false,
          'attending': null
        };
        scope.uniqueSignUps.push(yes);
        scope.uniqueSignUps.push(no);
        scope.uniqueSignUps.push(nada);
      }

      function getUniqueTeams() {
        scope.uniqueTeams = _.chain(scope.serveTeams).map(function(team) {
          return {
            'name': team.name,
            'groupId': team.groupId
          };
        }).uniq('groupId').value();
      }

      function getUniqueTimes() {
        scope.uniqueTimes = _.chain(scope.times).map(function(time) {
          return {
            time: time.time
          };
        }).uniq("time").sortBy(function(n) {
          return n.time;
        }).value();
      }

      function initCheckBoxes() {
        _.each(scope.uniqueMembers, function(member) {
          var found = filterState.findMember(member.contactId);
          if (found !== undefined) {
            member.selected = true;
          }
        });
        _.each(scope.uniqueTeams, function(team) {
          var found = filterState.findTeam(team.groupId);
          if (found !== undefined) {
            team.selected = true;
          }
        });
        _.each(scope.uniqueTimes, function(time) {
          var found = filterState.findTime(time.time);
          if (found !== undefined) {
            time.selected = true;
          }
        });
      }

      function initServeArrays() {
        scope.serveMembers = [];
        scope.serveTeams = [];
        scope.times = [];
      }

      function isFilterSet() {
        return filterState.isActive(); 
      }

      function isFromError(){
        return scope.filterdates.fromdate.$dirty && ( 
          scope.filterdates.fromdate.$error.fromDateToLarge ||
          scope.filterdates.fromdate.$error.date ||
          scope.filterdates.fromdate.$error.required);
      }

      function isToError(){
        return scope.filterdates.todate.$dirty && (
          scope.filterdates.todate.$error.fromDate || scope.filterdates.todate.$error.required || scope.filterdates.todate.$error.date);
      }

      function openFromDate($event) {
        $event.preventDefault();
        $event.stopPropagation();
        scope.datePickers.fromOpened = true;
      }

      function openToDate($event) {
        $event.preventDefault();
        $event.stopPropagation();
        scope.datePickers.toOpened = true;
      }

      /**
       * Takes a javascript date and returns a 
       * string formated MM/DD/YYYY
       * @param date - Javascript Date
       * @param days to add - How many days to add to the original date passed in
       * @return string formatted in the way we want to display
       */
      function formatDate(date, days=0){
        var d = moment(date);
        d.add(days, 'd');
        return d.format('MM/DD/YYYY');
      }

      function readyFilterByDate() {
        var now = moment();
        now.hour(0);
        var toDate = moment(scope.lastDate);
        toDate.hour(23);

        if( now.unix() > toDate.unix() ) {
          scope.filterdates.todate.$error.fromDate = true;
          $rootScope.$emit("notify", $rootScope.MESSAGES.generalError);
          return false;
        } else {
          scope.filterdates.todate.$error.fromDate = false;
        }

        if (scope.lastDate !== undefined && toDate.isValid()){ 
          var fromDate = moment(scope.filterFromDate);
          if (fromDate.isBefore(now, 'days')) {
            fromDate = now;
          }
          if (!scope.filterFromDate){ 
            scope.filterFromDate = now.format('MM/DD/YYYY');  
            fromDate = now;
          } else if (!fromDate.isValid()) {
            scope.filterdates.fromdate.$error.date = true;
            $rootScope.$emit("notify", $rootScope.MESSAGES.generalError);
            return false; 
          } 

          if ( fromDate.isAfter(toDate, 'days' )){
            scope.filterdates.fromdate.$error.fromDateToLarge = true;
            $rootScope.$emit("notify", $rootScope.MESSAGES.generalError);
            return false;
          } else {
            scope.filterdates.fromdate.$error.fromDateToLarge = false;
          } 
          $rootScope.$emit("filterByDates", {'fromDate': fromDate, 'toDate': toDate});
          return true;
        } else if (isToError()) {
          scope.filterdates.todate.$error.date = true;
          $rootScope.$emit("notify", $rootScope.MESSAGES.generalError);
          return false;
        } else {
          return false;  
        }
      }

      function toggleCollapse() {
        if ($rootScope.mobile) {
          scope.isCollapsed = !scope.isCollapsed;
        }
      }

      function toggleFamilyMember(member) {
        if (member.selected) {
          filterState.addFamilyMember(member.contactId);
        } else {
          filterState.removeFamilyMember(member.contactId);
        }
        filterAll();
      }

      function toggleSignedUp(signUp) {
        if (signUp.selected) {
          filterState.addSignUp(signUp);
        } else {
          filterState.removeSignUp(signUp);
        }
        filterAll();
      }

      function toggleTeam(team) {
        if (team.selected) {
          filterState.addTeam(team.groupId);
        } else {
          filterState.removeTeam(team.groupId);
        }
        filterAll();
      }

      function toggleTime(time) {
        if (time.selected) {
          filterState.addTime(time.time);
        } else {
          filterState.removeTime(time.time);
        }
        filterAll();
      }
    }
  }

})()
