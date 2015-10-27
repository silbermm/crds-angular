var app = angular.module('crossroads');
require('fullpage.js');
require('./explore.html');

app.controller('ExploreCtrl', require('./explore_controller'));
