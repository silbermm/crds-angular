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
      scope.displayEmail = displayEmail;
      scope.editProfile = editProfile;
      scope.frequency = getFrequency();
      scope.format = 'MM/dd/yyyy';
      scope.populateDates = populateDates;
      scope.isActiveTab = isActiveTab;
      scope.isCollapsed = true;
      scope.isFormValid = isFormValid;
      scope.modalInstance = {};
      scope.open = open;
      scope.openPanel = openPanel;
      scope.panelId = getPanelId;
      scope.roleChanged = roleChanged;
      scope.roles = null;
      scope.saveRsvp = saveRsvp;
      scope.setActiveTab = setActiveTab;
      scope.signedup = null;
      scope.showEdit = false;
      scope.showIcon = showIcon;

      scope.togglePanel = togglePanel;

      activate();
      //////////////////////////////////////

      function activate() {
        _.each(scope.team.members, function(m) {

        });
      }


      function attendingChanged() {
        roleChanged();
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

      function displayEmail(emailAddress) {
        if (emailAddress == undefined) {
          return false;
        }
        if (emailAddress.length > 0) {
          return true;
        }
        return false;
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

      function getFrequency() {
        var dateTime = moment(scope.oppServeDate + " " + scope.opportunity.time);
        var weeklyLabel = moment(scope.oppServeDate).format("dddd") + "s" + " " + dateTime.format("h:ma");

        var once = {
          value: 0,
          text: "Once " + dateTime.format("M/D/YYYY h:ma")
        };
        var everyWeek = {
          value: 1,
          text: "Every Week " + weeklyLabel
        };
        var everyOtherWeek = {
          value: 2,
          text: "Every Other Week " + weeklyLabel
        };

        return [once, everyWeek, everyOtherWeek];
      }

      function getPanelId() {
        return "team-panel-" + scope.dayIndex + scope.tabIndex + scope.teamIndex;
      }

      function isActiveTab(memberName) {
        return memberName === scope.currentActiveTab;
      };

      function isFormValid() {
        var validForm = {
          valid: true,
          messageStr: ''
        };
        validForm.valid = true;
        if (scope.currentMember.serveRsvp == null) {
          validForm.valid = false;
          validForm.messageStr = $rootScope.MESSAGES.selectSignUpAndFrequency;
        } else if (scope.currentMember.serveRsvp.attending == undefined) {
          validForm.valid = false;
          validForm.messageStr = $rootScope.MESSAGES.selectSignUpAndFrequency;
        } else if (scope.currentMember.currentOpportunity == null) {
          validForm.valid = false;
          validForm.messageStr = $rootScope.MESSAGES.selectFrequency;
        } else {
          var startDate = parseDate(scope.currentMember.currentOpportunity.toDt);
          var endDate = parseDate(scope.currentMember.currentOpportunity.fromDt);

          if (startDate < endDate) {
            validForm.valid = false;
            validForm.messageStr = $rootScope.MESSAGES.invalidDateRange;
          }
        }


        return validForm;
      }

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
                id: scope.currentMember.serveRsvp.roleId
              }, function(ret) {
                var dateNum = Number(ret.date * 1000);
                var toDate = new Date(dateNum);
                scope.currentMember.currentOpportunity.toDt = (toDate.getMonth() + 1) + "/" + toDate.getDate() + "/" + toDate.getFullYear();
              });
              break;
          }
        }
      }

      function roleChanged() {
        if (scope.currentMember.serveRsvp === undefined) {
          scope.currentMember.serveRsvp = {
            isSaved: false
          };
        } else {
          scope.currentMember.serveRsvp.isSaved = false;
        }
      }

      function saveRsvp() {
        //var invalid = false; //make this a function
        var validForm = isFormValid();
        if (validForm.valid == false) {
          $rootScope.$emit('notify', validForm.messageStr);
          return;
        }

        var saveRsvp = new ServeOpportunities.SaveRsvp();
        saveRsvp.contactId = scope.currentMember.contactId;
        saveRsvp.opportunityId = scope.currentMember.serveRsvp.roleId;
        saveRsvp.eventTypeId = scope.team.eventTypeId;
        saveRsvp.endDate = parseDate(scope.currentMember.currentOpportunity.toDt);
        saveRsvp.startDate = parseDate(scope.currentMember.currentOpportunity.fromDt);
        saveRsvp.signUp = scope.currentMember.serveRsvp.attending;
        saveRsvp.alternateWeeks = (scope.currentMember.currentOpportunity.frequency.value === 2);
        saveRsvp.$save(function(saved) {
          $rootScope.$emit("notify", $rootScope.MESSAGES.serveSignupSuccess);
          $rootScope.$broadcast('update.member', scope.currentMember);
          scope.currentMember.serveRsvp.isSaved = true;
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

      function showIcon(member) {
        if (member.serveRsvp === undefined) {
          return false;
        } else {
          if (member.serveRsvp !== null && (member.serveRsvp.isSaved || member.serveRsvp.isSaved === undefined)) {
            return true;
          } else {
            return false;
          }
        }
      }

      function togglePanel() {
        scope.isCollapsed = !scope.isCollapsed;
      };
    };
  }
})();
