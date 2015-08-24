(function() {
  'use strict';

  module.exports = SignupProgressController;

  SignupProgressController.$inject = [];

  /*
   * variables provided by the directive...
   * currentStep
   * totalSteps
   */
  function SignupProgressController() {

    var vm = this;
    vm.percentComplete = percentComplete;
    vm.percentString = percentString;

    activate();

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////
    function activate() { }

    function percentComplete() {
      if (vm.currentStep === 'thanks') {
        return 100;
      }

      var percent = Math.round(((vm.currentStep - 1) / vm.totalSteps) * 100);
      return percent;
    }

    function percentString() {
      var message = vm.percentComplete() + '%' + ' Completed';
      if (vm.currentStep <= vm.totalSteps) {
        message += ' | Step ' + vm.currentStep + ' of ' + vm.totalSteps;
      }

      return message;
    }

  }

})();
