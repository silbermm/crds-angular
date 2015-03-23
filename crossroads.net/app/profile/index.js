
require('./profile.module');
require('./profile.config');

require('./profile.html')

var app = require('angular').module('crossroads.profile')

app.controller('crdsProfileCtrl', ['$rootScope','Profile', 'Lookup', '$q', '$log','$scope',  require("./profile_controller")]);
 
// Shared Services
app.factory('Lookup', ["$resource", "Session", require('./services/profile_lookup_service')]);
app.factory('Profile', ['$resource',require('./services/profile_service')]);

// Personal
require("./personal/profile_personal.html");
app.controller("ProfilePersonalController", ["$rootScope", "$log","MESSAGES", "genders", "maritalStatuses", "serviceProviders", "states", "countries", "crossroadsLocations", "person", require('./personal/profile_personal_controller')]);
app.directive('uniqueEmail', ['$http', 'Session', require('./personal/profile_unique_email_directive') ]);
app.directive("validateDate", ["$log", require('./personal/profile_valid_date_directive')]);
 
// Skills
require('./skills/profile_skills.html');
app.controller("ProfileSkillsController", ['$rootScope', 'Skills', 'Session', '$log', require('./skills/profile_skills_controllers')]);
app.factory('Skills', ["$resource", require('./skills/profile_skills_service')]);

app.controller('crdsDatePickerCtrl', require('./profile_date_picker'));
require('./profile_account.html');
