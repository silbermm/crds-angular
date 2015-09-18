(function() {
  'use strict';

  module.exports = SearchRoutes;

  SearchRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function SearchRoutes($stateProvider, $urlMatcherFactory, $locationProvider, $stateParams) {

    $stateProvider
    .state('search', {
      parent: 'noSideBar',
      url: '/search?type&q',
      controller: 'SearchCtrl as search',
      templateUrl: 'search/search-results.html',
      data: {
        type: ''
      },
      resolve:{
          type: ['$stateParams', function($stateParams){
              return $stateParams.type;
          }],
          searchString: ['$stateParams', function($stateParams){
              return $stateParams.q;
          }]
      }
    });
  }

})();
