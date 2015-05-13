'use strict';

var app = require("angular").module('crossroads');

require('./corkboard-listings.html');
require('./corkboard-listing-detail.html');
require('./give-something.html');
require('./post-event.html');
require('./post-job.html');
require('./post-need.html');

app.controller("CorkboardCtrl", ['$log', '$state', require("./corkboard.controller")]);