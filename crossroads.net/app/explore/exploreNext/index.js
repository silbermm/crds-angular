(function() {
  require('./exploreNext.html');

  var app = angular.module("crossroads");

  app.directive('exploreNext', require('./exploreNext.directive'));
})();
