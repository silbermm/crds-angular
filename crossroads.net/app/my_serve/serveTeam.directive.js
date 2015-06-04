"use strict()";
(function() {

  module.exports = ServeTeam;

  ServeTeam.$inject = ['$rootScope', '$log', 'Session', 'ServeOpportunities', 'Capacity', '$modal'];

  function ServeTeam($rootScope, $log, Session, ServeOpportunities, Capacity, $modal) {
    return {
      restrict: "EA",
      transclude: true,
      templateUrl: "my_serve/serveTeam.html",
      replace: true,
      scope: {
        team: '=',
        opportunity: '=',
        oppServeDate: '='
      },
      link: link
    };

    function link(scope, el, attr) {

      scope.closePanel = closePanel;
      scope.currentActiveTab = null;
      scope.currentMember = null;
      scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        showWeeks: 'false'
      };
      scope.datePickers = {fromOpened : false, toOpened: false };
      scope.displayEmail = displayEmail;
      scope.editProfile = editProfile;
      scope.frequency = getFrequency();
      scope.format = 'MM/dd/yyyy';
      scope.formErrors = {
        role: false,
        signup: false,
        frequency: false,
        from: false,
        to: false,
        dateRange: false
      };
      scope.populateDates = populateDates;
      scope.isActiveTab = isActiveTab;
      scope.isCollapsed = true;
      scope.isFormValid = isFormValid;
      scope.modalInstance = {};
      scope.openFromDate = openFromDate;
      scope.openToDate = openToDate;
      scope.openPanel = openPanel;
      scope.roleChanged = roleChanged;
      scope.roles = null;
      scope.saveRsvp = saveRsvp;
      scope.selectedRole = null;
      scope.setActiveTab = setActiveTab;
      scope.signedup = null;
      scope.showEdit = false;
      scope.showIcon = showIcon;
      scope.togglePanel = togglePanel;
      //////////////////////////////////////

      function allowProfileEdit() {
        var cookieId = Session.exists("userId");
        if (cookieId !== undefined) {
          scope.showEdit = Number(cookieId) === scope.currentMember.contactId;
        } else {
          scope.showEdit = false;
        }
      };

      function changeFromDate() {
        if(scope.currentMember.currentOpportunity !== undefined && scope.currentMember.currentOpportunity.fromDt !== undefined){
          var m = moment( scope.currentMember.currentOpportunity.fromDt );
          if(m.isValid()){
            scope.formErrors.dateRange = false;
            scope.formErrors.from = false;
          }
        }
      }
      
      function changeToDate() {
        if(scope.currentMember.currentOpportunity !== undefined && scope.currentMember.currentOpportunity.toDt !== undefined){
          var m = moment( scope.currentMember.currentOpportunity.toDt );
          if(m.isValid()){
            scope.formErrors.dateRange = false;
            scope.formErrors.to = false;
          }
        }

      }

      function closePanel() {
        scope.isCollapsed = true;
      }

      function displayEmail(emailAddress) { 
        if (!emailAddress) {
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
        var weeklyLabel = moment(scope.oppServeDate).format("dddd") + "s" + " " + dateTime.format("h:mma");

        var once = {
          value: 0,
          text: "Once " + dateTime.format("M/D/YYYY h:mma")
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

      function isActiveTab(memberName) {
        return memberName === scope.currentActiveTab;
      };

      function isFormValid() {
        var validForm = {
          valid: true
        };
        validForm.valid = true;
        if (scope.currentMember.serveRsvp == null) {
          validForm.valid = false;
          scope.formErrors.role = true;   
        } else if (scope.currentMember.serveRsvp.roleId === undefined || scope.currentMember.serveRsvp.roleId === null || scope.currentMember.currentOpportunity === null) {
          validForm.valid = false;
          scope.formErrors.frequency = true;
        } else {
          if ( (scope.currentMember.currentOpportunity === undefined || 
                scope.currentMember.currentOpportunity === null || 
                scope.currentMember.currentOpportunity.frequency === null ||  
                scope.currentMember.currentOpportunity.frequency === undefined) && scope.currentMember.showFrequency 
           ) {
            validForm.valid = false;
            scope.formErrors.frequency = true;
          } 

          if(scope.currentMember.currentOpportunity !== undefined && scope.currentMember.currentOpportunity.toDt === undefined){
            validForm.valid = false;
            scope.formErrors.to = true; 
          } 

          if(scope.currentMember.currentOpportunity !== undefined && scope.currentMember.currentOpportunity.fromDt === undefined){
            validForm.valid = false;
            scope.formErrors.from = true; 
          }

          if(validForm.valid) {
            try {
              var startDate = parseDate(scope.currentMember.currentOpportunity.toDt);
            } catch(ex) {
              validForm.valid = false;
              scope.formErrors.from = true; 
            }

            try { 
              var endDate = parseDate(scope.currentMember.currentOpportunity.fromDt);
            } catch(ex) {
              validForm.valid = false;
              scope.formErrors.to = true; 
            }

            if (startDate < endDate) {
              validForm.valid = false;
              scope.formErrors.dateRange = true; 
            }
          }
        }
        return validForm;
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

      function openPanel(members) {
        if (scope.currentMember === null) {
          var sessionId = Number(Session.exists("userId"));
          scope.currentMember = members[0];
          scope.currentActiveTab = scope.currentMember.name;
        }
        _.each(scope.currentMember.roles, function(r){
          r.capacity = Capacity.get({id: r.roleId, eventId: scope.team.eventId, min: r.minimum, max: r.maximum});
        });
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
              scope.formErrors.frequency = false;
              scope.currentMember.currentOpportunity.fromDt = scope.oppServeDate;
              scope.currentMember.currentOpportunity.toDt = scope.oppServeDate;
              break;
            default:
              // every  or everyother
              scope.formErrors.frequency = false;
              var roleId =  (scope.currentMember.serveRsvp.roleId === 0) ? scope.currentMember.roles[0].roleId : scope.currentMember.serveRsvp.roleId;
              ServeOpportunities.LastOpportunityDate.get({
                id: roleId
              }, function(ret) {
                var dateNum = Number(ret.date * 1000);
                var toDate = new Date(dateNum);
                scope.currentMember.currentOpportunity.toDt = (toDate.getMonth() + 1) + "/" + toDate.getDate() + "/" + toDate.getFullYear();
              });
              break;
          }
        }
      }

      function roleChanged(selectedRole) {
        console.log(selectedRole);
        scope.formErrors.role = false;
        scope.selectedRole = selectedRole;
        if (scope.currentMember.serveRsvp === undefined) {
          scope.currentMember.serveRsvp = {
            isSaved: false
          };
        } else {
          scope.currentMember.serveRsvp.isSaved = false;
        }
        if (scope.selectedRole === undefined) {
          scope.currentMember.serveRsvp.attending = false ;
        } else {
          scope.currentMember.serveRsvp.attending = true;
        }
        scope.currentMember.showFrequency = true;
      }

      function saveRsvp() {
        var validForm = isFormValid();
 
        if (!validForm.valid) {
          $rootScope.$emit('notify',$rootScope.MESSAGES.generalError);
          return false;
        }
        var rsvp = new ServeOpportunities.SaveRsvp();
        rsvp.contactId = scope.currentMember.contactId;
        rsvp.opportunityId = scope.currentMember.serveRsvp.roleId;
        rsvp.opportunityIds = _.map(scope.currentMember.roles, function(role){ return role.roleId; });;
        rsvp.eventTypeId = scope.team.eventTypeId;
        rsvp.endDate = parseDate(scope.currentMember.currentOpportunity.toDt);
        rsvp.startDate = parseDate(scope.currentMember.currentOpportunity.fromDt);
        if (scope.currentMember.serveRsvp.roleId !==0 ) {
          rsvp.signUp = true;
        } else {
          rsvp.signUp = false;
        }
        rsvp.alternateWeeks = (scope.currentMember.currentOpportunity.frequency.value === 2);
        rsvp.$save(function(saved) {
          $rootScope.$emit("notify", $rootScope.MESSAGES.serveSignupSuccess);
          scope.currentMember.serveRsvp.isSaved = true;
          return true;
        }, function(err){
          $rootScope.$emit("notify", $rootScope.MESSAGES.generalError);
          return false; 
        });
      }

      function setActiveTab(member) {
        // Reset form errors 
        scope.formErrors = {
          role: false,
          signup: false,
          frequency: false,
          from: false,
          to: false
        };
        scope.currentActiveTab = member.name;
        if (scope.currentMember === null || member === scope.currentMember) {
          scope.togglePanel();
        } else if (member !== scope.currentMember && scope.isCollapsed) {
          scope.togglePanel();
        }
        scope.currentMember = member;
        _.each(scope.currentMember.roles, function(r){
          r.capacity = Capacity.get({id: r.roleId, eventId: scope.team.eventId, min: r.minimum, max: r.maximum});
        });
        allowProfileEdit();
      }

      function showIcon(member) {
        if (member.serveRsvp === undefined || member.serveRsvp === null) {
          return false;
        } else {
          scope.selectedRole = _.find(member.roles, function(r) {
            return r.roleId === member.serveRsvp.roleId;
          });
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
