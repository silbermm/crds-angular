(function() {
  'use strict';

  module.exports = TripRoutes;
  
  TripRoutes.$inject = ['$stateProvider'];

  function TripRoutes($stateProvider){

    $stateProvider
      .state('tripgiving', {
        parent: 'noSideBar',
        url: '/trips',
        controller: 'TripGivingCtrl as tripSearch',
        templateUrl: 'tripgiving/tripgiving.html',
        resolve: {
          Page: 'Page',
          CmsInfo: function(Page, $stateParams) {
            return Page.get({
              url: '/tripgiving/'
            }).$promise;
          }
        }
      })
      .state('mytrips', {
        parent: 'noSideBar',
        url: '/trips/mytrips',
        controller: 'MyTripsController as tripsController',
        templateUrl: 'mytrips/mytrips.html',
        data: {
          isProtected: true
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin,
          Trip: 'Trip',
          $cookies: '$cookies',
          MyTrips: function(Trip, $cookies) {
            return Trip.MyTrips.get({
              contact: $cookies.get('userId')
            }).$promise;
          }
        }
      })
 
  }

})();

