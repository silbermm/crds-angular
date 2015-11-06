(function() {
  'use strict';

  module.exports = CommunityGroupRoutes;

  CommunityGroupRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function CommunityGroupRoutes($stateProvider, $urlMatcherFactory, $locationProvider) {

    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'signupRouteType', /\/sign-up\/.*$/);

    $stateProvider.state('community-groups-signup', {
      parent: 'noSideBar',
      url: '{link:signupRouteType}',
      controller: 'CommunityGroupSignupController as groupsignup',
      templateUrl: 'community_groups_signup/communityGroupSignupForm.html',
      data: {
        isProtected: true,
        meta: {
          title: 'Community Group Signup',
          description: ''
        }
      },
      resolve: {
        loggedin: crds_utilities.checkLoggedin
      }
    });
  }

})();
