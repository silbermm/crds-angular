'use strict';

var app = angular.module('crossroads');

require('./view-all.html');
require('./view-all-music.html');
require('./view-all-messages.html');
require('./view-all-videos.html');
require('./series-single.html');
require('./series-single-lo-res.html');
require('./media-single.html');
require('./subscribe-btn-messages.html');
require('./subscribe-btn-music.html');
require('./subscribe-btn-videos.html');
require('./subscribe-btn-dropdown.html');
require('./media-list.html');
require('./message-action-buttons.html');
require('./media-details.html');
require('./templates/media.list.card.html');


app.controller('MediaCtrl', require('./media.controller'));
app.factory('Media', require('./services/media.service'));
app.directive("mediaListCard", require('./directives/media.list.card'));