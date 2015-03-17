'use strict';

require('./login_page.html');

var app = require("angular").module('crossroads');

app.controller("LoginCtrl", ["$scope", 
    '$rootScope',
    'AUTH_EVENTS', 
    'MESSAGES', 
    'AuthService', 
    "$cookieStore", 
    '$state', 
    '$log', 
    'Session', 
    '$timeout', 
    'User', require('./login_controller')])
;
app.directive("loginForm", ['$log','AUTH_EVENTS', require('./login_form_directive')]);
