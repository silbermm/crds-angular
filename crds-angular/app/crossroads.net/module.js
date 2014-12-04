var app = angular.module('crossroads', ['crdsProfile', 'ui.router']);

//app.config([
//    '$routeProvider', '$locationProvider', function($routeProvider, $locationProvider) {
//        $routeProvider.
//            when('/home', {
//                templateUrl: '/Scripts/app/crossroads.net/templates/home.html',
//                controller: 'HomeCtrl',
//                caseInsensitiveMatch: true
//            }).
//            when('/profile', {
//                templateUrl: '/Scripts/app/crossroads.net/templates/profile.html',
//                controller: 'ProfileCtrl',
//                caseInsensitiveMatch: true
//            }).
//            when('/login', {
//                templateUrl: '/Scripts/app/crossroads.net/templates/login.html',
//                controller: 'LoginCtrl'
//            }).
//            otherwise({
//                redirectTo: '/home'
//            });;
//        $locationProvider.html5Mode(true);
//    }
//]);

app.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise("/home");

        $stateProvider
            .state('home', {
                url: '/home',
                templateUrl: '/app/crossroads.net/templates/home.html',
                controller: 'HomeCtrl',
                data: {
                    require_login: false
                }
            })
            .state('login', {
                url: '/login',
                templateUrl: '/app/crossroads.net/templates/login.html',
                controller: 'LoginCtrl',
                data: {
                    require_login: false
                }
            })
            .state('profile', {
                url: '/profile',
                templateUrl: '/app/crossroads.net/templates/profile.html',
                controller: 'ProfileCtrl',
                data: {
                    require_login: true
                }
            });
    }
]);


app.run(function($rootScope, AUTH_EVENTS, AuthService) {
    $rootScope.$on('$stateChangeStart', function(event, next) {
        //var authorizedRoles = next.data.authorizedRoles;
        var requireLogin = next.data.require_login;
        console.log(requireLogin);
        //if (!AuthService.isAuthorized(authorizedRoles)) {
        //event.preventDefault();
        if (requireLogin) {
            if (AuthService.isAuthenticated()) {
                // user is not allowed
                $rootScope.$broadcast(AUTH_EVENTS.notAuthorized);
            } else {
                // user is not logged in
                event.preventDefault();
                $rootScope.$broadcast(AUTH_EVENTS.notAuthenticated);
            }
        }
        //}
    });
})