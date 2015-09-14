'use strict';

var app = angular.module('crossroads');

app.controller('GroupSignupController', [
    '$rootScope',
    '$scope',
    'Profile',
    'Group',
    '$log',
    '$stateParams',
    'Page',
    '$modal',
    require('./group_signup_controller')]);;

