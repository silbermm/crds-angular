(function() {
  'use strict';
  require('./signupPage.html');
  angular.module('crossroads.trips')
    .controller('TripsSignupController', require('./tripsSignup.controller'))
    // .controller('PagesController', require('./pages.controller'))
    .factory('TripsSignupService', require('./tripsSignup.service'));

  require('./signupProgress');
  require('./familySelectTool');

  require('./page0');
  require('./pageTemplates');
  // require('./page1');
  // require('./page2');
  // require('./page3');
  // require('./page4');
  // require('./page5');
  // require('./page6');
  // require('./thankyou');

})();
