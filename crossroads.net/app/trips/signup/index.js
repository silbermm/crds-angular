(function() {
  'use strict';
  require('./signupPage.html');
  angular.module('crossroads.trips')
    .controller('TripsSignupController', require('./tripsSignup.controller'))
    .controller('SignupStepController', require('./signupStep.controller'))
    .factory('TripsSignupService', require('./tripsSignup.service'));

  require('./signupProgress');
  require('./familySelectTool');

  require('./page0');
  require('./pageTemplates');

})();
