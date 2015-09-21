(function() {
  'use strict';

  var constants = require('../../../constants.js');

  angular.module(constants.MODULES.COMMON)
    .directive('profilePicture', require('./profilePicture.directive'))
    .controller('ProfilePictureController', require('./profilePicture.controller'))
    ;

  require('./profilePicture.html');
})();
