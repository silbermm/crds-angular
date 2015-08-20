(function() {
  'use strict()';

  var MODULE = 'crossroads.mptools';

  // HTML Files
  require('./trip.html');

  angular.module(MODULE).controller('TripParticipantController', require('./tripParticipant.controller'));

})();
