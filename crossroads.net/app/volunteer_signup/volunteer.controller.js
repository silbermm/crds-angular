"use strict";

(function() {

  module.exports = VolunteerController;

  VolunteerController.$inject = ['$log', '$filter', 'MESSAGES', 'Session', 'Opportunity', 'ServeOpportunities', 'CmsInfo'];

  function VolunteerController($log, $filter, MESSAGES, Session, Opportunity, ServeOpportunities, CmsInfo) {
    $log.debug("Inside VolunteerController");
    var vm = this;

    vm.pageInfo = pageInfo(CmsInfo);
    vm.displayEmail = displayEmail;
    vm.save = save;
    vm.showContent = true;
    vm.viewReady = false;

    init();

    function displayEmail(emailAddress) {
      if (emailAddress === null || emailAddress === undefined) {
        return false;
      }
      if (emailAddress.length > 0) {
        return true;
      }
      return false;
    }

    function init() {
      ServeOpportunities.QualifiedServers.query({
          groupId: vm.pageInfo.group,
          contactId: Session.exists('userId')
        }).$promise
        .then(function(response) {
          vm.participants = response;
          vm.viewReady = true;
        });
    }

    function pageInfo(cmsInfo) {
      // TODO need to check that we have data before assign
      // can we check for 404 on route?  and assume we have a page?
      return cmsInfo.pages[0];
    }

    function save(form) {
      var save = new ServeOpportunities.SaveQualifiedServers();
      save.opportunityId = vm.pageInfo.opportunity;
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
