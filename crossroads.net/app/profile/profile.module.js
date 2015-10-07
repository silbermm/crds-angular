(function() {
  'use strict';

  var app = angular.module('crossroads.profile', ['ngResource',
      'ngMessages',
      'ui.bootstrap',
      'ui.router',
      'crossroads.core',
      'crossroads.common']);

  require('./profile.config');

  require('./profile.html');

  app.controller('crdsProfileCtrl', ['$rootScope','Profile', 'Lookup', '$q', '$log','$scope', require('./profile_controller')]);

  // Modal
  require('./editProfile.html');
  app.controller('ProfileModalController', require('./profileModalController'));

  // Shared Services
  app.factory('Lookup', ['$resource', 'Session', require('./services/profile_lookup_service')]);
  app.factory('Profile', ['$resource',require('./services/profile_service')]);
  app.factory('ProfileReferenceData', ['Lookup', 'Profile', '$resolve', require('./services/profile_reference_data')]);

  // Skills
  require('./skills/profile_skills.html');
  app.controller('ProfileSkillsController', ['$rootScope', 'Skills', 'Session', '$log', require('./skills/profile_skills_controllers')]);
  app.factory('Skills', ['$resource', require('./skills/profile_skills_service')]);

  // Giving
  require('./giving');

  app.controller('crdsDatePickerCtrl', require('./profile_date_picker'));
  require('./profile_account.html');
})();
