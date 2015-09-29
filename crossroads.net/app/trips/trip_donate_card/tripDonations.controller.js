(function(){
  'use strict';
  module.exports = TripDonationsController;

  TripDonationsController.$inject = ['$rootScope', 'Trip'];

  /**
   * Takes in as an argument to the directive:
   *    - donation
   *    - tripTitle
   */
  function TripDonationsController($rootScope, Trip) {
    var vm = this;
    vm.getDisplayName = getDisplayName;
    vm.isMessageToggled = false;
    vm.loading = false;
    vm.sendMessage = sendMessage;
    vm.showReplyButton = showReplyButton;
    vm.toggleMessage = toggleMessage;

    activate();

    ////////////////////////////
    // IMPLEMENTATION DETAILS //
    ////////////////////////////

    function activate() {}

    function getDisplayName() {
      if (vm.donation.anonymous) {
        return 'Anonymous';
      }

      if (vm.donation.registeredDonor) {
        return vm.donation.donorNickname + ' ' + vm.donation.donorLastName;
      } else {
        return vm.donation.donorEmail;
      }
    }

    function sendMessage(message, onSuccess, onError) {
      console.log(message);
      Trip.Email.save({
        donorId: vm.donation.donorId,
        message: message,
        tripName: vm.tripTitle,
        donationDistributionId: vm.donation.donationDistributionId
      }, function(data) {
        //success!
        vm.loading = false;
        $rootScope.$emit('notify',
            $rootScope.MESSAGES.emailSent);
        vm.donation.messageSent = true;
        onSuccess();
      }, function(data) {
        // error!
        vm.loading = false;
        $rootScope.$emit('notify',
            $rootScope.MESSAGES.emailSendingError);
        onError();
      });
    }

    function showReplyButton() {
      if (vm.donation.anonymous) {
        return false;
      }

      return true;
    }

    function toggleMessage() {
      vm.isMessageToggled = !vm.isMessageToggled;
    }

  }
})();
