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
      addTeam: addTeam,
      addTime: addTime,
      findMember: findMember,
      findTeam: findTeam,
      findTime: findTime,
      removeFamilyMember: removeFamilyMember,
      removeTeam: removeTeam,
      removeTime: removeTime,
      getFamilyMembers:  getFamilyMembers,
      getTeams: getTeams,
      getTimes: getTimes
    };


    function addFamilyMember(memberId) {
      vm.memberIds.push(memberId);
    }

    function addTeam(team) {
      vm.teams.push(team);
    }

    function addTime(time) {
      vm.times.push(time);
    }

    function findMember(memberId){
      return _.find(vm.memberIds, function(m){
        return memberId === m;
      });
    }

    function findTeam(team){
      return _.find(vm.teams, function(t){
        return team === t;
      });
    }

    function findTime(time){
      return _.find(vm.times, function(t){
        return time === t;
      });
    }

    function removeFamilyMember(memberId){
      vm.memberIds = _.filter(vm.memberIds,function(m){
        return m !== memberId; 
      });
    }

    function removeTeam(team) {
      vm.teams = _.filter(vm.teams,function(t){
        return t !== team
      });
    }

    function removeTime(time) {
      vm.times = _.filter(vm.times,function(t){
        return t !== time
      });
    }

    function getFamilyMembers(){
      return vm.memberIds;
    }

    function getTeams() {
      return vm.teams;
    }

    function getTimes(){
      return vm.times;
    }
 
  }

})();
