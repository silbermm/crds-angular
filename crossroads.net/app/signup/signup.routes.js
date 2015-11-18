(function() {
  'use strict';

  module.exports = SignupRoutes;

  SignupRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function SignupRoutes($stateProvider, $urlMatcherFactory, $locationProvider) {

    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'signupRouteType', /\/sign-up\/.*$/);

    $stateProvider.state('signup', {
      parent: 'noSideBar',
      url: '{link:signupRouteType}',
      template: '<crds-signup></crds-signup>',
      data: {
        isProtected: true,
        meta: {
          title: 'Community Group Signup',
          description: ''
        }
      },
      resolve: {
        loggedin: crds_utilities.checkLoggedin,
        $stateParams: '$stateParams',
        Page: 'Page',
        SignupService: 'SignupService',
        $q: '$q',
        Group: 'Group',
        CmsInfo: function($q, Page, SignupService, Group, $stateParams) {
          var deferred = $q.defer();

          Page.get({link: $stateParams.link}).$promise.then(function(data){
            SignupService.cmsInfo = data;
            Group.Detail.get({groupId: data.pages[0].group}).$promise.then(function(group) {
                SignupService.group = group;
                deferred.resolve(); 
              }, function() {
                deferred.reject();
              });
          }, function() {
            deferred.reject();                                                 
          });

          return deferred.promise;
        }
      }
    });
  }

})();
