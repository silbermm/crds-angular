"use strict()";
(function(){

  module.exports = FilterState;
  
  function FilterState(){

    var filterState =  { 
      memberIds: [],
      times: [],
      teams:[],
      addFamilyMember: function (memberId) {
        filterState.memberIds.push(memberId);
      },
      addTeam: function (team) {
       filterState.teams.push(team);
      },
      addTime: function (time) {
        filterState.times.push(time);
      },

      clearAll: function () {
        filterState.memberIds = [];
        filterState.times = [];
        filterState.teams = [];
      },

      findMember: function(memberId){
        return _.find(filterState.memberIds, function(m){
          return memberId === m;
        });
      },
      findTeam: function(team){
        return _.find(filterState.teams, function(t){
          return team === t;
        });
      },
      findTime: function(time){
        return _.find(filterState.times, function(t){
          return time === t;
        });
      },
      removeFamilyMember: function(memberId){
        filterState.memberIds = _.filter(filterState.memberIds,function(m){
          return m !== memberId; 
        });
      },
      removeTeam: function(team) {
        filterState.teams = _.filter(filterState.teams,function(t){
          return t !== team
        });
      },
      removeTime: function(time) {
        filterState.times = _.filter(filterState.times,function(t){
          return t !== time
        });
      },
      getFamilyMembers: function(){ 
        return filterState.memberIds;
      } ,
      getTeams: function() {
        return filterState.teams;
      },
      getTimes: function(){
        return filterState.times;
      }
    };
    return filterState; 
  }

})();
