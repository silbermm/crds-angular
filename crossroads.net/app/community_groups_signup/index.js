'use strict';

var app = require("angular").module('crossroads');

    app.controller('GroupSignupController', ['$scope',
     '$rootScope',
     'Profile',
     '$log', require('./group_signup_controller')]);
;