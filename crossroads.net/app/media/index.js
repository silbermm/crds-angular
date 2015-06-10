'use strict';

var app = require("angular").module('crossroads');

require('./view-all.html');
require('./view-all-music.html');
require('./view-all-messages.html');
require('./view-all-videos.html');
require('./media-search.html');
require('./itunes-btn-messages.html');
require('./itunes-btn-music.html');
require('./itunes-btn-videos.html');

app.controller("MediaCtrl", ['$log', '$state', require("./media.controller")]);
