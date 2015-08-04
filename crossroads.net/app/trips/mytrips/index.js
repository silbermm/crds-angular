'use strict';

var app =  angular.module('crossroads');
require('./mytrips.html');
require('./mytripCard.html');
app.directive('myTripCard', require('./mytripCard.directive'));

app.controller('MyTripsController', require("./mytrips.controller"));