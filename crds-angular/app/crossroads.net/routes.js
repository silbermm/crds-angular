'use strict';
(function () {
    angular.module("crossroads")

    .config(['$stateProvider', '$urlRouterProvider','$httpProvider', function ($stateProvider, $urlRouterProvider, $httpProvider) {

        //================================================
        // Check if the user is connected
        //================================================
        var checkLoggedin = function ($q, $timeout, $http, $location, $rootScope) {
            console.log("checkLoggedIn");
            // Initialize a new promise
            var deferred = $q.defer();

            // Make an AJAX call to check if the user is logged in
            $http.get('api/authenticated').success(function (user) {
                // Authenticated
                if (user.userId !== undefined) {
                    $timeout(deferred.resolve, 0);
                    $rootScope.userid = user.userId;
                    $rootScope.username = user.username;
                // Not Authenticated
                } else {
                    Session.clear();
                    $rootScope.message = 'You need to log in.';
                    $timeout(function () { deferred.reject(); }, 0);
                    $location.url('/');
                }
            });

            return deferred.promise;
        };
        //================================================


 
        //================================================
        // Add an interceptor for AJAX errors
        //================================================
        $httpProvider.interceptors.push(function ($q, $log, $location) {
            return {
                'response': function (response) {
                    return response
                },
                'responseError': function (rejection) {
                    if(rejection.status == 401){
                        console.log("user is not authenticated!!!");
                        $location.url("/");
                    }
                    return $q.reject(rejection)
                }
            }
        });
    

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
        .state('profile', {
            url: '/profile',
            templateUrl: 'app/modules/profile/templates/profile.html',
            controller: 'crdsProfileCtrl as profile',
            resolve: {
                loggedin: checkLoggedin
            }
        })

        .state('profile.personal', {
            url: '/personal',
            templateUrl: 'app/modules/profile/templates/profile_personal.html',
            //controller: 'crdsProfileCtrl as profile',
            resolve: {
                loggedin: checkLoggedin
            }
        })
        .state("profile.account", {
            url: '/account',
            templateUrl: 'app/modules/profile/templates/profile_account.html',
            //controller: 'crdsProfileCtrl as profile'
        })
        ;

        $urlRouterProvider.otherwise("/home");
    }])










})()