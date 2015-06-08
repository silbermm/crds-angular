'use strict';

var app = require("angular").module('crossroads');

require('./view-all.html');

app.controller("MediaCtrl", ['$log', '$state', require("./media.controller")]);
