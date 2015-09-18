'use strict';
(function () {
  module.exports = function SearchCtrl($log, $state, $scope, Search, type, json, searchString, tag) {
    var vm = this;

    vm.json = json;
    vm.type = type;
    vm.tag = tag;
    $scope.searchString = searchString;
    vm.search = search;
    vm.isMedia = isMedia;
    vm.isCorkboard = isCorkboard;
    vm.results = {'hits': {'found': 0}};

    doSearch();


    function search() {
      $state.go('search', {type: vm.type, searchString:$scope.searchString});
    }

    function doSearch() {
      var filter = '';
      var parser = '';
      switch(vm.type){
        case 'media':
          filter = '(or type:\'Series\' type:\'Message\' type:\'Video\' type:\'Audio\' type:\'Music\' type:\'Media\')';
          if(vm.tag){
            $scope.searchString = '(term field=tags\''+vm.tag+'\')';
            parser = 'structured';
          }
          break;
        case 'corkboard':
          filter = '(or type:\'NEED\' type:\'ITEM\' type:\'EVENT\' type:\'JOB\')';
          break;
      }
      //, 'q.parser': parser
      Search.Search.get({q: $scope.searchString, fq: filter})
        .$promise.then(function(response) {
        vm.results = response;
      });
    }

    function isMedia(item) {
      return(
        item.type === 'Series' ||
        item.type === 'Message' ||
        item.type === 'Video' ||
        item.type === 'Audio' ||
        item.type === 'Music' ||
        item.type === 'Media'
      );
    }

    function isCorkboard(item) {
      return(
        item.type === 'NEED' ||
        item.type === 'ITEM' ||
        item.type === 'EVENT' ||
        item.type === 'JOB'
      );
    }
  };
})();
