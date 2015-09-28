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
    vm.isMessageToggled = false;
    vm.toggleMessage = toggleMessage;

    activate();

    ////////////////////////////
    // IMPLEMENTATION DETAILS //
    ////////////////////////////

    function activate() {}

    function toggleMessage() {
      vm.isMessageToggled = !vm.isMessageToggled;
    }

  }
})();
