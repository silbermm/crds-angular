'use strict';

var MODULE = 'crossroads.trips';

var app =  angular.module(MODULE);
require('./mytrips.html');
require('./mytripCard.html');
require('./tripDonations.html');
app.directive('myTripCard', require('./mytripCard.directive'));
app.directive('tripDonations', require('./tripDonations.directive'));

app.controller('MyTripsController', require("./mytrips.controller"));
