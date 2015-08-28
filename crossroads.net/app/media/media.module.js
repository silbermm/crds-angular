'use strict';
var MODULE = 'crossroads.media';

  var app = angular.module(MODULE, ['crossroads.core'])
    .config(require('./media.routes'))
    .factory('Media', require('./services/media.service'));

require('./templates/viewAll.html');
require('./templates/viewAllMusic.html');
require('./templates/viewAllSeries.html');
require('./templates/viewAllVideos.html');
require('./templates/seriesSingle.html');
require('./templates/series-single-lo-res.html');
require('./templates/mediaSingle.html');
require('./templates/subscribe-btn-messages.html');
require('./templates/subscribe-btn-music.html');
require('./templates/subscribe-btn-videos.html');
require('./templates/subscribe-btn-dropdown.html');
require('./templates/media-list.html');
require('./templates/messageActionButtons.html');
require('./templates/mediaDetails.html');
require('./templates/mediaListCard.html');
require('./templates/seriesListCard.html');
require('./templates/messagesListCard.html');

app.controller('MediaController', require('./media.controller'));
app.controller('SingleMediaController', require('./singleMedia.controller'));
app.controller('SingleSeriesController', require('./singleSeries.controller.js'));
app.filter('replaceNonAlphaNumeric', require('./filters/replaceNonAlphaNumeric.filter.js'));

app.directive("mediaListCard", require('./directives/mediaListCard.directive'));