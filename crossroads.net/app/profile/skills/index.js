(function() {

  var MODULES = require('crds-constants').MODULES;

  angular.module(MODULES.PROFILE)
    .directive('profileSkills', require('./profileSkills.directive'))
    .factory('Skills', require('./profileSkills.service'))
    .controller('ProfileSkillsController', require('./profileSkills.controller'))
    ;

  require('./profileSkills.html');

})();
