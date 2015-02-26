"use strict";

(function() {

  require("./home/home.html");
  require('./home');

  require('./login/login_page.html');

  angular.module("crossroads").config([ "$stateProvider", "$urlRouterProvider", "$httpProvider", function($stateProvider, $urlRouterProvider, $httpProvider) { 

    //================================================
    // Check if the user is connected
    //================================================
    var checkLoggedin = function($q, $timeout, $http, $location, $rootScope) {
        console.log("checkLoggedIn");
        // Initialize a new promise
        var deferred = $q.defer();

        // Make an AJAX call to check if the user is logged in
        $http.get(__API_ENDPOINT__ + "api/authenticated").success(function(user) {
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
