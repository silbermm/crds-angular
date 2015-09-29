(function() {
  'use strict';

  module.exports = TripRoutes;

  TripRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function TripRoutes($stateProvider, $urlMatcherFactory, $locationProvider) {

    $stateProvider
      .state('tripsearch', {
        parent: 'noSideBar',
        url: '/trips/search',
        controller: 'TripSearchCtrl as tripSearch',
        templateUrl: 'tripsearch/tripsearch.html',
        resolve: {
          Page: 'Page',
          CmsInfo: function(Page, $stateParams) {
            return Page.get({
              url: '/tripgiving/'
            }).$promise;
          }
        },
        data: {
          meta: {
            title: 'Trip Search',
            description: ''
          }
        }
      })
      .state('tripgiving', {
        parent: 'noSideBar',
        url: '/trips/giving/:eventParticipantId',
        controller: 'TripGivingController as tripGiving',
        templateUrl: 'tripgiving/tripgiving.html',
        resolve: {
          Trip: 'Trip',
          $stateParams: '$stateParams',
          TripParticipant: function(Trip, $stateParams) {
            return Trip.TripParticipant.get({
              tripParticipantId: $stateParams.eventParticipantId
            }).$promise;
          }
        },
        data: {
          meta: {
            title: 'Trip Giving',
            description: ''
          }
        }
      })
      .state('tripgiving.amount', {
        templateUrl: 'tripgivingTemplates/amount.html'
      })
      .state('tripgiving.login', {
        controller: 'LoginCtrl',
        templateUrl: 'tripgivingTemplates/login.html'
      })
      .state('tripgiving.register', {
        controller: 'RegisterCtrl',
        templateUrl: 'tripgivingTemplates/register.html'
      })
      .state('tripgiving.confirm', {
        templateUrl: 'tripgivingTemplates/confirm.html'
      })
      .state('tripgiving.account', {
        templateUrl: 'tripgivingTemplates/account.html'
      })
      .state('tripgiving.change', {
        templateUrl: 'tripgivingTemplates/change.html'
      })
      .state('tripgiving.thank-you', {
        templateUrl: 'tripgivingTemplates/thank_you.html'
      })
      .state('mytrips', {
        parent: 'noSideBar',
        url: '/trips/mytrips',
        controller: 'MyTripsController as tripsController',
        templateUrl: 'mytrips/mytrips.html',
        data: {
          isProtected: true,
          meta: {
            title: 'My Trips',
            description: ''
          }
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin,
          Trip: 'Trip',
          $cookies: '$cookies',
          MyTrips: function(Trip, $cookies) {
            return Trip.MyTrips.get().$promise;
          }
        }
      })
      .state('tripsignup', {
        parent: 'noSideBar',
        url: '/trips/:campaignId?invite',
        templateUrl: 'page0/page0.html',
        controller: 'Page0Controller as page0',
        data: {
          isProtected: true,
          meta: {
            title: 'Trip Signup',
            description: 'Select the family member you want to signup for a trip'
          }
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin,
          $cookies: '$cookies',
          Trip: 'Trip',
          $stateParams: '$stateParams',
          Campaign: function(Trip, $stateParams) {
            return Trip.Campaign.get({campaignId: $stateParams.campaignId}).$promise;
          },

          Family: function(Trip, $stateParams) {
            return Trip.Family.query({pledgeCampaignId: $stateParams.campaignId}).$promise;
          }
        }
      })
      .state('tripsignup.application', {
        parent: 'noSideBar',
        url: '/trips/:campaignId/signup/:contactId?invite',
        templateUrl: 'signup/signupPage.html',
        controller: 'TripsSignupController as tripsSignup',
        data: {
          isProtected: true,
          meta: {
            title: 'Trip Signup',
            description: ''
          }
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin,
          $cookies: '$cookies',
          contactId: function($cookies) {
            return $cookies.get('userId');
          },

          Trip: 'Trip',
          $stateParams: '$stateParams',
          Campaign: function(Trip, $stateParams) {
            return Trip.Campaign.get({campaignId: $stateParams.campaignId}).$promise;
          },

          Profile: 'Profile',
          Person: function(Profile, $stateParams, $cookies) {
            var cid = $cookies.get('userId');
            if ($stateParams.contactId) {
              cid = $stateParams.contactId;
            }

            return Profile.Person.get({contactId: cid}).$promise;
          },

          Lookup: 'Lookup',

          WorkTeams: function(Trip) {
            return Trip.WorkTeams.query().$promise;
          },

          pageId: function() {
            return 0;
          }
        }
      })
      .state('tripsignup.application.thankyou', {
        url: '/thankyou',
        templateUrl: 'pageTemplates/thankYou.html',
      })
      .state('tripsignup.application.page', {
        url: '/:stepId',
        templateUrl: function($stateParams) {
          var template = 'pageTemplates/signupPage' + $stateParams.stepId + '.html';
          return template;
        },

        controller: 'SignupStepController as signupStep',
      });
  }

})();
