(function () {
  'use strict';
  module.exports = ExploreCtrl;

  ExploreCtrl.$inject = ['$scope', '$window', '$stateParams', '$log', '$location', '$anchorScroll'];

function ExploreCtrl($scope, $window, $stateParams, $log, $location, $anchorScroll) {
		var vm = this;

    var index = parseInt($window.location.hash.slice(1), 10);

    $scope.snapAnimation = false; // turn animation off for the initial snap on page load
    if (index && angular.isNumber(index)) {
      $scope.snapIndex = index;
    } else {
      $scope.snapIndex = 0;
    }
    $scope.$on('arrow-up', function () {
      $scope.$apply(function () {
        $scope.snapIndex--;
      });
    });
    $scope.$on('arrow-down', function () {
      $scope.$apply(function () {
        $scope.snapIndex++;
      });
    });
    $scope.swipeUp = function () {
      $scope.snapIndex++;
    };
    $scope.swipeDown = function () {
      $scope.snapIndex--;
    };
    $scope.afterSnap = function (snapIndex) {
      $scope.snapAnimation = true; // turn animations on after the initial snap
      $window.location.hash = snapIndex;
    };

	};
})()
