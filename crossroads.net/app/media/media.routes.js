(function() {
  'use strict';

  module.exports = MediaRoutes;

  MediaRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider'];

  function MediaRoutes($stateProvider, $urlMatcherFactory) {

    $stateProvider
      .state('media', {
        abstract: true,
        parent: 'noSideBar',
        url: '',
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
        url: '/media',
        templateUrl: 'templates/viewAll.html',
        data: {
          meta: {
           title: 'Media',
           description: ''
          }
        }
      })
      .state('media.music', {
        url: '/music',
        templateUrl: 'templates/viewAllMusic.html',
        data: {
          meta: {
           title: 'Music',
           description: ''
          }
        }
      })
      .state('media.series', {
        url: '/series',
        templateUrl: 'templates/viewAllSeries.html',
        data: {
          meta: {
           title: 'Series',
           description: ''
          }
        }
      })
      .state('media.videos', {
        url: '/videos',
        templateUrl: 'templates/viewAllVideos.html',
        data: {
          meta: {
           title: 'Videos',
           description: ''
          }
        }
      })
      .state('media.seriesSingle', {
        url: '/series/{id:int}/:title?',
        controller: 'SingleSeriesController as series',
        templateUrl: 'templates/seriesSingle.html',
        resolve: {
          Media: 'Media',
          $stateParams: '$stateParams',
          $state: '$state',
          Selected: function(Media, Series, $stateParams, $state) {
            var singleSeries = _.find(Series.series, function (obj) {
              return (obj.id === $stateParams.id);
            });

            if (!singleSeries) {
              // Doing this here instead of controller to prevent flicker of unbound page
              $state.go('content', {link: '/page-not-found/'}, {location: 'replace'});
              return;
            }
            return singleSeries;
          },
          Meta: function (Selected, $state) {
            $state.next.data.meta = {
             title: Selected.title,
             description: ''
            };
            return $state.next.data.meta;
          },
          Messages: function (Media, Selected) {
            var item = Media.Messages({seriesId: Selected.id}).get().$promise;
            return item;
          }
        }
      })
      .state('media-single', {
        parent: 'screenWidth',
        url: '/media/single',
        controller: 'MediaController as media',
        templateUrl: 'templates/mediaSingle.html'
      })
      .state('messageSingle', {
        parent: 'screenWidth',
        url: '/message/:id/:title?',
        controller: 'SingleMediaController as singleMedia',
        templateUrl: 'templates/mediaSingle.html',
        data: {
          meta: {
           title: 'Message',
           description: ''
          }
        },
        resolve: {
          Media: 'Media',
          $stateParams: '$stateParams',
          $state: '$state',
          ItemProperty: function () {
            return 'message';
          },
          SingleMedia: function (Media, $stateParams, $state) {
            var item = Media.Messages({id: $stateParams.id}).get().$promise;
            item.then(redirectIfItemNotFound);
            return item;

            // Doing this here instead of controller to prevent flicker of unbound page
            function redirectIfItemNotFound(data) {
              var media = data.message;
              if (!media) {
                $state.go('content', {link: '/page-not-found/'}, {location: 'replace'});
              }
            }
          },
          Meta: function (SingleMedia, $state) {
            $state.next.data.meta = {
             title: SingleMedia.message.title,
             description: ''
            };
            return $state.next.data.meta;
          },
          ParentItemProperty: function() {
            return 'series';
          },
          ParentMedia: function (Media, SingleMedia) {
            var message = SingleMedia.message;
            if (!message) {
              return null;
            }

            var parent = Media.Series({id: message.series}).get().$promise;
            return parent;
          },
          ImageURL: function (SingleMedia) {
            return _.get(SingleMedia.message, 'video.still.filename');
          }
        }
      })
      .state('mediaSingle', {
        parent: 'screenWidth',
        url: '/media/{id:int}/:title?',
        controller: 'SingleMediaController as singleMedia',
        templateUrl: 'templates/mediaSingle.html',
        data: {
          meta: {}
        },
        resolve: {
          Media: 'Media',
          $rootScope: '$rootScope',
          $stateParams: '$stateParams',
          $state: '$state',
          ItemProperty: function (SingleMedia) {
            return Object.keys(SingleMedia)[0];
          },
          SingleMedia: function (Media, $stateParams, $state) {
            var item = Media.Medias({id: $stateParams.id}).get().$promise;
            item.then(redirectIfItemNotFound);
            return item;

            // Doing this here instead of controller to prevent flicker of unbound page
            function redirectIfItemNotFound(data) {
              var media = data[Object.keys(data)[0]];
              if (!media) {
                $state.go('content', {link: '/page-not-found/'}, {location: 'replace'});
              }
            }
          },
          Meta: function (SingleMedia, $state) {
            $state.next.data.meta = {
             title: SingleMedia[Object.keys(SingleMedia)[0]].title,
             description: ''
            };
            return $state.next.data.meta;
          },
          ParentItemProperty: function() {
            return null;
          },
          ParentMedia: function () {
            return null;
          },
          ImageURL: function (SingleMedia) {
            return _.get(SingleMedia[Object.keys(SingleMedia)[0]], 'still.filename');
          }
        }
      })
      ;
  }
})();
