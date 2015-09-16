(function() {
  'use strict';

  module.exports = SearchRoutes;

  SearchRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function SearchRoutes($stateProvider, $urlMatcherFactory, $locationProvider, $stateParams) {

    $stateProvider
    .state('search', {
      parent: 'noSideBar',
      url: '/search?type&json&searchString&tag',
      controller: 'SearchCtrl as search',
      templateUrl: 'search/search-results.html',
      data: {
        type: ''
      },
      resolve:{
          tag: ['$stateParams', function($stateParams){
              return $stateParams.tag;
          }],
          type: ['$stateParams', function($stateParams){
              return $stateParams.type;
          }],
          json: ['$stateParams', function($stateParams){
              return $stateParams.json;
          }],
          searchString: ['$stateParams', function($stateParams){
              return $stateParams.searchString;
          }]
      }
    });
  }

})();
