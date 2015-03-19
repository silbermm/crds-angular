'use strict()';
(function(){

  var getCookie = require('./utilities/cookies'); 
  
  var app = require("angular").module("crossroads"); 
  app.config(AppConfig);
  
  function AppConfig($httpProvider){
    $httpProvider.defaults.useXDomain = true;
    $httpProvider.defaults.headers.common['Authorization']= getCookie('sessionId');
  }

})();


