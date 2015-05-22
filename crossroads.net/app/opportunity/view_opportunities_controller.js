
"use strict";

(function() {

  module.exports = ViewOpportunitiesController;

  ViewOpportunitiesController.$inject = ['$log', '$filter', 'MESSAGES', 'Opportunity', 'ServeOpportunities', 'Participants'];

  function ViewOpportunitiesController($log, $filter, MESSAGES, Opportunity, ServeOpportunities, Participants) {
    var vm = this;

    vm.displayEmail = displayEmail;
    vm.participants = Participants;
    vm.save = save;

    function displayEmail(emailAddress) {
      if (emailAddress === null || emailAddress === undefined) {
        return false;
      }
      if (emailAddress.length > 0) {
        return true;
      }
      return false;
    }

    function save(form) {
      var save = new ServeOpportunities.SaveQualifiedServers();
      save.opportunityId = 27705;
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
