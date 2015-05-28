
"use strict";

(function() {

  module.exports = VolunteerController;

  VolunteerController.$inject = ['$log', '$filter', 'MESSAGES', 'Session', 'Opportunity', 'ServeOpportunities', 'Page', 'CmsInfo'];

  function VolunteerController($log, $filter, MESSAGES, Session, Opportunity, ServeOpportunities, Page, CmsInfo) {
    $log.debug("Inside VolunteerController");
    var vm = this;

    vm.pageInfo = pageInfo(CmsInfo);
    vm.displayEmail = displayEmail;
    vm.save = save;
    vm.showContent = true;
    getParticipants(vm.pageInfo.group);

    function displayEmail(emailAddress) {
      if (emailAddress === null || emailAddress === undefined) {
        return false;
      }
      if (emailAddress.length > 0) {
        return true;
      }
      return false;
    }

    function getParticipants(groupId){
      ServeOpportunities.QualifiedServers.query({
        groupId: groupId,
        contactId: Session.exists('userId')
      }).$promise
      .then(function(response) {vm.participants = response;});
    }

    function pageInfo(cmsInfo) {
      // TODO need to check that we have data before assign
      return cmsInfo.pages[0];
    }

    function save(form) {
      var save = new ServeOpportunities.SaveQualifiedServers();
      save.opportunityId = 115;
      //just get participants that have checkbox checkLoggedin
      save.participants = _.pluck(_.filter(vm.participants, {
        add: true
      }), 'participantId');

      save.$save().then(function() {
        vm.created = true;
      }, function() {
        vm.rejected = true;
      });
    }
  }
})();
