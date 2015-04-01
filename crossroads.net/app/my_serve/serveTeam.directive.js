"use strict()";

(function(){
  var moment = require('moment');

  module.exports = ServeTeam;


  ServeTeam.$inject = ['$log', 'Session'];

  function ServeTeam($log,Session){
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
        dayIndex: '='
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

      activate();
     //////////////////////////////////////


      function activate(){
      }

      function closePanel(){
        scope.isCollapsed = true;
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

      function openPanel(members){
        if(scope.currentMember === null){
          var sessionId = Number(Session.exists("userId"));
          scope.currentMember = members[0];
          scope.currentActiveTab = scope.currentMember.name;
        }
        $log.debug("isCollapsed = " + scope.isCollapsed);
        scope.isCollapsed = !scope.isCollapsed;
      }

      function setActiveTab(member){
        scope.currentActiveTab = member.name;
        scope.currentMember =  member;
        scope.isCollapsed = false;
      }

    };

  }

})();
