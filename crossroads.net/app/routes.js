(function() {
  'use strict';

  module.exports = AppConfig;

  AppConfig.$inject = ['$stateProvider',
    '$urlRouterProvider',
    '$httpProvider',
    '$urlMatcherFactoryProvider',
    '$locationProvider'
  ];

  function AppConfig($stateProvider,
    $urlRouterProvider,
    $httpProvider,
    $urlMatcherFactory,
    $locationProvider) {

    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'contentRouteType', /^\/.*/);
    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'signupRouteType', /\/sign-up\/.*$/);
    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'volunteerRouteType', /\/volunteer-sign-up\/.*$/);

    $stateProvider
      .state('noSideBar',{
        abstract:true,
        templateUrl: 'templates/noSideBar.html'
      })
      .state('leftSidebar',{
        abstract:true,
        templateUrl: 'templates/leftSidebar.html'
      })
      .state('rightSidebar',{
        abstract:true,
        templateUrl: 'templates/rightSidebar.html'
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
        controller: 'HomeCtrl',
        data: {
          meta: {
           title: 'Home',
           description: ''
          }
        }
      })
      .state('homealso', {
        parent: 'noSideBar',
        url: '/home',
        templateUrl: 'home/home.html',
        controller: 'HomeCtrl',
        data: {
          meta: {
           title: 'Home',
           description: ''
          }
        }
      })
      .state('login', {
        parent: 'noSideBar',
        url: '/login',
        templateUrl: 'login/login_page.html',
        controller: 'LoginCtrl',
        data: {
          isProtected: false,
          meta: {
           title: 'Login',
           description: ''
          }
        }
      })
      .state('logout', {
        url: '/logout',
        controller: 'LogoutController',
        data: {
          isProtected: false,
          meta: {
           title: 'Logout',
           description: ''
          }
        }
      })
      .state('register', {
        parent: 'noSideBar',
        url: '/register',
        templateUrl: 'register/register_form.html',
        controller: 'RegisterCtrl',
        data: {
          meta: {
           title: 'Register',
           description: ''
          }
        }
      })
      .state('profile', {
        parent: 'noSideBar',
        url: '/profile',
        resolve: {
          loggedin: crds_utilities.checkLoggedin
        },
        data: {
          isProtected: true,
          meta: {
           title: 'Profile',
           description: ''
          }
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
        data: {
          meta: {
           title: 'Profile',
           description: ''
          }
        }
      })
      .state("go-trip-select", {
        parent: 'noSideBar',
        url: "/go/:trip_location/select-person",
        templateUrl: "gotrips/signup-select-person.html",
        controller: 'GoTripsCtrl as gotrip'
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
        abstract: true,
        parent: 'noSideBar',
        url: '/media',
        controller: 'MediaController as media',
        template: '<ui-view/>',
        resolve: {
          Media: 'Media',
          Series: function (Media) {
            return Media.Series().get().$promise;
          },
          Musics: function (Media) {
            return Media.Musics().get().$promise;
          },
          Videos: function (Media) {
            return Media.Videos().get().$promise;
          }
        },
        data: {
          meta: {
           title: 'Media',
           description: ''
          }
        }
      })
      .state('media.all', {
        url: '',
        templateUrl: 'media/viewAll.html',
      })
      .state('media.music', {
        url: '/music',
        templateUrl: 'media/viewAllMusic.html',
        data: {
          meta: {
           title: 'Music',
           description: ''
          }
        }
      })
      .state('media.series', {
        url: '/series',
        templateUrl: 'media/viewAllSeries.html',
        data: {
          meta: {
           title: 'Series',
           description: ''
          }
        }
      })
      .state('media.videos', {
        url: '/videos',
        templateUrl: 'media/viewAllVideos.html',
        data: {
          meta: {
           title: 'Videos',
           description: ''
          }
        }
      })
      .state('media.seriesSingle', {
        url: '/series/single/:title',
        controller: 'SeriesController as series',
        templateUrl: 'media/seriesSingle.html',
          resolve: {
            Media: 'Media',
            $stateParams: '$stateParams',
            Meta: function ($stateParams) {
              return {
               title: $stateParams.title,
               description: ''
              };
            },
            Messages: function (Media, Series, $stateParams) {
              var series = getSeriesByTitle(Series.series, $stateParams.title);
              var item = Media.Messages().get({ seriesId: series.id }).$promise;
              return item;

              function getSeriesByTitle(series, seriesTitle) {
                return _.find(series, function(obj) {
                  return (obj.title === seriesTitle);
                });
              }
            }
          }
      })
      .state('media-series-single-lo-res', {
        parent: 'noSideBar',
        url: '/media/series/single/lores',
        controller: 'MediaController as media',
        templateUrl: 'media/series-single-lo-res.html'
      })
      .state('media-single', {
        parent: 'screenWidth',
        url: '/media/single',
        controller: 'MediaController as media',
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
          isProtected: true,
          meta: {
           title: 'Signup to Serve',
           description: ''
          }
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
        controller: 'GiveController as give',
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
        controller: 'GiveController as give',
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
        controller: 'GiveController as give',
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
        controller: 'GiveController as give',
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
        controller: 'GiveController as give',
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
          isProtected: true,
          meta: {
           title: 'Community Group Signup',
           description: ''
          }
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
          isProtected: true,
          meta: {
           title: 'Volunteer Signup',
           description: ''
          }
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
          isProtected: true,
          meta: {
           title: 'Volunteer Signup',
           description: ''
          }
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
      .state('corkboard', {
        url: '/corkboard/',
        resolve: {
          RedirectToSubSite: function ($window, $location) {
            // Force browser to do a full reload to load corkboard's index.html
            $window.location.href = $location.path();
          }
        },
        data: {
          preventRouteAuthentication: true,
          meta: {
           title: 'Corkboard',
           description: ''
          }
        }
      })
      .state('tools', {
        parent: 'noSideBar',
        abstract: true,
        url: '/mptools',
        templateUrl: 'mp_tools/tools.html',
        data: {
          hideMenu: true,
          isProtected: true,
          meta: {
           title: 'Tools',
           description: ''
          }
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
          isProtected: true,
          meta: {
           title: 'Kids Club Application',
           description: ''
          }
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
      .state('tools.tripParticipants', {
        url: '/tripParticipants',
        controller: 'TripParticipantController as trip',
        templateUrl: 'trip_participants/trip.html',
        resolve: {
          MPTools: 'MPTools',
          Trip: 'Trip',
          PageInfo: function(MPTools, Trip) {
            var params = MPTools.getParams();
            return Trip.TripFormResponses.get({
              selectionId: params.selectedRecord,
              selectionCount: params.selectedCount,
              recordId: params.recordId
            }).$promise.then(function(data) {
                    // promise fulfilled
                    return data;
                }, function(error) {
                    // promise rejected, could log the error with: console.log('error', error);
                    var data = {};
                    data.errors = error;
                    return error;
                });
          }
        }
      })
      .state('tools.checkBatchProcessor', {
        url: '/checkBatchProcessor',
        controller: 'CheckBatchProcessor as checkBatchProcessor',
        templateUrl: 'check_batch_processor/checkBatchProcessor.html',
        data: {
          isProtected: true,
          meta: {
           title: 'Check Batch Processor',
           description: ''
          }
        }
      })
      .state('content', {
        url: '{link:contentRouteType}',
        // This url will match a slash followed by anything (including additional slashes).
        views: {
          '': {
            controller: 'ContentCtrl',
            templateProvider: function($rootScope,
              $templateFactory,
              $stateParams,
              Page,
              ContentPageService,
              ContentSiteConfigService) {
              var promise;

              var link = addTrailingSlashIfNecessary($stateParams.link);
              promise = Page.get({ url: link }).$promise;

              return promise.then(function(promise) {

                if (promise.pages.length > 0) {
                  ContentPageService.page = promise.pages[0];
                } else {
                  var notFoundRequest = Page.get({ url: '/page-not-found/' }, function() {
                    if (notFoundRequest.pages.length > 0) {
                      ContentPageService.page.content = notFoundRequest.pages[0].content;
                      ContentPageService.page.pageType = '';
                    } else {
                      ContentPageService.page.content = '404 Content not found';
                      ContentPageService.page.pageType = '';
                    }
                  });
                }

                $rootScope.meta = {
                  title: ContentPageService.page.title +
                    ' | ' +
                    ContentSiteConfigService.siteconfig.title,
                  description: ContentPageService.page.metaDescription,
                  extraMeta: ContentPageService.page.extraMeta
                };

                switch(ContentPageService.page.pageType){
                  case 'NoHeaderOrFooter':
                    return $templateFactory.fromUrl('templates/noHeaderOrFooter.html');
                  case 'LeftSidebar':
                    return $templateFactory.fromUrl('templates/leftSideBar.html');
                  case 'RightSidebar':
                    return $templateFactory.fromUrl('templates/rightSideBar.html');
                  case 'ScreenWidth':
                    return $templateFactory.fromUrl('templates/screenWidth.html');
                  default:
                    return $templateFactory.fromUrl('templates/noSideBar.html');
                }
              });
            }
          },
          '@content': {
            templateUrl: 'content/content.html',
          },
          'sidebar@content': {
            templateUrl: 'content/sidebarContent.html'
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

  function addTrailingSlashIfNecessary(link) {
    if (_.endsWith(link, '/') === false) {
      return link + '/';
    }

    return link;
  }

})();
