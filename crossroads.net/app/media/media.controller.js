'use strict';
(function () {
  module.exports = MediaCtrl;

    MediaCtrl.$inject = ['$scope', '$log', '$http', '$location', 'Media'];

    function MediaCtrl($scope, $log, $http, $location, Media) {
      var vm = this;
      vm.msgisopen = true;
      vm.musicisopen = false;
      vm.series = getMedia();

      function getMedia() {
        var promise = Media.Series().get().$promise; // query would be for an array
        promise.then(function(response) {
          debugger;

          // convert the json blob to an array
          var seriesArray = _.chunk(response['series']);

          // order by date
          seriesArray.sort(function(a,b) {
            a = new Date(a['startDate']);
            b = new Date(a['startDate']);
            return a > b ? -1 : a < b ? 1 : 0;
          });

          return seriesArray;
        }, function (error) {
          // todo -- add the error
          debugger;
        });

        return response;
      }

    }

})();
