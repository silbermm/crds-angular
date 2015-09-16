'use strict';
(function () {
  module.exports = function SearchCtrl($log, $state, $scope, Search, json) {
    var vm = this;

    vm.json=json;
    vm.search = search;
    vm.results = {'hits': {'found': 0}};
    function search() {
      Search.Search.get({q: $scope.searchString}).$promise.then(function(response) {
        vm.results = response;
      });
    }
  };
})();
