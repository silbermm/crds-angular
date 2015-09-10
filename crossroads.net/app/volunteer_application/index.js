(function(){
  'use strict()';

  var app = angular.module('crossroads');
  require('./volunteerApplicationForm.html');
  //require('../profile/personal/profile_personal.html');
  //require('../profile');

  app.controller('VolunteerApplicationController', require('./volunteerApplication.controller'));
  app.factory('Opportunity', ['$resource', 'Session', require('../services/opportunity_service')]);
  app.factory('VolunteerService', require('./volunteer.service'));

})();
