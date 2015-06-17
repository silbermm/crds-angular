'use strict';

var app = require("angular").module('crossroads');

require('./search-results.html');

app.controller("SearchCtrl", ['$log', '$state', require("./search.controller")]);