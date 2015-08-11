(function(){
  'use strict'; 
 
  var MODULE = 'crossroads.trips';

  angular.module(MODULE, ['crossroads.core']);

  angular.module(MODULE, require('./trips.routes'));
  
  require('./mytrips');
  require('./tripgiving');

})();

