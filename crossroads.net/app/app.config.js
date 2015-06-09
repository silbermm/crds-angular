'use strict()';
(function(){

  var app = angular.module("crossroads.core");
  app.config(AppConfig);

  AppConfig.$inject = ['$httpProvider', '$locationProvider'];

  function AppConfig($httpProvider, $locationProvider){
    $httpProvider.defaults.useXDomain = true;
    $httpProvider.defaults.headers.common['Authorization']= getCookie('sessionId');
  }

})();
