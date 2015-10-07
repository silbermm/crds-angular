(function() {
  'use strict()';

  var module = 'crossroads.profile';

  require('./profile_giving.html');
  angular.module(module).controller('ProfileGivingController', require('./profile_giving.controller'));

})();
