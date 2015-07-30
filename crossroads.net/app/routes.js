(function() {
  'use strict';

  module.exports = AppConfig;

  AppConfig.$inject = ['$stateProvider',
    '$urlRouterProvider',
    '$httpProvider',
    '$urlMatcherFactoryProvider',
    '$locationProvider',
    'ContentPageServiceProvider'
  ];

  function AppConfig($stateProvider,
    $urlRouterProvider,
    $httpProvider,
    $urlMatcherFactory,
    $locationProvider,
    ContentPageService) {

    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'contentRouteType', /^\/.*/);
    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'signupRouteType', /\/sign-up\/.*$/);
    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'volunteerRouteType', /\/volunteer-sign-up\/.*$/);

    $stateProvider
      .state('noSideBar',{
        abstract:true,
        templateUrl: 'templates/noSideBar.html'
      })
      .state('leftSideBar',{
        abstract:true,
        templateUrl: 'templates/leftSideBar.html'
      })
      .state('rightSideBar',{
        abstract:true,
        templateUrl: 'templates/rightSideBar.html'
      })
      .state('screenWidth',{
        abstract:true,
        templateUrl: 'templates/screenWidth.html'
      })
      .state('noHeaderOrFooter',{
        abstract:true,
        templateUrl: 'templates/noHeaderOrFooter.html'
      })
      .state('home', {
        parent: 'noSideBar',
        url: '/',
        templateUrl: 'home/home.html',
        controller: 'HomeCtrl'
      })
      .state('homealso', {
        parent: 'noSideBar',
        url: '/home',
        templateUrl: 'home/home.html',
        controller: 'HomeCtrl'
      })
      .state('login', {
        parent: 'noSideBar',
        url: '/login',
        templateUrl: 'login/login_page.html',
        controller: 'LoginCtrl',
        data: {
          isProtected: false
        }
      })
      .state('logout', {
        url: '/logout',
        controller: 'LogoutController',
        data: {
          isProtected: false
        }
      })
      .state('register', {
        parent: 'noSideBar',
        url: '/register',
        templateUrl: 'register/register_form.html',
        controller: 'RegisterCtrl'
      })
      .state('profile', {
        parent: 'noSideBar',
        url: '/profile',
        resolve: {
          loggedin: crds_utilities.checkLoggedin
        },
        data: {
          isProtected: true
        },
        views: {
          '': {
            templateUrl: 'profile/profile.html',
            controller: 'crdsProfileCtrl as profile',
            resolve: {
              loggedin: crds_utilities.checkLoggedin
            },
          },
          'personal@profile': {
            templateUrl: 'personal/profile_personal.html',
            data: {
              isProtected: true
            },
          },
          'account@profile': {
            templateUrl: 'profile/profile_account.html',
            data: {
              isProtected: true
            }
          },
          'skills@profile': {
            controller: 'ProfileSkillsController as profile',
            templateUrl: 'skills/profile_skills.html',
            data: {
              isProtected: true
            }
          }
        }
      })
      .state('myprofile', {
        parent: 'noSideBar',
        url: '/myprofile',
        controller: 'MyProfileCtrl as myProfile',
        templateUrl: 'myprofile/myprofile.html',
      })
      .state("mytrips", {
        url: "/mytrips",
        templateUrl: "mytrips/mytrips.html"
      })
      .state("go-trip-signup", {
        parent: 'noSideBar',
        url: "/go/:trip_location/signup",
        templateUrl: "gotrips/signup-page-1.html",
        controller: 'GoTripsCtrl as gotrip'
      })
      .state("go-trip-signup-page-2", {
        parent: 'noSideBar',
        url: "/go/:trip_location/signup/2",
        templateUrl: "gotrips/signup-page-2.html",
        controller: 'GoTripsCtrl as gotrip'
      })
      .state("go-trip-signup-page-3", {
        parent: 'noSideBar',
        url: "/go/:trip_location/signup/3",
        templateUrl: "gotrips/signup-page-3.html",
        controller: 'GoTripsCtrl as gotrip'
      })
      .state("go-trip-signup-page-4", {
        parent: 'noSideBar',
        url: "/go/:trip_location/signup/4",
        templateUrl: "gotrips/signup-page-4.html",
        controller: 'GoTripsCtrl as gotrip'
      })
      .state("go-trip-signup-page-5", {
        parent: 'noSideBar',
        url: "/go/:trip_location/signup/5",
        templateUrl: "gotrips/signup-page-5.html",
        controller: 'GoTripsCtrl as gotrip'
      })
      .state("go-trip-signup-page-confirmation", {
        parent: 'noSideBar',
        url: "/go/:trip_location/signup/confirmation",
        templateUrl: "gotrips/signup-page-confirmation.html",
        controller: 'GoTripsCtrl as gotrip'
      })
      .state('media', {
        parent: 'noSideBar',
        url: '/media',
        controller: 'MediaCtrl as media',
        templateUrl: 'media/view-all.html'
      })
      .state('media-music', {
        parent: 'noSideBar',
        url: '/media/music',
        controller: 'MediaCtrl as media',
        templateUrl: 'media/view-all-music.html'
      })
      .state('media-messages', {
        parent: 'noSideBar',
        url: '/media/messages',
        controller: 'MediaCtrl as media',
        templateUrl: 'media/view-all-messages.html'
      })
      .state('media-videos', {
        parent: 'noSideBar',
        url: '/media/videos',
        controller: 'MediaCtrl as media',
        templateUrl: 'media/view-all-videos.html'
      })
      .state('media-series-single', {
        parent: 'noSideBar',
        url: '/media/series/single',
        controller: 'MediaCtrl as media',
        templateUrl: 'media/series-single.html'
      })
      .state('media-series-single-lo-res', {
        parent: 'noSideBar',
        url: '/media/series/single/lores',
        controller: 'MediaCtrl as media',
        templateUrl: 'media/series-single-lo-res.html'
      })
      .state('media-single', {
        parent: 'screenWidth',
        url: '/media/single',
        controller: 'MediaCtrl as media',
        templateUrl: 'media/media-single.html'
      })
      .state('blog', {
        parent: 'noSideBar',
        url: '/blog',
        controller: 'BlogCtrl as blog',
        templateUrl: 'blog/blog-index.html'
      })
      .state('blog-post', {
        parent: 'noSideBar',
        url: '/blog/post',
        controller: 'BlogCtrl as blog',
        templateUrl: 'blog/blog-post.html'
      })
      .state('adbox', {
        parent: 'noSideBar',
        url: '/adbox',
        controller: 'AdboxCtrl as adbox',
        templateUrl: 'adbox/adbox-index.html'
      })
      .state('serve-signup', {
        parent: 'noSideBar',
        url: '/serve-signup',
        controller: 'MyServeController as serve',
        templateUrl: 'my_serve/myserve.html',
        data: {
          isProtected: true
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin,
          ServeOpportunities: 'ServeOpportunities',
          $cookies: '$cookies',
          Groups: function(ServeOpportunities, $cookies) {
            return ServeOpportunities.ServeDays.query({
              id: $cookies.get('userId')
            }).$promise;
          }
        }
      })
      .state('styleguide', {
        parent: 'noSideBar',
        url: '/styleguide',
        controller: 'StyleguideCtrl as styleguide',
        templateUrl: 'styleguide/styleguide.html'
      })
      .state('thedaily', {
        parent: 'noSideBar',
        url: '/thedaily',
        templateUrl: 'thedaily/thedaily.html'
      })
      .state('give', {
        parent: 'noSideBar',
        url: '/give',
        controller: 'GiveCtrl as give',
        templateUrl: 'give/give.html',
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
      .state('give.amount', {
        templateUrl: 'give/amount.html'
      })
      .state('give.login', {
        controller: 'LoginCtrl',
        templateUrl: 'give/login.html'
      })
      .state('give.register', {
        controller: 'RegisterCtrl',
        templateUrl: 'give/register.html'
      })
      .state('give.confirm', {
        templateUrl: 'give/confirm.html'
      })
      .state('give.account', {
        templateUrl: 'give/account.html'
      })
      .state('give.change', {
        templateUrl: 'give/change.html'
      })
      .state('give.thank-you', {
        templateUrl: 'give/thank_you.html'
      })
      //Not a child route of give because I did not want to use the parent give template
      .state('history', {
        parent: 'noSideBar',
        url: '/give/history',
        templateUrl: 'give/history.html'
      })
      .state('demo', {
        parent: 'noSideBar',
        //abstract: true,
        url: '/demo',
        template: '<p>demo</p>'
      })
      .state('tripgiving', {
        parent: 'noSideBar',
        url: '/tripgiving',
        controller: 'TripGivingCtrl as tripSearch',
        templateUrl: 'tripgiving/tripgiving.html',
        resolve: {
          Page: 'Page',
          CmsInfo: function(Page, $stateParams) {
            return Page.get({
              url: '/tripgiving/'
            }).$promise;
          }
        }
      })
      .state('go_trip_giving_results', {
        parent: 'noSideBar',
        url: '/go_trip_giving_results',
        controller: 'TripGivingCtrl as gotripresults',
        templateUrl: 'tripgiving/tripgivingresults.html'
      })
      .state('/demo/guest-giver', {
        parent: 'noSideBar',
        url: '/demo/guest-giver',
        templateUrl: 'guest_giver/give.html'
      })
      .state('/demo/guest-giver/login', {
        parent: 'noSideBar',
        url: '/demo/guest-giver/login',
        templateUrl: 'guest_giver/give-login.html'
      })
      .state('/demo/guest-giver/login-guest', {
        parent: 'noSideBar',
        url: '/demo/guest-giver/login-guest',
        controller: 'GiveCtrl as give',
        templateUrl: 'guest_giver/give-login-guest.html'
      })
      .state('/demo/guest-giver/give-confirmation', {
        parent: 'noSideBar',
        url: '/demo/guest-giver/confirmation',
        templateUrl: 'guest_giver/give-confirmation.html'
      })
      .state('/demo/guest-giver/give-register', {
        parent: 'noSideBar',
        url: '/demo/guest-giver/register',
        templateUrl: 'guest_giver/give-register.html'
      })
      .state('/demo/guest-giver/give-logged-in-bank-info', {
        parent: 'noSideBar',
        url: '/demo/guest-giver/logged-in-bank-info',
        controller: 'GiveCtrl as give',
        templateUrl: 'guest_giver/give-logged-in-bank-info.html'
      })
      .state('/demo/guest-giver/give-confirm-amount', {
        parent: 'noSideBar',
        url: '/demo/guest_giver/give-confirm-amount',
        templateUrl: 'guest_giver/give-confirm-amount.html'
      })
      .state('/demo/guest-giver/give-change-information', {
        parent: 'noSideBar',
        url: '/demo/guest_giver/give-change-information',
        controller: 'GiveCtrl as give',
        templateUrl: 'guest_giver/give-change-information.html'
      })
      .state('/demo/logged-in-giver/existing-giver', {
        parent: 'noSideBar',
        url: '/demo/logged-in-giver/existing-giver',
        templateUrl: 'guest_giver/give-logged-in.html'
      })
      .state('/demo/logged-in-giver/change-information', {
        parent: 'noSideBar',
        url: '/demo/logged-in-giver/change-information',
        controller: 'GiveCtrl as give',
        templateUrl: 'guest_giver/give-change-information-logged-in.html'
      })
      .state('/demo/logged-in-giver/new-giver', {
        parent: 'noSideBar',
        url: '/demo/logged-in-giver/new-giver',
        templateUrl: 'guest_giver/give-logged-in-new-giver.html'
      })
      .state('/demo/go-trip-giving', {
        parent: 'noSideBar',
        url: '/demo/go-trip-giving',
        templateUrl: 'trip_giving/give.html'
      })
      .state('search', {
        parent: 'noSideBar',
        url: '/search-results',
        controller: 'SearchCtrl as search',
        templateUrl: 'search/search-results.html'
      })
      .state('community-groups-signup', {
        parent: 'noSideBar',
        url: '{link:signupRouteType}',
        controller: 'GroupSignupController as groupsignup',
        templateUrl: 'community_groups_signup/group_signup_form.html',
        data: {
          isProtected: true
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin
        }
      })
      .state('volunteer-request', {
        parent: 'noSideBar',
        url: '{link:volunteerRouteType}',
        controller: 'VolunteerController as volunteer',
        templateUrl: 'volunteer_signup/volunteer_signup_form.html',
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
      .state('volunteer-application', {
        parent: 'noSideBar',
        url: '/volunteer-application/:appType/:id',
        controller: 'VolunteerApplicationController as volunteer',
        templateUrl: 'volunteer_application/volunteerApplicationForm.html',
        data: {
          isProtected: true
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin,
          Page: 'Page',
          PageInfo: function($q, Profile, Page, $stateParams) {
            var deferred = $q.defer();
            var contactId = $stateParams.id;

            Profile.Person.get({
              contactId: contactId
            }).$promise.then(
              function(contact) {
                var age = contact.age;
                var cmsPath = '/kids-club-applicant-form/adult-applicant-form/';
                if ((age >= 10) && (age <= 15)) {
                  cmsPath = '/kids-club-applicant-form/student-applicant-form/';
                }
                Page.get({
                    url: cmsPath
                  }).$promise.then(function(cmsInfo) {
                      deferred.resolve({
                        contact, cmsInfo
                      });
                    }
                  );
              });
            return deferred.promise;
          },
          Volunteer: 'VolunteerService',
          Family: function(Volunteer) {
            return Volunteer.Family.query({
              contactId: crds_utilities.getCookie('userId')
            }).$promise;
          }
        }
      })
      .state('errors/404', {
        parent: 'noSideBar',
        url: '/errors/404',
        templateUrl: 'errors/404.html'
      })
      .state('errors/500', {
        parent: 'noSideBar',
        url: '/errors/500',
        templateUrl: 'errors/500.html'
      })
      .state('tools', {
        parent: 'noSideBar',
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
      .state('tools.su2s', {
        url: '/su2s',
        controller: 'SignupToServeController as su2s',
        templateUrl: 'signup_to_serve/su2s.html'
      })
      .state('tools.kcApplicant', {
        url: '/kcapplicant',
        controller: 'KCApplicantController as applicant',
        templateUrl: 'kc_applicant/applicant.html',
        data: {
          isProtected: true
        },
        resolve: {
          loggedin: crds_utilities.checkLoggedin,
          Profile: 'Profile',
          MPTools: 'MPTools',
          Contact: function(Profile, MPTools) {
            var params = MPTools.getParams();
            return Profile.Person.get({
              contactId: params.recordId
            }).$promise;
          },
          Page: 'Page',
          CmsInfo: function(Page, $stateParams) {
            return Page.get({
              url: '/volunteer-application/kids-club/'
            }).$promise;
          }

        }
      })
      .state('content', {
        url: '{link:contentRouteType}',
        // This url will match a slash followed by anything (including additional slashes).
        views: {
          '': {
            controller: 'ContentCtrl',
            templateProvider: function($templateFactory, $stateParams, Page, ContentPageService) {
              var promise;
              promise = Page.get({ url: $stateParams.link }).$promise;

              return promise.then(function(promise) {
                if (promise.pages.length > 0) {
                  ContentPageService.page = promise.pages[0];
                } else {
                  var notFoundRequest = Page.get({ url: '/page-not-found/' }, function() {
                    if (notFoundRequest.pages.length > 0) {
                      ContentPageService.page.renderedContent = notFoundRequest.pages[0].renderedContent;
                      ContentPageService.page.pageType = '';
                    } else {
                      ContentPageService.page.renderedContent = '404 Content not found';
                      ContentPageService.page.pageType = '';
                    }
                  });
                }
                switch(ContentPageService.page.pageType){
                  case 'NoHeaderOrFooter':
                    return $templateFactory.fromUrl('templates/noHeaderOrFooter.html');
                  default:
                    return $templateFactory.fromUrl('templates/noSideBar.html');
                }
              });
            }
          },
          '@content': {
            templateUrl: 'content/content.html'
          }
        }
      });
    //Leave the comment below.  Once we have a true 404 page hosted in the same domain, this is how we
    //will handle the routing.
    //.state('404', {
    //    templateUrl: __CMS_ENDPOINT__ + '/page-not-found/'
    //});

    $urlRouterProvider.otherwise('/');
  }
})();
