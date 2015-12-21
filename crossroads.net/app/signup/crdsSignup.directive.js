(function() {
  'use strict';

  var constants = require('crds-constants');

  module.exports = crdsSignup;

  crdsSignup.$inject = ['SignupService', 'Lookup'];

  function crdsSignup(SignupService, Lookup) {
    return {
      restrict: 'E',
      scope: {},
      controller: crdsSignupController,
      controllerAs: 'signup',
      bindToController: true,
      templateUrl: 'signup/signup.html'
    };

    function crdsSignupController() {
      /*jshint validthis: true */
      var vm = this;
      vm.locations = locations();
      vm.cmsInfo = SignupService.cmsInfo;
      vm.family = SignupService.family;
      vm.group = SignupService.group;
      vm.signupService = SignupService;
      vm.showCommunityGroups = showCommunityGroups;
      vm.showOnetimeEvent = showOnetimeEvent;

      function showCommunityGroups() {
        return vm.signupService.cmsInfo.pages[0].className === constants.CMS.PAGENAMES.COMMUNITYGROUPS;
      }

      function showOnetimeEvent() {
        return vm.signupService.cmsInfo.pages[0].className === constants.CMS.PAGENAMES.ONETIMEEVENTS;
      }

      function locations() {
        return Lookup.query({
          table: 'crossroadslocations'
        }, function(data) {
          return data;
        });
      }

    }
  }
})();
