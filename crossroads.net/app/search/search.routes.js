(function() {
  'use strict';

  module.exports = SearchRoutes;

  SearchRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function SearchRoutes($stateProvider, $urlMatcherFactory, $locationProvider, $stateParams) {

    $stateProvider
    .state('search', {
      parent: 'noSideBar',
      url: '/search-results?json',
      controller: 'SearchCtrl as search',
      templateUrl: 'search/search-results.html',
      resolve:{
          json: ['$stateParams', function($stateParams){
              return $stateParams.json;
          }]
      }
    });
  }

})();
