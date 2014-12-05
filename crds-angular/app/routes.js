angular.module("crossroads").config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise("/home");
    $stateProvider
        .state('home', {
            url: '/home',
            templateUrl: '/app/home/home.html',
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
}])
;