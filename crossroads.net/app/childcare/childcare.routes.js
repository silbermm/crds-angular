(function() {
  'use strict';

  module.exports = ChildcareRoutes;

  ChildcareRoutes.$inject = ['$stateProvider'];

  function ChildcareRoutes($stateProvider) {
    $stateProvider
      .state('childcare.event', {
        parent: 'noSideBar',
        url: '/childcare/:eventId',
        template: '<childcare></childcare>',
        date: {
          isProtected: true,
          meta: {
            title: 'Childcare Signup',
            description: 'Choose which of your children you want to enroll in childcare during your event'
          }
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin
        }
      })
      ;
  }

})();
