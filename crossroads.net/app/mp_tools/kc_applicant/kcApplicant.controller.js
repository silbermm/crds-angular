(function(){
  'use strict()';

  module.exports = KCApplicantController;

  KCApplicantController.$inject = ['$log', '$window', 'MPTools', 'Contact' ];

  function KCApplicantController($log, $window, MPTools, Contact) {
    
    var vm = this;
    vm.cancel = cancel;
    vm.params = MPTools.getParams();
    vm.showError = showError;
    vm.viewReady = true;


    activate();
    //////////////////////
    
    function activate(){

    }

    function cancel(){
      $window.close();
    }

    function showError(){
      return vm.params.selectedCount > 1 || vm.params.recordDescription === undefined || vm.params.recordId === '-1';
    }


    
  }

})();
