'use strict';

var app = require("angular").module('crossroads');

require('./blog-index.html');
require('./blog-post.html');

app.controller("BlogCtrl", ['$log', '$state', require("./blog.controller")]);