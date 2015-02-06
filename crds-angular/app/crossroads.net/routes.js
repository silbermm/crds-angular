"use strict";
(function() {
    angular.module("crossroads")
        .config([
            "$stateProvider", "$urlRouterProvider", "$httpProvider", function($stateProvider, $urlRouterProvider, $httpProvider) {


                //================================================
                // Check if the user is connected
                //================================================
                var checkLoggedin = function($q, $timeout, $http, $location, $rootScope) {
                    console.log("checkLoggedIn");
                    // Initialize a new promise
                    var deferred = $q.defer();

                    // Make an AJAX call to check if the user is logged in
                    $http.get("api/authenticated").success(function(user) {
                        // Authenticated
                        if (user.userId !== undefined) {
                            $timeout(deferred.resolve, 0);
                            $rootScope.userid = user.userId;
                            $rootScope.username = user.username;
                            // Not Authenticated
                        } else {
                            Session.clear();
                            $rootScope.message = "You need to log in.";
                            $timeout(function() { deferred.reject(); }, 0);
                            $location.url("/");
                        }
                    }).error(function(e) {

                    });

                    return deferred.promise;
                };
                //================================================


                //================================================
                // Add an interceptor for AJAX errors
                //
                // Commented out because it is redirecting pages you should have access to without logging in to home
                //================================================
                //$httpProvider.interceptors.push(function ($q, $log, $location) {
                //    return {
                //        'response': function (response) {
                //            return response
                //        },
                //        'responseError': function (rejection) {
                //            if(rejection.status == 401){
                //                console.log("user is not authenticated!!!");
                //                $location.url("/");
                //            }
                //            return $q.reject(rejection)
                //        }
                //    }
                //});


                $stateProvider
                    .state("home", {
                        url: "/home",
                        templateUrl: "app/crossroads.net/home/home.html",
                        controller: "HomeCtrl"
                    })
                    .state("login", {
                        url: "/login",
                        templateUrl: "app/crossroads.net/login/login_page.html",
                        controller: "LoginCtrl",
                        data: {
                            isProtected: false
                        }
                    })
                    .state("register", {
                        url: "/register",
                        templateUrl: "app/crossroads.net/register/register_form.html",
                        controller: "RegisterCtrl"
                    })
                    .state("profile", {
                        url: "/profile",
                        templateUrl: "app/modules/profile/profile.html",
                        controller: "crdsProfileCtrl as profile",
                        data: {
                            isProtected: true
                        },
                        resolve: {
                            loggedin: checkLoggedin
                        }
                    })
                    .state("profile.personal", {
                        url: "/personal",
                        controller: "ProfilePersonalController as profile",
                        templateUrl: "app/modules/profile/personal/profile_personal.html",
                        data: {
                            isProtected: true
                        },
                        resolve: {
                            Profile: "Profile",
                            Lookup: "Lookup",
                            genders: function(Lookup) {
                                return Lookup.query({ table: "genders" }).$promise;
                            },
                            maritalStatuses: function(Lookup) {
                                return Lookup.query({ table: "maritalstatus" }).$promise;
                            },
                            serviceProviders: function(Lookup) {
                                return Lookup.query({ table: "serviceproviders" }).$promise;
                            },
                            states: function(Lookup) {
                                return Lookup.query({ lookup: "states" }).$promise;
                            },
                            countries: function(Lookup) {
                                return Lookup.query({ table: "countries" }).$promise;
                            },
                            crossroadsLocations: function(Lookup) {
                                return Lookup.query({ table: "crossroadslocations" }).$promise;
                            },
                            person: function(Profile) {
                                return Profile.Personal.get().$promise;
                            }
                        }
                    })
                    .state("profile.account", {
                        url: "/account",
                        templateUrl: "app/modules/profile/templates/profile_account.html",
                        data: {
                            isProtected: true
                        }
                    })
                    .state("profile.skills", {
                        url: "/skills",
                        controller: "ProfileSkillsController as profile",
                        templateUrl: "app/modules/profile/skills/profile_skills.html",
                        data: {
                            isProtected: true
                        }
                    })
                    .state("opportunities", {
                        url: "/opportunities",
                        controller: "ViewOpportunitiesController as opportunity",
                        templateUrl: "app/modules/opportunity/view/view_opportunities.html",
                        data: {
                            isProtected: true
                        },
                        resolve: {
                            loggedin: checkLoggedin
                        }
                    });

                $urlRouterProvider.otherwise("/home");
            }
        ]);


})()