(function() {
  'use strict';

  module.exports = TripsSignupController;

  TripsSignupController.$inject = ['$log', 'Session',  'Campaign', 'WorkTeams', '$location', 'Trip', '$q'];

  function TripsSignupController($log, Session, Campaign, WorkTeams, $location, Trip, $q) {

    var vm = this;

    vm.ageLimitReached = true;
    vm.campaign = Campaign;
    vm.currentPage = 1;
    vm.numberOfPages = 0;
    vm.pageHasErrors = true;
    vm.pageTitle = vm.campaign.formName;
    vm.privateInvite = $location.search()['invite'];
    vm.registrationNotOpen = true;
    vm.tripName = vm.campaign.name;
    vm.whyPlaceholder = '';
    vm.workTeams = WorkTeams;

    activate();

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////
    function activate() {
      pageHasErrors();
      switch (vm.pageTitle) {
        case 'GO NOLA Application':
          vm.friendlyPageTitle = 'New Orleans';
          vm.numberOfPages = 5;
          break;
        case 'GO South Africa Application':
          vm.friendlyPageTitle = 'South Africa';
          vm.numberOfPages = 6;
          break;
        case 'GO India Application':
          vm.friendlyPageTitle = 'India';
          vm.numberOfPages = 6;
          vm.whyPlaceholder = 'Please be specific. ' +
            'In instances where we have a limited number of spots, we strongly consider responses to this question.';
          break;
        case 'GO Nicaragua Application':
          vm.friendlyPageTitle = 'Nicaragua';
          vm.numberOfPages = 6;
          break;
      }
    }

    function ageLimitReached() {
      if (Session.exists('age') && Session.exists('age') < Campaign.ageLimit) {
        //Under age limit, check for exceptions
        if (Session.exists('userId') && _.includes(Campaign.ageExceptions, Number(Session.exists('userId')))) {
          return false;
        }

        return true;
      }

      return false;
    }

    function pageHasErrors() {
      $log.debug('pageHasErrors Function');
      vm.ageLimitReached = ageLimitReached();
      var promise = registrationNotOpen();
      promise.then(function(regNotOpen) {
        vm.registrationNotOpen = regNotOpen;
        if (vm.ageLimitReached || vm.registrationNotOpen) {
          vm.pageHasErrors = true;
        } else {
          vm.pageHasErrors = false;
        }

      }, function(reason) {
        vm.pageHasErrors = true;
      });
    }

    function registrationNotOpen() {
      return $q(function(resolve, reject) {
        $log.debug('guid: ' + vm.privateInvite);
        if (vm.privateInvite === undefined) {
          var regStart = moment(vm.campaign.registrationStart);
          var regEnd = moment(vm.campaign.registrationEnd);
          var today = moment();
          if (today.isBetween(regStart, regEnd)) {
            resolve(false);
          } else {
            resolve(true);
          }
        } else {
          $log.debug('verify guid');
          var valid = Trip.ValidatePrivateInvite.get({
            pledgeCampaignId: vm.campaign.id,
            guid: vm.privateInvite
          }, function(data) {
            $log.debug('guid: ' + data);
            resolve(!data);
          }, function(error) {
            resolve(false);
          });
        }
      });
    }
  }

})();
