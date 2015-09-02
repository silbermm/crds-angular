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
        }
      })
      .state('media.all', {
        url: '/media',
        templateUrl: 'templates/viewAll.html',
      })
      .state('media.music', {
        url: '/music',
        templateUrl: 'templates/viewAllMusic.html'
      })
      .state('media.series', {
        url: '/series',
        templateUrl: 'templates/viewAllSeries.html'
      })
      .state('media.videos', {
        url: '/videos',
        templateUrl: 'templates/viewAllVideos.html'
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
          Messages: function (Media, Selected) {
            var item = Media.Messages({seriesId: Selected.id}).get().$promise;
            return item;
          }
        }
      })
      .state('media-series-single-lo-res', {
        parent: 'noSideBar',
        url: '/media/series/single/lores',
        controller: 'MediaController as media',
        templateUrl: 'templates/series-single-lo-res.html'
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
        resolve: {
          Media: 'Media',
          $stateParams: '$stateParams',
          $state: '$state',
          ItemProperty: function () {
            return 'messages';
          },
          SingleMedia: function (Media, $stateParams, $state) {
            var item = Media.Messages({id: $stateParams.id}).get().$promise;
            item.then(redirectIfItemNotFound);
            return item;

            // Doing this here instead of controller to prevent flicker of unbound page
            function redirectIfItemNotFound(data) {
              var media = data.messages[0];
              if (!media) {
                $state.go('content', {link: '/page-not-found/'}, {location: 'replace'});
              }
            }
          },
          ParentItemProperty: function() {
            return 'series';
          },
          ParentMedia: function (Media, SingleMedia) {
            var message = SingleMedia.messages[0];
            if (!message) {
              return null;
            }

            var parent = Media.Series({id: message.series}).get().$promise;
            return parent;
          },
          ImageURL: function (SingleMedia) {
            return _.get(SingleMedia.messages[0], 'video.still.filename');
          }
        }
      })
      .state('mediaSingle', {
        parent: 'screenWidth',
        url: '/media/{id:int}/:title?',
        controller: 'SingleMediaController as singleMedia',
        templateUrl: 'templates/mediaSingle.html',
        resolve: {
          Media: 'Media',
          $stateParams: '$stateParams',
          $state: '$state',
          ItemProperty: function () {
            return 'media';
          },
          SingleMedia: function (Media, $stateParams, $state) {
            var item = Media.Medias({id: $stateParams.id}).get().$promise;
            item.then(redirectIfItemNotFound);
            return item;

            // Doing this here instead of controller to prevent flicker of unbound page
            function redirectIfItemNotFound(data) {
              var media = data.media[0];
              if (!media) {
                $state.go('content', {link: '/page-not-found/'}, {location: 'replace'});
              }
            }
          },
          ParentItemProperty: function() {
            return null;
          },
          ParentMedia: function () {
            return null;
          },
          ImageURL: function (SingleMedia) {
            return _.get(SingleMedia.media[0], 'still.filename');
          }
        }
      })
      ;
  }
})();