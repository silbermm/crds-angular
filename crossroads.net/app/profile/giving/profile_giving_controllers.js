(function() {
  'use strict';

  module.exports = GivingProfileController;

  GivingProfileController.$inject = ['$log', 'GivingHistoryService', 'Profile'];

  function GivingProfileController($log, GivingHistoryService, Profile) {
    var vm = this;

    activate();

    function activate() {
    }
  }
})();
