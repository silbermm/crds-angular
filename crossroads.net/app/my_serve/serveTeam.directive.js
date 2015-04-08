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
        oppServeDate: '='
      },
      link : link
    };

    function link(scope, el, attr) {

      scope.closePanel = closePanel;
      scope.currentActiveTab = null;
      scope.currentMember = null;
      scope.isActiveTab = isActiveTab;
      scope.isCollapsed = true;
      scope.isSignedUp = isSignedUp;
      scope.openPanel = openPanel;
      scope.panelId = getPanelId;
      scope.roles = null;
      scope.setActiveTab = setActiveTab;
      scope.signedup = null;
      scope.editProfile = editProfile;
      scope.modalInstance = {};
      scope.showEdit = false;
      scope.frequency = [{value:0, text:"Once (12/16/14 8:30am)"}, {value:1, text:"Every Week (Sundays 8:30am)"}, {value:2, text:"Every Other Week (Sundays 8:30am)"}];
      scope.format = 'MM/dd/yyyy';
      scope.open = open;
      scope.getLastDate = getLastDate;
      scope.dateOptions = {formatYear: 'yy',startingDay: 1};
      scope.fromDt = scope.oppServeDate;

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

      function getLastDate(){
        if(scope.currentMember === null)
        {
          console.log("You did it wrong.");
          return false;
        }
        scope.toDt = ServeOpportunities.LastOpportunityDate({id:currentMember.currentOpportunity.roleId})
        return true;
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

      function setActiveTab(member){
        scope.currentActiveTab = member.name;
        scope.currentMember =  member;
        scope.isCollapsed = false;
        allowProfileEdit();
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

          modalInstance.result.then(function(person){
            personToEdit.name = person.nickName===null?person.firstName:person.nickName;
            $rootScope.$emit("personUpdated", person);
          }, function(){
            console.log("canceled");
          });
      };
    };

  }

})();
