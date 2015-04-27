'use strict()';
(function(){
  
  angular.module('crossroads.mptools').controller('SignupToServeController', SignupToServeController);

  SignupToServeController.$inject = ['$log', '$location', 'MPTools', 'Su2sData'];

  function SignupToServeController($log, $location, MPTools, Su2sData){
    var vm = this; 
  
    vm.allParticipants = [];
    vm.frequency = [{
        value: 0,
        text: "Once"
      }, {
        value: 1,
        text: "Every Week"
      }, {
        value: 2,
        text: "Every Other Week"
      }];
    vm.group = {};
    vm.open = open;
    vm.params = MPTools.getParams();
    vm.populateDates = populateDates;
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

    function open($event, opened) {
        $event.preventDefault();
        $event.stopPropagation();
        vm[opened] = true;
      }

    function populateDates() {
        if (vm.currentMember !== null) {
          vm.currentMember.currentOpportunity.fromDt = vm.oppServeDate;
          switch (vm.currentMember.currentOpportunity.frequency.value) {
            case null:
              vm.currentMember.currentOpportunity.fromDt = null;
              vm.currentMember.currentOpportunity.toDT = null;
              break;
            case 0:
              // once...
              vm.currentMember.currentOpportunity.fromDt = vm.oppServeDate;
              vm.currentMember.currentOpportunity.toDt = vm.oppServeDate;
              break;
            default:
              // every  or everyother
              ServeOpportunities.LastOpportunityDate.get({
                id: vm.currentMember.serveRsvp.roleId
              }, function(ret) {
                var dateNum = Number(ret.date * 1000);
                var toDate = new Date(dateNum);
                vm.currentMember.currentOpportunity.toDt = (toDate.getMonth() + 1) + "/" + toDate.getDate() + "/" + toDate.getFullYear();
              });
              break;
          }
        }
      }

    function showError(){
      return vm.params.selectedCount > 1 || vm.params.recordDescription === undefined;
    }
  }

})();
