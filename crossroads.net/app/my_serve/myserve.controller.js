'use strict()';
(function(){
  var moment = require('moment');

  module.exports = MyServeController;

  MyServeController.$inject = ['$rootScope', '$log', 'ServeOpportunities', 'Session', 'filterState'];

  function MyServeController($rootScope, $log, ServeOpportunities, Session, filterState){

    var vm = this;

    vm.clear = clear;
    vm.convertToDate = convertToDate;
    vm.dateOptions = { formatYear: 'yy', startingDay: 1 };
    vm.disabled = disabled;
    vm.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    vm.format = vm.formats[0];
    vm.groups = [];
    vm.isCollapsed = true;
    vm.open = open;
    vm.original = [];
    vm.today = today;
    vm.toggleMin = toggleMin;
    vm.repeating = '2';


    activate();

    $rootScope.$on("personUpdated", function(event, data) {
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
      $rootScope.$broadcast("rerunFilters", vm.groups);
    })

    ////////////////////////////
    // Implementation Details //
    ////////////////////////////
    function activate(){
      today();
      toggleMin();
      getGroups();
    }
    
    function getGroups(){
      vm.groups = ServeOpportunities.query();
    };

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

    function toggleMin() {
      vm.minDate = vm.minDate ? null : new Date();
    };

    function open($event) {
      $event.preventDefault();
      $event.stopPropagation();
      vm.opened = true;
    };


  }

})();
