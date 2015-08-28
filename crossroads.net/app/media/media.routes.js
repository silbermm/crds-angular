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
        templateUrl: 'media/viewAll.html',
      })
      .state('media.music', {
        url: '/music',
        templateUrl: 'media/viewAllMusic.html'
      })
      .state('media.series', {
        url: '/series',
        templateUrl: 'media/viewAllSeries.html'
      })
      .state('media.videos', {
        url: '/videos',
        templateUrl: 'media/viewAllVideos.html'
      })
      .state('media.seriesSingle', {
        url: '/series/{id:int}/:title?',
        controller: 'SingleSeriesController as series',
        templateUrl: 'media/seriesSingle.html',
        resolve: {
          Media: 'Media',
          $stateParams: '$stateParams',
          Selected: function(Media, Series, $stateParams) {
            return _.find(Series.series, function (obj) {
              return (obj.id === $stateParams.id);
            });
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
        templateUrl: 'media/series-single-lo-res.html'
      })
      .state('media-single', {
        parent: 'screenWidth',
        url: '/media/single',
        controller: 'MediaController as media',
        templateUrl: 'media/mediaSingle.html'
      })
      .state('messageSingle', {
        parent: 'screenWidth',
        url: '/message/:id/:title?',
        controller: 'SingleMediaController as singleMedia',
        templateUrl: 'media/mediaSingle.html',
        resolve: {
          Media: 'Media',
          $stateParams: '$stateParams',
          ItemProperty: function () {
            return 'messages';
          },
          SingleMedia: function (Media, $stateParams) {
            var item = Media.Messages({id: $stateParams.id}).get().$promise;
            return item;
          },
          ParentItemProperty: function() {
            return 'series';
          },
          ParentMedia: function (Media, SingleMedia) {
            if (!SingleMedia.messages[0]) {
              return null;
            }

            var seriesId = SingleMedia.messages[0].series;
            var parent = Media.Series({id: seriesId}).get().$promise;
            return parent;
          },
          ImageURL: function (SingleMedia) {
            if (!SingleMedia.messages[0]) {
              return null;
            }
            return _.get(SingleMedia.messages[0], 'video.still.filename');
          }
        }
      })
      .state('mediaSingle', {
        parent: 'screenWidth',
        url: '/media/{id:int}/:title?',
        controller: 'SingleMediaController as singleMedia',
        templateUrl: 'media/mediaSingle.html',
        resolve: {
          Media: 'Media',
          $stateParams: '$stateParams',
          ItemProperty: function () {
            return 'media';
          },
          SingleMedia: function (Media, $stateParams) {
            var item = Media.Medias({id: $stateParams.id}).get().$promise;
            return item;
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