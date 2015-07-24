'use strict';
module.exports = function($rootScope, $scope, $state, $stateParams, $log, ContentPageService) {
  $scope.main = 'ContentCtrl';
  $scope.params = $stateParams;
  $scope.page = ContentPageService.page;
};
