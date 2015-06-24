'use strict';

var app = require("angular").module('crossroads');

require('./blog-index.html');

app.controller("BlogCtrl", ['$log', '$state', require("./blog.controller")]);