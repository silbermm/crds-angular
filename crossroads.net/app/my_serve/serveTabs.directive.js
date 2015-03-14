"use strict()";

(function(){
  module.exports = function ServeTabs($log){
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
      scope.signedup = null;
      scope.currentActiveTab = scope.opportunity.members[0].name;
      scope.currentMember = scope.opportunity.members[0];
      scope.isActiveTab = isActiveTab;
      scope.isSignedUp = isSignedUp;
      scope.roles = null;
      scope.setActiveTab = setActiveTab;

      $log.debug(scope.roles);

      function isActiveTab(memberName){
        return memberName === scope.currentActiveTab
      };

      function setActiveTab(member){
        scope.currentActiveTab = member.name;
        scope.currentMember =  member;
      };
    

    function isSignedUp(opportunity){
      if(scope.currentMember === undefined){
        return false;
      } else {
        $log.debug("looking for matching opportunities");
        return _.find(opportunity.members, function(m){
          $log.debug(m);
          return m.name === scope.currentMember.name && m.signedup === 'yes';
        });
      }
    };

  };

}
  
})();
