'use strict';
var MODULE = 'crossroads.media';

  var app = angular.module(MODULE, ['crossroads.core'])
    .config(require('./media.routes'))
    .factory('Media', require('./services/media.service'));

require('./viewAll.html');
require('./viewAllMusic.html');
require('./viewAllSeries.html');
require('./viewAllVideos.html');
require('./seriesSingle.html');
require('./series-single-lo-res.html');
require('./mediaSingle.html');
require('./subscribe-btn-messages.html');
require('./subscribe-btn-music.html');
require('./subscribe-btn-videos.html');
require('./subscribe-btn-dropdown.html');
require('./media-list.html');
require('./messageActionButtons.html');
require('./mediaDetails.html');
require('./templates/mediaListCard.html');
require('./templates/seriesListCard.html');
require('./templates/messagesListCard.html');

app.controller('MediaController', require('./media.controller'));
app.controller('SingleMediaController', require('./singleMedia.controller'));
app.controller('SingleSeriesController', require('./singleSeries.controller.js'));
app.filter('replaceNonAlphaNumeric', require('./filters/replaceNonAlphaNumeric.filter.js'));

app.directive("mediaListCard", require('./directives/mediaListCard.directive'));