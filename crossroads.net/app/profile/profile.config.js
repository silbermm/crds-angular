'use strict()';
(function(){
 
  var app = angular.module('crossroads.profile');
  app.config(config);

  function config($httpProvider){
    $httpProvider.defaults.timeout = 15000;
    $httpProvider.defaults.useXDomain = true; 
    $httpProvider.defaults.headers.common['Authorization']= getCookie('sessionId');
  };
  
})()
