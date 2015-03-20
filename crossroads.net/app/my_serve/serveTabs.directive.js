"use strict()";

(function(){
  module.exports = ServeTabs;

    
  ServeTabs.$inject = ['$log', 'Session'];
 
  function ServeTabs($log,Session){
    return {
      restrict: "EA",
      transclude: true,
      templateUrl : "my_serve/serveTabs.html",
      scope : {
        opportunity: '=',
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
      scope.roles = null;
      scope.setActiveTab = setActiveTab;
      scope.signedup = null;


     ////////////////////////////////////// 
    
      function closePanel(){
        scope.isCollapsed = true;
      }

      function isActiveTab(memberName){
        return memberName === scope.currentActiveTab
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
          scope.currentMember = _.find(members, function(m){
            return Number(m.contactId) === sessionId;
          });
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
