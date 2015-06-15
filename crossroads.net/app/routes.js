"use strict";

(function() {

  angular.module("crossroads").config(["$stateProvider", "$urlRouterProvider", "$httpProvider", "$urlMatcherFactoryProvider", "$locationProvider", function($stateProvider, $urlRouterProvider, $httpProvider, $urlMatcherFactory, $locationProvider) {

    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, "contentRouteType", /^\/.*/);
    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, "signupRouteType", /\/sign-up\/.*$/);
    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, "volunteerRouteType", /\/volunteer-sign-up\/.*$/);

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
          loggedin: crds_utilities.checkLoggedin
        },
        data: {
          isProtected: true
        },
        views: {
          "": {
            templateUrl: "profile/profile.html",
            controller: "crdsProfileCtrl as profile",
            resolve: {
              loggedin: crds_utilities.checkLoggedin
            },
          },
          "personal@profile": {
            templateUrl: "personal/profile_personal.html",
            data: {
              isProtected: true
            },
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
          loggedin: crds_utilities.checkLoggedin
        }
      })
      .state("mytrips", {
        url: "/mytrips",
        templateUrl: "mytrips/mytrips.html"
      })
      .state("media", {
        url: "/media",
        controller: "MediaCtrl as media",
        templateUrl: "media/view-all.html"
      })
      .state("media-music", {
        url: "/media/music",
        controller: "MediaCtrl as media",
        templateUrl: "media/view-all-music.html"
      })
      .state("media-messages", {
        url: "/media/messages",
        controller: "MediaCtrl as media",
        templateUrl: "media/view-all-messages.html"
      })
      .state("media-videos", {
        url: "/media/videos",
        controller: "MediaCtrl as media",
        templateUrl: "media/view-all-videos.html"
      })
      .state("media-series-single", {
        url: "/media/series/single",
        controller: "MediaCtrl as media",
        templateUrl: "media/series-single.html"
      })
      .state("media-series-single-lo-res", {
        url: "/media/series/single/lores",
        controller: "MediaCtrl as media",
        templateUrl: "media/series-single-lo-res.html"
      })
      .state("serve-signup", {
        url: "/serve-signup",
        controller: "MyServeController as serve",
        templateUrl: "my_serve/myserve.html",
        data: {
          isProtected: true
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin,
          ServeOpportunities: 'ServeOpportunities',
          Groups: function(ServeOpportunities) {
            return ServeOpportunities.ServeDays.query({
              id: crds_utilities.getCookie('userId')
            }).$promise;
          }
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
        resolve: {
          programList: function(getPrograms) {
            // TODO The number one relates to the programType in MP. At some point we should fetch
            // that number from MP based in human readable input here.
            return getPrograms.Programs.get({
              programType: 1
            }).$promise;
          }
        }
      })
      .state("give.amount", {
        url: "/amount",
        templateUrl: "give/amount.html"
      })
      .state("give.login", {
        url: "/login",
        controller: "LoginCtrl",
        templateUrl: "give/login.html"
      })
      .state("give.register", {
        url: "/register",
        controller: "RegisterCtrl",
        templateUrl: "give/register.html"
      })
      .state("give.confirm", {
        url: "/confirm",
        templateUrl: "give/confirm.html"
      })
      .state("give.account", {
        url: "/account",
        templateUrl: "give/account.html"
      })
      .state("give.change", {
        url: "/change",
        templateUrl: "give/change.html",
        resolve: {
          programList: function(getPrograms) {
            // TODO The number one relates to the programType in MP. At some point we should fetch
            // that number from MP based in human readable input here.
            return getPrograms.Programs.get({
              programType: 1
            }).$promise;
          }
        }
      })
      .state("give.thank-you", {
        url: "/thank-you",
        templateUrl: "give/thank_you.html"
      })
      //Not a child route of give because I did not want to use the parent give template
      .state("history", {
        url: "/give/history",
        templateUrl: "give/history.html"
      })
      .state("demo", {
        //abstract: true,
        url: '/demo',
        template: '<p>demo</p>'
      })
      .state("go_trip_giving", {
        url: "/go_trip_giving",
        controller: "GoTripGivingCtrl as gotripsearch",
        templateUrl: "go_trip_giving/go_trip_giving.html"
      })
      .state("go_trip_giving_results", {
        url: "/go_trip_giving_results",
        controller: "GoTripGivingCtrl as gotripresults",
        templateUrl: "go_trip_giving/go_trip_giving_results.html"
      })
      .state("/demo/guest-giver", {
        url: "/demo/guest-giver",
        templateUrl: "guest_giver/give.html"
      })
      .state("/demo/guest-giver/login", {
        url: "/demo/guest-giver/login",
        templateUrl: "guest_giver/give-login.html"
      })
      .state("/demo/guest-giver/login-guest", {
        url: "/demo/guest-giver/login-guest",
        controller: "GiveCtrl as give",
        templateUrl: "guest_giver/give-login-guest.html"
      })
      .state("/demo/guest-giver/give-confirmation", {
        url: "/demo/guest-giver/confirmation",
        templateUrl: "guest_giver/give-confirmation.html"
      })
      .state("/demo/guest-giver/give-register", {
        url: "/demo/guest-giver/register",
        templateUrl: "guest_giver/give-register.html"
      })
      .state("/demo/guest-giver/give-logged-in-bank-info", {
        url: "/demo/guest-giver/logged-in-bank-info",
        controller: "GiveCtrl as give",
        templateUrl: "guest_giver/give-logged-in-bank-info.html"
      })
      .state("/demo/guest-giver/give-confirm-amount", {
        url: "/demo/guest_giver/give-confirm-amount",
        templateUrl: "guest_giver/give-confirm-amount.html"
      })
      .state("/demo/guest-giver/give-change-information", {
        url: "/demo/guest_giver/give-change-information",
        controller: "GiveCtrl as give",
        templateUrl: "guest_giver/give-change-information.html"
      })
      .state("/demo/logged-in-giver/existing-giver", {
        url: "/demo/logged-in-giver/existing-giver",
        templateUrl: "guest_giver/give-logged-in.html"
      })
      .state("/demo/logged-in-giver/change-information", {
        url: "/demo/logged-in-giver/change-information",
        controller: "GiveCtrl as give",
        templateUrl: "guest_giver/give-change-information-logged-in.html"
      })
      .state("/demo/logged-in-giver/new-giver", {
        url: "/demo/logged-in-giver/new-giver",
        templateUrl: "guest_giver/give-logged-in-new-giver.html"
      })
      .state("/demo/go-trip-giving", {
        url: "/demo/go-trip-giving",
        templateUrl: "trip_giving/give.html"
      })
        .state("search", {
        url: "/search-results",
        controller: "SearchCtrl as search",
        templateUrl: "search/search-results.html"
      })
      .state("community-groups-signup", {
        url: "{link:signupRouteType}",
        controller: "GroupSignupController as groupsignup",
        templateUrl: "community_groups_signup/group_signup_form.html",
        data: {
          isProtected: true
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin
        }
      })
      .state("volunteer-request", {
        url: "{link:volunteerRouteType}",
        controller: "VolunteerController as volunteer",
        templateUrl: "volunteer_signup/volunteer_signup_form.html",
        data: {
          isProtected: true
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin,
          Page: 'Page',
          CmsInfo: function(Page, $stateParams) {
            return Page.get({
              url: $stateParams.link
            }).$promise;
          }
        }
      })
      .state("volunteer-application", {
        url: "/volunteer-application/:appType/:id",
        controller: "VolunteerApplicationController as volunteer",
        templateUrl: "volunteer_application/volunteerApplicationForm.html",
        data: {
          isProtected: true
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin,
          Page: 'Page',
          CmsInfo: function(Page, $stateParams) {
            var path = '/volunteer-application/' + $stateParams.appType + '/';
            return Page.get({
              url: path
            }).$promise;
          },
          Profile: 'Profile',
          Contact: function(Profile, $stateParams) {
            var contactId = $stateParams.id;
            return Profile.Person.get({
              contactId: contactId
            }).$promise;
          },
          Volunteer: 'VolunteerService',
          Family: function(Volunteer) {
            return Volunteer.Family.query({
              contactId: crds_utilities.getCookie('userId')
            }).$promise;
          }
        }
      })
      .state("errors/404", {
        url: "/errors/404",
        templateUrl: "errors/404.html"
      })
      .state("errors/500", {
        url: "/errors/500",
        templateUrl: "errors/500.html"
      })
      .state("tools", {
        abstract: true,
        url: '/mptools',
        templateUrl: 'mp_tools/tools.html',
        data: {
          hideMenu: true,
          isProtected: true
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin
        }
      })
      .state("tools.su2s", {
        url: '/su2s',
        controller: 'SignupToServeController as su2s',
        templateUrl: 'signup_to_serve/su2s.html'
      })
      .state("content", {
        // This url will match a slash followed by anything (including additional slashes).
        url: "{link:contentRouteType}",
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
