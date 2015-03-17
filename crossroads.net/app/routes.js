"use strict";

(function () {

    require("./home/home.html");
    require('./home');

    require('./login/login_page.html');
    require('./register/register_form.html');

    require('./content');

    require('./opportunity');

    require('./profile/profile.html');

    require('./styleguide');
    require('./give');
    require('./myprofile');

    require('./profile/personal/profile_personal.html');
    require('./profile/profile_account.html');
    require('./profile/skills/profile_skills.html');
    require('./opportunity/view_opportunities.html');
    require('./content/content.html');
    var getCookie = require('./utilities/cookies');

    angular.module("crossroads").config(["$stateProvider", "$urlRouterProvider", "$httpProvider", function ($stateProvider, $urlRouterProvider, $httpProvider) {

        $httpProvider.defaults.useXDomain = true;
        $httpProvider.defaults.headers.common['Authorization'] = getCookie('sessionId');

        //================================================
        // Check if the user is connected
        //================================================
        var checkLoggedin = function ($q, $timeout, $http, $location, $rootScope) {
            console.log("CONFIG: checkLoggedIn");
            var deferred = $q.defer();
            $httpProvider.defaults.headers.common['Authorization'] = getCookie('sessionId');
            $http({
                method: 'GET',
                url: __API_ENDPOINT__ + "api/authenticated",
                headers: {
                    'Authorization': getCookie('sessionId')
                }
            }).success(function (user) {
                // Authenticated
                if (user.userId !== undefined) {
                    $timeout(deferred.resolve, 0);
                    $rootScope.userid = user.userId;
                    $rootScope.username = user.username;
                } else {
                    Session.clear();
                    $rootScope.message = "You need to log in.";
                    $timeout(function () {
                        deferred.reject();
                    }, 0);
                    $location.url("/");
                }
            }).error(function (e) {
                console.log(e);
                console.log("ERROR: trying to authenticate");
            });
            return deferred.promise;
        };

        $stateProvider
            .state("home", {
                url: "/",
                templateUrl: "home/home.html",
                controller: "HomeCtrl"
            })
            .state("homealso", {
                url: "/home",
                templateUrl: "home/home.html",
                controller: "HomeCtrl"
            })
            .state("login", {
                url: "/login",
                templateUrl: "login/login_page.html",
                controller: "LoginCtrl",
                data: {
                    isProtected: false
                }
            })
            .state("register", {
                url: "/register",
                templateUrl: "register/register_form.html",
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
                views: {
                    "": {
                        templateUrl: "profile/profile.html",
                        controller: "crdsProfileCtrl as profile",
                        resolve: {
                            loggedin: checkLoggedin
                        },
                    },
                    "personal@profile": {
                        controller: "ProfilePersonalController as profile",
                        templateUrl: "personal/profile_personal.html",
                        data: {
                            isProtected: true
                        },
                        resolve: {
                            loggedin: checkLoggedin,
                            Profile: "Profile",
                            Lookup: "Lookup",
                            genders: function (Lookup) {

                                return Lookup.query({
                                    table: "genders"
                                }).$promise;
                            },
                            maritalStatuses: function (Lookup) {
                                return Lookup.query({
                                    table: "maritalstatus"
                                }).$promise;
                            },
                            serviceProviders: function (Lookup) {
                                return Lookup.query({
                                    table: "serviceproviders"
                                }).$promise;
                            },
                            states: function (Lookup) {
                                return Lookup.query({
                                    table: "states"
                                }).$promise;
                            },
                            countries: function (Lookup) {
                                return Lookup.query({
                                    table: "countries"
                                }).$promise;
                            },
                            crossroadsLocations: function (Lookup) {
                                return Lookup.query({
                                    table: "crossroadslocations"
                                }).$promise;
                            },
                            person: function (Profile) {
                                return Profile.Personal.get().$promise;
                            }
                        }
                    },
                    "account@profile": {
                        templateUrl: "profile/profile_account.html",
                        data: {
                            isProtected: true
                        }
                    },
                    "skills@profile": {
                        controller: "ProfileSkillsController as profile",
                        templateUrl: "skills/profile_skills.html",
                        data: {
                            isProtected: true
                        }
                    }
                }
            })
            .state("myprofile", {
                url: "/myprofile",
                controller: "MyProfileCtrl as myProfile",
                templateUrl: "myprofile/myprofile.html",
                data: {
                    isProtected: true
                },
                resolve: {
                    loggedin: checkLoggedin
                }
            })
            .state("opportunities", {
                url: "/opportunities",
                controller: "ViewOpportunitiesController as opportunity",
                templateUrl: "opportunity/view_opportunities.html",
                data: {
                    isProtected: true
                },
                resolve: {
                    loggedin: checkLoggedin
                }
            })
            .state("styleguide", {
                url: "/styleguide",
                controller: "StyleguideCtrl as styleguide",
                templateUrl: "styleguide/styleguide.html"
            })
            .state("give", {
                url: "/give",
                controller: "GiveCtrl as give",
                templateUrl: "give/give.html",
                data: {
                    isProtected: true
                },
                resolve: {
                    loggedin: checkLoggedin
                }
            })
            .state("content", {
                url: "/:urlsegment",
                controller: "ContentCtrl",
                templateUrl: "content/content.html"
            });
        //Leave the comment below.  Once we have a true 404 page hosted in the same domain, this is how we 
        //will handle the routing. 
        //.state("404", {
        //    templateUrl: __CMS_ENDPOINT__ + "/page-not-found/"
        //});

        $urlRouterProvider.otherwise("/");
                    }]);
})()