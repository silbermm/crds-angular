'use strict';
require('angular-module-resource');

// Define atrium-events module
var app = require('angular').module('atrium-events', ['ngResource']);

// Events Controller
app.controller('EventsController',['$scope','$log', '$http', 'Events', require('./events_controller')]);

// Events Service
app.factory('Events', ['$resource','$log', require('./service/events_service')]);
