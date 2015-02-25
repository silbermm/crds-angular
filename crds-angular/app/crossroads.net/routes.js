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
                    url: "/",
                    templateUrl: "app/crossroads.net/home/home.html",
                    controller: "HomeCtrl"
                })
                .state("homealso", {
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
                    resolve: {
                        loggedin: checkLoggedin
                    },
                    data: {
                        isProtected: true
                    },
                    views : {
                       "" : {
                           templateUrl: "app/modules/profile/profile.html",
                           controller: "crdsProfileCtrl as profile"
                       },
                       "personal@profile" : {
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
                                   return Lookup.query({ table: "states" }).$promise;
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
                       },
                       "account@profile" : {               
                           templateUrl: "app/modules/profile/templates/profile_account.html",
                           data: {
                               isProtected: true
                           }
                       },
                       "skills@profile" : {
                           controller: "ProfileSkillsController as profile",
                           templateUrl: "app/modules/profile/skills/profile_skills.html",
                           data: {
                               isProtected: true
                           }
                       }
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
                })
                 .state("atrium_events", {
                     url: "/atrium-events/:location",
                     controller: "AtriumEventsCtrl",
                     templateUrl: "app/crossroads.net/events/atrium-events.html"
                 })
                .state("content", {
                    url: "/:urlsegment",
                    controller: "ContentCtrl",
                    templateUrl: "app/crossroads.net/content/content.html"
                });
                    //Leave the comment below.  Once we have a true 404 page hosted in the same domain, this is how we 
                    //will handle the routing. 
                    //.state("404", {
                    //    templateUrl: "http://content.crossroads.net/page-not-found/"
                    //});

                $urlRouterProvider.otherwise("/");
            }
        ]);


})()