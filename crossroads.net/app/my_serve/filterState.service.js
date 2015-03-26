"use strict()";
(function(){

  module.exports = FilterState;
  
  function FilterState(){
    var vm = this; 
    vm.memberIds = [];
    vm.times = [];
    vm.teams = [];

    return {
      addFamilyMember: addFamilyMember,
      removeFamilyMember: removeFamilyMember,
      getFamilyMembers:  getFamilyMembers
    };


    function addFamilyMember(memberId) {
      vm.memberIds.push(memberId);
    }

    function removeFamilyMember(memberId){
      vm.memberIds = _.filter(vm.memberIds,function(m){
        return m !== memberId; 
      });
    }

    function getFamilyMembers(){
      return vm.memberIds;
    }
 
  }

})();
