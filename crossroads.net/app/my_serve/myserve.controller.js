'use strict()';
(function(){
  var moment = require('moment');

  module.exports = MyServeController;

  MyServeController.$inject = ['$rootScope', '$log', 'filterState', 'Session', 'ServeOpportunities', 'Groups'];

  function MyServeController($rootScope, $log, filterState, Session, ServeOpportunities, Groups){

    var vm = this;

    vm.convertToDate = convertToDate;
    vm.filterState = filterState;
    vm.groups = Groups;
    vm.loadMore = false;
    vm.loadNextMonth = loadNextMonth;
    vm.loadText = "Load More";
    vm.original = [];
    vm.showButton = showButton;
    vm.showNoOpportunitiesMsg = showNoOpportunitiesMsg;

    activate();

    $rootScope.$on("personUpdated", personUpdateHandler);

    $rootScope.$on("filterDone", function(event, data) {
      vm.groups = data;
    });

    $rootScope.$on("filterByDates", function(event, data) {
      loadOpportunitiesByDate(data.fromDate, data.toDate).then(function(opps){
        vm.groups = opps;    
        vm.original = opps;
      },function(err){
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      });
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

    /**
     * This function will fetch a new set of serve opportunities between two dates
     * The dates passed in should be in epoch formatted in milliseconds
     * @param fromDate the epoch formatted beginning date
     * @param toDate the epoch formated end date
     * @returns a promise
     */
    function loadOpportunitiesByDate(fromDate, toDate){
      return ServeOpportunities.ServeDays.query({ 
        id: Session.exists('userId'), 
        from: fromDate/1000, 
        to: toDate/1000 
      }).$promise;
    }

    function loadNextMonth() {
      if(vm.groups[0].day !== undefined){ 
        vm.loadMore = true;
        vm.loadText = "Loading..."
          
        var lastDate = new Date(vm.groups[vm.groups.length -1].day);
        lastDate.setDate(lastDate.getDate() + 1); 
        var newDate = new Date(lastDate);
        newDate.setDate(newDate.getDate() + 28);
          
        loadOpportunitiesByDate(lastDate.getTime(), newDate.getTime()).then(function(more){
          if(more.length === 0){
            $rootScope.$emit('notify', $rootScope.MESSAGES.serveSignupMoreError);
          } else {
            _.each(more, function(m){
              vm.groups.push(m);
            });
          }
          vm.loadMore = false;
          vm.loadText = "Load More";
        }, function(e){
          vm.loadMore = false;  
          vm.loadText = "Load More";
        });
      }
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

    function showButton(){
      if (showNoOpportunitiesMsg()){
        return false;
      } else { 
        return !filterState.isActive();
      }
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
