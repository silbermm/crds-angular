(function() {
  'use strict';

  module.exports = ChildcareRoutes;

  ChildcareRoutes.$inject = ['$stateProvider'];

  function ChildcareRoutes($stateProvider) {
    $stateProvider
      .state('childcare', {
        parent: 'noSideBar',
        url: '/childcare'
      })
      .state('childcare', {
        parent: 'noSideBar',
        url: '/childcare/:eventId',

      })
      ;
  }

})();
