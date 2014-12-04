angular.module('crossroads').controller('appCtrl', ['$scope', 'AuthService', function ($scope, AuthService) {
    $scope.main = "appCtrl";

    $scope.currentUser = null;
    $scope.isAuthorized = AuthService.isAuthorized;
    $scope.isLoginPage = false;

    console.log('isLoginPage: ' + $scope.isLoginPage);

    $scope.setCurrentUser = function(user) {
        $scope.currentUser = user;
    };
}]);

angular.module('crossroads').controller('LoginCtrl', ['$scope', '$rootScope', 'AUTH_EVENTS', 'AuthService', function ($scope, $rootScope, AUTH_EVENTS, AuthService) {
    $scope.main = "LoginCtrl";

    $scope.credentials = { username: '', password: '' };
    $scope.isLoginPage = true;
    console.log('isLoginPage: ' + $scope.isLoginPage);

    $scope.login = function (credentials) {
        AuthService.login(credentials).then(function (user) {
            $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
            $scope.setCurrentUser(user);
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
        });
    };

}]);

angular.module('crossroads').controller('HomeCtrl', ['$scope', function ($scope) {
    $scope.main = "HomeCtrl";

    $scope.credentials = { username: '', password: '' };

    $scope.login = function(credentials) {
        AuthService.login(credentials).then(function (user) {
            $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
            $scope.setCurrentUser(user);
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
        });
    };

    
}]);

angular.module('crossroads').controller('ProfileCtrl', [
    '$scope', function($scope) {
        $scope.main = 'ProfileCtrl';
    }
]);

angular.module('crossroads').constant('AUTH_EVENTS', {
    loginSuccess: 'auth-login-success',
    loginFailed: 'auth-login-failed',
    logoutSuccess: 'auth-logout-success',
    sessionTimeout: 'auth-session-timeout',
    notAuthenticated: 'auth-not-authenticated',
    notAuthorized: 'auth-not-authorized'
});

angular.module('crossroads').factory('AuthService', function($http, Session) {
    var authService = {};

    authService.login = function(credentials) {
        return $http
            .post('/api/login', credentials)
            .then(function(res) {
                Session.create(res.data.id, res.data.username);
                return res.data.username;
            });
    };

    authService.isAuthenticated = function() {
        return !!Session.userId;
    };

    authService.isAuthorized = function(authorizedRoles) {
        if (!angular.isArray(authorizedRoles)) {
            authorizedRoles = [authorizedRoles];
        }
        return (authService.isAuthenticated() &&
            authorizedRoles.indexOf(Session.userRole) !== -1);
    };

    return authService;
});

angular.module('crossroads').service('Session', function() {
    this.create = function(sessionId, userId) {
        this.id = sessionId;
        this.userId = userId;
        //this.userRole = userRole;
    };
    this.destroy = function() {
        this.id = null;
        this.userId = null;
        //this.userRole = null;
    };
    return this;
});

angular.module('crossroads').directive('loginDialog', function(AUTH_EVENTS) {
    return {
        restrict: 'A',
        template: '<div ng-if="visible" ng-include="\'/app/crossroads.net/templates/login-dialog.html\'">',
        link: function(scope) {
            var showDialog = function () {
                console.log('not logged in');
                scope.visible = true;
            };

            scope.visible = false;
            scope.$on(AUTH_EVENTS.notAuthenticated, showDialog);
            scope.$on(AUTH_EVENTS.sessionTimeout, showDialog);
        }
    };
});


