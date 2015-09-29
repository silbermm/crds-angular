(function(){
  'use strict';
  module.exports = TripDonationsController;

  TripDonationsController.$inject = [];

  /**
   * Takes in as an argument to the directive:
   *    - donation
   */
  function TripDonationsController() {
    var vm = this;
    vm.getDisplayName = getDisplayName;
    vm.isMessageToggled = false;
    vm.sendMessage = sendMessage;
    vm.showReplyButton = showReplyButton;
    vm.toggleMessage = toggleMessage;

    activate();

    ////////////////////////////
    // IMPLEMENTATION DETAILS //
    ////////////////////////////

    function activate() {}

    function getDisplayName() {
      if (vm.donation.registeredDonor) {
        return vm.donation.donorNickname + ' ' + vm.donation.donorLastName;
      } else {
        return vm.donation.donorEmail;
      }
    }

    function sendMessage(message) {

    }

    function showReplyButton() {
      if (vm.donation.anonymousDonor) {
        return false;
      }

      return true;
    }

    function toggleMessage() {
      vm.isMessageToggled = !vm.isMessageToggled;
    }

  }
})();
