(function() {
  'use strict';
  var constants = require('../../../constants');

  angular.module(constants.MODULES.COMMON).
    controller('ProfilePersonalController', require('./profilePersonal.controller'))
  .controller('ConfirmPasswordCtrl', require('./confirmPassword.controller'))
  .directive('uniqueEmail', ['$http', 'Session', 'User', require('./profileUniqueEmail.directive')])
  .directive('validateDate', ['$log', require('./profileValidDate.directive')])
  .directive('profilePersonal', require('./profilePersonal.directive'))
  ;

  require('./profilePersonal.template.html');
  require('./confirmPassword.html');

})();
