'use strict()';
(function(){
  var moment = require('moment');

  module.exports = MyServeController;

  MyServeController.$inject = ['$rootScope', '$log', 'Session', 'filterState', 'Groups'];

  function MyServeController($rootScope, $log, Session, filterState, Groups){

    var vm = this;

    vm.clear = clear;
    vm.convertToDate = convertToDate;
    vm.dateOptions = { formatYear: 'yy', startingDay: 1 };
    vm.disabled = disabled;
    vm.filterState = filterState;
    vm.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    vm.format = vm.formats[0];
    vm.groups = Groups;
    vm.open = open;
    vm.original = [];
    vm.showNoOpportunitiesMsg = showNoOpportunitiesMsg;
    vm.today = today;
    vm.toggleMin = toggleMin;

    activate();

    $rootScope.$on("personUpdated", personUpdateHandler);

    $rootScope.$on("filterDone", function(event, data) {
      vm.groups = data;
    });

    ////////////////////////////
    // Implementation Details //
    ////////////////////////////

    function activate(){
      today();
      toggleMin();
      console.log(Groups);
    }
   
    function today() {
      vm.dt = new Date();
    };

    function clear() {
      vm.dt = null;
    };

    function convertToDate(date){
      // date comes in as mm/dd/yyyy, convert to yyyy-mm-dd for moment to handle
      var d = new Date(date);
      return d;
    };

    function disabled (date, mode) {
      return ( mode === 'day' && ( date.getDay() === 0 || date.getDay() === 6 ) );
    };

    function open($event) {
      $event.preventDefault();
      $event.stopPropagation();
      vm.opened = true;
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

    function toggleMin() {
      vm.minDate = vm.minDate ? null : new Date();
    };
  }

})();
