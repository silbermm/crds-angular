(function(){
   
  var app = angular.module('crossroads.core');
  app.filter('time', require('./time.filter'));

  require('./html.filter');
  require('./truncate.filter');
})()
