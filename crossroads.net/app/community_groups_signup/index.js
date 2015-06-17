'use strict';

var app = angular.module('crossroads');
require('../profile/personal/profile_personal.html');
require('../profile');

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

