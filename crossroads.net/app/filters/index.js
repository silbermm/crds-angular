(function(){
   
  var app = angular.module('crossroads.filters', []);
  app.filter('time', require('./time.filter'));
    
  require('./html.filter');
})()
