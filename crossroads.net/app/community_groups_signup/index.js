'use strict';

var app = require("angular").module('crossroads');

    app.controller('GroupSignupController', ['$scope',
     '$rootScope',
     '$log', require('./group_signup_controller')]);
;