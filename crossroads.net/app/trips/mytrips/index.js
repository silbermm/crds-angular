'use strict';

var app =  angular.module('crossroads');
require('./mytrips.html');
require('./mytripCard.html');
require('./tripDonations.html');
app.directive('myTripCard', require('./mytripCard.directive'));
app.directive('tripDonations', require('./tripDonations.directive'));

app.controller('MyTripsController', require("./mytrips.controller"));