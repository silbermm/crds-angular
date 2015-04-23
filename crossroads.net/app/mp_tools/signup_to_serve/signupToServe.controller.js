'use strict()';
(function(){
  
  angular.module('crossroads.mptools').controller('SignupToServeController', SignupToServeController);

  SignupToServeController.$inject = ['$log', '$location', 'MPTools', 'Su2sData'];

  function SignupToServeController($log, $location, MPTools, Su2sData){
    var vm = this; 
  
    vm.allParticipants = [];
    vm.group = {};
    vm.params = MPTools.getParams();
    vm.showError = showError;
    vm.ready = false;

    activate();

    ////////////////////////////////////////////
    
    function activate(){
      Su2sData.get({"oppId": vm.params.recordId}, function(g){
        vm.group = g;
        vm.allParticipants = g.Participants;
        vm.ready = true;
      });
    }

    function showError(){
      return vm.params.selectedCount > 1 || vm.params.recordDescription === undefined;
    }
  }

})();
