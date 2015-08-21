(function() {
  'use strict';

  require('./familySelect.html');
  angular.module('crossroads.trips').directive('familySelect', require('./familySelect.directive'));

})();
