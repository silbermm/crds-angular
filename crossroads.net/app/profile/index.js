'use strict';

require('angular-module-resource');
require('angular-bootstrap-npm');
require('angular-ui-router');
require('angular-messages');
require('../password_field/password_field_directive');
require('../email_field/email_field_directive');

var getCookie = require('../utilities/cookies'); 

var app = require('angular').module('crdsProfile', ['ngResource', 'ngMessages', 'ui.bootstrap', 'ui.router', 'password_field','email_field'])
  .config(['$httpProvider', '$stateProvider', '$urlRouterProvider', function ($httpProvider, $stateProvider, $urlRouterProvider) {
    $httpProvider.defaults.timeout = 15000;
    $httpProvider.defaults.useXDomain = true; 
    $httpProvider.defaults.headers.common['Authorization']= getCookie('sessionId');
}]);

app.controller('crdsProfileCtrl', ['$rootScope','Profile', 'Lookup', '$q', '$log','$scope',  require("./profile_controller")]);
 
// Shared Services
app.factory('Lookup', ["$resource", "Session", require('./services/profile_lookup_service')]);
app.factory('Profile', ['$resource',require('./services/profile_service')]);

// Personal
//require("./personal/profile_personal.html");
app.controller("ProfilePersonalController", ["$rootScope", "$log","MESSAGES", "genders", "maritalStatuses", "serviceProviders", "states", "countries", "crossroadsLocations", "person", require('./personal/profile_personal_controller')]);
app.directive('uniqueEmail', ['$http', require('./personal/profile_unique_email_directive') ]);
app.directive("validateDate", ["$log", require('./personal/profile_valid_date_directive')]);
 
// Skills
require('./skills/profile_skills.html');
app.controller("ProfileSkillsController", ['$rootScope', 'Skills', '$log', require('./skills/profile_skills_controllers')]);
app.factory('Skills', ["$resource", require('./skills/profile_skills_service')]);

app.controller('crdsDatePickerCtrl', require('./profile_date_picker'));