"use strict()";

(function(){
  var moment = require('moment');

  module.exports = ServeTeam;

  ServeTeam.$inject = ['$rootScope', '$log', 'Session', 'ServeOpportunities', '$modal'];

  function ServeTeam($rootScope,$log,Session, ServeOpportunities, $modal){
    return {
      restrict: "EA",
      transclude: true,
      templateUrl : "my_serve/serveTeam.html",
      replace: true,
      scope : {
        team: '=',
        opportunity: '=',
        teamIndex: '=',
        tabIndex: '=',
        dayIndex: '=',
        oppServeDate: '=',
        eventTypeId: '=?'
      },
      link : link
    };

    function link(scope, el, attr) {

      scope.closePanel = closePanel;
      scope.currentActiveTab = null;
      scope.currentMember = null;
      scope.dateOptions = {formatYear: 'yy',startingDay: 1, showWeeks: 'false'};
      scope.editProfile = editProfile;
      scope.frequency = [{value:0, text:"Once (12/16/14 8:30am)"}, {value:1, text:"Every Week (Sundays 8:30am)"}, {value:2, text:"Every Other Week (Sundays 8:30am)"}];
      scope.format = 'MM/dd/yyyy';
      scope.populateDates = populateDates;
      scope.isActiveTab = isActiveTab;
      scope.isCollapsed = true;
      scope.isSignedUp = isSignedUp;
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

      function activate(){
      }

      function allowProfileEdit() {
        var cookieId = Session.exists("userId");
        if(cookieId !== undefined){
          scope.showEdit = Number(cookieId) === scope.currentMember.contactId;
        }
        else {
        scope.showEdit = false;
      }
      };

      function closePanel(){
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
            person : function(){
              return personToEdit;
            }
          }
        });
        modalInstance.result.then(function (person) {
            personToEdit.name = person.nickName === null ? person.firstName : person.nickName;
            $rootScope.$emit("personUpdated", person);
        });
      };

      function populateDates(){
        if(scope.currentMember !== null){
          scope.currentMember.currentOpportunity.fromDt = scope.oppServeDate;
          switch(scope.currentMember.currentOpportunity.frequency.value) {
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
              ServeOpportunities.LastOpportunityDate.get({id:scope.currentMember.currentOpportunity.roleId}, function(ret){
                var dateNum = Number(ret.date * 1000); 
                var toDate = new Date(dateNum);
                scope.currentMember.currentOpportunity.toDt = (toDate.getMonth() + 1) + "/" + toDate.getDate() + "/" + toDate.getFullYear();
              });
              break;
          }
        }
      }

      function getPanelId(){
        return "team-panel-" + scope.dayIndex + scope.tabIndex + scope.teamIndex;
      }

      function isActiveTab(memberName){
        return memberName === scope.currentActiveTab;
      };


      function isSignedUp(opportunity){
        if(scope.currentMember === undefined){
          return false;
        } else {
          return _.find(opportunity.members, function(m){
            return m.name === scope.currentMember.name && m.signedup === 'yes';
          });
        }
      }

      function open($event, opened){
        $event.preventDefault();
        $event.stopPropagation();
        scope[opened] = true;
      }

      function openPanel(members){
        if(scope.currentMember === null){
          var sessionId = Number(Session.exists("userId"));
          scope.currentMember = members[0];
          scope.currentActiveTab = scope.currentMember.name;
        }
        $log.debug("isCollapsed = " + scope.isCollapsed);
        scope.isCollapsed = !scope.isCollapsed;
        allowProfileEdit();
      }

      function parseDate(stringDate){
        // Date we are parsing looks like this 4/3/2008
        var dateArr = stringDate.split("/");
        var d = moment(dateArr[2] + "-" + dateArr[0] + "-" + dateArr[1]);  
        return d.format('X');
      }

      function saveRsvp(){
        var saveRsvp = new ServeOpportunities.SaveRsvp();
        saveRsvp.contactId = scope.currentMember.contactId;
        saveRsvp.opportunityId = scope.currentMember.currentOpportunity.roleId;
        saveRsvp.eventTypeId = scope.eventTypeId;
        saveRsvp.endDate = parseDate(scope.currentMember.currentOpportunity.toDt);
        saveRsvp.startDate = parseDate(scope.currentMember.currentOpportunity.fromDt);
        saveRsvp.$save();
      }

      function setActiveTab(member){
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
