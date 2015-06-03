'use strict()';
(function(){

  var getCookie = require('./utilities/cookies');

  var app = angular.module("crossroads");
  app.config(AppConfig);

  AppConfig.$inject = ['$httpProvider', '$locationProvider'];

  function AppConfig($httpProvider, $locationProvider){
    $httpProvider.defaults.useXDomain = true;
    $httpProvider.defaults.headers.common['Authorization']= getCookie('sessionId');
  }

})();
