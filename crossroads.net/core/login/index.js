'use strict';

require('./login_page.html');

var app = angular.module('crossroads.core');

app.controller("LoginCtrl", ["$scope", 
    '$rootScope',
    'AUTH_EVENTS', 
    'MESSAGES', 
    'AuthService', 
    '$state', 
    '$log', 
    'Session', 
    '$timeout', 
    'User', require('./login_controller')])
;
app.directive("loginForm", ['$log','AUTH_EVENTS', require('./login_form_directive')]);
