(function() {
  'use strict()';

  var MODULE = require('crds-constants').MODULES.MPTOOLS;

  // HTML Files
  require('./contact.html');

  angular.module(MODULE)
    .directive('volunteerContact', require('./volunteerContact.component'))

    //.service('VolunteerContactService', require('./volunteerContact.service'))
    ;

  require('./volunteer_contact_form');

})();
