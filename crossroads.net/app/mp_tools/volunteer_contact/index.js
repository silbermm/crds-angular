(function() {
  'use strict()';

  var MODULE = 'crossroads.mptools';

  // HTML Files
  require('./contact.html');

  angular.module(MODULE).controller('VolunteerContactController', require('./volunteerContact.controller'));

})();
