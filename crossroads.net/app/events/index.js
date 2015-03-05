'use strict';
require('angular-module-resource');

// Define atrium-events module
var app = require('angular').module('atrium-events', ['ngResource'], function($locationProvider) {
	// This is needed in order to be able to use the $location.search() to get the query parameters
	$locationProvider.html5Mode(true);
});

// Events Controller
app.controller('EventsController',['$scope','$log', '$http', '$location', 'Events', require('./events_controller')]);

// Events Service
app.factory('Events', ['$resource','$log', require('./service/events_service')]);
