(function(){
  'use strict()';
  require('angular-ui-select');

  var MODULE = 'crossroads.mptools';

  angular.module(MODULE, ['crossroads.core', 'crossroads.common','crossroads.give', 'ui.select']);
  angular.module(MODULE).config(require('./mpTools.config'));
  angular.module(MODULE).factory('MPTools', require('./mpTools.service'));
  angular.module(MODULE).run(require('./mpTools.run'));

  // Require any needed html files
  require('./tools.html');

  // The SU2S Tool
  require('./signup_to_serve');

  // The KC Appliant Tool
  require('./kc_applicant');

  // The Check Batch Processor Tool
  require('./check_batch_processor');

  // The Trip Participant Tool
  require('./trip_participants');

  // The Trip Private Invitation Tool
  require('./trip_private_invite');

  // The GP Export Tool
  require('./gp_export');
})();
