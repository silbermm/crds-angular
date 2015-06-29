'use strict()';
(function(){

  module.exports = ServeModalController;

  ServeModalController.$inject = ['$scope', '$rootScope', '$modalInstance', 'filterFromDate', 'lastDate'];

  function ServeModalController($scope, $rootScope, $modalInstance, filterFromDate, lastDate){
    $scope.fromDate = formatDate(filterFromDate);
    $scope.toDate = formatDate(lastDate);
    $scope.format = 'MM/dd/yy';
    $scope.isFromError = isFromError;
    $scope.isToError = isToError;
    $scope.dateOptions = {
      formatYear: 'yy',
      startingDay: 1,
      showWeeks: 'false'
    };
    $scope.datePickers = { fromOpened : false, toOpened: false };
    $scope.openFromDate = openFromDate;
    $scope.openToDate = openToDate;

    $scope.readyFilterByDate = readyFilterByDate;
    $scope.ok = function () {
      $modalInstance.close($scope.fromDate, $scope.toDate);
    };
    function isFromError(){
      return $scope.filterdates.fromdate.$dirty && (
        $scope.filterdates.fromdate.$error.fromDateToLarge ||
        $scope.filterdates.fromdate.$error.date ||
        $scope.filterdates.fromdate.$error.required);
    }
    function isToError(){
      return $scope.filterdates.todate.$dirty && (
        $scope.filterdates.todate.$error.fromDate || $scope.filterdates.todate.$error.required || $scope.filterdates.todate.$error.date);
    }
    function readyFilterByDate() {
      var now = moment();
      now.hour(0);
      var toDate = moment($scope.toDate);
      toDate.hour(23);

      if( now.unix() > toDate.unix() ) {
        $scope.filterdates.todate.$error.fromDate = true;
        $rootScope.$emit("notify", $rootScope.MESSAGES.generalError);
        return false;
      } else {
        $scope.filterdates.todate.$error.fromDate = false;
      }

      if ($scope.toDate !== undefined && toDate.isValid()){
        if ($scope.fromDate === undefined) {
          $scope.filterdates.fromdate.$error.date = true;
          $rootScope.$emit("notify", $rootScope.MESSAGES.generalError);
          return false;
        }
        var fromDate = moment($scope.fromDate);
        if (fromDate.isBefore(now, 'days')) {
          fromDate = now;
        }
        if (!fromDate.isValid()) {
          $scope.filterdates.fromdate.$error.date = true;
          $rootScope.$emit("notify", $rootScope.MESSAGES.generalError);
          return false;
        }

        if ( fromDate.isAfter(toDate, 'days' )){
          $scope.filterdates.fromdate.$error.fromDateToLarge = true;
          $rootScope.$emit("notify", $rootScope.MESSAGES.generalError);
          return false;
        } else {
          $scope.filterdates.fromdate.$error.fromDateToLarge = false;
        }
        $rootScope.$emit("filterByDates", {'fromDate': fromDate, 'toDate': toDate});
        $modalInstance.close({ fromDate:$scope.fromDate, toDate: $scope.toDate });
      } else if (isToError()) {
        $scope.filterdates.todate.$error.date = true;
        $rootScope.$emit("notify", $rootScope.MESSAGES.generalError);
        return false;
      } else {
        return false;
      }
    }

    function openFromDate($event) {
      $event.preventDefault();
      $event.stopPropagation();
      $scope.datePickers.fromOpened = true;
    }

    function openToDate($event) {
      $event.preventDefault();
      $event.stopPropagation();
      $scope.datePickers.toOpened = true;
    }

    /**
     * Takes a javascript date and returns a
     * string formated MM/DD/YYYY
     * @param date - Javascript Date
     * @param days to add - How many days to add to the original date passed in
     * @return string formatted in the way we want to display
     */
    function formatDate(date, days=0){
      var d = moment(date);
      d.add(days, 'd');
      return d.format('MM/DD/YY');
    }
  }


})();
