'use strict()';
(function(){
  
  var moment = require('moment');
  var formatDate = require('../../../core/crds_utilities').formatDate;

  module.exports = ServeModalController;

  ServeModalController.$inject = ['$rootScope', '$modalInstance', 'dates'];

  function ServeModalController($rootScope, $modalInstance, dates){
    
    var vm = this;

    vm.apply = apply;
    vm.dateOptions = {
      formatYear: 'yy',
      startingDay: 1,
      showWeeks: 'false'
    };
    vm.datePickers = { fromOpened : false, toOpened: false };
    vm.format = 'MM/dd/yy';
    vm.fromDate = formatDate(dates.fromDate);
    vm.fromDateError = false;
    vm.isFromError = isFromError;
    vm.isToError = isToError;
    vm.toDate = formatDate(dates.toDate);
    vm.toDateError = false;
    vm.openFromDate = openFromDate;
    vm.openToDate = openToDate;
    vm.readyFilterByDate = readyFilterByDate;

    ///////////////////////////////////////////

    function apply() {
      $modalInstance.close(vm.fromDate, vm.toDate);
    }

    function isFromError(){
      return vm.filterdates.fromdate.$dirty && (
        vm.filterdates.fromdate.$error.fromDateToLarge ||
        vm.filterdates.fromdate.$error.date ||
        vm.filterdates.fromdate.$error.required);
    }

    function isToError(){
      return vm.filterdates.todate.$dirty && (
        vm.filterdates.todate.$error.fromDate || 
        vm.filterdates.todate.$error.required || 
        vm.filterdates.todate.$error.date
      );
    }

    function readyFilterByDate() {
      var now = moment();
      now.hour(0);
      var toDate = moment(vm.toDate, 'MM/DD/YY');
      toDate.hour(23); 

      if( now.unix() > toDate.unix() ) {
        
        vm.filterdates.todate.$error.fromDate = true;
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        return false;
      } else {
        vm.filterdates.todate.$error.fromDate = false;
      }

      if (vm.toDate !== undefined && toDate.isValid()){
        if (vm.fromDate === undefined) {
          vm.filterdates.fromdate.$error.date = true;
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          return false;
        }
        var fromDate = moment(vm.fromDate);
        if (fromDate.isBefore(now, 'days')) {
          fromDate = now;
        }
        if (!fromDate.isValid()) {
          vm.filterdates.fromdate.$error.date = true;
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          return false;
        }

        if ( fromDate.isAfter(toDate, 'days' )){
          vm.filterdates.fromdate.$error.fromDateToLarge = true;
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          return false;
        } else {
          vm.filterdates.fromdate.$error.fromDateToLarge = false;
        }
        $rootScope.$emit('filterByDates', {'fromDate': fromDate, 'toDate': toDate});
        $modalInstance.close({ fromDate:vm.fromDate, toDate: vm.toDate });
      } else if (isToError()) {
        vm.filterdates.todate.$error.date = true;
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        return false;
      } else {
        return false;
      }
    }

    function openFromDate($event) {
      $event.preventDefault();
      $event.stopPropagation();
      vm.datePickers.fromOpened = true;
    }

    function openToDate($event) {
      $event.preventDefault();
      $event.stopPropagation();
      vm.datePickers.toOpened = true;
    }
    
  }

})();
