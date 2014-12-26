'use strict';
(function () {
angular.module("crossroads").config(['$stateProvider', '$urlRouterProvider',  function ($stateProvider, $urlRouterProvider) {
    
    $stateProvider
        .state('home', {
            url: '/home',
            templateUrl: 'app/crossroads.net/home/home.html',
            controller: 'HomeCtrl',
            data: {
                require_login: false
            }
        })
        .state('login', {
            url: '/login',
            templateUrl: 'app/crossroads.net/login/login.html',
            controller: 'LoginCtrl',
            data: { 
                require_login: false
            }
        })
    ;

    $urlRouterProvider.otherwise("/home");

  }])
})()
