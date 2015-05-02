(function(){
   
  var app = require('angular').module('crossroads.filters', []);
  app.filter('time', require('./time.filter'));
    
  require('./html.filter');
})()
