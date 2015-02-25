angular.module('crossroads').controller('AtriumEventsCtrl', ['$scope','$log', '$stateParams', function ($scope,$log,$stateParams) {
    $log.debug("AtriumEventsController Loaded");
    $log.debug($stateParams);
    $scope.main = "AtriumEventsCtrl";
}]);