'use strict';

var app = angular.module('crossroads');
require('./tripgiving.html');

app.controller("TripGivingCtrl", require("./tripgiving.controller"));

require('./tripparticipantcard/tripParticipantCard.html');
app.directive("tripParticipantCard", require('./tripparticipantcard/tripParticipantCard.directive'));
