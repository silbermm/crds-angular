'use strict';

var app = require("angular").module('crossroads');

require('./corkboard-listings.html');
require('./corkboard-create.html');
require('./forms/give-something.html');
require('./forms/post-event.html');
require('./forms/post-job.html');
require('./forms/post-need.html');
require('./corkboard-listing-detail.html');

app.controller("CorkboardCtrl", ['$scope', require("./corkboard.controller")]);