(function(){

  'use strict()';
  require('angular-ui-select');
  

  var MODULE = 'crossroads.mptools';

  angular.module(MODULE, ['crossroads.core', 'ui.select']); 
  angular.module(MODULE).config(require('./mpTools.config'));
  angular.module(MODULE).factory('MPTools', require('./mpTools.service'));
  angular.module(MODULE).run(require('./mpTools.run'));

  // Require any needed html files
  require('./tools.html');

  // The SU2S Tool
  require('./signup_to_serve');

  // The KC Appliant Tool
  require('./kc_applicant');

  // The Trip Participant Tool
  require('./trip_participants');

})();
