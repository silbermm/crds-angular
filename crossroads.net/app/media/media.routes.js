(function() {
  'use strict';

  module.exports = MediaRoutes;

  MediaRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider'];

  function MediaRoutes($stateProvider, $urlMatcherFactory) {

    $stateProvider
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
        }
      })
      .state('media.all', {
        url: '',
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
        controller: 'SeriesController as series',
        templateUrl: 'media/seriesSingle.html',
        resolve: {
          Media: 'Media',
          $stateParams: '$stateParams',
          Messages: function (Media, Series, $stateParams) {
            debugger;
            var series = getSeriesById(Series.series, $stateParams.id)
            if (!series) {
              return null;
            }

            var item = Media.Messages({seriesId: series.id}).get().$promise;
            return item;

            function getSeriesById(series, seriesID) {
              return _.find(series, function (obj) {
                return (obj.id === seriesID);
              });
            };
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
        url: '/media/message/:id/:title?',
        controller: 'SingleMediaController as singleMedia',
        templateUrl: 'media/mediaSingle.html',
        resolve: {
          Media: 'Media',
          $stateParams: '$stateParams',
          ItemProperty: function () {
            return 'messages';
          },
          SingleMedia: function (Media, $stateParams) {
            var item = Media.Messages({title: $stateParams.id}).get().$promise;
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
            debugger;
            return _.get(SingleMedia.messages[0], 'video.still.filename');
          }
        }
      })
      .state('videoSingle', {
        parent: 'screenWidth',
        url: '/media/video/:title',
        controller: 'SingleMediaController as singleMedia',
        templateUrl: 'media/mediaSingle.html',
        resolve: {
          Media: 'Media',
          $stateParams: '$stateParams',
          ItemProperty: function () {
            return 'videos';
          },
          SingleMedia: function (Media, $stateParams) {
            var item = Media.Videos({title: $stateParams.title}).get().$promise;
            return item;
          },
          ParentItemProperty: function() {
            return null;
          },
          ParentMedia: function (Media, SingleMedia) {
            return null;
          },
          ImageURL: function (SingleMedia) {
            return _.get(SingleMedia.videos[0], 'still.filename');
          }
        }
      })
      .state('musicSingle', {
        parent: 'screenWidth',
        url: '/media/music/:title',
        controller: 'SingleMediaController as singleMedia',
        templateUrl: 'media/mediaSingle.html',
        resolve: {
          Media: 'Media',
          $stateParams: '$stateParams',
          ItemProperty: function () {
            return 'musics';
          },
          SingleMedia: function (Media, $stateParams) {
            var item = Media.Musics({title: $stateParams.title}).get().$promise;
            return item;
          },
          ParentItemProperty: function() {
            return null;
          },
          ParentMedia: function (Media, SingleMedia) {
            return null;
          },
          ImageURL: function (SingleMedia) {
            debugger;
            return _.get(SingleMedia.musics[0], 'still.filename');
          }
        }
      })
      ;
  }
})();