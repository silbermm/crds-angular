(function() {
  'use strict';

  module.exports = SignupProgressController;

  SignupProgressController.$inject = [];

  function SignupProgressController() {
    // variables provided by the directive...
    //   * currentStep
    //   * totalSteps
    var vm = this;
    vm.percentComplete = percentComplete;
    activate();

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////
    function activate() { }

    function percentComplete() {
      return Math.round(((vm.currentStep - 1) / vm.totalSteps) * 100);
    }

  }

})();
