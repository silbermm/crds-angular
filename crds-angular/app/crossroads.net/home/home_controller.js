angular.module('crossroads').controller('HomeCtrl', ['$scope', function ($scope) {
    $scope.main = "HomeCtrl";

    $scope.credentials = { username: '', password: '' };

    $scope.login = function (credentials) {
        AuthService.login(credentials).then(function (user) {
            $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
            $scope.setCurrentUser(user);
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
        });
    };
}]);