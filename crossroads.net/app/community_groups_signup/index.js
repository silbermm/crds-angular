'use strict';

var app = require("angular").module('crossroads');

    app.controller('GroupSignupController', [
     '$rootScope',
     '$scope',
     'Profile',
     'Group',
     '$log',
     '$stateParams', 
	 'Page',
	 require('./group_signup_controller')]);
;