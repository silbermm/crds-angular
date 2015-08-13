'use strict';

var MODULE = 'crossroads.trips';

var app =  angular.module(MODULE);
require('./tripgiving.html');

app.controller('TripGivingCtrl', require('./tripgiving.controller'));

require('./tripparticipantcard/tripParticipantCard.html');
app.directive('tripParticipantCard', require('./tripparticipantcard/tripParticipantCard.directive'));

app.factory('Trip', require('../../services/trip.service'));
