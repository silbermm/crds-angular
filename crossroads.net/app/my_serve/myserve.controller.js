'use strict()';
(function(){
  var moment = require('moment');

  module.exports = MyServeController;

  MyServeController.$inject = ['$rootScope', '$log', 'filterState', 'Session', 'ServeOpportunities', 'Groups'];

  function MyServeController($rootScope, $log, filterState, Session, ServeOpportunities, Groups){

    var vm = this;

    vm.convertToDate = convertToDate;
    vm.dateOptions = { formatYear: 'yy', startingDay: 1 };
    vm.filterState = filterState;
    vm.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    vm.format = vm.formats[0];
    vm.groups = Groups;
    vm.loadMore = false;
    vm.loadNextMonth = loadNextMonth;
    vm.loadText = "Load More";
    vm.original = [];
    vm.showNoOpportunitiesMsg = showNoOpportunitiesMsg;

    activate();

    $rootScope.$on("personUpdated", personUpdateHandler);

    $rootScope.$on("filterDone", function(event, data) {
      vm.groups = data;
    });

    ////////////////////////////
    // Implementation Details //
    ////////////////////////////

    function activate(){
    }

    
    function convertToDate(date){
      // date comes in as mm/dd/yyyy, convert to yyyy-mm-dd for moment to handle
      var d = new Date(date);
      return d;
    };

    function loadNextMonth() {
      vm.loadMore = true;
      vm.loadText = "Loading...";
      var lastDate = vm.groups[vm.groups.length -1].day;
      var date = new Date(lastDate);
      date.setDate(date.getDate() + 1); 
      var newDate = new Date(lastDate);
      newDate.setDate(newDate.getDate() + 28);
      ServeOpportunities.ServeDays.query({ 
        id: Session.exists('userId'), 
        from: date.getTime()/1000, 
        to: newDate.getTime()/1000 
      }, function(more){
        _.each(more, function(m){
          vm.groups.push(m);
        });
        vm.loadMore = false;
        vm.loadText = "Load More";
      }, function(){
        vm.loadMore = false;  
        vm.loadText = "Load More";
      });
    };
   
    function personUpdateHandler(event, data) {
      vm.groups = angular.copy(vm.original);
      _.each(vm.groups, function(group) {
        _.each(group.serveTimes, function(serveTime) {
          _.each(serveTime.servingTeams, function(servingTeam) {
            _.each(servingTeam.members, function(member) {
              if (member.contactId === data.contactId) {
                member.name = data.nickName===null?data.firstName:data.nickName;
                member.nickName = data.nickName;
                member.lastName = data.lastName;
                member.emailAddress = data.emailAddress;
              }
            })
          })
        })
      })
      vm.original = angular.copy(vm.groups);
      $rootScope.$broadcast("rerunFilters", vm.groups);
    }

    function showNoOpportunitiesMsg(){
      return vm.groups.length < 1 || totalServeTimesLength() === 0;
    }

    function totalServeTimesLength(){
      var len = _.reduce(vm.groups, function(total,n){
        return total + n.serveTimes.length;
      }, 0);
      return len;
    }
  }

})();
