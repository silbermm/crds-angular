'use strict';

var app = require("angular").module('crossroads');

require('./view-all.html');
require('./view-all-music.html');
require('./view-all-messages.html');
require('./view-all-other.html');

app.controller("MediaCtrl", ['$log', '$state', require("./media.controller")]);
