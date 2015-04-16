"use strict()";

(function() {
  var moment = require('moment');

  module.exports = ServeTeam;

  ServeTeam.$inject = ['$rootScope', '$log', 'Session', 'ServeOpportunities', '$modal'];

  function ServeTeam($rootScope, $log, Session, ServeOpportunities, $modal) {
    return {
      restrict: "EA",
      transclude: true,
      templateUrl: "my_serve/serveTeam.html",
      replace: true,
      scope: {
        team: '=',
        opportunity: '=',
        teamIndex: '=',
        tabIndex: '=',
        dayIndex: '=',
        oppServeDate: '='
      },
      link: link
    };

    function link(scope, el, attr) {
      scope.attendingChanged = attendingChanged;
      scope.closePanel = closePanel;
      scope.currentActiveTab = null;
      scope.currentMember = null;
      scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        showWeeks: 'false'
      };
      scope.editProfile = editProfile;
      scope.frequency = [{
        value: 0,
        text: "Once"
      }, {
        value: 1,
        text: "Every Week"
      }, {
        value: 2,
        text: "Every Other Week"
      }];
      scope.format = 'MM/dd/yyyy';
      scope.populateDates = populateDates;
      scope.isActiveTab = isActiveTab;
      scope.isCollapsed = true;
      scope.modalInstance = {};
      scope.open = open;
      scope.openPanel = openPanel;
      scope.panelId = getPanelId;
      scope.roles = null;
      scope.saveRsvp = saveRsvp;
      scope.setActiveTab = setActiveTab;
      scope.signedup = null;
      scope.showEdit = false;

      scope.togglePanel = togglePanel;

      activate();
      //////////////////////////////////////

      function activate() {}

      function attendingChanged() {
        scope.currentMember.showFrequency = true;
      }

      function allowProfileEdit() {
        var cookieId = Session.exists("userId");
        if (cookieId !== undefined) {
          scope.showEdit = Number(cookieId) === scope.currentMember.contactId;
        } else {
          scope.showEdit = false;
        }
      };

      function closePanel() {
        scope.isCollapsed = true;
      }

      function editProfile(personToEdit) {
        var modalInstance = $modal.open({
          templateUrl: 'profile/editProfile.html',
          backdrop: true,
          controller: "ProfileModalController as modal",
          // This is needed in order to get our scope
          // into the modal - by default, it uses $rootScope
          scope: scope,
          resolve: {
            person: function() {
              return personToEdit;
            }
          }
        });
        modalInstance.result.then(function(person) {
          personToEdit.name = person.nickName === null ? person.firstName : person.nickName;
          $rootScope.$emit("personUpdated", person);
        });
      };

      function populateDates() {
        if (scope.currentMember !== null) {
          scope.currentMember.currentOpportunity.fromDt = scope.oppServeDate;
          switch (scope.currentMember.currentOpportunity.frequency.value) {
            case null:
              scope.currentMember.currentOpportunity.fromDt = null;
              scope.currentMember.currentOpportunity.toDT = null;
              break;
            case 0:
              // once...
              scope.currentMember.currentOpportunity.fromDt = scope.oppServeDate;
              scope.currentMember.currentOpportunity.toDt = scope.oppServeDate;
              break;
            default:
              // every  or everyother
              ServeOpportunities.LastOpportunityDate.get({
                id: scope.currentMember.currentOpportunity.roleId
              }, function(ret) {
                var dateNum = Number(ret.date * 1000);
                var toDate = new Date(dateNum);
                scope.currentMember.currentOpportunity.toDt = (toDate.getMonth() + 1) + "/" + toDate.getDate() + "/" + toDate.getFullYear();
              });
              break;
          }
        }
      }

      function getPanelId() {
        return "team-panel-" + scope.dayIndex + scope.tabIndex + scope.teamIndex;
      }

      function isActiveTab(memberName) {
        return memberName === scope.currentActiveTab;
      };
 
      function open($event, opened) {
        $event.preventDefault();
        $event.stopPropagation();
        scope[opened] = true;
      }

      function openPanel(members) {
        if (scope.currentMember === null) {
          var sessionId = Number(Session.exists("userId"));
          scope.currentMember = members[0];
          scope.currentActiveTab = scope.currentMember.name;
        }
        $log.debug("isCollapsed = " + scope.isCollapsed);
        scope.isCollapsed = !scope.isCollapsed;
        allowProfileEdit();
      }

      function parseDate(stringDate) {
        var m = moment(stringDate);

        if (!m.isValid()) {
          var dateArr = stringDate.split("/");
          var dateStr = dateArr[2] + " " + dateArr[0] + " " + dateArr[1];
          // https://github.com/moment/moment/issues/1407
          // moment("2014 04 25", "YYYY MM DD"); // string with format
          m = moment(dateStr, "YYYY MM DD");

          if (!m.isValid()) {
            //throw error
            throw new Error("Parse Date Failed Moment Validation");
          }
        }
        $log.debug('date: ' + m.format('X'));
        return m.format('X');
      }

      function saveRsvp() {
        var saveRsvp = new ServeOpportunities.SaveRsvp();
        saveRsvp.contactId = scope.currentMember.contactId;
        saveRsvp.opportunityId = scope.currentMember.serveRsvp.roleId;
        saveRsvp.eventTypeId = scope.team.eventTypeId;
        saveRsvp.endDate = parseDate(scope.currentMember.currentOpportunity.toDt);
        saveRsvp.startDate = parseDate(scope.currentMember.currentOpportunity.fromDt);
        saveRsvp.signUp = (scope.currentMember.currentOpportunity.signedup === "1");
        saveRsvp.alternateWeeks = (scope.currentMember.currentOpportunity.frequency.value === 2);
        saveRsvp.$save(function(saved){
          $rootScope.$emit("notify", $rootScope.MESSAGES.serveSignupSuccess );           
        });
      }

      function setActiveTab(member) {
        scope.currentActiveTab = member.name;
        if (scope.currentMember === null || member === scope.currentMember) {
          scope.togglePanel();
        } else if (member !== scope.currentMember && scope.isCollapsed) {
          scope.togglePanel();
        }
        scope.currentMember = member;
        allowProfileEdit();
      }

      function togglePanel() {
        scope.isCollapsed = !scope.isCollapsed;
      };
    };
  }
})();
