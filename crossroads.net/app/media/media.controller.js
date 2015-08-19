'use strict';
(function () {
  module.exports = MediaCtrl;

  MediaCtrl.$inject = ['$scope', '$log', '$http', '$location', 'Series'];

  function MediaCtrl($scope, $log, $http, $location, Series) {
    var vm = this;
    vm.msgisopen = true;
    vm.musicisopen = false;
    vm.series = sortSeries(Series.series);
  }

  function sortSeries(series) {
    var seriesArray = _.values(series);

    // order by date desc
    seriesArray.sort(function (a, b) {
      a = new Date(a['startDate']);
      b = new Date(b['startDate']);
      return a > b ? -1 : a < b ? 1 : 0;
    });

    return seriesArray;
  }
})();
