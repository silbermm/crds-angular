(function() {
  'use strict';
  require('./signupPage.html');
  angular.module('crossroads.trips')
    .controller('TripsSignupController', require('./tripsSignup.controller'))
    .controller('PagesController', require('./pages.controller'))
    .factory('SignupPage2Service', require('./page2/signupPage2.service'));

  require('./signupProgress');
  require('./familySelectTool');

  require('./page1');
  require('./page2');
  require('./page3');
  require('./page4');
  require('./page5');
  require('./page6');
  require('./thankyou');

})();
