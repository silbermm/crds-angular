'use strict()';
(function(){

  var app = angular.module("crossroads.core");
  app.config(AppConfig);

  AppConfig.$inject = ['$httpProvider', '$locationProvider'];

  function AppConfig($httpProvider, $locationProvider){
    $httpProvider.defaults.useXDomain = true;
    // TODO: Do we need to reference differently here?
    $httpProvider.defaults.headers.common['Authorization']= crds_utilities.getCookie('sessionId');
  }

})();
